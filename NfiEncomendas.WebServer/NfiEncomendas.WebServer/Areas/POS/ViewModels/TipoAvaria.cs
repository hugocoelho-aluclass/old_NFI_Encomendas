namespace NfiEncomendas.WebServer.Areas.POS.ViewModels.TipoAvarias
{
    public class TipoAvaria
    {
        public int NumTipoAvaria { get; set; }
        public string Nome { get; set; }

        public bool Anulado { get; set; }
        public bool InfoExtra { get; set; }
        public TipoAvaria()
        {
        }

        public Models.TipoAvarias ToModel()
        {
            Models.TipoAvarias res = (new NfiEncomendas.WebServer.BusinessLogic.TipoAvariasBL()).LerTipoAvaria(this.NumTipoAvaria); //new Models.Operadores();
            res.NomeTipoAvaria = this.Nome;
            res.Anulado = this.Anulado;
            res.NumTipoAvaria = this.NumTipoAvaria;
            res.InfoExtra = this.InfoExtra;
            return res;
        }
    }


    public class PagGestaoTipoAvariasEdit
    {
        public TipoAvaria TipoAvaria { get; set; }
        public int idProx { get; set; }
        public int idAnt { get; set; }

        public PagGestaoTipoAvariasEdit()
        {
            TipoAvaria = new TipoAvaria();
            idProx = 999;
            idAnt = 0;
        }

        public void TipoAvariaParaVM(Models.TipoAvarias item)
        {
            TipoAvaria.NumTipoAvaria = item.NumTipoAvaria;
            TipoAvaria.Nome = item.NomeTipoAvaria;
            TipoAvaria.Anulado = item.Anulado;
            TipoAvaria.InfoExtra = item.InfoExtra;

        }
    }
}