namespace NfiEncomendas.WebServer.Areas.POS.ViewModels.Series
{
    public class Serie
    {
        public string NumSerie { get; set; }
        public string Nome { get; set; }
        public bool Inativa { get; set; }
        public bool SerieDefeito { get; set; }
        public int UltimoDoc { get; set; }


        public Serie()
        {
        }

        public Models.Series ToModel()
        {
            Models.Series res = (new NfiEncomendas.WebServer.BusinessLogic.SeriesBL()).LerSerie(this.NumSerie); //new Models.Operadores();
            res.NomeSerie = this.Nome;
            res.Inativa = this.Inativa;
            res.SerieDefeito = this.SerieDefeito;
            res.UltimoDoc = this.UltimoDoc;
            return res;
        }
    }


    public class PagGestaoSeriesEdit
    {
        public Serie Serie { get; set; }
        public string idProx { get; set; }
        public string idAnt { get; set; }

        public PagGestaoSeriesEdit()
        {
            Serie = new Serie();
            idProx = "";
            idAnt = "";
        }

        public void SerieParaVM(Models.Series item)
        {
            Serie.NumSerie = item.NumSerie;
            Serie.Nome = item.NomeSerie;
            Serie.Inativa = item.Inativa;
            Serie.SerieDefeito = item.SerieDefeito;
            Serie.UltimoDoc = item.UltimoDoc;
        }
    }
}