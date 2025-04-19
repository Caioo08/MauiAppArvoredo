using MauiAppArvoredo.Views;

namespace MauiAppArvoredo;

public partial class TelaInicial : ContentPage
{
    public TelaInicial()
	{
		InitializeComponent();

        var login = new Login();

        if(login.acessoadmin == true)
        {
            DisplayAlert("Acesso", "Acesso de administrador", "OK");
        }
        else if(login.acessoadmin == false)
        {
            DisplayAlert("OK", "", "OK");
        }
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
        var login = new Login();
        login.acessoadmin = false;
    }

    private void estoque_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Estoque());
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
            Navigation.PushAsync(new Pedidos());
        }
        catch (Exception ex)
        {
            DisplayAlert("Não encontrado", ex.Message, "OK");
        }
    }
}