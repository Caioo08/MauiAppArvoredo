using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using MauiAppArvoredo.Views;

namespace MauiAppArvoredo;

public partial class TelaInicial : ContentPage
{
	public TelaInicial()
	{
		InitializeComponent();

 
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
            Navigation.PushAsync(new Estoque());
        }
        catch (Exception ex)
        {
            DisplayAlert("Não encontrado", ex.Message, "OK");
        }
    }
}