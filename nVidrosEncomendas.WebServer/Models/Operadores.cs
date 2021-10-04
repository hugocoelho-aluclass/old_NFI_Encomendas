using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NVidrosEncomendas.WebServer.Models
{
    public class Operadores
    {
        public Operadores()
        {
            ImagemAvatar = "";
            DepartamentosSav = new List<DepartamentoSav>();
            DashboardDesde = new DateTime(2020, 01, 01);
        }

        [Key]
        public int UtilizadorId { get; set; }

        public virtual string AspIdentityId { get; set; }

        [Required]
        [StringLength(100)]
        public string NomeCompleto { get; set; }

        [Required]
        [StringLength(15)]
        public string NomeLogin { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(50)]
        public string ImagemAvatar { get; set; }

        [Required]
        public string Password { get; set; }

        public bool Ativo { get; set; }
        public bool Admin { get; set; }

        public bool Anulado { get; set; }

        public bool Sav { get; set; }
        public bool AdminSav { get; set; }

        public bool Comercial { get; set; }

        public virtual List<DepartamentoSav> DepartamentosSav { get; set; }

        public virtual Operadores CriadoPor { get; set; }
        public DateTime CriadoData { get; set; }

        public virtual Operadores EditadoPor { get; set; }
        public DateTime EditadoData { get; set; }

        public DateTime DashboardDesde { get; set; }

    }
}