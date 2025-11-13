using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace MauiAppArvoredo.Services
{
    public class ArvoredoApiService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://arvoredoapi.vercel.app";
        private const string ADMIN_KEY = "68e553e6f1c4fffd11c95840";

        public ArvoredoApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BASE_URL),
                Timeout = TimeSpan.FromSeconds(30)
            };

            _httpClient.DefaultRequestHeaders.Add("x-api-key", ADMIN_KEY);
        }

        // ========== ESTOQUE DE MADEIRAS ==========
        public async Task<List<ApiEstoqueMadeira>> GetEstoqueMadeirasAsync(bool? acabando = null)
        {
            try
            {
                var url = "/estoquemadeiras";
                if (acabando.HasValue)
                    url += $"?acabando={acabando.Value.ToString().ToLower()}";

                return await _httpClient.GetFromJsonAsync<List<ApiEstoqueMadeira>>(url) ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar estoque: {ex.Message}");
                return new();
            }
        }

        public async Task<ApiEstoqueMadeira> GetEstoqueMadeiraByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<ApiEstoqueMadeira>($"/estoquemadeiras/{id}");
        }

        public async Task<ApiEstoqueMadeira> CreateEstoqueMadeiraAsync(ApiEstoqueMadeira estoque)
        {
            var response = await _httpClient.PostAsJsonAsync("/estoquemadeiras", estoque);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ApiEstoqueMadeira>();
        }

        public async Task<ApiEstoqueMadeira> UpdateEstoqueMadeiraAsync(string id, ApiEstoqueMadeira estoque)
        {
            var response = await _httpClient.PutAsJsonAsync($"/estoquemadeiras/{id}", estoque);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ApiEstoqueMadeira>();
        }

        // ========== ORÇAMENTOS (como Pedidos) ==========
        public async Task<List<ApiOrcamento>> GetOrcamentosAsync(string? nomeCliente = null)
        {
            try
            {
                var url = "/orcamentos";
                if (!string.IsNullOrEmpty(nomeCliente))
                    url += $"?nomeCliente={Uri.EscapeDataString(nomeCliente)}";

                return await _httpClient.GetFromJsonAsync<List<ApiOrcamento>>(url) ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar orçamentos: {ex.Message}");
                return new();
            }
        }

        public async Task<ApiOrcamento> GetOrcamentoByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<ApiOrcamento>($"/orcamentos/{id}");
        }

        public async Task<ApiOrcamento> CreateOrcamentoAsync(ApiOrcamento orcamento)
        {
            var response = await _httpClient.PostAsJsonAsync("/orcamentos", orcamento);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ApiOrcamento>();
        }

        // ========== VENDAS (Pedidos Finalizados) ==========
        public async Task<List<ApiVenda>> GetVendasAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<ApiVenda>>("/vendas") ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar vendas: {ex.Message}");
                return new();
            }
        }

        public async Task<ApiVenda> CreateVendaAsync(ApiVenda venda)
        {
            var response = await _httpClient.PostAsJsonAsync("/vendas", venda);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ApiVenda>();
        }

        // ========== MADEIRAS (para preencher pickers) ==========
        public async Task<List<ApiMadeira>> GetMadeirasAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<ApiMadeira>>("/madeiras") ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar madeiras: {ex.Message}");
                return new();
            }
        }

        // ========== TAMANHOS ==========
        public async Task<List<ApiTamanho>> GetTamanhosAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<ApiTamanho>>("/tamanhos") ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar tamanhos: {ex.Message}");
                return new();
            }
        }
    }

    // ========== MODELS DA API ==========

    public class ApiEstoqueMadeira
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("madeiraId")]
        public string MadeiraId { get; set; }

        [JsonPropertyName("tamanhoId")]
        public string TamanhoId { get; set; }

        [JsonPropertyName("quantidade")]
        public int Quantidade { get; set; }

        [JsonPropertyName("acabando")]
        public bool Acabando { get; set; }

        [JsonPropertyName("madeira")]
        public ApiMadeiraInfo? Madeira { get; set; }

        [JsonPropertyName("tamanho")]
        public ApiTamanhoInfo? Tamanho { get; set; }
    }

    public class ApiMadeira
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("fornecedorId")]
        public string? FornecedorId { get; set; }
    }

    public class ApiMadeiraInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }
    }

    public class ApiTamanho
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }
    }

    public class ApiTamanhoInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }
    }

    public class ApiOrcamento
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("usuarioId")]
        public string UsuarioId { get; set; }

        [JsonPropertyName("clienteId")]
        public string? ClienteId { get; set; }

        [JsonPropertyName("nome")]
        public string? Nome { get; set; }

        [JsonPropertyName("cpf")]
        public string? Cpf { get; set; }

        [JsonPropertyName("valorTotal")]
        public decimal ValorTotal { get; set; }

        [JsonPropertyName("dataCriacao")]
        public DateTime? DataCriacao { get; set; }

        [JsonPropertyName("orcamentoE")]
        public List<ApiOrcamentoItem>? OrcamentoE { get; set; }
    }

    public class ApiOrcamentoItem
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("orcamentoId")]
        public string? OrcamentoId { get; set; }

        [JsonPropertyName("estoqueMadeiraId")]
        public string? EstoqueMadeiraId { get; set; }

        [JsonPropertyName("quantidade")]
        public int Quantidade { get; set; }

        [JsonPropertyName("valorVenda")]
        public decimal ValorVenda { get; set; }

        [JsonPropertyName("valorTotal")]
        public decimal ValorTotal { get; set; }
    }

    public class ApiVenda
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("usuarioId")]
        public string UsuarioId { get; set; }

        [JsonPropertyName("clienteId")]
        public string ClienteId { get; set; }

        [JsonPropertyName("valorTotal")]
        public decimal ValorTotal { get; set; }

        [JsonPropertyName("dataCriacao")]
        public DateTime? DataCriacao { get; set; }

        [JsonPropertyName("pago")]
        public bool Pago { get; set; }

        [JsonPropertyName("vendasE")]
        public List<ApiVendaItem>? VendasE { get; set; }
    }

    public class ApiVendaItem
    {
        [JsonPropertyName("estoqueMadeiraId")]
        public string? EstoqueMadeiraId { get; set; }

        [JsonPropertyName("quantidade")]
        public int Quantidade { get; set; }

        [JsonPropertyName("valorVenda")]
        public decimal ValorVenda { get; set; }

        [JsonPropertyName("valorTotal")]
        public decimal ValorTotal { get; set; }
    }
}