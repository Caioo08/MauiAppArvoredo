using SQLite;

namespace MauiAppArvoredo.Models
{
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Nome { get; set; }

        [MaxLength(200)]
        public string Senha { get; set; }
    }
}
