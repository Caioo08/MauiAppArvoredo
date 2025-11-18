using System.Net.Http.Headers;
using Newtonsoft.Json;
using MauiAppArvoredo.Models;

namespace MauiAppArvoredo.Services
{
    public class ApiClient
    {
        private readonly HttpClient _http;

        private const string BASE_URL = "https://arvoredoapi.vercel.app";
        private const string API_KEY = "68e553e6f1c4fffd11c95840";


        public ApiClient()
        {
            _http = new HttpClient();

            // 🔑 obrigatório para a API permitir requisição

            _http.DefaultRequestHeaders.Add("x-api-key", API_KEY);
        }

        // ================================
        // LISTAR PRODUTOS
        // ================================
        public async Task<List<Produto>> GetProdutosAsync()
        {
            var response = await _http.GetAsync($"{BASE_URL}/produtos");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Produto>>(json);
        }

        // ================================
        // BUSCAR POR ID
        // ================================
        public async Task<Produto> GetProdutoByIdAsync(int id)
        {
            var response = await _http.GetAsync($"{BASE_URL}/produtos/{id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Produto>(json);
        }

        public async Task<(bool ok, string body)> UpdateProdutoAsync(int id, Produto produto)
        {
            var json = JsonConvert.SerializeObject(produto);
            System.Diagnostics.Debug.WriteLine("PUT payload: " + json);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _http.PutAsync($"{BASE_URL}/produtos/{id}", content);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) return (true, body);
            return (false, $"Status: {(int)response.StatusCode} {response.ReasonPhrase}\nBody: {body}");
        }

        public async Task<(bool ok, string body)> AddProdutoAsync(Produto produto)
        {
            var json = JsonConvert.SerializeObject(produto);
            System.Diagnostics.Debug.WriteLine("POST payload: " + json);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _http.PostAsync($"{BASE_URL}/produtos", content);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) return (true, body);
            return (false, $"Status: {(int)response.StatusCode} {response.ReasonPhrase}\nBody: {body}");
        }

        // ================================
        // EXCLUIR PRODUTO
        // ================================
        public async Task DeleteProdutoAsync(int id)
        {
            var response = await _http.DeleteAsync($"{BASE_URL}/produtos/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
