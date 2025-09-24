namespace MauiAppArvoredo;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
        ChecarUsuarioAsync();



    }
    public async void ChecarUsuarioAsync()
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app.db3");
        var db = new Helpers.DatabaseService(dbPath);
        var usuarioExiste = await db.GetUsuarioAsync("admin@teste.com", "1234");
    }
}