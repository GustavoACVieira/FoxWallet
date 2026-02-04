using FoxWallet.Services;
using FoxWallet.ViewModels.WindowsVM;
using System.Windows;

namespace FoxWallet.Views.Windows {
    /// <summary>
    /// Interaction logic for AnoRelatorioWindow.xaml
    /// </summary>
    public partial class AnoRelatorioWindow : Window {
        private readonly AnoRelatorioViewModel arvm;
        public AnoRelatorioWindow(RelatorioService rs, bool conditional) {
            InitializeComponent();

            arvm = new AnoRelatorioViewModel(rs, conditional);

            DataContext = arvm;

            arvm.RequestClose += () => this.Close();
        }
    }
}
