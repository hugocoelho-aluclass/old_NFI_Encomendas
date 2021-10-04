
namespace NVidrosEncomendas.WebServer.Areas.POS.ViewModels.ExtraVidros
{
    public class VidrosExtra
    {
        public VidrosExtra()
        {
        }
        public int Num { get; set; }
        public string Nome { get; set; }

        public bool Anulado { get; set; }
        public int Id { get; set; }
        public Models.VidrosExtra ToModel()
        {
            Models.VidrosExtra res = (new BusinessLogic.VidrosExtraBL()).Ler(this.Id);
            res.Nome = this.Nome;
            res.Anulado = this.Anulado;
            res.Num = this.Num;           
            return res;
        }
    }


    public class PagEdit
    {
        public VidrosExtra Item { get; set; }

        public PagEdit()
        {
            Item = new VidrosExtra();
        }

        public void ToVm(Models.VidrosExtra item)
        {
            Item.Nome = item.Nome;
            Item.Id = item.Id;
            Item.Num = item.Num;
        }
    }
}