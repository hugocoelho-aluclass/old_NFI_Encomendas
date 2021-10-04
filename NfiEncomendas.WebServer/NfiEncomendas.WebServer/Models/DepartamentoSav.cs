using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NfiEncomendas.WebServer.Models
{
    public class DepartamentoSav
    {
        [Key]
        public int IdDepartamentoSav { get; set; }

        public int NumDepartamentoSav { get; set; }

        public string NomeDepartamentoSav { get; set; }

        public bool Anulado { get; set; }
        public virtual List<Operadores> Operadores { get; set; }
    }
}