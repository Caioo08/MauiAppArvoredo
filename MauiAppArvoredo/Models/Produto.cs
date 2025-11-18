using Newtonsoft.Json;

namespace MauiAppArvoredo.Models
{
    public class Produto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        // API espera "nome"
        [JsonProperty("nome")]
        public string Nome { get; set; } = "";

        // Auxiliar legível no app
        [JsonIgnore]
        public string Descricao
        {
            get => Nome;
            set => Nome = value;
        }

        [JsonProperty("quantidade")]
        public int Quantidade { get; set; }

        [JsonProperty("quantidadeMin")]
        public int QuantidadeMin { get; set; }

        [JsonProperty("unidade")]
        public string Unidade { get; set; } = "";

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("madeiraId")]
        public int? MadeiraId { get; set; }
    }
}
