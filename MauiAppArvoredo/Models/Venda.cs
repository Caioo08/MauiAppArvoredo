using SQLite;
using Newtonsoft.Json;

namespace MauiAppArvoredo.Models
{
    /// <summary>
    /// Modelo de Venda/Pedido Finalizado
    /// Representa vendas confirmadas sincronizadas com a API
    /// </summary>
    public class Venda
    {
        [PrimaryKey, AutoIncrement]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; } = "";

        [JsonProperty("usuarioId")]
        public int UsuarioId { get; set; }

        [JsonProperty("clienteId")]
        public int? ClienteId { get; set; }

        [JsonProperty("valorTotal")]
        public double ValorTotal { get; set; }

        [JsonProperty("dataCriacao")]
        public DateTime DataCriacao { get; set; }

        [JsonProperty("dataPagamento")]
        public DateTime? DataPagamento { get; set; }

        [JsonProperty("pago")]
        public bool Pago { get; set; }

        [JsonProperty("forma")]
        public string FormaPagamento { get; set; } = "Dinheiro";

        // Campos extras para exibição no app
        [JsonIgnore]
        public string NomeCliente { get; set; } = "";

        [JsonIgnore]
        public string StatusTexto => Pago ? "✅ Pago" : "⏳ Pendente";

        [JsonIgnore]
        public Color StatusColor => Pago ? Colors.Green : Colors.Orange;

        [JsonIgnore]
        public string DataFormatada => DataCriacao.ToString("dd/MM/yyyy HH:mm");

        [JsonIgnore]
        public string ValorFormatado => $"R$ {ValorTotal:N2}";

        [JsonIgnore]
        public int ApiId { get; set; } // ID original da API

        [JsonIgnore]
        public bool Sincronizado { get; set; } = false; // Se já foi sincronizado com API
    }

    /// <summary>
    /// Item de Venda - Produto vendido
    /// </summary>
    public class ItemVenda
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int VendaId { get; set; } // FK -> Venda

        [JsonProperty("produtoId")]
        public int? ProdutoId { get; set; }

        [JsonProperty("quantidade")]
        public int Quantidade { get; set; }

        [JsonProperty("valorVenda")]
        public double ValorVenda { get; set; }

        [JsonProperty("valorTotal")]
        public double ValorTotal { get; set; }

        // Campos extras
        [JsonIgnore]
        public string NomeProduto { get; set; } = "";

        [JsonIgnore]
        public string Unidade { get; set; } = "un";

        [JsonIgnore]
        public string DescricaoCompleta => $"{NomeProduto} - {Quantidade} {Unidade}";

        [JsonIgnore]
        public string ValorFormatado => $"R$ {ValorTotal:N2}";
    }

    /// <summary>
    /// DTO para criar venda via API
    /// </summary>
    public class VendaCreateDto
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("usuarioId")]
        public int UsuarioId { get; set; }

        [JsonProperty("clienteId")]
        public int? ClienteId { get; set; }

        [JsonProperty("pago")]
        public bool Pago { get; set; }

        [JsonProperty("forma")]
        public string Forma { get; set; }

        [JsonProperty("vendaE")]
        public List<ItemVendaCreateDto> VendaE { get; set; }
    }

    /// <summary>
    /// DTO para item de venda na criação
    /// </summary>
    public class ItemVendaCreateDto
    {
        [JsonProperty("produtoId")]
        public int? ProdutoId { get; set; }

        [JsonProperty("quantidade")]
        public int Quantidade { get; set; }

        [JsonProperty("valorVenda")]
        public double ValorVenda { get; set; }

        [JsonProperty("valorTotal")]
        public double ValorTotal { get; set; }
    }

    /// <summary>
    /// Resposta da API ao criar/buscar venda
    /// </summary>
    public class VendaApiResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("usuarioId")]
        public int UsuarioId { get; set; }

        [JsonProperty("clienteId")]
        public int? ClienteId { get; set; }

        [JsonProperty("valorTotal")]
        public double ValorTotal { get; set; }

        [JsonProperty("dataCriacao")]
        public DateTime DataCriacao { get; set; }

        [JsonProperty("dataPagamento")]
        public DateTime? DataPagamento { get; set; }

        [JsonProperty("pago")]
        public bool Pago { get; set; }

        [JsonProperty("forma")]
        public string Forma { get; set; }

        [JsonProperty("vendaE")]
        public List<ItemVendaApiResponse> VendaE { get; set; }
    }

    /// <summary>
    /// Item de venda retornado pela API
    /// </summary>
    public class ItemVendaApiResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("vendaId")]
        public int VendaId { get; set; }

        [JsonProperty("produtoId")]
        public int? ProdutoId { get; set; }

        [JsonProperty("quantidade")]
        public int Quantidade { get; set; }

        [JsonProperty("valorVenda")]
        public double ValorVenda { get; set; }

        [JsonProperty("valorTotal")]
        public double ValorTotal { get; set; }

        [JsonProperty("produto")]
        public ProdutoSimples Produto { get; set; }
    }

    public class ProdutoSimples
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("unidade")]
        public string Unidade { get; set; }
    }
}