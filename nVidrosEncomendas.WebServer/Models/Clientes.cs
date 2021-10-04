﻿using System.ComponentModel.DataAnnotations;

namespace NVidrosEncomendas.WebServer.Models
{
    public class Anexos
    {
        [Key]
        public int Id { get; set; }
        public string NomeFicheiro { get; set; }

        public bool Anulado { get; set; }
        public Anexos()
        {
            Anulado = false;
        }
    }
}