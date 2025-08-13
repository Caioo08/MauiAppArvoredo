using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _db.CreateTableAsync<EstoqueItem>().Wait();
        }

        public Task<int> SalvarItemAsync(EstoqueItem item)
        {
            if (item.Id != 0)
                return _db.UpdateAsync(item);
            else
                return _db.InsertAsync(item);
        }

        public Task<List<EstoqueItem>> GetItensAsync()
        {
            return _db.Table<EstoqueItem>().ToListAsync();
        }
    }
}
