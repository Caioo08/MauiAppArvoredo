using Microsoft.Maui.Graphics;

namespace MauiAppArvoredo.Models
{
    public class ProdutoModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string TipoNome { get; set; } // nome da madeira ou tipo
        public int Quantidade { get; set; }
        public int QuantidadeMin { get; set; }
        public string Unidade { get; set; }
        public double Valor { get; set; }
        public string StatusTexto { get; set; }
        public Color StatusColor { get; set; }
    }
}
