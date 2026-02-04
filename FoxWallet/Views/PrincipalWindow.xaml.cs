using FoxWallet.Services;
using FoxWallet.Entities;
using FoxWallet.ViewModels;
using System.Windows;

namespace FoxWallet.Views {
    /// <summary>
    /// Interaction logic for PrincipalWindow.xaml
    /// </summary>
    public partial class PrincipalWindow : Window {
        private readonly ReceitaService rs;
        private readonly DespesaService ds;
        private readonly PrincipalViewModel pvm;
        private readonly StorageService st;
        private readonly RelatorioService rrs;

        public PrincipalWindow() {
            InitializeComponent();

            st = new StorageService();
            rs = new ReceitaService(st);
            ds = new DespesaService(st);
            rrs = new RelatorioService(rs, ds);
            pvm = new PrincipalViewModel(rs, ds, st, rrs);

            DataContext = pvm;

            pvm.RequestClose += () => this.Close();
            pvm.Minimize += () => WindowState = WindowState.Minimized;
            pvm.MaximizeRestore += () => {
                if (WindowState == WindowState.Maximized)
                    WindowState = WindowState.Normal;
                else if (WindowState == WindowState.Normal)
                    WindowState = WindowState.Maximized;
            };
        }

        private void Barra_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                DragMove();
        }

        private void ListaRec_SizeChanged(object sender, SizeChangedEventArgs e) {
            if (GridRec.Columns.Count == 0)
                return;

            var larguraTotal = ListaRec.ActualWidth - 35;
            var larguraColuna = larguraTotal / GridRec.Columns.Count;

            foreach (var col in GridRec.Columns)
                col.Width = larguraColuna;
        }

        private void ListaDesp_SizeChanged(object sender, SizeChangedEventArgs e) {
            if (GridDesp.Columns.Count == 0)
                return;

            var larguraTotal = ListaDesp.ActualWidth - 35;
            var larguraColuna = larguraTotal / GridDesp.Columns.Count;

            foreach (var col in GridDesp.Columns)
                col.Width = larguraColuna;
        }
    }
}
