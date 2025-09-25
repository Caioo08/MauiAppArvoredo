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
            pedido.Status = "Concluído";
            await App.Database.UpdatePedidoAsync(pedido);

            pedidos.Remove(pedido);
            ListaPedidos.ItemsSource = null;
            ListaPedidos.ItemsSource = pedidos.Where(p => p.Status == "Pendente").ToList();
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
