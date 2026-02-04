using FoxWallet.Entities;
using FoxWallet.Utils;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FoxWallet.ViewModels.WindowsVM {
    public class EditarReceitaViewModel : ViewModelBase {
        public event Action RequestClose;
        private readonly ReceitaService _service;
        private readonly Receitas RecSelecionada;
        public readonly ReceitasViewModel RecVM;

        public string EObs { get; set; }
        public decimal EValor { get; set; }
        public DateTime EDataRec { get; set; }
        public TipoReceita ETipoRec { get; set; }

        public ICollectionView ReceitasFitradasView { get; set; }
        public Array ETR => Enum.GetValues(typeof(TipoReceita));

        public ICommand EditarReceitaCommand { get; }
        public ICommand FecharJanelaEditarReceitaCommand { get; }

        public EditarReceitaViewModel(ReceitaService service, Receitas recSelecionada, ICollectionView receitasFitradasView) {
            _service = service;
            RecSelecionada = recSelecionada;
            ReceitasFitradasView = receitasFitradasView;

            if (RecSelecionada != null) {
                EObs = RecSelecionada.Observacoes;
                EValor = RecSelecionada.Preco;
                EDataRec = RecSelecionada.dataReceita;
                ETipoRec = RecSelecionada.tipoReceita;

                EditarReceitaCommand = new SimpleCommand(_ => EditarReceita(), _ => true);
            } else
                MessageBox.Show("Escolha uma receita!");

            FecharJanelaEditarReceitaCommand = new SimpleCommand(_ => RequestClose(), _ => true);
        }

        public void EditarReceita() {
            RecSelecionada.Observacoes = EObs;
            RecSelecionada.Preco = EValor;
            RecSelecionada.dataReceita = EDataRec;
            RecSelecionada.tipoReceita = ETipoRec;

            _service.Salvar();
            ReceitasFitradasView.Refresh();
            RequestClose?.Invoke();
        }
    }
}
