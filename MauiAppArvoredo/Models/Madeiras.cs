using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MauiAppArvoredo.Models
{
    public class Madeiras
    {
        string _tipo;

        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string Tipo {
            get => _tipo;
            set
            {
                if (value == null)
                    throw new Exception("Nome inválido");

                _tipo = value;
            }
        }

    }
}
