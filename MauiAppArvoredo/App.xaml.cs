

using MauiAppArvoredo.Helpers;

namespace MauiAppArvoredo
{
    public partial class App : Application
    {
        public static DatabaseService Database { get; private set; }

        public App()
        {
            InitializeComponent();

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "estoque.db3");
            Database = new DatabaseService(dbPath);

            MainPage = new AppShell();



        
        }
    }
}
