using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using MauiAppArvoredo.Models;

namespace MauiAppArvoredo.Services
{
    /// <summary>
    /// Serviço para integração com a API de Vendas
    /// Gerencia todas as operações de vendas com o backend
    /// </summary>
    public class VendaApiService
    {
        private readonly HttpClient _http;
        private const string BASE_URL = "https://arvoredoapi.vercel.app";
        private const string API_KEY = "68e553e6f1c4fffd11c95840";

        public VendaApiService()
        {
            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("x-api-key", API_KEY);
            _http.Timeout = TimeSpan.FromSeconds(30);
        }

        // ================================
        // CRIAR VENDA
        // ================================

        /// <summary>
        /// Cria uma nova venda na API
        /// </summary>
        public async Task<(bool sucesso, VendaApiResponse venda, string erro)> CriarVendaAsync(VendaCreateDto vendaDto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(vendaDto, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.Indented
                });

                System.Diagnostics.Debug.WriteLine($"📤 POST /vendas");
                System.Diagnostics.Debug.WriteLine($"Payload: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync($"{BASE_URL}/vendas", content);

                var responseBody = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"📥 Status: {(int)response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"Response: {responseBody}");

                if (response.IsSuccessStatusCode)
                {
                    var vendaResponse = JsonConvert.DeserializeObject<VendaApiResponse>(responseBody);
                    return (true, vendaResponse, null);
                }
                else
                {
                    var erro = TratarErroApi(responseBody, response.StatusCode);
                    return (false, null, erro);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"Erro de conexão: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Erro inesperado: {ex.Message}");
            }
        }

        // ================================
        // LISTAR VENDAS
        // ================================

        /// <summary>
        /// Lista todas as vendas da API
        /// </summary>
        public async Task<(bool sucesso, List<VendaApiResponse> vendas, string erro)> ListarVendasAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"📤 GET /vendas");

                var response = await _http.GetAsync($"{BASE_URL}/vendas");
                var responseBody = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"📥 Status: {(int)response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var vendas = JsonConvert.DeserializeObject<List<VendaApiResponse>>(responseBody);
                    return (true, vendas ?? new List<VendaApiResponse>(), null);
                }
                else
                {
                    var erro = TratarErroApi(responseBody, response.StatusCode);
                    return (false, null, erro);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"Erro de conexão: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Erro inesperado: {ex.Message}");
            }
        }

        // ================================
        // BUSCAR VENDA POR ID
        // ================================

        /// <summary>
        /// Busca uma venda específica por ID
        /// </summary>
        public async Task<(bool sucesso, VendaApiResponse venda, string erro)> BuscarVendaPorIdAsync(int id)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"📤 GET /vendas/{id}");

                var response = await _http.GetAsync($"{BASE_URL}/vendas/{id}");
                var responseBody = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"📥 Status: {(int)response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var venda = JsonConvert.DeserializeObject<VendaApiResponse>(responseBody);
                    return (true, venda, null);
                }
                else
                {
                    var erro = TratarErroApi(responseBody, response.StatusCode);
                    return (false, null, erro);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"Erro de conexão: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Erro inesperado: {ex.Message}");
            }
        }

        // ================================
        // ATUALIZAR STATUS DE PAGAMENTO
        // ================================

        /// <summary>
        /// Marca uma venda como paga
        /// </summary>
        public async Task<(bool sucesso, string erro)> MarcarComoPagaAsync(int vendaId)
        {
            try
            {
                var payload = new { pago = true };
                var json = JsonConvert.SerializeObject(payload);

                System.Diagnostics.Debug.WriteLine($"📤 PUT /vendas/{vendaId}");
                System.Diagnostics.Debug.WriteLine($"Payload: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _http.PutAsync($"{BASE_URL}/vendas/{vendaId}", content);

                var responseBody = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"📥 Status: {(int)response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    var erro = TratarErroApi(responseBody, response.StatusCode);
                    return (false, erro);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erro de conexão: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, $"Erro inesperado: {ex.Message}");
            }
        }

        // ================================
        // DELETAR VENDA
        // ================================

        /// <summary>
        /// Deleta uma venda da API
        /// </summary>
        public async Task<(bool sucesso, string erro)> DeletarVendaAsync(int vendaId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"📤 DELETE /vendas/{vendaId}");

                var response = await _http.DeleteAsync($"{BASE_URL}/vendas/{vendaId}");
                var responseBody = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"📥 Status: {(int)response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    var erro = TratarErroApi(responseBody, response.StatusCode);
                    return (false, erro);
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erro de conexão: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, $"Erro inesperado: {ex.Message}");
            }
        }

        // ================================
        // HELPERS
        // ================================

        /// <summary>
        /// Trata erros retornados pela API
        /// </summary>
        private string TratarErroApi(string responseBody, System.Net.HttpStatusCode statusCode)
        {
            try
            {
                var erro = JsonConvert.DeserializeObject<dynamic>(responseBody);
                string mensagem = erro?.message ?? erro?.error ?? responseBody;

                return $"Erro {(int)statusCode}: {mensagem}";
            }
            catch
            {
                return $"Erro {(int)statusCode}: {responseBody}";
            }
        }

        /// <summary>
        /// Normaliza a forma de pagamento para valores aceitos pela API
        /// </summary>
        public static string NormalizarFormaPagamento(string forma)
        {
            if (string.IsNullOrWhiteSpace(forma))
                return "Dinheiro";

            string formaNormalizada = forma.Trim().ToLower();

            return formaNormalizada switch
            {
                "dinheiro" => "Dinheiro",
                "débito" or "debito" => "Debito",
                "crédito" or "credito" => "Credito",
                "pix" => "PIX",
                _ => "Dinheiro"
            };
        }
    }
}