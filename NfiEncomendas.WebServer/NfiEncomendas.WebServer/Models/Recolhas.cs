using System;
using System.ComponentModel.DataAnnotations;


namespace NfiEncomendas.WebServer.Models
{
    public class Recolhas
    {
        [Key]
        public int Id { get; set; }

        public DateTime DataPedidoRecolha { get; set; } = new DateTime(2010, 01, 01);
        public DateTime DataRecolha { get; set; } = new DateTime(2010, 01, 01);

        public DateTime DataChegadaPrevista { get; set; } = new DateTime(2010, 01, 01);

        public bool RecolhaCompleta { get; set; } = false;

        public string EstadoProduto { get; set; } = "";

        public EstadoRecolha EstadoRecolha { get; set; }

        public Recolhas()
        {
        }
    }
}