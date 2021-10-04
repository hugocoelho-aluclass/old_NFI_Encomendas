namespace NfiEncomendas.WebServer.Areas.POS.ViewModels.EstadoRecolha
{
    public class EstadoRecolha
    {
        public int Id { get; set; }

        public string NomeEstado { get; set; }
        public string Cor { get; set; } = "#ffffff";

        public bool EstadoFechaRecolha { get; set; } = false;
        public bool Anulado { get; set; }

        public EstadoRecolha()
        {
        }

        public Models.EstadoRecolha ToModel()
        {
            Models.EstadoRecolha res = (new NfiEncomendas.WebServer.BusinessLogic.EstadoRecolhaBL()).Ler(this.Id);
            res.NomeEstado = this.NomeEstado;
            res.Cor = this.Cor;
            res.Anulado = this.Anulado;
            res.EstadoFechaRecolha = this.EstadoFechaRecolha;
            return res;
        }
    }


    public class PagGestaoEstadoRecolhaEdit
    {
        public EstadoRecolha EstadoRecolha { get; set; }


        public PagGestaoEstadoRecolhaEdit()
        {
            EstadoRecolha = new EstadoRecolha();

        }

        public void EstadoRecolhaParaVM(Models.EstadoRecolha item)
        {
            EstadoRecolha.Id = item.Id;
            EstadoRecolha.NomeEstado = item.NomeEstado;
            EstadoRecolha.Anulado = item.Anulado;
            EstadoRecolha.EstadoFechaRecolha = item.EstadoFechaRecolha;
            EstadoRecolha.Cor = item.Cor;

        }
    }
}