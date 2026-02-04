using System;
using System.ComponentModel;

namespace FoxWallet.Entities {
    public class Receitas : INotifyPropertyChanged {
        public Guid Id { get; set; } = Guid.NewGuid();

        private string _observacoes;
        public string Observacoes {
            get => _observacoes; 
            set {
                if (_observacoes != value) {
                    _observacoes = value;
                    OnPropertyChanged(nameof(Observacoes));
                }
            }
        }

        private decimal _preco;
        public decimal Preco {
            get => _preco;
            set {
                if (_preco != value) {
                    _preco = value;
                    OnPropertyChanged(nameof(Preco));
                }
            }
        }

        private DateTime _dataReceita;
        public DateTime dataReceita {
            get => _dataReceita; 
            set {
                if (_dataReceita != value) {
                    _dataReceita = value;
                    OnPropertyChanged(nameof(dataReceita));
                }
            }
        }

        private TipoReceita _tipoReceita;
        public TipoReceita tipoReceita {
            get => _tipoReceita;
            set {
                if (_tipoReceita != value) {
                    _tipoReceita = value;
                    OnPropertyChanged(nameof(tipoReceita));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
