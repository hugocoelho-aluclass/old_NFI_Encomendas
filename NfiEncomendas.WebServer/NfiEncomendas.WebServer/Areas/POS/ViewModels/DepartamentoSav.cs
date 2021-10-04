namespace NfiEncomendas.WebServer.Areas.POS.ViewModels.DepartamentoSavs
{
    public class DepartamentoSav
    {
        public int NumDepartamentoSav { get; set; }
        public string Nome { get; set; }
        public bool Anulado { get; set; }

        public DepartamentoSav()
        {
        }

        public Models.DepartamentoSav ToModel()
        {
            Models.DepartamentoSav res = (new NfiEncomendas.WebServer.BusinessLogic.DepartamentoSavsBL()).LerDepartamentoSav(this.NumDepartamentoSav); //new Models.Operadores();
            res.NomeDepartamentoSav = this.Nome;
            res.Anulado = this.Anulado;
            res.NumDepartamentoSav = this.NumDepartamentoSav;

            return res;
        }
    }


    public class PagGestaoDepartamentoSavsEdit
    {
        public DepartamentoSav DepartamentoSav { get; set; }
        public int idProx { get; set; }
        public int idAnt { get; set; }

        public PagGestaoDepartamentoSavsEdit()
        {
            DepartamentoSav = new DepartamentoSav();
            idProx = 999;
            idAnt = 0;
        }

        public void DepartamentoSavParaVM(Models.DepartamentoSav item)
        {
            DepartamentoSav.NumDepartamentoSav = item.NumDepartamentoSav;
            DepartamentoSav.Nome = item.NomeDepartamentoSav;
            DepartamentoSav.Anulado = item.Anulado;
        }
    }
}