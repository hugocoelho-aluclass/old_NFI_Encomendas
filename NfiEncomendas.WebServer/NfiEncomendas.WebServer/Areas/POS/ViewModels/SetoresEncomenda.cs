
namespace NfiEncomendas.WebServer.Areas.POS.ViewModels.SetoresEncomenda
{
    public class SetorEncomenda
    {
        public SetorEncomenda()
        {
        }
        public int NumSetor { get; set; }
        public string Nome { get; set; }

        public bool Anulado { get; set; }
        public int Id { get; set; }
        public Models.SetorEncomendas ToModel()
        {
            Models.SetorEncomendas res = (new NfiEncomendas.WebServer.BusinessLogic.SetorEncomendasBL()).LerSetor(this.NumSetor);
            res.Nome = this.Nome;
            res.Anulado = this.Anulado;
            res.NumSetor = this.NumSetor;
            //res.IdSetor = this.Id;
            return res;
        }
    }


    public class PagGestaoSetoresEncomendaEdit
    {
        public SetorEncomenda Setor { get; set; }
        public int idProx { get; set; }
        public int idAnt { get; set; }

        public PagGestaoSetoresEncomendaEdit()
        {
            Setor = new SetorEncomenda();
            idProx = 999;
            idAnt = 0;
        }

        public void SetorParaVM(Models.SetorEncomendas item)
        {
            Setor.NumSetor = item.NumSetor;
            Setor.Nome = item.Nome;
            Setor.Anulado = item.Anulado;
        }
    }
}