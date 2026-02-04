using FoxWallet.Entities;
using FoxWallet.Services;
using FoxWallet.Utils;
using FoxWallet.ViewModels.WindowsVM;
using FoxWallet.Views.Windows;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace FoxWallet.ViewModels {
    public class PrincipalViewModel : ViewModelBase {
        // --- Actions ---
        public event Action RequestClose;
        public event Action Minimize;
        public event Action MaximizeRestore;

        // --- Relatorios ---
        public readonly RelatorioService RelService;

        // --- Storage ---
        public StorageService Sservice;

        // --- Orçamento ---
        public decimal Orcamento => ReceitasVM.ValorTOTReceitas - DespesasVM.ValorTOTDespesas;

        // --- Mês atual ---
        private string _mA;
        public string MA {
            get { return _mA; }
            set { _mA = value; OnPropertyChanged("MA"); }
        }

        // --- PopUp ---
        public bool PopUpRelatorioIsOpen;

        // --- Receita ---
        private readonly ReceitaService Rservice;

        public ReceitasViewModel ReceitasVM { get; }
        public FiltroReceitasViewModel FiltroReceitaVM { get; }

        // --- Despesa ---
        private readonly DespesaService Dservice;

        public DespesasViewModel DespesasVM { get; }
        public FiltroDespesasViewModel FiltroDespesasVM { get; }

        // --- ICollectionViews ---
        public ICollectionView ReceitasFiltradaView { get; set; }
        public ICollectionView DespesasFiltradaView { get; set; }

        // --- Commands ---
        public ICommand AbrirNovaReceitaCommand { get; }
        public ICommand FiltrarReceitaCommand { get; }
        public ICommand AbrirNovaDespesaCommand { get; }
        public ICommand FiltrarDespesaCommand { get; }
        public ICommand FecharJanelaPrincipalCommand { get; }
        public ICommand MinimizarJanelaPrincipalCommand { get; }
        public ICommand MaximizarJanelaPrincipalCommand { get; }

        public ICommand RelatorioMesPDFCommand { get; }
        public ICommand RelatorioAnoPDFCommand { get; }
        public ICommand RelatorioMesCSVCommand { get; }
        public ICommand RelatorioAnoCSVCommand { get; }

        // Construtor
        public PrincipalViewModel(ReceitaService rs, DespesaService ds, StorageService ss, RelatorioService rrs) {
            // --- Service ---
            Rservice = rs;
            Dservice = ds;
            Sservice = ss;
            RelService = rrs;

            // --- ViewModels ---
            FiltroReceitaVM = new FiltroReceitasViewModel();
            ReceitasVM = new ReceitasViewModel(rs, FiltroReceitaVM);

            FiltroDespesasVM = new FiltroDespesasViewModel();
            DespesasVM = new DespesasViewModel(ds, FiltroDespesasVM, DespesasFiltradaView);

            // --- Views Filtradas ---
            ReceitasFiltradaView = ReceitasVM.ReceitasFiltradaView;
            DespesasFiltradaView = DespesasVM.DespesasFiltradaView;

            // - Mês atual -
            MA = $"Data: {DateTime.Now.ToShortDateString()}";

            // --- Instancias Commands ---
            AbrirNovaReceitaCommand = new SimpleCommand(_ => AbrirAddReceita(rs), _ => true);
            FiltrarReceitaCommand = new SimpleCommand(_ => ReceitasFiltradaView.Refresh(), _ => true);

            AbrirNovaDespesaCommand = new SimpleCommand(_ => AbrirAddDespesa(ds), _ => true);
            FiltrarDespesaCommand = new SimpleCommand(_ => DespesasFiltradaView.Refresh(), _ => true);

            FecharJanelaPrincipalCommand = new SimpleCommand(_ => RequestClose(), _ => true);
            MaximizarJanelaPrincipalCommand = new SimpleCommand(_ => MaximizeRestore(), _ => true);
            MinimizarJanelaPrincipalCommand = new SimpleCommand(_ => Minimize(), _ => true);

            RelatorioMesPDFCommand = new SimpleCommand(_ => AbrirMesRelatorio(true), _ => true);
            RelatorioAnoPDFCommand = new SimpleCommand(_ => AbrirAnoRelatorio(true), _ => true);
            RelatorioMesCSVCommand = new SimpleCommand(_ => AbrirMesRelatorio(false), _ => true);
            RelatorioAnoCSVCommand = new SimpleCommand(_ => AbrirAnoRelatorio(false), _ => true);

            // --- Config filtros e services ---
            ReceitasFiltradaView = CollectionViewSource.GetDefaultView(ReceitasVM.listReceitas);
            ReceitasFiltradaView.Filter = FiltroReceitaVM.FiltrarReceitas;

            Rservice.receitas.CollectionChanged += (_, __) => {
                ReceitasFiltradaView.Refresh();
                OnPropertyChanged(nameof(ReceitasVM.ValorTOTReceitas));
                OnPropertyChanged(nameof(Orcamento));
            };

            FiltroReceitaVM.FiltroAlterado += () => {
                ReceitasFiltradaView.Refresh();
                OnPropertyChanged(nameof(Orcamento));
            };

            DespesasFiltradaView = CollectionViewSource.GetDefaultView(DespesasVM.listDespesas);
            DespesasFiltradaView.Filter = FiltroDespesasVM.FiltrarDespesas;

            Dservice.despesas.CollectionChanged += (_, __) => {
                DespesasFiltradaView.Refresh();
                OnPropertyChanged(nameof(DespesasVM.ValorTOTDespesas));
                OnPropertyChanged(nameof(Orcamento));
            };

            FiltroDespesasVM.FiltroAlterado += () => {
                DespesasFiltradaView.Refresh();
                OnPropertyChanged(nameof(Orcamento));
            };

            // --- Salvamento ---
            Rservice.receitas.CollectionChanged += (_, __) => SalvarTudo();
            Dservice.despesas.CollectionChanged += (_, __) => SalvarTudo();

            // --- Edição Changed ---
            ReceitasFiltradaView.CollectionChanged += Receitas_CollectionChanged;

            foreach (var r in Rservice.receitas)
                r.PropertyChanged += Receita_PropertyChanged;

            DespesasFiltradaView.CollectionChanged += Despesas_CollectionChanged;

            foreach (var d in Dservice.despesas)
                d.PropertyChanged += Receita_PropertyChanged;
        }

        // --- Métodos ---
        public void AbrirAddReceita(ReceitaService rs) {
            var nrvm = new NovaReceitaViewModel(rs);

            var novaReceita = new NovaReceitaWindow(rs) {
                DataContext = nrvm,
                Owner = Application.Current.MainWindow
            };

            nrvm.RequestClose += () => novaReceita.Close();
            novaReceita.ShowDialog();
        }

        public void AbrirAddDespesa(DespesaService ds) {
            var ndvm = new NovaDespesaViewModel(ds);

            var novaDespesa = new NovaDespesaWindow(ds) {
                DataContext = ndvm,
                Owner = Application.Current.MainWindow
            };

            ndvm.RequestClose += () => novaDespesa.Close();
            novaDespesa.ShowDialog();
        }

        public void AbrirMesRelatorio(bool conditional) {
            var mavm = new MesAnoViewModel(RelService, conditional);

            var mesAnoRel = new MesAnoRelatorioWindow(RelService, conditional) {
                DataContext = mavm,
                Owner = Application.Current.MainWindow
            };

            mavm.RequestClose += () => mesAnoRel.Close();
            mesAnoRel.ShowDialog();
        }

        public void AbrirAnoRelatorio(bool conditional) {
            var arvm = new AnoRelatorioViewModel(RelService, conditional);

            var AnoRel = new AnoRelatorioWindow(RelService, conditional) {
                DataContext = arvm,
                Owner = Application.Current.MainWindow
            };

            arvm.RequestClose += () => AnoRel.Close();
            AnoRel.ShowDialog();
        }

        public void SalvarTudo() {
            Sservice.Salvar(new BancoLocal {
                Receitas = Rservice.receitas.ToList(),
                Despesas = Dservice.despesas.ToList()
            });
        }

        private void Receitas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.NewItems != null)
                foreach (Receitas r in e.NewItems)
                    r.PropertyChanged += Receita_PropertyChanged;

            if (e.OldItems != null)
                foreach (Receitas r in e.OldItems)
                    r.PropertyChanged -= Receita_PropertyChanged;

            OnPropertyChanged(nameof(ReceitasVM.ValorTOTReceitas));
        }

        private void Receita_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(Receitas.Preco) || e.PropertyName == nameof(Receitas.dataReceita)) {
                ReceitasFiltradaView.Refresh();
                ReceitasVM.OnPropertyChanged(nameof(ReceitasViewModel.ValorTOTReceitas));
                OnPropertyChanged(nameof(Orcamento));
            }
        }

        private void Despesas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.NewItems != null)
                foreach (Despesas d in e.NewItems)
                    d.PropertyChanged += Despesa_PropertyChanged;

            if (e.OldItems != null)
                foreach (Despesas d in e.OldItems)
                    d.PropertyChanged -= Despesa_PropertyChanged;

            OnPropertyChanged(nameof(DespesasVM.ValorTOTDespesas));
        }

        private void Despesa_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(Despesas.Preco) || e.PropertyName == nameof(Despesas.dataDespesa)) {
                DespesasFiltradaView.Refresh();
                DespesasVM.OnPropertyChanged(nameof(DespesasViewModel.ValorTOTDespesas));
                OnPropertyChanged(nameof(Orcamento));
            }
        }
    }
}