using FoxWallet.Services;
using FoxWallet.ViewModels.WindowsVM;
using System.Windows;

namespace FoxWallet.Views.Windows {
    /// <summary>
    /// Interaction logic for NovaDespesaWindow.xaml
    /// </summary>
    public partial class NovaDespesaWindow : Window {
        private readonly NovaDespesaViewModel ndvm;
        public NovaDespesaWindow(DespesaService ds) {
            InitializeComponent();

            ndvm = new NovaDespesaViewModel(ds);

            DataContext = ndvm;

            ndvm.RequestClose += () => this.Close();
        }
    }
}
