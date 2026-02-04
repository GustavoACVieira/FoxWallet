using FoxWallet.Entities;
using FoxWallet.Services;
using FoxWallet.Utils;
using FoxWallet.ViewModels.WindowsVM;
using FoxWallet.Views.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace FoxWallet.ViewModels {
    public class DespesasViewModel : ListaFinanceiraViewModel<Despesas> {
        private readonly DespesaService despesaService;

        public ICollectionView DespesasFiltradaView { get; }
        public decimal ValorTOTDespesas => DespesasFiltradaView?.Cast<Despesas>().Sum(x => x.Preco) ?? 0m;
        public ObservableCollection<Despesas> listDespesas => despesaService.despesas;

        private Despesas _DespesaSelecionada;
        public Despesas DespesaSelecionada {
            get => _DespesaSelecionada;
            set {
                _DespesaSelecionada = value;
                OnPropertyChanged(nameof(DespesaSelecionada));
            }
        }

        FiltroDespesasViewModel filtroVM { get; }

        public ICommand EditarCommand { get; }
        public ICommand ExcluirCommand { get; }

        public DespesasViewModel(DespesaService ds, FiltroDespesasViewModel fdvm, ICollectionView despesasFiltradas) : base(ds.despesas) {
            despesaService = ds;
            filtroVM = fdvm;

            DespesasFiltradaView = CollectionViewSource.GetDefaultView(despesaService.despesas);
            DespesasFiltradaView.Filter = filtroVM.FiltrarDespesas;

            filtroVM.FiltroAlterado += () => {
                DespesasFiltradaView.Refresh();
                OnPropertyChanged(nameof(ValorTOTDespesas));
            };

            despesaService.PropertyChanged += (_, e) => {
                if (e.PropertyName == nameof(despesaService.TotalDespesas)) {
                    DespesasFiltradaView.Refresh();
                    OnPropertyChanged(nameof(ValorTOTDespesas));
                }
                if (e.PropertyName == nameof(despesaService.despesas))
                    OnPropertyChanged(nameof(listDespesas));
            };

            EditarCommand = new SimpleCommand(_ => AbrirEdicao(ds), _ => true);
            ExcluirCommand = new SimpleCommand(_ => Excluir(), _ => true);
        }

        protected override bool Filtrar(object obj) {
            if (filtroVM == null)
                return true;

            return filtroVM.FiltrarDespesas(obj);
        }
        protected override decimal CalcularValor(Despesas d) => d.Preco;

        public void Excluir() => despesaService.despesas.Remove(DespesaSelecionada);

        public void AbrirEdicao(DespesaService ds) {
            var ervm = new EditarDespesaViewModel(ds, DespesaSelecionada, DespesasFiltradaView);

            var editReceita = new EdicaoDespesaWindow(ds, DespesaSelecionada, DespesasFiltradaView) {
                DataContext = ervm,
                Owner = Application.Current.MainWindow
            };

            ervm.RequestClose += () => editReceita.Close();
            editReceita.ShowDialog();
        }
    }
}
