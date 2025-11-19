using SQLite;
using MauiAppArvoredo.Models;
using MauiAppArvoredo.Views;

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

            // 🔥 NOVAS TABELAS DE VENDAS
            _db.CreateTableAsync<Venda>().Wait();
            _db.CreateTableAsync<ItemVenda>().Wait();
        }

        // ======== USUÁRIOS ========
        public Task<int> AddUsuarioAsync(Usuario usuario)
        {
            return _db.InsertAsync(usuario);
        }

        public Task<Usuario> GetUsuarioAsync(string email, string senha)
        {
            return _db.Table<Usuario>()
                      .Where(u => u.Senha == senha)
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

        // ======== 🔥 VENDAS (NOVO) ========

        /// <summary>
        /// Retorna todas as vendas ordenadas por data (mais recente primeiro)
        /// </summary>
        public Task<List<Venda>> GetVendasAsync()
        {
            return _db.Table<Venda>()
                      .OrderByDescending(v => v.DataCriacao)
                      .ToListAsync();
        }

        /// <summary>
        /// Busca uma venda específica por ID local
        /// </summary>
        public Task<Venda> GetVendaAsync(int id)
        {
            return _db.Table<Venda>()
                      .Where(v => v.Id == id)
                      .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Busca vendas pendentes de pagamento
        /// </summary>
        public Task<List<Venda>> GetVendasPendentesAsync()
        {
            return _db.Table<Venda>()
                      .Where(v => v.Pago == false)
                      .OrderByDescending(v => v.DataCriacao)
                      .ToListAsync();
        }

        /// <summary>
        /// Busca vendas já pagas
        /// </summary>
        public Task<List<Venda>> GetVendasPagasAsync()
        {
            return _db.Table<Venda>()
                      .Where(v => v.Pago == true)
                      .OrderByDescending(v => v.DataCriacao)
                      .ToListAsync();
        }

        /// <summary>
        /// Salva uma nova venda
        /// </summary>
        public Task<int> SaveVendaAsync(Venda venda)
        {
            return _db.InsertAsync(venda);
        }

        /// <summary>
        /// Atualiza uma venda existente
        /// </summary>
        public Task<int> UpdateVendaAsync(Venda venda)
        {
            return _db.UpdateAsync(venda);
        }

        /// <summary>
        /// Deleta uma venda
        /// </summary>
        public Task<int> DeleteVendaAsync(Venda venda)
        {
            return _db.DeleteAsync(venda);
        }

        /// <summary>
        /// Marca uma venda como paga
        /// </summary>
        public async Task<bool> MarcarVendaComoPagaAsync(int vendaId)
        {
            var venda = await GetVendaAsync(vendaId);
            if (venda == null) return false;

            venda.Pago = true;
            venda.DataPagamento = DateTime.Now;

            await UpdateVendaAsync(venda);
            return true;
        }

        // ======== 🔥 ITENS DE VENDA (NOVO) ========

        /// <summary>
        /// Retorna todos os itens de uma venda específica
        /// </summary>
        public Task<List<ItemVenda>> GetItensByVendaAsync(int vendaId)
        {
            return _db.Table<ItemVenda>()
                      .Where(i => i.VendaId == vendaId)
                      .ToListAsync();
        }

        /// <summary>
        /// Salva um item de venda
        /// </summary>
        public Task<int> SaveItemVendaAsync(ItemVenda item)
        {
            if (item.Id != 0)
                return _db.UpdateAsync(item);
            else
                return _db.InsertAsync(item);
        }

        /// <summary>
        /// Deleta um item de venda
        /// </summary>
        public Task<int> DeleteItemVendaAsync(ItemVenda item)
        {
            return _db.DeleteAsync(item);
        }

        /// <summary>
        /// Deleta todos os itens de uma venda
        /// </summary>
        public async Task<int> DeleteAllItensVendaAsync(int vendaId)
        {
            var itens = await GetItensByVendaAsync(vendaId);
            int count = 0;

            foreach (var item in itens)
            {
                count += await DeleteItemVendaAsync(item);
            }

            return count;
        }

        // ======== RELATÓRIOS E ESTATÍSTICAS ========

        /// <summary>
        /// Retorna o total de vendas realizadas
        /// </summary>
        public async Task<int> GetTotalVendasAsync()
        {
            var vendas = await GetVendasAsync();
            return vendas.Count;
        }

        /// <summary>
        /// Retorna o valor total de todas as vendas
        /// </summary>
        public async Task<double> GetValorTotalVendasAsync()
        {
            var vendas = await GetVendasAsync();
            return vendas.Sum(v => v.ValorTotal);
        }

        /// <summary>
        /// Retorna vendas de um período específico
        /// </summary>
        public async Task<List<Venda>> GetVendasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _db.Table<Venda>()
                           .Where(v => v.DataCriacao >= dataInicio && v.DataCriacao <= dataFim)
                           .OrderByDescending(v => v.DataCriacao)
                           .ToListAsync();
        }

        /// <summary>
        /// Retorna vendas do mês atual
        /// </summary>
        public Task<List<Venda>> GetVendasMesAtualAsync()
        {
            var inicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var fim = inicio.AddMonths(1).AddDays(-1);
            return GetVendasPorPeriodoAsync(inicio, fim);
        }
    }
}