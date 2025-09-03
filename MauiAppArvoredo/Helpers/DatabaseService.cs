using SQLite;
using MauiAppArvoredo.Models;

namespace MauiAppArvoredo.Helpers
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _db;

        public DatabaseService(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<Usuario>().Wait();
        }

        // ======== USUÁRIOS ========
        public Task<int> AddUsuarioAsync(Usuario usuario)
        {
            return _db.InsertAsync(usuario);
        }

        public Task<Usuario> GetUsuarioAsync(string email, string senha)
        {
            return _db.Table<Usuario>()
                      .Where(u => u.Email == email && u.Senha == senha)
                      .FirstOrDefaultAsync();
        }
    }
}
