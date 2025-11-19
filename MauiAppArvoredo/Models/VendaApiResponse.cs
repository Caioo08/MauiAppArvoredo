using System;
using System.Collections.Generic;

namespace MauiAppArvoredo.Models
{
    public class VendaApiResponse
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Nome { get; set; } // cliente
        public double ValorTotal { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataPagamento { get; set; }
        public bool Pago { get; set; }
        public string Forma { get; set; }

        public List<VendaItemApiResponse> VendaE { get; set; } = new List<VendaItemApiResponse>();
    }

    public class VendaItemApiResponse
    {
        public string Tipo { get; set; } // Produto, Madeira ou Peça
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public double ValorVenda { get; set; }
        public double ValorTotal { get; set; }
    }
}
