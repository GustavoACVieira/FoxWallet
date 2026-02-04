using FoxWallet.Entities;
using FoxWallet.Utils;
using System;
using System.Windows.Input;

namespace FoxWallet.ViewModels.WindowsVM {
    public class NovaReceitaViewModel : ViewModelBase {
        public event Action RequestClose;
        private readonly ReceitaService _service;
        public string Obs { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataRec { get; set; } = DateTime.Now;
        public TipoReceita TipoRec { get; set; }
        public Array TR => Enum.GetValues(typeof(TipoReceita));

        public ICommand AdicionarReceitaCommand { get; }
        public ICommand FecharJanelaReceitaCommand { get; }

        public NovaReceitaViewModel(ReceitaService service) {
            _service = service;

            AdicionarReceitaCommand = new SimpleCommand(_ => CriarReceita(), _ => true);
            FecharJanelaReceitaCommand = new SimpleCommand(_ => RequestClose(), _ => true);
        }

        public void CriarReceita() {
            _service.AddReceita(new Receitas {
                Observacoes = Obs,
                Preco = Valor,
                dataReceita = DataRec,
                tipoReceita = TipoRec
            });
            RequestClose?.Invoke();
        }
    }
}
