using System;
using System.ComponentModel;

namespace FoxWallet.Entities {
    public class Despesas : INotifyPropertyChanged {
        public Guid Id { get; set; } = Guid.NewGuid();

        private string _Observacoes;
        public string Observacoes {
            get => _Observacoes;
            set {
                if (_Observacoes != value) {
                    _Observacoes = value;
                    OnPropertyChanged(nameof(Observacoes));
                }
            }
        }

        private decimal _Preco;
        public decimal Preco {
            get => _Preco;
            set {
                if (_Preco != value) {
                    _Preco = value;
                    OnPropertyChanged(nameof(Preco));
                }
            }
        }

        private DateTime _dataDespesa;
        public DateTime dataDespesa {
            get => _dataDespesa;
            set {
                if (_dataDespesa != value) {
                    _dataDespesa = value;
                    OnPropertyChanged(nameof(_dataDespesa));
                }
            }
        }

        private TipoDespesa _tipoDespesa;
        public TipoDespesa tipoDespesa {
            get => _tipoDespesa;
            set {
                if (_tipoDespesa != value) {
                    _tipoDespesa = value;
                    OnPropertyChanged(nameof(TipoDespesa));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
