using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace FoxWallet.ViewModels {
    public abstract class ListaFinanceiraViewModel<T> : ViewModelBase {
        protected readonly ObservableCollection<T> listaFinanceira;
        public ICollectionView View { get; }

        protected ListaFinanceiraViewModel(ObservableCollection<T> lf) {
            listaFinanceira = lf;

            View = CollectionViewSource.GetDefaultView(listaFinanceira);
            View.Filter = Filtrar;

            listaFinanceira.CollectionChanged += (_, __) => {
                View.Refresh();
                OnPropertyChanged(nameof(Total));
            };
        }

        public decimal Total => View.Cast<T>()?.Sum(CalcularValor) ?? 0.00m;

        protected abstract bool Filtrar(object obj);
        protected abstract decimal CalcularValor(T item);
    }
}
