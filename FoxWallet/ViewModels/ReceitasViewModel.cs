using FoxWallet.Entities;
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
    public class ReceitasViewModel : ListaFinanceiraViewModel<Receitas> {
        private readonly ReceitaService receitaService;

        public ICollectionView ReceitasFiltradaView { get; }

        public decimal ValorTOTReceitas => ReceitasFiltradaView?.Cast<Receitas>().Sum(x => x.Preco) ?? 0m;
        public ObservableCollection<Receitas> listReceitas => receitaService.receitas;
        private Receitas _receitasSelecionadas;
        public Receitas ReceitaSelecionada {
            get => _receitasSelecionadas;
            set {
                _receitasSelecionadas = value;
                OnPropertyChanged(nameof(ReceitaSelecionada));
            }
        }

        FiltroReceitasViewModel _FiltroVM { get; }

        public ICommand EditarCommand { get; }
        public ICommand ExcluirCommand { get; }

        public ReceitasViewModel(ReceitaService rs, FiltroReceitasViewModel ffvm) : base(rs.receitas) {
            receitaService = rs;
            _FiltroVM = ffvm;

            ReceitasFiltradaView = CollectionViewSource.GetDefaultView(receitaService.receitas);
            ReceitasFiltradaView.Filter = _FiltroVM.FiltrarReceitas;

            _FiltroVM.FiltroAlterado += () => {
                ReceitasFiltradaView.Refresh();
                OnPropertyChanged(nameof(ValorTOTReceitas));
            };

            receitaService.PropertyChanged += (_, e) => {
                if (e.PropertyName == nameof(receitaService.TotalReceitas)) {
                    ReceitasFiltradaView?.Refresh();
                    OnPropertyChanged(nameof(ValorTOTReceitas));
                }
                if (e.PropertyName == nameof(receitaService.receitas))
                    OnPropertyChanged(nameof(listReceitas));
            };

            EditarCommand = new SimpleCommand(_ => AbrirEdicao(rs), _ => true);
            ExcluirCommand = new SimpleCommand(_ => Excluir(), _ => true);
        }

        protected override decimal CalcularValor(Receitas r) => r.Preco;

        public void Excluir() => receitaService.receitas.Remove(ReceitaSelecionada);

        public void AbrirEdicao(ReceitaService rs) {
            var ervm = new EditarReceitaViewModel(rs, ReceitaSelecionada, ReceitasFiltradaView);

            var editReceita = new EdicaoReceitaWindow(rs, ReceitaSelecionada, ReceitasFiltradaView) {
                DataContext = ervm,
                Owner = Application.Current.MainWindow
            };

            ervm.RequestClose += () => editReceita.Close();
            editReceita.ShowDialog();
        }

        protected override bool Filtrar(object obj) {
            if (_FiltroVM == null)
                return true;

            return _FiltroVM.FiltrarReceitas(obj);
        }
    }
}
