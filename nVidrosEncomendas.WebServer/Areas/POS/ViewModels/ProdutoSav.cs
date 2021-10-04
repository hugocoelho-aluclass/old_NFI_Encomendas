namespace NVidrosEncomendas.WebServer.Areas.POS.ViewModels.ProdutoSavs
{
    public class ProdutoSav
    {
        public int NumProdutoSav { get; set; }
        public string Nome { get; set; }
        public bool Anulado { get; set; }

        public ProdutoSav()
        {
        }

        public Models.ProdutoSav ToModel()
        {
            Models.ProdutoSav res = (new NVidrosEncomendas.WebServer.BusinessLogic.ProdutoSavsBL()).LerProdutoSav(this.NumProdutoSav); //new Models.Operadores();
            res.NomeProdutoSav = this.Nome;
            res.Anulado = this.Anulado;
            res.NumProdutoSav = this.NumProdutoSav;

            return res;
        }
    }


    public class PagGestaoProdutoSavsEdit
    {
        public ProdutoSav ProdutoSav { get; set; }
        public int idProx { get; set; }
        public int idAnt { get; set; }

        public PagGestaoProdutoSavsEdit()
        {
            ProdutoSav = new ProdutoSav();
            idProx = 999;
            idAnt = 0;
        }

        public void ProdutoSavParaVM(Models.ProdutoSav item)
        {
            ProdutoSav.NumProdutoSav = item.NumProdutoSav;
            ProdutoSav.Nome = item.NomeProdutoSav;
            ProdutoSav.Anulado = item.Anulado;
        }
    }
}