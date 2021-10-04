
using System;

namespace NfiEncomendas.WebServer.Areas.POS.ViewModels.Recolhas
{
    public class Recolha
    {
        public int Id { get; set; }

        public DateTime DataPedidoRecolha { get; set; } = new DateTime(2010, 01, 01);
        //public DateTime dprString = 
        //private DateTime _ataPedidoRecolha;

        //public DateTime MyProperty
        //{
        //    get { return myVar; }
        //    set { myVar = value; }
        //}


        public DateTime DataRecolha { get; set; } = new DateTime(2010, 01, 01);

        public DateTime DataChegadaPrevista { get; set; } = new DateTime(2010, 01, 01);

        public bool RecolhaCompleta { get; set; } = false;

        public Models.EstadoRecolha EstadoRecolha { get; set; }

        public string EstadoProduto { get; set; } = "";

        public Recolha()
        {

        }
    }
    public class RecolhaLinha
    {


        public string DataPedidoRecolha { get; set; } = "-";
        public string DataRecolha { get; set; } = "-";

        public string DataChegadaPrevista { get; set; } = "-";

        public bool RecolhaCompleta { get; set; } = false;

        public string EstadoRecolha { get; set; }
        public string EstadoRecolhaCor { get; set; }

        public RecolhaLinha()
        {

        }

        public void AfterMap()
        {
            if (DataPedidoRecolha == "2010-01-01") DataPedidoRecolha = "";
            if (DataRecolha == "2010-01-01") DataRecolha = "";
            if (DataChegadaPrevista == "2010-01-01") DataChegadaPrevista = "";


        }
    }
}