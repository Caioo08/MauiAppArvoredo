using System;
using System.Collections.Generic;
using Microsoft.Maui.Graphics;

namespace MauiAppArvoredo.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public int ApiId { get; set; }
        public string NomeCliente { get; set; }
        public double ValorTotal { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataPagamento { get; set; }
        public bool Pago { get; set; }
        public string FormaPagamento { get; set; }
        public bool Sincronizado { get; set; }

        // Lista de itens
        public List<ItemVenda> Itens { get; set; } = new List<ItemVenda>();

        // =======================
        // Propriedades calculadas
        // =======================
        public string ValorFormatado => $"R$ {ValorTotal:N2}";
        public string StatusTexto => Pago ? "Pago" : "Pendente";
        public Color StatusColor => Pago ? Colors.Green : Colors.Orange;
        public string DataFormatada => DataCriacao.ToString("dd/MM/yyyy HH:mm");
    }

    public class VendaCreateDto
    {
        public string NomeCliente { get; set; }
        public double ValorTotal { get; set; }
        public string Descricao { get; set; }
        public string FormaPagamento { get; set; }
    }
}
