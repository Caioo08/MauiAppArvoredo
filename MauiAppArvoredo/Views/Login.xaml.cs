namespace MauiAppArvoredo;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
    }

    public void entrar_Clicked(object sender, EventArgs e)
    {
        string usuario = username.Text;
        string senha = password.Text;

        if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
        {
            DisplayAlert("Erro", "Preencha todos os campos", "OK");
        }
        else if (usuario == "caio" && senha == "123")
        {
            // Navegue para a próxima página
            Navigation.PushAsync(new TelaInicial());
        }
        else if (usuario == "pp" && senha == "22")
        {
            // Navegue para a próxima página
            Navigation.PushAsync(new TelaInicial());
        }
        else
        {
            DisplayAlert("Erro", "Usuário ou senha incorretos", "OK");
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