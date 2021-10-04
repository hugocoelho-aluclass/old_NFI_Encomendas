
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs
{
    public class SavGet
    {
        public int Id { get; set; }
        public string NomeSerie { get; set; }
        public int NumDoc { get; set; }
        public DateTime DataSav { get; set; }
        public int NumCliente { get; set; }
        public int ProdutoNum { get; set; }
        public int TipoAvariaNum { get; set; }
        public int DepartamentoNum { get; set; }

        public string TipoAvariaExtra { get; set; }

        public string DescricaoSav { get; set; }
        public string Causa { get; set; }
        public string AcaoImplementar { get; set; }


        public int EstadoSavNum { get; set; }
        public DateTime? DataEstado { get; set; }

        public string NotasAdicionais { get; set; }

        public DateTime? DataRespostaAoCliente { get; set; }
        public string RespostaAoCliente { get; set; }

        public string CriadoPor { get; set; }

        public DateTime? CriadoData { get; set; }

        public bool DireitoNaoConformidade { get; set; }

        public bool NovaSav { get; set; }

        public bool MarcarResolvida { get; set; }
        public DateTime? DataResolvida { get; set; }
        public DateTime? DataExpedicao { get; set; }

        public string NotasResolvida { get; set; }
        public string Ref { get; set; }
        public List<IdNome> Anexos { get; set; }

        public int SemanaEntrega { get; set; }
        public bool Anulada { get; set; }

        public double? Custos { get; set; }

        public double? CustosTransporte { get; set; }

        public string CustosDescricao { get; set; }

        public bool TemRecolha { get; set; }


        public Recolhas.Recolha Recolha { get; set; } = new Recolhas.Recolha();
        // public Setores.Setor Setor { get; set; } = new Setores.Setor();
        public int SetorId { get; set; }


    }

    public class Savs
    {

        public int Id { get; set; }
        public DateTime DataSav { get; set; }
        public int NumCliente { get; set; }
        public int ProdutoNum { get; set; }
        public int TipoAvariaNum { get; set; }
        public string TipoAvariaExtra { get; set; }
        public int DepartamentoNum { get; set; }

        public string DescricaoSav { get; set; }
        public string Causa { get; set; }
        public string AcaoImplementar { get; set; }

        public int EstadoSavNum { get; set; }
        public DateTime? DataEstado { get; set; }

        public string NotasAdicionais { get; set; }

        public DateTime? DataRespostaAoCliente { get; set; }
        public string RespostaAoCliente { get; set; }

        public bool DireitoNaoConformidade { get; set; }

        public bool NovaSav { get; set; }
        public bool Anulada { get; set; }
        public List<IdNome> Anexos { get; set; }

        public bool MarcarResolvida { get; set; }
        public DateTime DataResolvida { get; set; }

        public DateTime? DataExpedicao { get; set; }

        public string NotasResolvida { get; set; }
        public string NomeSerie { get; set; }

        public string Ref { get; set; }
        public int SemanaEntrega { get; set; }
        public double? Custos { get; set; }
        public double? CustosTransporte { get; set; }

        public string CustosDescricao { get; set; }

        public bool TemRecolha { get; set; } = false;

        public ViewModels.Recolhas.Recolha Recolha { get; set; }
        public int SetorId { get; set; }


        public Savs()
        {
        }

        public Models.Savs ToModel(Models.AppDbContext db)
        {
            BusinessLogic.SavsBL encBl = new NVidrosEncomendas.WebServer.BusinessLogic.SavsBL(db);
            BusinessLogic.EstadoSavBL estBl = new BusinessLogic.EstadoSavBL(db);
            BusinessLogic.TipoAvariasBL tencBl = new BusinessLogic.TipoAvariasBL(db);
            BusinessLogic.ClientesBL clBl = new BusinessLogic.ClientesBL(db);
            BusinessLogic.ProdutoSavsBL prodBl = new BusinessLogic.ProdutoSavsBL(db);
            BusinessLogic.DepartamentoSavsBL depBl = new BusinessLogic.DepartamentoSavsBL(db);
            BusinessLogic.AnexosBL anexoBl = new BusinessLogic.AnexosBL(db);
            BusinessLogic.SeriesBL serieBl = new BusinessLogic.SeriesBL(db);
            BusinessLogic.RecolhaBL recolhaBl = new BusinessLogic.RecolhaBL(db);
            BusinessLogic.SetorBL setorBl = new BusinessLogic.SetorBL(db);

            if (this.NovaSav) this.Id = -1;

            Models.Savs res = encBl.LerSav(this.Id).Key; //new Models.Operadores();
            Mapper.Map<ViewModels.Savs.Savs, Models.Savs>(this, res);

            res.Id = this.Id;
            if (res.Cliente == null || res.Cliente.NumCliente != this.NumCliente)
            {
                res.Cliente = clBl.LerCliente(this.NumCliente);
            }
            if (res.Produto == null || res.Produto.NumProdutoSav != this.ProdutoNum)
            {
                res.Produto = prodBl.LerProdutoSav(this.ProdutoNum);
            }
            if (res.TipoAvaria == null || res.TipoAvaria.NumTipoAvaria != this.TipoAvariaNum)
            {
                res.TipoAvaria = tencBl.LerTipoAvaria(this.TipoAvariaNum);
            }
            if (res.Departamento == null || res.Departamento.NumDepartamentoSav != this.TipoAvariaNum)
            {
                res.Departamento = depBl.LerDepartamentoSav(this.DepartamentoNum);
            }
            if (res.Estado == null || res.Estado.IdEstadoSav != this.EstadoSavNum)
            {
                res.Estado = estBl.LerEstadoSav(this.EstadoSavNum);
            }
            if (res.SerieDoc == null || res.SerieDoc.NomeSerie != this.NomeSerie)
            {
                res.SerieDoc = serieBl.LerSerie(this.NomeSerie);
            }

            if (res.Setor != null)
            {
                res.Setor = setorBl.LerSetor(this.SetorId);
            }


            res.TemRecolha = this.TemRecolha;
            if (this.Recolha != null && this.Recolha.Id != 0)
            {
                Mapper.Map<ViewModels.Recolhas.Recolha, Models.Recolhas>(this.Recolha, res.Recolha);
                Models.Recolhas r = res.Recolha;
                recolhaBl.AtualizaRecolhas(res.Recolha, ref r);
                res.Recolha = r;
            }

            //if (this.Setor && )

            if (res.Anexos == null) res.Anexos = new List<Models.Anexos>();
            res.Anexos.Clear();
            foreach (var item in this.Anexos)
            {
                res.Anexos.Add(anexoBl.LerAnexo(item.Id));
            }
            return res;
        }
    }

    public class Compras
    {
        public int IdCompraSavs { get; set; }
        public string NotasFornecedor { get; set; }
        public string Material { get; set; }
        public int LinhaCompra { get; set; }
        public DateTime? DataEntrega { get; set; }
        public DateTime? DataPedido { get; set; }
    }


    public class Cliente
    {
        public int NumCliente { get; set; }
        public string NomeCliente { get; set; }
    }


    public class Serie
    {
        public string NumSerie { get; set; }
        public int UltDoc { get; set; }
        public bool SerieDefeito { get; set; }
    }

    public class PagEditSav
    {

        public SavGet Sav { get; set; }

        public IEnumerable<IdNome> Clientes { get; set; }
        public IEnumerable<IdNome> Departamentos { get; set; }
        public IEnumerable<IdNome> EstadoSav { get; set; }
        public IEnumerable<IdNome> TipoAvaria { get; set; }
        public IEnumerable<IdNome> ProdutoSav { get; set; }
        public IEnumerable<IdNome> Setores { get; set; }

        public List<Serie> Series { get; set; }

        public IEnumerable<IdNomeText> EstadosRecolha { get; set; }

        public PagEditSav()
        {

        }
    }

    public class PagSavPesquisa
    {

        public IEnumerable<IdNome> Clientes { get; set; }
        public IEnumerable<IdNomePreSelect> Departamentos { get; set; }
        public IEnumerable<IdNomePreSelect> EstadoSav { get; set; }
        public IEnumerable<IdNome> EstadoRecolha { get; set; }
        public IEnumerable<IdNome> TipoAvaria { get; set; }
        public IEnumerable<IdNome> ProdutoSav { get; set; }
        public IEnumerable<IdNome> Setores { get; set; }


        public List<Serie> Series { get; set; }

        public PagSavPesquisa()
        {
            Series = new List<Serie>();
        }
    }

    public class PagSavPesquisaParam
    {

        public List<IdNome> Clientes { get; set; } = new List<IdNome>();

        public string Serie { get; set; } = "";

        public bool NumDocAteBool { get; set; } = false;
        public int NumDocAteValue { get; set; }

        public bool NumDocDesdeBool { get; set; } = false;
        public int NumDocDesdeValue { get; set; }

        public bool NDiasAteBool { get; set; } = false;
        public int NDiasAteValue { get; set; }

        public bool NDiasDesdeBool { get; set; } = false;
        public int NDiasDesdeValue { get; set; }

        public bool DataEntradaAteBool { get; set; } = false;
        public DateTime DataEntradaAteValue { get; set; }

        public bool DataEntradaDesdeBool { get; set; }
        public DateTime DataEntradaDesdeValue { get; set; }

        public string Ref { get; set; }

        public bool DataResolvidoAteBool { get; set; } = false;
        public DateTime DataResolvidoAteValue { get; set; }

        public bool DataResolvidoDesdeBool { get; set; } = false;
        public DateTime DataResolvidoDesdeValue { get; set; }
        public List<IdNome> Departamentos { get; set; } = new List<IdNome>();
        public List<IdNome> EstadoSav { get; set; } = new List<IdNome>();
        public List<IdNome> EstadoRecolha { get; set; } = new List<IdNome>();
        public List<IdNome> Produtos { get; set; } = new List<IdNome>();

        public List<IdNome> TipoAvaria { get; set; } = new List<IdNome>();
        public List<IdNome> Setor { get; set; } = new List<IdNome>();


        public int OrdemPesquisa { get; set; } = 0;

        public bool SemanaEntregaAteBool { get; set; } = false;
        public int SemanaEntregaAteValue { get; set; }

        public bool SemanaEntregaDesdeBool { get; set; } = false;
        public int SemanaEntregaDesdeValue { get; set; }

        public bool Recolha { get; set; } = false;


        public PagSavPesquisaParam()
        {
        }
    }

    public class EstadoSav
    {
        public int NumEstado { get; set; }
        public string Nome { get; set; }
    }

    public class TipoAvaria
    {
        public int Num { get; set; }
        public string Nome { get; set; }
    }
    public class SavsLinha2
    {
        public int Id { get; set; }
        public string NomeSerie { get; set; }
        public int NumDoc { get; set; }

        public string SerieNum
        {
            get
            {
                return NomeSerie + "/" + NumDoc.ToString().PadLeft(6, '0');

            }
        }

        public string QuemColocou { get; set; }
        public string NomeCliente { get; set; }
        public string NomeDepartamento { get; set; }
        public string NomeProduto { get; set; }

        public string NomeTipoAvaria { get; set; }
        public string NomeEstado { get; set; }
        public bool EstadoResolvido { get; set; }
        public string CssEstado { get; set; }
        public string DataEntrada { get; set; }
        public string DataResposta { get; set; }
        public string DataExpedicao { get; set; }

        public string DataResolvido { get; set; }
        public bool DireitoNC { get; set; }
        public int SemanaEntrega { get; set; }

        public int NumDias { get; set; }

        private string refProp;

        private bool _separador;
        public bool Separador { get { return _separador; } set { _separador = value; if (_separador) CssEstado = "sep"; } }

        public string SeparadorTexto { get; set; }
        public string SeparadorTexto2 { get; set; }

        private double? custos, custosTransporte;

        public double? Custos
        {
            get { return custos; }
            set { custos = value.HasValue ? (double?)Math.Round(value.Value, 2) : null; }
        }

        public double? CustosTransporte
        {
            get { return custosTransporte; }
            set { custosTransporte = value.HasValue ? (double?)Math.Round(value.Value, 2) : null; }
        }


        public string CustomEnum { get; set; }


        public bool TemRecolha { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ViewModels.Recolhas.RecolhaLinha Recolha { get; set; }
        public string Ref
        {
            get
            {
                return refProp != null ? refProp : "  ";
                //forma de nao dar null em json

            }
            set { refProp = value; }
        }

        public SavsLinha2()
        {
            Ref = "";
            Separador = false;
        }




    }

    public class PagSavPesquisaRes
    {
        public List<ViewModels.Savs.SavsLinha2> Savs { get; set; }
        public bool AgruparCliente { get; set; }
        public bool AgruparDepartamento { get; set; }
        public bool AgruparTipoAvaria { get; set; }
        public bool AgruparProduto { get; set; }
        public bool AgruparSemanaEntrega { get; set; }
        public bool AgruparNumDias { get; set; }
        public bool AgruparEstado { get; set; }

        public string DataRelatorio
        {
            get
            {
                Models.Savs s = new Models.Savs();

                return DateTime.Now.ToString("yyyy-MM-dd");

            }
        }
        public PagSavPesquisaRes()
        {
            AgruparCliente = false;
        }
    }
}