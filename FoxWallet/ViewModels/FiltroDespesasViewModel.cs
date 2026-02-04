using FoxWallet.Entities;
using FoxWallet.Utils;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace FoxWallet.ViewModels {
    public class FiltroDespesasViewModel : ViewModelBase {
        private readonly ICollectionView DespesasFiltradasView;
        public event Action FiltroAlterado;

        private TipoDespesa? _tipoSelecionadoDesp;
        public TipoDespesa? TipoSelecionadoDesp {
            get => _tipoSelecionadoDesp;
            set {
                _tipoSelecionadoDesp = value;
                OnPropertyChanged(nameof(TipoSelecionadoDesp));
                DespesasFiltradasView.Refresh();
            }
        }

        // PopUp
        private bool _PopFiltroDespIsOpen;
        public bool PopFiltroDespIsOpen {
            get => _PopFiltroDespIsOpen;
            set {
                _PopFiltroDespIsOpen = value;
                OnPropertyChanged(nameof(PopFiltroDespIsOpen));
            }
        }

        // Declarações relacionadas ao PopUp
        public DateTime? DataInicioDesp { get; set; } = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
        public DateTime? DataFimDesp { get; set; } = DateTime.Now.AddDays(1);

        public decimal? ValorMinDesp { get; set; } = 0.00m;
        public decimal? ValorMaxDesp { get; set; } = 10000000.00m;

        // Temp
        private DateTime? _TempDataInicioDesp = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
        public DateTime? TempDataInicioDesp {
            get => _TempDataInicioDesp;
            set {
                _TempDataInicioDesp = value;
                OnPropertyChanged(nameof(_TempDataInicioDesp));
            }
        }

        public DateTime? _TempDataFimDesp;
        public DateTime? TempDataFimDesp {
            get => _TempDataFimDesp;
            set {
                _TempDataFimDesp = value;
                OnPropertyChanged(nameof(TempDataFimDesp));
            }
        }

        private decimal? _TempValorMinDesp;
        public decimal? TempValorMinDesp {
            get => _TempValorMinDesp;
            set {
                _TempValorMinDesp = value;
                OnPropertyChanged(nameof(TempValorMinDesp));
            }
        }

        private decimal? _TempValorMaxDesp;
        public decimal? TempValorMaxDesp {
            get => _TempValorMaxDesp;
            set {
                _TempValorMaxDesp = value;
                OnPropertyChanged(nameof(TempValorMaxDesp));
            }
        }

        public ICommand SalvarCommand { get; }
        public ICommand AbrirPopUpCommand { get; }
        public ICommand FecharCommand { get; }
        public ICommand LimparCommand { get; }

        public FiltroDespesasViewModel() {
            SalvarCommand = new SimpleCommand(_ => SalvarFiltro(), _ => true);
            AbrirPopUpCommand = new SimpleCommand(_ => AbrirPopUpFiltro(), _ => true);
            FecharCommand = new SimpleCommand(_ => PopFiltroDespIsOpen = false, _ => true);
            LimparCommand = new SimpleCommand(_ => LimparFiltro(), _ => true);
        }

        public void LimparFiltro() {
            DataInicioDesp = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
            DataFimDesp = DateTime.Now.AddDays(1);
            ValorMinDesp = 0;
            ValorMaxDesp = 10000000.00m;

            FiltroAlterado?.Invoke();
        }

        public void SalvarFiltro() {
            DataInicioDesp = TempDataInicioDesp;
            DataFimDesp = TempDataFimDesp;
            ValorMinDesp = TempValorMinDesp;
            ValorMaxDesp = TempValorMaxDesp;

            FiltroAlterado?.Invoke();
            PopFiltroDespIsOpen = false;
        }

        public void AbrirPopUpFiltro() {
            TempDataInicioDesp = DataInicioDesp;
            TempDataFimDesp = DataFimDesp;
            TempValorMinDesp = ValorMinDesp;
            TempValorMaxDesp = ValorMaxDesp;

            PopFiltroDespIsOpen = true;
        }

        public bool FiltrarDespesas(object obj) {
            if (!(obj is Despesas r))
                return false;

            if (TipoSelecionadoDesp != null && r.tipoDespesa != TipoSelecionadoDesp)
                return false;

            if (DataInicioDesp != null && r.dataDespesa < DataInicioDesp)
                return false;

            if (DataFimDesp != null && r.dataDespesa > DataFimDesp)
                return false;

            if (ValorMinDesp != null && r.Preco < ValorMinDesp)
                return false;

            if (ValorMaxDesp != null && r.Preco > ValorMaxDesp)
                return false;

            return true;
        }
    }
}
