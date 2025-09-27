using MauiAppArvoredo.Models;
using Microsoft.Maui.Controls;

namespace MauiAppArvoredo.Views;

public partial class Pedidos : ContentPage
{
    private List<Pedido> pedidos = new List<Pedido>();

    public Pedidos()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CarregarPedidos();
    }

    private async Task CarregarPedidos()
    {
        pedidos = await App.Database.GetPedidosAsync();
        ListaPedidos.ItemsSource = pedidos.Where(p => p.Status == "Pendente").ToList();
    }

    private async void BtnNovoPedido_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditarPedido());
    }

    private async void BtnDarBaixa_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var pedido = button.BindingContext as Pedido;

        if (pedido != null)
        {
            // 🔹 Carrega os itens do pedido
            var itensPedido = await App.Database.GetItensByPedidoAsync(pedido.Id);

            foreach (var itemPedido in itensPedido)
            {
                // Nome do itemPedido está no formato "Madeira - Formato - Tamanho"
                var partes = itemPedido.Nome.Split(" - ");
                if (partes.Length < 3) continue;

                string nomeMadeira = partes[0];
                string formato = partes[1];
                string tamanho = partes[2];

                // Localiza a madeira
                var madeira = (await App.Database.GetMadeirasAsync())
                              .FirstOrDefault(m => m.Nome == nomeMadeira);

                if (madeira != null)
                {
                    // Busca item correspondente no estoque
                    var itensEstoque = await App.Database.GetItensByMadeiraAsync(madeira.Id);
                    var itemEstoque = itensEstoque
                        .FirstOrDefault(i => i.Formato == formato && i.Tamanho == tamanho);

                    if (itemEstoque != null)
                    {
                        // Desconta a quantidade
                        itemEstoque.Quantidade -= itemPedido.Quantidade;
                        if (itemEstoque.Quantidade < 0)
                            itemEstoque.Quantidade = 0; // evita valores negativos

                        await App.Database.SaveItemMadeiraAsync(itemEstoque);
                    }
                }
            }

            // Atualiza status do pedido
            pedido.Status = "Concluído";
            await App.Database.UpdatePedidoAsync(pedido);

            // Atualiza lista na tela
            pedidos.Remove(pedido);
            ListaPedidos.ItemsSource = null;
            ListaPedidos.ItemsSource = pedidos.Where(p => p.Status == "Pendente").ToList();

            await DisplayAlert("Sucesso", "Pedido concluído e estoque atualizado!", "OK");
        }
    }

    private async void BtnDetalhes_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var pedido = button.BindingContext as Pedido;

        if (pedido != null)
        {
            await Navigation.PushAsync(new EditarPedido(pedido));
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
