using FoxWallet.Entities;
using FoxWallet.Services;
using FoxWallet.ViewModels.WindowsVM;
using System.ComponentModel;
using System.Windows;

namespace FoxWallet.Views.Windows {
    /// <summary>
    /// Interaction logic for EdicaoDespesaWindow.xaml
    /// </summary>
    public partial class EdicaoDespesaWindow : Window {
        EditarDespesaViewModel edvm;
        public EdicaoDespesaWindow(DespesaService service, Despesas despSelecionada, ICollectionView despFiltradaView) {
            InitializeComponent();

            edvm = new EditarDespesaViewModel(service, despSelecionada, despFiltradaView);

            DataContext = edvm;

            edvm.RequestClose += () => this.Close();
        }
    }
}
