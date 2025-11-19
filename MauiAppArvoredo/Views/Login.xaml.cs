using MauiAppArvoredo.Helpers;
using MauiAppArvoredo.Models;
using MauiAppArvoredo.Services;

namespace MauiAppArvoredo;

public partial class Login : ContentPage
{
    private readonly DatabaseService _db;
    private readonly UsuarioApiService _apiService;

    public Login()
    {
        InitializeComponent();

        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app.db3");
        _db = new DatabaseService(dbPath);
        _apiService = new UsuarioApiService();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Validação básica
        if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(SenhaEntry.Text))
        {
            await DisplayAlert("Atenção", "Por favor, preencha email/login e senha", "OK");
            return;
        }

        // Desabilita o botão durante o processo
        var button = sender as Button;
        button.IsEnabled = false;
        button.Text = "AUTENTICANDO...";

        try
        {
            // Tenta autenticar pela API
            var resultado = await _apiService.AutenticarAsync(EmailEntry.Text, SenhaEntry.Text);

            if (resultado.sucesso)
            {
                // Salva usuário localmente para uso offline
                var usuarioLocal = new Usuario
                {
                    Nome = resultado.usuario.Nome,
                    Senha = resultado.usuario.Senha
                };

                // Verifica se já existe localmente
                var usuarioExistente = await _db.GetUsuarioAsync(EmailEntry.Text, SenhaEntry.Text);
                if (usuarioExistente == null)
                {
                    await _db.AddUsuarioAsync(usuarioLocal);
                }

                await DisplayAlert("✓ Sucesso", $"Bem-vindo(a), {resultado.usuario.Nome}!", "OK");

                // Navega para a tela principal
                await Navigation.PushAsync(new TelaInicial());
            }
            else
            {
                // Se falhar na API, tenta no banco local (modo offline)
                var usuarioLocal = await _db.GetUsuarioAsync(EmailEntry.Text, SenhaEntry.Text);

                if (usuarioLocal != null)
                {
                    await DisplayAlert("⚠️ Modo Offline", "Login realizado com credenciais locais. Algumas funcionalidades podem estar limitadas.", "OK");
                    await Navigation.PushAsync(new TelaInicial());
                }
                else
                {
                    await DisplayAlert("✗ Erro", resultado.erro ?? "Usuário ou senha inválidos", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            // Em caso de erro de conexão, tenta login local
            try
            {
                var usuarioLocal = await _db.GetUsuarioAsync(EmailEntry.Text, SenhaEntry.Text);

                if (usuarioLocal != null)
                {
                    await DisplayAlert("⚠️ Modo Offline", "Sem conexão com a internet. Login realizado localmente.", "OK");
                    await Navigation.PushAsync(new TelaInicial());
                }
                else
                {
                    await DisplayAlert("✗ Erro", "Não foi possível conectar ao servidor e não há credenciais salvas localmente.", "OK");
                }
            }
            catch
            {
                await DisplayAlert("✗ Erro", $"Erro ao fazer login: {ex.Message}", "OK");
            }
        }
        finally
        {
            // Reabilita o botão
            button.IsEnabled = true;
            button.Text = "ENTRAR";
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        // Validação básica
        if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(SenhaEntry.Text))
        {
            await DisplayAlert("Atenção", "Preencha email e senha para registrar", "OK");
            return;
        }

        // Solicita o nome do usuário
        string nome = await DisplayPromptAsync("Registro", "Digite seu nome completo:", "OK", "Cancelar", "Nome Completo", maxLength: 100);

        if (string.IsNullOrWhiteSpace(nome))
        {
            await DisplayAlert("Atenção", "Nome é obrigatório para o registro", "OK");
            return;
        }

        // Desabilita o botão durante o processo
        var button = sender as Button;
        button.IsEnabled = false;
        button.Text = "REGISTRANDO...";

        try
        {
            // Cria o DTO para enviar à API
            var novoUsuario = new UsuarioCreateDto
            {
                Login = EmailEntry.Text.Trim(),
                Email = EmailEntry.Text.Trim(),
                Senha = SenhaEntry.Text,
                Nome = nome.Trim(),
                NivelAcesso = 1, // Nível mobile
                Ativo = true
            };

            // Tenta criar na API
            var resultado = await _apiService.CriarUsuarioAsync(novoUsuario);

            if (resultado.sucesso)
            {
                // Salva também localmente
                var usuarioLocal = new Usuario
                {
                    Nome = resultado.usuario.Nome,
                    Senha = resultado.usuario.Senha
                };

                await _db.AddUsuarioAsync(usuarioLocal);

                await DisplayAlert("✓ Sucesso", "Usuário registrado com sucesso! Você já pode fazer login.", "OK");

                // Limpa os campos
                EmailEntry.Text = string.Empty;
                SenhaEntry.Text = string.Empty;
            }
            else
            {
                await DisplayAlert("✗ Erro", resultado.erro ?? "Não foi possível registrar o usuário", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("✗ Erro", $"Erro ao registrar usuário: {ex.Message}\n\nVerifique sua conexão com a internet.", "OK");
        }
        finally
        {
            // Reabilita o botão
            button.IsEnabled = true;
            button.Text = "REGISTRAR";
        }
    }

    private async void OnSairClicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Inicio());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}