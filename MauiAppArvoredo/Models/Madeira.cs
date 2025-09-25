using SQLite;

namespace MauiAppArvoredo.Models
{
    public class Madeira
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100), NotNull]
        public string Nome { get; set; } // Ex: Carvalho, Peroba
    }
}
