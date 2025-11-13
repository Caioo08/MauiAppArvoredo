using MauiAppArvoredo.Models;
using MauiAppArvoredo.Services;
using Microsoft.Maui.Controls;

namespace MauiAppArvoredo.Views;

public partial class Pedidos : ContentPage
{
    private readonly ArvoredoApiService _apiService;
    private List<ApiOrcamento> orcamentos = new List<ApiOrcamento>();

    public Pedidos()
    {
        InitializeComponent();
        _apiService = new ArvoredoApiService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CarregarOrcamentosDaApi();
    }

    private async Task CarregarOrcamentosDaApi()
    {
        try
        {
            // Mostra loading
            ListaPedidos.IsVisible = false;

            // Busca orçamentos da API
            orcamentos = await _apiService.GetOrcamentosAsync();

            // Converte para exibição (simulando pedidos)
            var pedidosExibicao = orcamentos.Select(o => new PedidoExibicao
            {
                Id = o.Id,
                Cliente = o.Nome ?? "Cliente não informado",
                Descricao = o.Descricao,
                ValorTotal = o.ValorTotal,
                DataCriacao = o.DataCriacao,
                Status = "Pendente" // Pode ser customizado
            }).ToList();

            ListaPedidos.ItemsSource = pedidosExibicao;
            ListaPedidos.IsVisible = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar os pedidos: {ex.Message}", "OK");
        }
    }

    private async void BtnNovoPedido_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditarPedido());
    }

    private async void BtnDarBaixa_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var pedido = button?.BindingContext as PedidoExibicao;

        if (pedido == null) return;

        bool confirmar = await DisplayAlert(
            "Confirmar Baixa",
            $"Deseja dar baixa no pedido de {pedido.Cliente}?\n\nValor: R$ {pedido.ValorTotal:F2}",
            "Sim",
            "Não"
        );

        if (!confirmar) return;

        try
        {
            // Busca o orçamento completo
            var orcamentoCompleto = await _apiService.GetOrcamentoByIdAsync(pedido.Id);

            if (orcamentoCompleto?.OrcamentoE != null)
            {
                // Cria uma venda baseada no orçamento
                var venda = new ApiVenda
                {
                    Descricao = orcamentoCompleto.Descricao ?? "Venda",
                    UsuarioId = orcamentoCompleto.UsuarioId ?? "usuario_padrao",
                    ClienteId = orcamentoCompleto.ClienteId ?? "cliente_padrao",
                    ValorTotal = orcamentoCompleto.ValorTotal,
                    Pago = true,
                    VendasE = orcamentoCompleto.OrcamentoE.Select(item => new ApiVendaItem
                    {
                        EstoqueMadeiraId = item.EstoqueMadeiraId,
                        Quantidade = item.Quantidade,
                        ValorVenda = item.ValorVenda,
                        ValorTotal = item.ValorTotal
                    }).ToList()
                };

                // Cria a venda na API
                await _apiService.CreateVendaAsync(venda);

                // Atualiza estoque localmente (se necessário)
                foreach (var item in orcamentoCompleto.OrcamentoE)
                {
                    if (!string.IsNullOrEmpty(item.EstoqueMadeiraId))
                    {
                        try
                        {
                            var estoqueItem = await _apiService.GetEstoqueMadeiraByIdAsync(item.EstoqueMadeiraId);
                            if (estoqueItem != null)
                            {
                                estoqueItem.Quantidade -= item.Quantidade;
                                if (estoqueItem.Quantidade < 0) estoqueItem.Quantidade = 0;

                                await _apiService.UpdateEstoqueMadeiraAsync(item.EstoqueMadeiraId, estoqueItem);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao atualizar estoque: {ex.Message}");
                        }
                    }
                }

                await DisplayAlert("Sucesso", "Pedido finalizado e estoque atualizado!", "OK");
                await CarregarOrcamentosDaApi();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível dar baixa: {ex.Message}", "OK");
        }
    }

    private async void BtnDetalhes_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var pedido = button?.BindingContext as PedidoExibicao;

        if (pedido == null) return;

        try
        {
            // Busca detalhes do orçamento
            var orcamento = await _apiService.GetOrcamentoByIdAsync(pedido.Id);

            if (orcamento != null)
            {
                var itensTexto = orcamento.OrcamentoE != null && orcamento.OrcamentoE.Any()
                    ? string.Join("\n", orcamento.OrcamentoE.Select(i =>
                        $"• Qtd: {i.Quantidade} - R$ {i.ValorTotal:F2}"))
                    : "Nenhum item cadastrado";

                await DisplayAlert(
                    $"Pedido de {pedido.Cliente}",
                    $"📝 Descrição: {orcamento.Descricao}\n\n" +
                    $"📦 Itens:\n{itensTexto}\n\n" +
                    $"💰 Valor Total: R$ {orcamento.ValorTotal:F2}\n" +
                    $"📅 Data: {orcamento.DataCriacao?.ToString("dd/MM/yyyy HH:mm") ?? "N/A"}",
                    "Fechar"
                );
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar detalhes: {ex.Message}", "OK");
        }
    }

    private void BtnSair_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new TelaInicial());
        }
        catch (Exception ex)
        {
            DisplayAlert("Não encontrado", ex.Message, "OK");
        }
    }
}

// Classe auxiliar para exibição
public class PedidoExibicao
{
    public string Id { get; set; }
    public string Cliente { get; set; }
    public string Descricao { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime? DataCriacao { get; set; }
    public string Status { get; set; }
}