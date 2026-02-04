using System.Collections.Generic;

namespace FoxWallet.Entities {
    public class BancoLocal {
        public List<Receitas> Receitas { get; set; } = new List<Receitas>();
        public List<Despesas> Despesas { get; set; } = new List<Despesas>();
    }
}
