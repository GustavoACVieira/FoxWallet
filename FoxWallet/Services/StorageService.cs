using FoxWallet.Entities;
using System;
using System.IO;
using System.Text.Json;

namespace FoxWallet.Services {
    public class StorageService {
        private readonly string caminho = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FoxWallet", "dados.json");

        public void Salvar(BancoLocal banco) {
            var pasta = Path.GetDirectoryName(caminho);

            if (!Directory.Exists(pasta))
                Directory.CreateDirectory(pasta);

            var json = JsonSerializer.Serialize(banco, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(caminho, json);
        }

        public BancoLocal Carregar() {
            if (!File.Exists(caminho))
                return new BancoLocal();

            var json = File.ReadAllText(caminho);

            return JsonSerializer.Deserialize<BancoLocal>(json) ?? new BancoLocal();
        }
    }
}
