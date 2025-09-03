namespace MauiAppArvoredo;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app.db3");
        var db = new Helpers.DatabaseService(dbPath);

        // Se não houver usuários, abre o login
        var usuarioExiste = db.GetUsuarioAsync("admin@teste.com", "1234").Result;

        if (usuarioExiste == null)
        {
            // Vai para o login
            MainPage = new NavigationPage(new Inicio());
        }
        else
        {
            // Mantém a navegação padrão
            MainPage = new AppShell();
        }
    }
}