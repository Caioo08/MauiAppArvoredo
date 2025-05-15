using MauiAppArvoredo.Models;
using SQLite;

namespace MauiAppArvoredo.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _connection;

        public SQLiteDatabaseHelper(string path) 
        {
            _connection = new SQLiteAsyncConnection(path);
            _connection.CreateTableAsync<Madeiras>().Wait();
        }

        public Task<int> Insert(Madeiras m) 
        { 
            return _connection.InsertAsync(m);
        }

        public Task<List<Madeiras>> Update(Madeiras m) 
        {
            string SQL = "UPDATE Madeiras SET Tipo=? WHERE id=?";

            return _connection.QueryAsync<Madeiras>(SQL, m.Tipo);
        }

        public Task<int> Delete(int id) 
        { 
            return _connection.Table<Madeiras>().DeleteAsync(i => i.id == id);
        }  

        public Task<List<Madeiras>> GetAll() 
        {
            return _connection.Table<Madeiras>().ToListAsync();
        } 

    }
}
