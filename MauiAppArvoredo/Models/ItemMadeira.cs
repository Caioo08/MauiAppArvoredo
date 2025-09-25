using SQLite;

namespace MauiAppArvoredo.Models
{
    public class ItemMadeira
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int MadeiraId { get; set; } // FK -> Madeira

        public string Formato { get; set; } // Ripa, Tábua, Viga
        public string Tamanho { get; set; } // Ex: 2m x 10cm
        public int Quantidade { get; set; }
    }
}
