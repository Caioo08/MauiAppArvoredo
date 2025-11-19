using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace MauiAppArvoredo.Services
{
    /// <summary>
    /// Serviço para integração com a API de Usuários
    /// </summary>
    public class UsuarioApiService
    {
        private readonly HttpClient _http;
        private const string BASE_URL = "https://arvoredoapi.vercel.app";
        private const string API_KEY = "68e553e6f1c4fffd11c95840";

        public UsuarioApiService()
        {
            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("x-api-key", API_KEY);
            _http.Timeout = TimeSpan.FromSeconds(30);
        }

        // ================================
        // AUTENTICAR USUÁRIO
        // ================================

        /// <summary>
        /// Autentica um usuário pelo email (login) e senha
        /// Retorna apenas usuários com nivelAcesso = 1 (mobile)
        /// </summary>
        public async Task<(bool sucesso, UsuarioApiResponse usuario, string erro)> AutenticarAsync(string login, string senha)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"📤 GET /usuarios");

                var response = await _http.GetAsync($"{BASE_URL}/usuarios");
                var responseBody = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"📥 Status: {(int)response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var usuarios = JsonConvert.DeserializeObject<List<UsuarioApiResponse>>(responseBody);

                    // Busca usuário com login e senha correspondentes
                    var usuario = usuarios?.FirstOrDefault(u =>
                        u.Login.Equals(login, StringComparison.OrdinalIgnoreCase) &&
                        u.Senha == senha &&
                        u.NivelAcesso == 1 && // Apenas nível 1 (mobile)
                        u.Ativo == true
                    );

                    if (usuario != null)
                    {
                        return (true, usuario, null);
                    }
                    else
                    {
                        return (false, null, "Usuário ou senha inválidos, ou sem permissão de acesso mobile");
                    }
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
        // CRIAR USUÁRIO
        // ================================

        /// <summary>
        /// Cria um novo usuário com nível de acesso 1 (mobile)
        /// </summary>
        public async Task<(bool sucesso, UsuarioApiResponse usuario, string erro)> CriarUsuarioAsync(UsuarioCreateDto usuarioDto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(usuarioDto, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Newtonsoft.Json.Formatting.Indented
                });

                System.Diagnostics.Debug.WriteLine($"📤 POST /usuarios");
                System.Diagnostics.Debug.WriteLine($"Payload: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync($"{BASE_URL}/usuarios", content);

                var responseBody = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"📥 Status: {(int)response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"Response: {responseBody}");

                if (response.IsSuccessStatusCode)
                {
                    var usuario = JsonConvert.DeserializeObject<UsuarioApiResponse>(responseBody);
                    return (true, usuario, null);
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
        // BUSCAR USUÁRIO POR ID
        // ================================

        /// <summary>
        /// Busca um usuário específico por ID
        /// </summary>
        public async Task<(bool sucesso, UsuarioApiResponse usuario, string erro)> BuscarUsuarioPorIdAsync(int id)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"📤 GET /usuarios/{id}");

                var response = await _http.GetAsync($"{BASE_URL}/usuarios/{id}");
                var responseBody = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"📥 Status: {(int)response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var usuario = JsonConvert.DeserializeObject<UsuarioApiResponse>(responseBody);
                    return (true, usuario, null);
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
    }

    // ================================
    // DTOs
    // ================================

    /// <summary>
    /// Resposta da API para usuário
    /// </summary>
    public class UsuarioApiResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("senha")]
        public string Senha { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("nivelAcesso")]
        public int NivelAcesso { get; set; }

        [JsonProperty("ativo")]
        public bool Ativo { get; set; }

        [JsonProperty("dataCriacao")]
        public DateTime DataCriacao { get; set; }
    }

    /// <summary>
    /// DTO para criar usuário
    /// </summary>
    public class UsuarioCreateDto
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("senha")]
        public string Senha { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("nivelAcesso")]
        public int NivelAcesso { get; set; } = 1; // Sempre 1 para mobile

        [JsonProperty("ativo")]
        public bool Ativo { get; set; } = true;
    }
}