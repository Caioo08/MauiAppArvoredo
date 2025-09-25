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

            // Tabelas existentes
            _db.CreateTableAsync<Usuario>().Wait();
            _db.CreateTableAsync<Madeira>().Wait();
            _db.CreateTableAsync<ItemMadeira>().Wait();

            // Tabelas de pedidos
            _db.CreateTableAsync<Pedido>().Wait();
            _db.CreateTableAsync<ItemPedido>().Wait();
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

        // ======== MADEIRAS ========
        public Task<List<Madeira>> GetMadeirasAsync()
        {
            return _db.Table<Madeira>().ToListAsync();
        }

        public Task<int> SaveMadeiraAsync(Madeira madeira)
        {
            if (madeira.Id != 0)
                return _db.UpdateAsync(madeira);
            else
                return _db.InsertAsync(madeira);
        }

        public Task<int> DeleteMadeiraAsync(Madeira madeira)
        {
            return _db.DeleteAsync(madeira);
        }

        // ======== ITENS DE MADEIRA ========
        public Task<List<ItemMadeira>> GetItensByMadeiraAsync(int madeiraId)
        {
            return _db.Table<ItemMadeira>()
                      .Where(i => i.MadeiraId == madeiraId)
                      .ToListAsync();
        }

        public Task<int> SaveItemMadeiraAsync(ItemMadeira item)
        {
            if (item.Id != 0)
                return _db.UpdateAsync(item);
            else
                return _db.InsertAsync(item);
        }

        public Task<int> DeleteItemMadeiraAsync(ItemMadeira item)
        {
            return _db.DeleteAsync(item);
        }

        // ======== PEDIDOS ========
        public Task<List<Pedido>> GetPedidosAsync()
        {
            return _db.Table<Pedido>().ToListAsync();
        }

        public Task<Pedido> GetPedidoAsync(int id)
        {
            return _db.Table<Pedido>()
                      .Where(p => p.Id == id)
                      .FirstOrDefaultAsync();
        }

        public Task<int> SavePedidoAsync(Pedido pedido)
        {
            return _db.InsertAsync(pedido);
        }

        public Task<int> UpdatePedidoAsync(Pedido pedido)
        {
            return _db.UpdateAsync(pedido);
        }

        public Task<int> DeletePedidoAsync(Pedido pedido)
        {
            return _db.DeleteAsync(pedido);
        }

        // ======== ITENS DE PEDIDO ========
        public Task<List<ItemPedido>> GetItensByPedidoAsync(int pedidoId)
        {
            return _db.Table<ItemPedido>()
                      .Where(i => i.PedidoId == pedidoId)
                      .ToListAsync();
        }

        public Task<int> SaveItemPedidoAsync(ItemPedido item)
        {
            if (item.Id != 0)
                return _db.UpdateAsync(item);
            else
                return _db.InsertAsync(item);
        }

        public Task<int> DeleteItemPedidoAsync(ItemPedido item)
        {
            return _db.DeleteAsync(item);
        }
    }
}
