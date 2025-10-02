using MauiAppArvoredo.Helpers;
using MauiAppArvoredo.Models;

namespace MauiAppArvoredo;

public partial class App : Application
{
    // Banco principal (estoque)
    static DatabaseService database;
    public static DatabaseService Database
    {
        get
        {
            if (database == null)
            {
                string dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "maderapp.db3");
                database = new DatabaseService(dbPath);
            }
            return database;
        }
    }

    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();

        SeedMadeiras();      // popula as madeiras padrão
        ChecarUsuarioAsync(); // checa admin
        RemoverPauBrasil();
    }

    private async void SeedMadeiras()
    {
        var padrao = new string[] { "Eucalipto", "Peroba", "Jatobá", "Nogueira" };
        var madeiras = await Database.GetMadeirasAsync();

        foreach (var nome in padrao)
        {
            if (!madeiras.Any(m => m.Nome == nome))
                await Database.SaveMadeiraAsync(new Madeira { Nome = nome });
        }
    }
    private async void RemoverPauBrasil()
    {
        var madeiras = await Database.GetMadeirasAsync();
        var pauBrasil = madeiras.FirstOrDefault(m => m.Nome == "Pau-Brasil");
        if (pauBrasil != null)
        {
            await Database.DeleteMadeiraAsync(pauBrasil);
        }
    }
    private async void ChecarUsuarioAsync()
    {
        string dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "app.db3");

        var db = new DatabaseService(dbPath);

        var usuarioExiste = await db.GetUsuarioAsync("admin@teste.com", "1234");
        if (usuarioExiste != null)
        {
            Console.WriteLine("Usuário admin encontrado!");
        }
        else
        {
            Console.WriteLine("Usuário admin NÃO encontrado.");
        }
    }
}
