using System;
using System.ComponentModel.DataAnnotations;

namespace NfiEncomendas.WebServer.Models
{
    public class EncomendaExtra
    {
        [Key]
        public int IdEncomendaExtra { get; set; }

        public int NumExtra { get; set; }

        public string Nome { get; set; }

        public bool Anulado { get; set; }

        public EncomendaExtra()
        {
            Anulado = false;
        }
    }
}