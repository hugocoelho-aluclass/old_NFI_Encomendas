using System;
using System.ComponentModel.DataAnnotations;

namespace NVidrosEncomendas.WebServer.Models
{
    public class Relatorios
    {
        [Key]
        public int Id { get; set; }

        public string NomeUtilizador { get; set; }
        public string HtmlQuery { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public string NomeFicheiro { get; set; }
        public string TipoFicheiro { get; set; }
        public string UniqueId { get; set; }
        public DateTime DataGerado { get; set; }

        public Relatorios()
        {

        }
    }
}