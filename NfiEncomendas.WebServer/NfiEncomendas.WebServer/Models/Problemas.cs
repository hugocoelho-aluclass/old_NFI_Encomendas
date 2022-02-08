using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NfiEncomendas.WebServer.Models
{
    public class Problemas
    {
        [Key]
        public int IdProblema { get; set; }

        [StringLength(50)]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string DescricaoCausa { get; set; }

        public virtual DepartamentoSav Departamento { get; set; }

        public string Acompanhamento { get; set; }

        public string AcaoImplementar { get; set; }

        public DateTime DataCriacao { get; set; }

        public int? Eficacia { get; set; }

        public string AvaliacaoEficacia { get; set; }

        public bool Fechado { get; set; }

        public DateTime? DataAvaliacao { get; set; }

        public int? IdAnterior { get; set; }


        public Problemas()
        {
            
        }
    }
}