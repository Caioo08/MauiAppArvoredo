using MauiAppArvoredo.Models;
using MauiAppArvoredo.Services;

namespace MauiAppArvoredo.Views
{
    public partial class Vendas : ContentPage
    {
        private readonly VendaApiService _apiService;
        private List<Venda> _todasVendas = new();
        private List<Venda> _vendasExibidas = new();
        private string _filtroAtivo = "todas";

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

        private async Task CarregarVendasAsync()
        {
            try
            {
                MostrarLoading("Carregando vendas...");

                _todasVendas = await App.Database.GetVendasAsync();

                await SincronizarComApiAsync(false);

                AplicarFiltro();
                AtualizarResumo();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                EsconderLoading();
            }
        }

        private async Task SincronizarComApiAsync(bool mostrarMensagem = true)
        {
            try
            {
                if (mostrarMensagem)
                    MostrarLoading("Sincronizando...");

                var resultado = await _apiService.ListarVendasAsync();

                if (resultado.sucesso && resultado.vendas != null)
                {
                    foreach (var vendaApi in resultado.vendas)
                    {
                        var existente = _todasVendas.FirstOrDefault(v => v.ApiId == vendaApi.Id);

                        if (existente == null)
                        {
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
                            existente.Pago = vendaApi.Pago;
                            existente.DataPagamento = vendaApi.DataPagamento;
                            existente.Sincronizado = true;

                            await App.Database.UpdateVendaAsync(existente);
                        }
                    }

                    _todasVendas = await App.Database.GetVendasAsync();

                    if (mostrarMensagem)
                        await DisplayAlert("Sucesso",
                            $"{resultado.vendas.Count} venda(s) sincronizada(s)", "OK");
                }
            }
            catch (Exception ex)
            {
                if (mostrarMensagem)
                    await DisplayAlert("Erro", ex.Message, "OK");
            }
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

            lblMensagemVazia.Text = _filtroAtivo switch
            {
                "pendentes" => "Nenhuma venda pendente",
                "pagas" => "Nenhuma venda paga",
                _ => "Nenhuma venda cadastrada"
            };
        }

        private void AtualizarResumo()
        {
            lblTotalVendas.Text = _todasVendas.Count.ToString();
            lblValorTotal.Text = $"R$ {_todasVendas.Sum(v => v.ValorTotal):N2}";
            lblVendasPendentes.Text = _todasVendas.Count(v => !v.Pago).ToString();
        }

        private async void OnVendaSelecionada(object sender, TappedEventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is Venda venda)
                await Navigation.PushAsync(new DetalhesVenda(venda));
        }

        private async void OnSincronizarClicked(object sender, EventArgs e)
        {
            await SincronizarComApiAsync(true);
            AplicarFiltro();
            AtualizarResumo();
        }

        private async void OnVoltarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

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

            if (descricao.Contains(" - "))
                return descricao.Split(" - ")[1];

            if (descricao.StartsWith("Venda "))
                return descricao.Replace("Venda ", "").Trim();

            return descricao;
        }
    }
}
