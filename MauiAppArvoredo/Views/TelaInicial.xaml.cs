using MauiAppArvoredo.Views;
using MauiAppArvoredo.Services;

namespace MauiAppArvoredo;

public partial class TelaInicial : ContentPage
{
    private readonly ApiClient _api;

    public TelaInicial()
    {
        InitializeComponent();

        _api = new ApiClient(); // Cria a instância do cliente API
    }

    private void sair_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Login());
        }
        catch (Exception ex)
        {
            DisplayAlert("Não encontrado", ex.Message, "OK");
        }
    }

    private void estoque_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Passa a instância do ApiClient para a página Estoque
            Navigation.PushAsync(new Estoque(_api));
        }
        catch (Exception ex)
        {
            DisplayAlert("Não encontrado", ex.Message, "OK");
        }
    }

    private void pedidos_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Vendas());
        }
        catch (Exception ex)
        {
            DisplayAlert("Não encontrado", ex.Message, "OK");
        }
    }
}
