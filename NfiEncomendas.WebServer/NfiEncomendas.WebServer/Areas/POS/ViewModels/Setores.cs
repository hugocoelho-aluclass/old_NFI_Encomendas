
namespace NfiEncomendas.WebServer.Areas.POS.ViewModels.Setores
{
    public class Setor
    {
        public Setor()
        {
        }
        public int NumSetor { get; set; }
        public string Nome { get; set; }

        public bool Anulado { get; set; }
        public int Id { get; set; }
        public Models.Setor ToModel()
        {
            Models.Setor res = (new NfiEncomendas.WebServer.BusinessLogic.SetorBL()).LerSetor(this.NumSetor);
            res.Nome = this.Nome;
            res.Anulado = this.Anulado;
            res.NumSetor = this.NumSetor;
            //res.IdSetor = this.Id;
            return res;
        }
    }


    public class PagGestaoSetoresEdit
    {
        public Setor Setor { get; set; }
        public int idProx { get; set; }
        public int idAnt { get; set; }

        public PagGestaoSetoresEdit()
        {
            Setor = new Setor();
            idProx = 999;
            idAnt = 0;
        }

        public void SetorParaVM(Models.Setor item)
        {
            Setor.NumSetor = item.NumSetor;
            Setor.Nome = item.Nome;
            Setor.Anulado = item.Anulado;
        }
    }
}