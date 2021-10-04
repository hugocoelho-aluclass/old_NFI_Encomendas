using System.ComponentModel.DataAnnotations;


namespace NVidrosEncomendas.WebServer.Models
{
    public class EstadoSav
    {
        [Key]
        public int IdEstadoSav { get; set; }

        //public int NumEstadoEncomenda { get; set; }

        public string NomeEstadoSav { get; set; }

        public int SubEstado { get; set; } // 1 - branco, 2 - amarelo, 3-vermelho, 4 verde
        //1- em resolucao,2- aguarda por aprovacao 3- pendente, 4-fechado

        public bool MarcaEncerrado { get; set; }
        public bool PreSeleccionadoNaPesquisa { get; set; }

        public bool Anulado { get; set; }
    }
}