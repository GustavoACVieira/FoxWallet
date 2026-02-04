using FoxWallet.Services;
using FoxWallet.ViewModels.WindowsVM;
using System.Windows;

namespace FoxWallet.Views.Windows {
    /// <summary>
    /// Interaction logic for MesAnoRelatorioWindow.xaml
    /// </summary>
    public partial class MesAnoRelatorioWindow : Window {
        private readonly MesAnoViewModel mavm;
        public MesAnoRelatorioWindow(RelatorioService rs, bool conditional) {
            InitializeComponent();

            mavm = new MesAnoViewModel(rs, conditional);

            DataContext = mavm;

            mavm.RequestClose += () => this.Close();
        }
    }
}
