using MauiAppArvoredo.Models;
using MauiAppArvoredo.Services;

namespace MauiAppArvoredo.Views
{
    public partial class DetalhesVenda : ContentPage
    {
        private Venda _venda;
        private List<ItemVenda> _itens;
        private readonly VendaApiService _apiService;

        public DetalhesVenda(Venda venda)
        {
            InitializeComponent();
            _venda = venda;
            _apiService = new VendaApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarDetalhesAsync();
        }

        private async Task CarregarDetalhesAsync()
        {
            try
            {
                // Carrega dados principais da venda
                lblCliente.Text = _venda.NomeCliente;
                lblData.Text = _venda.DataFormatada;
                lblStatus.Text = _venda.StatusTexto;
                lblStatus.TextColor = _venda.StatusColor;
                lblFormaPagamento.Text = $"?? {_venda.FormaPagamento}";
                lblValorTotal.Text = _venda.ValorFormatado;
                lblDescricao.Text = string.IsNullOrEmpty(_venda.Descricao)
                    ? "Sem descrição"
                    : _venda.Descricao;

                // Mostra data de pagamento se estiver pago
                if (_venda.Pago && _venda.DataPagamento.HasValue)
                {
                    containerDataPagamento.IsVisible = true;
                    lblDataPagamento.Text = _venda.DataPagamento.Value.ToString("dd/MM/yyyy HH:mm");
                }

                // Mostra botão "Marcar como Pago" se ainda não estiver pago
                btnMarcarPago.IsVisible = !_venda.Pago;

                // Carrega itens da venda
                _itens = await App.Database.GetItensByVendaAsync(_venda.Id);
                ListaItens.ItemsSource = _itens;

                // Atualiza total de itens
                if (_itens != null && _itens.Any())
                {
                    lblTotalItens.Text = $"Total de {_itens.Count} item(ns) • {_itens.Sum(i => i.Quantidade)} unidade(s)";
                }
                else
                {
                    lblTotalItens.Text = "Nenhum item nesta venda";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao carregar detalhes:\n{ex.Message}", "OK");
            }
        }

        // ================================
        // MARCAR COMO PAGO
        // ================================

        private async void OnMarcarPagoClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert(
                "Confirmar Pagamento",
                $"Deseja marcar esta venda como paga?\n\nValor: {_venda.ValorFormatado}",
                "Sim, marcar como pago",
                "Cancelar"
            );

            if (!confirmar)
                return;

            try
            {
                MostrarLoading("Marcando como pago...");

                // Atualiza no banco local
                bool sucesso = await App.Database.MarcarVendaComoPagaAsync(_venda.Id);

                if (sucesso)
                {
                    // Tenta atualizar na API também (se tiver ApiId)
                    if (_venda.ApiId > 0)
                    {
                        var resultado = await _apiService.MarcarComoPagaAsync(_venda.ApiId);

                        if (!resultado.sucesso)
                        {
                            await DisplayAlert("Aviso",
                                "Venda marcada como paga localmente, mas não foi possível sincronizar com a API.\n\n" +
                                $"Erro: {resultado.erro}",
                                "OK");
                        }
                    }

                    // Recarrega a venda atualizada
                    _venda = await App.Database.GetVendaAsync(_venda.Id);

                    // Atualiza a interface
                    await CarregarDetalhesAsync();

                    await DisplayAlert("Sucesso", "? Venda marcada como paga!", "OK");
                }
                else
                {
                    await DisplayAlert("Erro", "Não foi possível marcar a venda como paga.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao marcar como pago:\n{ex.Message}", "OK");
            }
            finally
            {
                EsconderLoading();
            }
        }

        // ================================
        // NAVEGAÇÃO
        // ================================

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
    }
}