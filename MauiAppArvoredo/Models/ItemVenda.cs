using SQLite;

public class ItemVenda
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int VendaId { get; set; } // chave estrangeira para Venda

    public string Produto { get; set; }

    public int Quantidade { get; set; }

    public double Valor { get; set; }
}
