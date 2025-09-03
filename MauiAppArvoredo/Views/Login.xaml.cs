using MauiAppArvoredo.Helpers;
using MauiAppArvoredo.Models;

namespace MauiAppArvoredo;

public partial class Login : ContentPage
{
    private readonly DatabaseService _db;

    public Login()
    {
        InitializeComponent();

        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app.db3");
        _db = new DatabaseService(dbPath);
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var usuario = await _db.GetUsuarioAsync(EmailEntry.Text, SenhaEntry.Text);

        if (usuario != null)
        {
            await DisplayAlert("Sucesso", "Login realizado!", "OK");
            // Vai para a tela principal
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            await DisplayAlert("Erro", "Usuário ou senha inválidos", "OK");
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(SenhaEntry.Text))
        {
            await DisplayAlert("Erro", "Preencha email e senha", "OK");
            return;
        }

        var usuario = new Usuario
        {
            Nome = EmailEntry.Text, // Pode usar o próprio email como nome por enquanto
            Email = EmailEntry.Text,
            Senha = SenhaEntry.Text
        };

        try
        {
            await _db.AddUsuarioAsync(usuario);
            await DisplayAlert("Sucesso", "Usuário registrado!", "OK");
        }
        catch
        {
            await DisplayAlert("Erro", "Esse email já está registrado", "OK");
        }
    }
}
