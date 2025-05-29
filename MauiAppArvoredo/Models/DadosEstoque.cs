using System.Collections.Generic;
namespace MauiAppArvoredo.Models
{
    public static class DadosEstoque
    {
        // Dicionário para armazenar os dados do estoque
        // Chave: "Madeira-Tipo-Tamanho" (ex: "Eucalipto-Viga-6 metros")
        // Valor: Quantidade
        private static Dictionary<string, int> estoque = new Dictionary<string, int>();
        // Método para salvar/atualizar uma quantidade no estoque
        public static void SalvarQuantidade(string madeira, string tipo, string tamanho, int quantidade)
        {
            string chave = $"{madeira}-{tipo}-{tamanho}";
            estoque[chave] = quantidade;
        }
        // Método para obter uma quantidade do estoque
        public static int ObterQuantidade(string madeira, string tipo, string tamanho)
        {
            string chave = $"{madeira}-{tipo}-{tamanho}";
            return estoque.ContainsKey(chave) ? estoque[chave] : 0;
        }
        // Método para obter todas as quantidades de uma madeira específica
        public static Dictionary<string, int> ObterQuantidadesPorMadeira(string madeira)
        {
            var resultado = new Dictionary<string, int>();
            foreach (var item in estoque)
            {
                if (item.Key.StartsWith($"{madeira}-"))
                {
                    // Remove o prefixo da madeira da chave para ficar só "Tipo-Tamanho"
                    string chaveSimplificada = item.Key.Substring($"{madeira}-".Length);
                    resultado[chaveSimplificada] = item.Value;
                }
            }
            return resultado;
        }
        // Método para verificar se existem dados para uma madeira
        public static bool TemDados(string madeira)
        {
            foreach (var chave in estoque.Keys)
            {
                if (chave.StartsWith($"{madeira}-"))
                {
                    return true;
                }
            }
            return false;
        }
        // Método para limpar todos os dados (se necessário)
        public static void LimparTodos()
        {
            estoque.Clear();
        }
    }
}