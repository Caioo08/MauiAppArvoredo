using SQLite;

namespace MauiAppArvoredo.Models;

public class Pedido
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Cliente { get; set; }
    public string Status { get; set; } = "Pendente"; // Pendente ou Concluído
}
