using FoxWallet.Entities;
using FoxWallet.Services;
using FoxWallet.Utils;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

namespace FoxWallet.ViewModels.WindowsVM {
    public class AnoRelatorioViewModel : ViewModelBase {
        public event Action RequestClose;

        private RelatorioService relService;
        public ObservableCollection<MesItem> Meses { get; }

        private int _ano;
        public int Ano {
            get => _ano;
            set {
                _ano = value;
                OnPropertyChanged(nameof(Ano));
            }
        }

        public ICommand FecharJanelaCommand { get; }
        public ICommand GerarRelatorioCommand { get; }

        public AnoRelatorioViewModel(RelatorioService rs, bool conditional) {
            relService = rs;

            Meses = new ObservableCollection<MesItem>(Enumerable.Range(1, 12).Select(x => {
                var nome = new DateTime(2000, x, 1).ToString("MMMM", new CultureInfo("pt-BR"));

                return new MesItem {
                    Numero = x,
                    Nome = char.ToUpper(nome[0]) + nome.Substring(1)
                };
            }));

            FecharJanelaCommand = new SimpleCommand(_ => RequestClose(), _ => true);

            if (conditional)
                GerarRelatorioCommand = new SimpleCommand(_ => relService.GerarRelatorioAnoPDF(Meses, Ano, RequestClose), _ => true);
            else
                GerarRelatorioCommand = new SimpleCommand(_ => relService.GerarRelatorioAnoCSV(Meses, Ano, RequestClose), _ => true);
        }
    }
}
