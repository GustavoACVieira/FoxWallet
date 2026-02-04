using FoxWallet.Entities;
using FoxWallet.Utils;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace FoxWallet.ViewModels {
    public class FiltroReceitasViewModel : ViewModelBase {
        private readonly ICollectionView ReceitasFiltradaView;
        public event Action FiltroAlterado;

        private TipoReceita? _tipoSelecionadoRec;
        public TipoReceita? TipoSelecionadoRec {
            get => _tipoSelecionadoRec;
            set {
                _tipoSelecionadoRec = value;
                OnPropertyChanged(nameof(TipoSelecionadoRec));
                ReceitasFiltradaView.Refresh();
            }
        }

        // PopUp
        private bool _PopFiltroRecIsOpen;
        public bool PopFiltroRecIsOpen {
            get => _PopFiltroRecIsOpen;
            set {
                _PopFiltroRecIsOpen = value;
                OnPropertyChanged(nameof(PopFiltroRecIsOpen));
            }
        }
        
        // Declarações relacionadas ao popup
        public DateTime? DataInicioRec { get; set; } = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
        public DateTime? DataFimRec { get; set; } = DateTime.Now.AddDays(1);

        public decimal? ValorMinRec { get; set; } = 0.00m;
        public decimal? ValorMaxRec { get; set; } = 10000000.00m;

        // Temp
        private DateTime? _TempDataInicioRec = DateTime.Now;
        public DateTime? TempDataInicioRec {
            get => _TempDataInicioRec;
            set {
                _TempDataInicioRec = value;
                OnPropertyChanged(nameof(TempDataInicioRec));
            }
        }

        private DateTime? _TempDataFimRec;
        public DateTime? TempDataFimRec {
            get => _TempDataFimRec;
            set {
                _TempDataFimRec = value;
                OnPropertyChanged(nameof(TempDataFimRec));
            }
        }

        private decimal? _TempValorMinRec;
        public decimal? TempValorMinRec {
            get => _TempValorMinRec;
            set {
                _TempValorMinRec = value;
                OnPropertyChanged(nameof(TempValorMinRec));
            }
        }

        private decimal? _TempValorMaxRec;
        public decimal? TempValorMaxRec {
            get => _TempValorMaxRec;
            set {
                _TempValorMaxRec = value;
                OnPropertyChanged(nameof(TempValorMaxRec));
            }
        }

        public ICommand SalvarCommand { get; }
        public ICommand AbrirPopUpCommand { get; }
        public ICommand FecharCommand { get; }
        public ICommand LimparCommand { get; }

        public FiltroReceitasViewModel() {
            SalvarCommand = new SimpleCommand(_ => SalvarFiltro(), _ => true);
            AbrirPopUpCommand = new SimpleCommand(_ => AbrirPopUpFiltro(), _ => true);
            FecharCommand = new SimpleCommand(_ => PopFiltroRecIsOpen = false, _ => true);
            LimparCommand = new SimpleCommand(_ => LimparFiltro(), _ => true);
        }

        public void LimparFiltro() {
            DataInicioRec = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
            DataFimRec = DateTime.Now.AddDays(1);
            ValorMinRec = 0;
            ValorMaxRec = 10000000.00m;

            FiltroAlterado?.Invoke();
        }

        public void SalvarFiltro() {
            DataInicioRec = TempDataInicioRec;
            DataFimRec = TempDataFimRec;
            ValorMinRec = TempValorMinRec;
            ValorMaxRec = TempValorMaxRec;

            FiltroAlterado?.Invoke();
            PopFiltroRecIsOpen = false;
        }

        public bool FiltrarReceitas(object obj) {
            if (!(obj is Receitas r))  return false;

            if (TipoSelecionadoRec != null && r.tipoReceita != TipoSelecionadoRec) return false;

            if (DataInicioRec != null && r.dataReceita < DataInicioRec) return false;

            if (DataFimRec != null && r.dataReceita > DataFimRec) return false;

            if (ValorMinRec != null && r.Preco < ValorMinRec) return false;

            if (ValorMaxRec != null && r.Preco > ValorMaxRec) return false;

            return true;
        }

        public void AbrirPopUpFiltro() {
            TempDataInicioRec = DataInicioRec;
            TempDataFimRec = DataFimRec;
            TempValorMinRec = ValorMinRec;
            TempValorMaxRec = ValorMaxRec;

            PopFiltroRecIsOpen = true;
        }
    }
}
