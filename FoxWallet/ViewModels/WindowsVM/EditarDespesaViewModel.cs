using FoxWallet.Entities;
using FoxWallet.Services;
using FoxWallet.Utils;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FoxWallet.ViewModels.WindowsVM {
    public class EditarDespesaViewModel {
        public event Action RequestClose;
        private readonly DespesaService _service;
        private readonly Despesas DespSelecionada;

        public string EObs { get; set; }
        public decimal EValor { get; set; }
        public DateTime EDataDesp { get; set; }
        public TipoDespesa ETipoDesp { get; set; }

        public ICollectionView DespesasFiltradaView { get; set; }
        public Array ETD => Enum.GetValues(typeof(TipoDespesa));

        public ICommand EditarDespesaCommand { get; }
        public ICommand FecharJanelaEditarDespesaCommand { get; }

        public EditarDespesaViewModel(DespesaService service, Despesas despSelecionada, ICollectionView despFiltradaView) {
            _service = service;
            DespSelecionada = despSelecionada;
            DespesasFiltradaView = despFiltradaView;

            if (DespSelecionada != null) {
                EObs = DespSelecionada.Observacoes;
                EValor = DespSelecionada.Preco;
                EDataDesp = DespSelecionada.dataDespesa;
                ETipoDesp = DespSelecionada.tipoDespesa;

                EditarDespesaCommand = new SimpleCommand(_ => EditarDespesa(), _ => true);
            } else
                MessageBox.Show("Selecione uma despesa");
            
            FecharJanelaEditarDespesaCommand = new SimpleCommand(_ => RequestClose(), _ => true);
        }

        public void EditarDespesa() {
            DespSelecionada.Observacoes = EObs;
            DespSelecionada.Preco = EValor;
            DespSelecionada.dataDespesa = EDataDesp;
            DespSelecionada.tipoDespesa = ETipoDesp;

            _service.Salvar();
            DespesasFiltradaView.Refresh();
            RequestClose?.Invoke();
        }
    }
}
