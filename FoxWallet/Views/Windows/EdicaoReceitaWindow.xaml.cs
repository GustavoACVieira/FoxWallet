using FoxWallet.Entities;
using FoxWallet.ViewModels.WindowsVM;
using System.ComponentModel;
using System.Windows;

namespace FoxWallet.Views.Windows {
    /// <summary>
    /// Interaction logic for EdicaoReceitaWindow.xaml
    /// </summary>
    public partial class EdicaoReceitaWindow : Window {
        EditarReceitaViewModel ervm;
        public EdicaoReceitaWindow(ReceitaService rs, Receitas rec, ICollectionView recFiltradaview) {
            InitializeComponent();

            ervm = new EditarReceitaViewModel(rs, rec, recFiltradaview);

            DataContext = ervm;

            ervm.RequestClose += () => this.Close();
        }
    }
}
