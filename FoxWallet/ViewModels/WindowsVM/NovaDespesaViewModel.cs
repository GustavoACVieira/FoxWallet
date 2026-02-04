using FoxWallet.Entities;
using FoxWallet.Services;
using FoxWallet.Utils;
using System;
using System.Windows.Input;

namespace FoxWallet.ViewModels.WindowsVM {
    public class NovaDespesaViewModel {
        public event Action RequestClose;
        public DespesaService _service;
        public string Obs { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataDesp { get; set; } = DateTime.Now;
        public TipoDespesa TipoDesp { get; set; }
        public Array TD => Enum.GetValues(typeof(TipoDespesa));

        public ICommand AdicionarDespesaCommand { get; }
        public ICommand FecharJanelaDespesaCommand { get; }

        public NovaDespesaViewModel(DespesaService rs) {
            _service = rs;

            AdicionarDespesaCommand = new SimpleCommand(_ => CriarDespesa(), _ => true);
            FecharJanelaDespesaCommand = new SimpleCommand(_ => RequestClose(), _ => true);
        }

        public void CriarDespesa() {
            _service.AddDespesa(new Despesas {
                Observacoes = Obs,
                Preco = Valor,
                dataDespesa = DataDesp,
                tipoDespesa = TipoDesp
            });
            RequestClose?.Invoke();
        }
    }
}
