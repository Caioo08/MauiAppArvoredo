using MauiAppArvoredo.Models;
using MauiAppArvoredo.Views;
namespace MauiAppArvoredo;

public partial class Inicio : ContentPage
{
    public Inicio()
	{
		InitializeComponent();
        tempo();
    }

    private void tempo()
    {
        Label horaLabel = new Label
        {
            Text = "Sincronizado desde:" + DateTime.Now.ToString("HH:mm"),
            FontSize = 16,
            TextColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center
        };
        
        Footer.Add(horaLabel);
        
    }

    private void login_Clicked(object sender, EventArgs e)
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

    private async void sincronizar_Clicked(object sender, EventArgs e)
    {

    }

    private void sobre_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new InfoPage());
        }
        catch (Exception ex)
        {
            DisplayAlert("Não encontrado", ex.Message, "OK");
        }
    }
}