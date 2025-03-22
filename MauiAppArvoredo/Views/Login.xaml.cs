namespace MauiAppArvoredo;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private void entrar_Clicked(object sender, EventArgs e)
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

    private void voltar_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Inicio());
        }
        catch(Exception ex)
        {
            DisplayAlert("Não encontrado", ex.Message, "OK");
        }
    }
}