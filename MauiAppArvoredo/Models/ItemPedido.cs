using SQLite;

namespace MauiAppArvoredo.Models;

public class ItemPedido
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int PedidoId { get; set; } // FK para Pedido
    public string Nome { get; set; }
    public int Quantidade { get; set; }
}
