using FoxWallet.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace FoxWallet.Services {
    public class DespesaService : INotifyPropertyChanged {
        private readonly StorageService storageService;
        private BancoLocal bancoLocal;

        public ObservableCollection<Despesas> despesas { get; }

        public decimal TotalDespesas => despesas.Sum(x => x.Preco);

        public DespesaService(StorageService st) {
            storageService = st;
            bancoLocal = storageService.Carregar();

            despesas = new ObservableCollection<Despesas>(bancoLocal.Despesas);

            despesas.CollectionChanged += (_, __) => OnPropertyChanged(nameof(TotalDespesas));
        }

        public void AddDespesa(Despesas desp) => despesas.Add(desp);

        public void Salvar() {
            bancoLocal.Despesas = despesas.ToList();
            storageService.Salvar(bancoLocal);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
