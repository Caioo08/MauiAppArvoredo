using MauiAppArvoredo.Models;
using MauiAppArvoredo.Services;
using System.Collections.ObjectModel;

namespace MauiAppArvoredo.Views
{
    public partial class Vendas : ContentPage
    {
        private readonly VendaApiService _vendaService;
        private ObservableCollection<Venda> _todasVendas;
        private string _filtroAtual = "todas"; // todas, pendentes, pagas

        public Vendas()
        {
            InitializeComponent();
            _vendaService = new VendaApiService();
            _todasVendas = new ObservableCollection<Venda>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarVendasAsync();
        }

        // ================================
        // CARREGAR VENDAS DA API
        // ================================

        private async Task CarregarVendasAsync()
        {
            try
            {
                MostrarLoading("Carregando vendas...");

                // Chama o serviço para listar vendas da API
                var (sucesso, vendasApi, erro) = await _vendaService.ListarVendasAsync();

                if (sucesso && vendasApi != null)
                {
                    _todasVendas.Clear();

                    foreach (var vendaApi in vendasApi)
                    {
                        // Converte cada item da API para o modelo Venda
                        var venda = new Venda
                        {
                            Id = vendaApi.Id,               // Id local (pode ser igual ao da API)
                            ApiId = vendaApi.Id,            // Id usado para abrir detalhes
                            NomeCliente = vendaApi.Nome ?? "Cliente não informado",
                            ValorTotal = vendaApi.ValorTotal,
                            DataCriacao = vendaApi.DataCriacao,
                            Pago = vendaApi.Pago,
                            FormaPagamento = vendaApi.Forma ?? "Não informado",
                            Descricao = vendaApi.Descricao ?? ""
                        };

                        _todasVendas.Add(venda);
                    }

                    // Aplica o filtro atual (Todas, Pendentes, Pagas)
                    AplicarFiltro(_filtroAtual);
                    AtualizarResumo();
                }
                else
                {
                    await DisplayAlert("Erro", erro ?? "Não foi possível carregar as vendas", "OK");
                    ListaVendas.ItemsSource = new List<Venda>();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao carregar vendas: {ex.Message}", "OK");
            }
            finally
            {
                EsconderLoading();
            }
        }


        // ================================
        // CONVERTER DADOS DA API
        // ================================

        private Venda ConverterParaVendaLocal(VendaApiResponse vendaApi)
        {
            return new Venda
            {
                Id = vendaApi.Id,
                NomeCliente = vendaApi.Nome ?? "Cliente não informado",
                ValorTotal = vendaApi.ValorTotal,
                Pago = vendaApi.Pago,
                DataCriacao = vendaApi.DataCriacao,
                FormaPagamento = vendaApi.Forma ?? "Não informado",
                Descricao = vendaApi.Descricao ?? "",
                ApiId = vendaApi.Id
            };
        }


        // ================================
        // FILTROS
        // ================================

        private void OnFiltroTodasClicked(object sender, EventArgs e)
        {
            _filtroAtual = "todas";
            AplicarFiltro("todas");
            AtualizarBotoesFiltro();
        }

        private void OnFiltroPendentesClicked(object sender, EventArgs e)
        {
            _filtroAtual = "pendentes";
            AplicarFiltro("pendentes");
            AtualizarBotoesFiltro();
        }

        private void OnFiltroPagasClicked(object sender, EventArgs e)
        {
            _filtroAtual = "pagas";
            AplicarFiltro("pagas");
            AtualizarBotoesFiltro();
        }

        private void AplicarFiltro(string filtro)
        {
            List<Venda> vendasFiltradas;

            switch (filtro)
            {
                case "pendentes":
                    vendasFiltradas = _todasVendas.Where(v => !v.Pago).ToList();
                    lblMensagemVazia.Text = "Nenhuma venda pendente";
                    break;

                case "pagas":
                    vendasFiltradas = _todasVendas.Where(v => v.Pago).ToList();
                    lblMensagemVazia.Text = "Nenhuma venda paga";
                    break;

                default: // todas
                    vendasFiltradas = _todasVendas.ToList();
                    lblMensagemVazia.Text = "Nenhuma venda encontrada";
                    break;
            }

            ListaVendas.ItemsSource = vendasFiltradas;
        }

        private void AtualizarBotoesFiltro()
        {
            // Reset todos
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

            // Ativa o selecionado
            Button btnAtivo = _filtroAtual switch
            {
                "pendentes" => btnPendentes,
                "pagas" => btnPagas,
                _ => btnTodas
            };

            btnAtivo.BackgroundColor = Color.FromArgb("#391b01");
            btnAtivo.TextColor = Colors.White;
            btnAtivo.BorderWidth = 0;
        }

        // ================================
        // ATUALIZAR RESUMO
        // ================================

        private void AtualizarResumo()
        {
            int totalVendas = _todasVendas.Count;
            double valorTotal = _todasVendas.Sum(v => v.ValorTotal);
            int vendasPendentes = _todasVendas.Count(v => !v.Pago);

            lblTotalVendas.Text = totalVendas.ToString();
            lblValorTotal.Text = $"R$ {valorTotal:N2}";
            lblVendasPendentes.Text = vendasPendentes.ToString();
        }

        // ================================
        // SINCRONIZAR COM API
        // ================================

        private async void OnSincronizarClicked(object sender, EventArgs e)
        {
            await CarregarVendasAsync();
            await DisplayAlert("Sucesso", "Vendas sincronizadas com sucesso!", "OK");
        }


        // ================================
        // VOLTAR
        // ================================

        private async void OnVoltarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // ================================
        // LOADING
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