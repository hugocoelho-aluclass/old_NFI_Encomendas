

namespace NfiEncomendas.WebServer.Areas.POS.ViewModels.TipoEncomendas
{
    public class TipoEncomenda
    {
        public int NumTipoEncomenda { get; set; }
        public string Nome { get; set; }
        public string Setor { get; set; }
        public bool Anulado { get; set; }
        public int SetorId { get; set; }

        public int? CapacidadePrix { get; set; }

        public int? CapacidadeWis { get; set; }

        public int? CapacidadeResto { get; set; }


        public TipoEncomenda()
        {
        }

        //public Models.TipoEncomendas ToModel(Models.AppDbContext db)
        //{
        //    Models.TipoEncomendas res = (new NfiEncomendas.WebServer.BusinessLogic.TipoEncomendasBL(db)).LerTipoEncomenda(this.NumTipoEncomenda); //new Models.Operadores();
        //    BusinessLogic.TipoEncomendasBL setorBl = new BusinessLogic.TipoEncomendasBL(db);



        //    res.NomeTipoEncomenda = this.Nome;
        //    res.Anulado = this.Anulado;
        //    res.NumTipoEncomenda = this.NumTipoEncomenda;
        //   // res.SetorEncomenda = this.Setor;            
        //    if (this.SetorId != 0)
        //    {
        //        res.SetorEncomenda = setorBl.LerSetor(this.SetorId);
        //            //this.NumTipoEncomenda = enc.TipoEncomenda.NumTipoEncomenda;

        //    }
        //    else
        //    {
        //        res.SetorEncomenda = null;
        //    }
        //    //res.SetorEncomenda =    this.SetorId
        //   // res.Setor

        //    return res;
        //}

        public Models.TipoEncomendas ToModel(Models.AppDbContext db)
        {
            Models.TipoEncomendas res = (new NfiEncomendas.WebServer.BusinessLogic.TipoEncomendasBL(db)).LerTipoEncomenda(this.NumTipoEncomenda);
            res.NomeTipoEncomenda = this.Nome;
            res.Anulado = this.Anulado;
            res.NumTipoEncomenda = this.NumTipoEncomenda;
            res.SetorEncomenda = (new NfiEncomendas.WebServer.BusinessLogic.SetorEncomendasBL(db)).LerSetor(this.SetorId);
            res.CapacidadePrix = this.CapacidadePrix;
            res.CapacidadeWis= this.CapacidadeWis;
            res.CapacidadeResto = this.CapacidadeResto;
            //res = this.InfoExtra;
            return res;
        }
    }


    public class PagGestaoTipoEncomendasEdit
    {
        public TipoEncomenda TipoEncomenda { get; set; }
        public int idProx { get; set; }
        public int idAnt { get; set; }

        public PagGestaoTipoEncomendasEdit()
        {
            TipoEncomenda = new TipoEncomenda();
            idProx = 999;
            idAnt = 0;
        }

        public void TipoEncomendaParaVM(Models.TipoEncomendas item)
        {
            TipoEncomenda.NumTipoEncomenda = item.NumTipoEncomenda;
            TipoEncomenda.Nome = item.NomeTipoEncomenda;
            TipoEncomenda.Anulado = item.Anulado;
            TipoEncomenda.Setor = item.SetorEncomenda != null ? item.SetorEncomenda.Nome : "";
            TipoEncomenda.SetorId = item.SetorEncomenda != null ? item.SetorEncomenda.IdSetorEncomenda : -1;
            TipoEncomenda.CapacidadePrix = item.CapacidadePrix;
            TipoEncomenda.CapacidadeWis = item.CapacidadeWis;
            TipoEncomenda.CapacidadeResto = item.CapacidadeResto;
        }
    }
}