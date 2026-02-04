using FoxWallet.Entities;
using FoxWallet.ViewModels.WindowsVM;
using System.Windows;

namespace FoxWallet.Views.Windows {
    /// <summary>
    /// Interaction logic for NovaReceita.xaml
    /// </summary>
    public partial class NovaReceitaWindow : Window {
        private readonly NovaReceitaViewModel nrvm;
        public NovaReceitaWindow(ReceitaService rs) {
            InitializeComponent();

            nrvm = new NovaReceitaViewModel(rs);

            DataContext = nrvm;

            nrvm.RequestClose += () => this.Close();
        }
    }
}