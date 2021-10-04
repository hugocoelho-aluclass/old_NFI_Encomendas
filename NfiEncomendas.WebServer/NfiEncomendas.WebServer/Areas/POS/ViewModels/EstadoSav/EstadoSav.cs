namespace NfiEncomendas.WebServer.Areas.POS.ViewModels.EstadoSav
{
    public class EstadoSav
    {
        public int NumEstadoSav { get; set; }
        public string Nome { get; set; }
        public bool Anulado { get; set; }
        public int SubEstado { get; set; } // 1 - branco, 2 - amarelo, 3-vermelho, 4 verde
        //1- em resolucao,2- aguarda por aprovacao 3- pendente, 4-fechado

        public bool MarcaEncerrado { get; set; }
        public bool PreSeleccionadoNaPesquisa { get; set; }

        public EstadoSav()
        {
        }

        public Models.EstadoSav ToModel()
        {
            Models.EstadoSav res = (new NfiEncomendas.WebServer.BusinessLogic.EstadoSavBL()).LerEstadoSav(this.NumEstadoSav); //new Models.Operadores();
            res.NomeEstadoSav = this.Nome;
            res.Anulado = this.Anulado;
            res.MarcaEncerrado = this.MarcaEncerrado;
            res.SubEstado = this.SubEstado;
            res.PreSeleccionadoNaPesquisa = this.PreSeleccionadoNaPesquisa;

            return res;
        }
    }


    public class PagGestaoEstadoSavEdit
    {
        public EstadoSav EstadoSav { get; set; }
        public int idProx { get; set; }
        public int idAnt { get; set; }

        public PagGestaoEstadoSavEdit()
        {
            EstadoSav = new EstadoSav();
            idProx = 999;
            idAnt = 0;
        }

        public void EstadoSavParaVM(Models.EstadoSav item)
        {
            EstadoSav.NumEstadoSav = item.IdEstadoSav;
            EstadoSav.Nome = item.NomeEstadoSav;
            EstadoSav.Anulado = item.Anulado;
            EstadoSav.SubEstado = item.SubEstado;
            EstadoSav.MarcaEncerrado = item.MarcaEncerrado;
            EstadoSav.PreSeleccionadoNaPesquisa = item.PreSeleccionadoNaPesquisa;
        }
    }
}

