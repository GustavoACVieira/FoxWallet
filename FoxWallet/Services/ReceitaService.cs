using FoxWallet.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace FoxWallet.Entities {
    public class ReceitaService : INotifyPropertyChanged {
        private readonly StorageService storageService;
        private BancoLocal bancoLocal;

        public ObservableCollection<Receitas> receitas { get; }

        public decimal TotalReceitas => receitas.Sum(x => x.Preco);

        public ReceitaService(StorageService st) {
            storageService = st;
            bancoLocal = storageService.Carregar();

            receitas = new ObservableCollection<Receitas>(bancoLocal.Receitas);

            receitas.CollectionChanged += (_, __) => OnPropertyChanged(nameof(TotalReceitas));

            receitas.CollectionChanged += (_, __) => Salvar();
        }

        public void AddReceita(Receitas rec) => receitas.Add(rec);

        public void Salvar() {
            bancoLocal.Receitas = receitas.ToList();
            storageService.Salvar(bancoLocal);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
