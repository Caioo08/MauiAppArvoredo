using MauiAppArvoredo.Models;
using MauiAppArvoredo.Services;

namespace MauiAppArvoredo.Views
{
    public partial class Vendas : ContentPage
    {
        private readonly VendaApiService _apiService;
        private List<Venda> _todasVendas = new List<Venda>();
        private List<Venda> _vendasExibidas = new List<Venda>();
        private string _filtroAtivo = "todas"; // todas, pendentes, pagas

        public Vendas()
        {
            InitializeComponent();
            _apiService = new VendaApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarVendasAsync();
        }

        // ================================
        // CARREGAMENTO DE DADOS
        // ================================

        /// <summary>
        /// Carrega vendas do banco local e da API
        /// </summary>
        private async Task CarregarVendasAsync()
        {
            try
            {
                MostrarLoading("Carregando vendas...");

                // Carrega vendas locais
                _todasVendas = await App.Database.GetVendasAsync();

                // Tenta sincronizar com API
                await SincronizarComApiAsync(mostrarMensagem: false);

                // Aplica filtro ativo
                AplicarFiltro();

                // Atualiza resumo
                AtualizarResumo();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao carregar vendas:\n{ex.Message}", "OK");
            }
            finally
            {
                EsconderLoading();
            }
        }

        /// <summary>
        /// Sincroniza vendas com a API
        /// </summary>
        private async Task SincronizarComApiAsync(bool mostrarMensagem = true)
        {
            try
            {
                if (mostrarMensagem)
                    MostrarLoading("Sincronizando...");

                var resultado = await _apiService.ListarVendasAsync();

                if (resultado.sucesso && resultado.vendas != null)
                {
                    // Converte vendas da API para modelo local
                    foreach (var vendaApi in resultado.vendas)
                    {
                        // Verifica se já existe localmente
                        var existente = _todasVendas.FirstOrDefault(v => v.ApiId == vendaApi.Id);

                        if (existente == null)
                        {
                            // Cria nova venda local
                            var novaVenda = new Venda
                            {
                                ApiId = vendaApi.Id,
                                Descricao = vendaApi.Descricao,
                                UsuarioId = vendaApi.UsuarioId,
                                ClienteId = vendaApi.ClienteId,
                                ValorTotal = vendaApi.ValorTotal,
                                DataCriacao = vendaApi.DataCriacao,
                                DataPagamento = vendaApi.DataPagamento,
                                Pago = vendaApi.Pago,
                                FormaPagamento = vendaApi.Forma ?? "Dinheiro",
                                NomeCliente = ExtrairNomeCliente(vendaApi.Descricao),
                                Sincronizado = true
                            };

                            await App.Database.SaveVendaAsync(novaVenda);

                            // Salva itens
                            if (vendaApi.VendaE != null)
                            {
                                foreach (var itemApi in vendaApi.VendaE)
                                {
                                    var item = new ItemVenda
                                    {
                                        VendaId = novaVenda.Id,
                                        ProdutoId = itemApi.ProdutoId,
                                        Quantidade = itemApi.Quantidade,
                                        ValorVenda = itemApi.ValorVenda,
                                        ValorTotal = itemApi.ValorTotal,
                                        NomeProduto = itemApi.Produto?.Nome ?? "Produto",
                                        Unidade = itemApi.Produto?.Unidade ?? "un"
                                    };

                                    await App.Database.SaveItemVendaAsync(item);
                                }
                            }
                        }
                        else
                        {
                            // Atualiza venda existente
                            existente.Pago = vendaApi.Pago;
                            existente.DataPagamento = vendaApi.DataPagamento;
                            existente.Sincronizado = true;

                            await App.Database.UpdateVendaAsync(existente);
                        }
                    }

                    // Recarrega lista atualizada
                    _todasVendas = await App.Database.GetVendasAsync();

                    if (mostrarMensagem)
                    {
                        await DisplayAlert("Sucesso",
                            $"? {resultado.vendas.Count} venda(s) sincronizada(s)",
                            "OK");
                    }
                }
                else if (mostrarMensagem && !string.IsNullOrEmpty(resultado.erro))
                {
                    await DisplayAlert("Aviso",
                        $"Não foi possível sincronizar:\n{resultado.erro}",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                if (mostrarMensagem)
                {
                    await DisplayAlert("Erro",
                        $"Erro ao sincronizar:\n{ex.Message}",
                        "OK");
                }
            }
        }

        // ================================
        // FILTROS
        // ================================

        private void OnFiltroTodasClicked(object sender, EventArgs e)
        {
            _filtroAtivo = "todas";
            AtualizarBotoesFiltro();
            AplicarFiltro();
        }

        private void OnFiltroPendentesClicked(object sender, EventArgs e)
        {
            _filtroAtivo = "pendentes";
            AtualizarBotoesFiltro();
            AplicarFiltro();
        }

        private void OnFiltroPagasClicked(object sender, EventArgs e)
        {
            _filtroAtivo = "pagas";
            AtualizarBotoesFiltro();
            AplicarFiltro();
        }

        private void AtualizarBotoesFiltro()
        {
            // Reset todos os botões
            btnTodas.BackgroundColor = Colors.Transparent;
            btnTodas.TextColor = Color.FromArgb("#391b01");
            btnTodas.BorderColor = Color.FromArgb("#391b01");
            btnTodas.BorderWidth = 1;

            btnPendentes.BackgroundColor = Colors.Transparent;
            btnPendentes.TextColor = Color.FromArgb("#391b01");
            btnPendentes.BorderColor = Color.FromArgb("#391b01");
            btnPendentes.BorderWidth = 1;

            btnPagas.BackgroundColor = Colors.Transparent;
            btnPagas.TextColor = Color.FromArgb("#391b01");
            btnPagas.BorderColor = Color.FromArgb("#391b01");
            btnPagas.BorderWidth = 1;

            // Ativa botão selecionado
            Button btnAtivo = _filtroAtivo switch
            {
                "todas" => btnTodas,
                "pendentes" => btnPendentes,
                "pagas" => btnPagas,
                _ => btnTodas
            };

            btnAtivo.BackgroundColor = Color.FromArgb("#391b01");
            btnAtivo.TextColor = Colors.White;
            btnAtivo.BorderWidth = 0;
        }

        private void AplicarFiltro()
        {
            _vendasExibidas = _filtroAtivo switch
            {
                "pendentes" => _todasVendas.Where(v => !v.Pago).ToList(),
                "pagas" => _todasVendas.Where(v => v.Pago).ToList(),
                _ => _todasVendas.ToList()
            };

            ListaVendas.ItemsSource = _vendasExibidas;

            // Atualiza mensagem quando vazio
            lblMensagemVazia.Text = _filtroAtivo switch
            {
                "pendentes" => "Nenhuma venda pendente",
                "pagas" => "Nenhuma venda paga",
                _ => "Sincronize para carregar vendas da API"
            };
        }

        // ================================
        // RESUMO E ESTATÍSTICAS
        // ================================

        private void AtualizarResumo()
        {
            lblTotalVendas.Text = _todasVendas.Count.ToString();
            lblValorTotal.Text = $"R$ {_todasVendas.Sum(v => v.ValorTotal):N2}";
            lblVendasPendentes.Text = _todasVendas.Count(v => !v.Pago).ToString();
        }

        // ================================
        // EVENTOS
        // ================================

        private async void OnVendaSelecionada(object sender, TappedEventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is Venda venda)
            {
                await Navigation.PushAsync(new DetalhesVenda(venda));
            }
        }

        private async void OnSincronizarClicked(object sender, EventArgs e)
        {
            await SincronizarComApiAsync(mostrarMensagem: true);
            AplicarFiltro();
            AtualizarResumo();
        }

        private async void OnVoltarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // ================================
        // HELPERS
        // ================================

        private void MostrarLoading(string mensagem)
        {
            lblLoadingText.Text = mensagem;
            LoadingOverlay.IsVisible = true;
        }

        private void EsconderLoading()
        {
            LoadingOverlay.IsVisible = false;
        }

        private string ExtrairNomeCliente(string descricao)
        {
            if (string.IsNullOrEmpty(descricao))
                return "Cliente";

            // Tenta extrair nome após " - "
            if (descricao.Contains(" - "))
            {
                var partes = descricao.Split(" - ");
                return partes.Length > 1 ? partes[1] : descricao;
            }

            // Tenta extrair nome após "Venda "
            if (descricao.Contains("Venda "))
            {
                return descricao.Replace("Venda ", "").Trim();
            }

            return descricao;
        }
    }
}