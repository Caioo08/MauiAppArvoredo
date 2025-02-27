namespace MauiAppArvoredo;

public partial class Inicio : ContentPage
{
	public Inicio()
	{
		InitializeComponent();
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

    private void sincronizar_Clicked(object sender, EventArgs e)
    {

    }

    private void configuracoes_Clicked(object sender, EventArgs e)
    {

    }
}