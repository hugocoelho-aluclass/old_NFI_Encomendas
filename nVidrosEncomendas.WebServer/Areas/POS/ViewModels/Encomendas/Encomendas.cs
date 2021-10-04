using Newtonsoft.Json;
using NVidrosEncomendas.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas
{
    public class Encomendas
    {
        public int IdEncomenda { get; set; }
        public string NomeSerie { get; set; }
        public int NumDoc { get; set; }

        public string NomeCliente { get; set; }
        public int NumCliente { get; set; }

        public string NomeTipoEncomenda { get; set; }

        public int NumVidroExtra { get; set; }
        public string NomeVidroExtra { get; set; }

        public string RefObra { get; set; }

        public int[] TiposEncomenda { get; set; }

        public string Producao { get; set; }

        public DateTime? DataEntrega { get; set; }
        public string DataExpedido { get; set; }

        public int SemanaEntrega { get; set; }

        public DateTime DataPedido { get; set; }
        public string DataPedidoString { get; set; }

        public string GuiaRemessa { get; set; }
        public string Notas { get; set; }

        public bool Anulada { get; set; }

        public int NumVidros { get; set; }
        public int Estado { get; set; }


        public bool NovaEncomenda { get; set; }
        public string NumSerie { get; set; }

        public int EstadoRow { get; set; }



        public Encomendas()
        {
            DataPedido = DateTime.Now.Date;
            TiposEncomenda = new int[0];
        }

        public void EncomendaToVM(Models.Encomendas enc)
        {
            this.IdEncomenda = enc.IdEncomenda;
            this.NomeSerie = enc.SerieDoc.NumSerie;
            this.NumDoc = enc.NumDoc;
            if (enc.Cliente != null)
            {
                this.NumCliente = enc.Cliente.NumCliente;
                this.NomeCliente = enc.Cliente.NomeCliente;
            }
            this.RefObra = enc.RefObra;
            this.DataPedido = enc.DataPedido;

            if (enc.VidrosExtra != null)
            {
                this.NumVidroExtra = enc.VidrosExtra.Num;
                this.NomeVidroExtra = enc.VidrosExtra.Nome;
            }


            this.NumVidros = enc.NumVidros;

            this.Producao = enc.Producao;
            this.DataExpedido = enc.DataExpedido;
            this.GuiaRemessa = enc.GuiaRemessa;
            this.Notas = enc.Notas;
            this.Estado = enc.Estado;
            this.DataEntrega = enc.DataEntrega;
            this.SemanaEntrega = enc.SemanaEntrega;
            this.Anulada = enc.Anulada;
            this.NumSerie = enc.SerieDoc.NomeSerie;
            this.NovaEncomenda = false;
            this.TiposEncomenda = enc.TiposEncomenda.Select(x => x.TipoEncomendaId).ToArray();

        }

        public Models.Encomendas ToModel(Models.AppDbContext db)
        {
            BusinessLogic.EncomendasBL encBl = new NVidrosEncomendas.WebServer.BusinessLogic.EncomendasBL(db);
            BusinessLogic.SeriesBL serBl = new BusinessLogic.SeriesBL(db);
            BusinessLogic.TipoEncomendasBL tencBl = new BusinessLogic.TipoEncomendasBL(db);
            BusinessLogic.ClientesBL clBl = new BusinessLogic.ClientesBL(db);
            BusinessLogic.VidrosExtraBL vidrosExtraBl = new BusinessLogic.VidrosExtraBL(db);

            if (this.NovaEncomenda) this.IdEncomenda = -1;

            Models.Encomendas res = encBl.LerEncomenda(this.IdEncomenda).Key;

            if (res.SerieDoc == null || res.SerieDoc.NumSerie != this.NomeSerie)
            {
                res.SerieDoc = serBl.ProcuraSerieOuDefeito(this.NomeSerie);
            }
            res.NumDoc = this.NumDoc;
            if (res.Cliente == null || res.Cliente.NumCliente != this.NumCliente)
            {
                res.Cliente = clBl.LerCliente(this.NumCliente);
            }


            if (this.TiposEncomenda != null && this.TiposEncomenda.Length > 0)
            {

                res.TiposEncomenda = this.TiposEncomenda.Select(x => new EncomendasTipoEncomenda()
                {
                    Encomenda = res,
                    TipoEncomendaId = x,
                    TipoEncomendas = tencBl.LerTipoEncomenda(x)
                }
                )
                .ToList();
            }
            else
            {
                res.TiposEncomenda = new List<Models.EncomendasTipoEncomenda>();
            }

            if (res.VidrosExtra == null || res.VidrosExtra.Num != this.NumVidroExtra)
            {
                res.VidrosExtra = vidrosExtraBl.Ler(this.NumVidroExtra);
            }


            res.RefObra = this.RefObra;
            res.Producao = this.Producao;
            res.DataPedido = this.DataPedido;
            res.DataEntrega = this.DataEntrega;
            res.DataExpedido = this.DataExpedido;

            res.SemanaEntrega = this.SemanaEntrega;
            res.GuiaRemessa = this.GuiaRemessa;
            res.Notas = this.Notas;
            res.Anulada = this.Anulada;
            res.NumVidros = this.NumVidros;
            res.Estado = this.Estado;

            return res;
        }
    }

    public class Compras
    {
        public int IdCompraEncomendas { get; set; }
        public string NotasFornecedor { get; set; }
        public string Material { get; set; }
        public int LinhaCompra { get; set; }
        public DateTime? DataEntrega { get; set; }
        public DateTime? DataPedido { get; set; }
    }
    public class Serie
    {
        public string NumSerie { get; set; }
        public int UltDoc { get; set; }
        public bool SerieDefeito { get; set; }
    }


    public class Cliente
    {
        public int NumCliente { get; set; }
        public string NomeCliente { get; set; }
    }

    public class TipoEncomenda
    {
        public int NumTipoEncomenda { get; set; }
        public string NomeTipoEncomenda { get; set; }
        public int SetorId { get; set; }
        public string SetorNome { get; set; }
    }

    public class SetorEncomenda
    {
        public int SetorId { get; set; }
        public string SetorNome { get; set; }

        public int[] TiposEncomenda { get; set; }
    }

    public class PagEditEncomenda
    {
        public List<Serie> Series { get; set; }
        public List<Cliente> Clientes { get; set; }

        public Encomendas Encomenda { get; set; }
        public List<TipoEncomenda> TipoEncomenda { get; set; }

        public List<IdNumNome> VidroExtras { get; set; }

        public PagEditEncomenda()
        {

        }
    }

    public class PagEncomendaPesquisa
    {
        public List<Cliente> Clientes { get; set; }

        public List<TipoEncomenda> TipoEncomenda { get; set; }

        public List<Serie> Series { get; set; }
        public List<SetorEncomenda> SetoresEncomenda { get; set; }


        public PagEncomendaPesquisa()
        {

        }
    }



    public class PagEncomendaPesquisaParam
    {
        public List<Cliente> Clientes { get; set; }

        public string Serie { get; set; }
        public bool NumDocAteBool { get; set; }
        public int NumDocAteValue { get; set; }
        public bool NumDocDesdeBool { get; set; }
        public int NumDocDesdeValue { get; set; }

        public bool DataEntregaAteBool { get; set; }
        public DateTime DataEntregaAteValue { get; set; }

        public bool DataEntregaDesdeBool { get; set; }
        public DateTime DataEntregaDesdeValue { get; set; }

        public bool DataPedidoAteBool { get; set; }
        public DateTime DataPedidoAteValue { get; set; }

        public bool DataPedidoDesdeBool { get; set; }
        public DateTime DataPedidoDesdeValue { get; set; }
        public List<EstadoEncomenda> EstadosEncomenda;

        public string GuiaRemessa { get; set; }
        public int MostrarGuiaRemessados { get; set; }
        public string RefObra { get; set; }

        public List<TipoEncomenda> TipoEncomenda { get; set; }

        public string TipoRetorno { get; set; }

        //public bool AgruparCliente { get; set; }.

        public int Ordenacao { get; set; }

        public bool SemanaEntregaAteBool { get; set; }
        public int SemanaEntregaAteValue { get; set; }

        public bool SemanaEntregaDesdeBool { get; set; }
        public int SemanaEntregaDesdeValue { get; set; }
        public PagEncomendaPesquisaParam()
        {
            Clientes = new List<Cliente>();
            DataEntregaAteBool = false;
            EstadosEncomenda = new List<EstadoEncomenda>();
            TipoEncomenda = new List<TipoEncomenda>();
            DataEntregaDesdeBool = false;
            DataEntregaAteBool = false;
            DataPedidoDesdeBool = false;
            DataPedidoAteBool = false;
            TipoRetorno = "json";
            SemanaEntregaAteBool = false;
            SemanaEntregaDesdeBool = false;
            Ordenacao = 0;
        }
    }

    public class EstadoEncomenda
    {
        public int NumEstado { get; set; }
        public string Nome { get; set; }
    }

    public class EncomendasLinhaBase
    {
        public int Id { get; set; }

        [JsonIgnore]
        public DateTime DataPedido { get; set; }
        public string DataPedidoString { get; set; }

        [JsonIgnore]
        public DateTime DataEntrega { get; set; }
        public string DataEntregaString { get; set; }



        [JsonIgnore]
        public string NomeSerie { get; set; }
        public string SerieNumEncomenda { get; set; }
        [JsonIgnore]
        public int NumDoc { get; set; }
        public string NomeCliente { get; set; }
        public int SemanaEntrega { get; set; }

        public string RefObra { get; set; }

        private int estado = 0;

        public int NumVidros { get; set; } = 0;

        public int Estado
        {
            get { return estado; }
            set
            {
                estado = value;
                if (estado == 0)
                {
                    EstadoDesc = "Pendente";
                }
                else if (estado == 1)
                {
                    EstadoDesc = "Em Produção";
                }
                else if (estado == 2)
                {
                    EstadoDesc = "Entregue";
                }
                else if (estado == 3)
                {
                    EstadoDesc = "Cancelada";
                }
                else if (estado == 4)
                {
                    EstadoDesc = "Pronta";
                }
            }
        }
        public string NomeTipoEncomenda { get; set; }

        public bool Remover { get; set; }

        public string EstadoDesc { get; set; }

        public bool Separador { get; set; }

        public virtual string ClasseEstado
        {
            get
            {
                {
                    return "estado-e" + Estado;
                }
            }
        }

        public EncomendasLinhaBase()
        {
            Separador = false;
        }

    }


    public class EncomendasLinha2 : EncomendasLinhaBase
    {

        public string GuiaRemessa { get; set; }

        public override string ClasseEstado
        {
            get
            {
                {
                    if (Estado == 2 && GuiaRemessa == "")
                    {
                        return "estado-blue";
                    }
                    return "estado-e" + Estado;
                }
            }
        }

        public EncomendasLinha2() : base()
        {

        }

    }

    public class EncomendasLinhaProd : EncomendasLinhaBase
    {

        public string DataAprovadoString { get; set; }

        [JsonIgnore]
        public DateTime DataAprovado { get; set; }

        public string DataExpedido { get; set; }
        public string Cor { get; set; }

    }

    public class EncomendasLinhaAdmin : EncomendasLinha2
    {

        public string DataAprovadoString { get; set; }

        public DateTime DataAprovado { get; set; }
        public string Cor { get; set; }

    }

    public class PagEncomendaPesquisaBase
    {

        public bool AgruparCliente { get; set; }
        public int Ordenacao { get; set; }
        public string DataRelatorio
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd");

            }
        }
        public PagEncomendaPesquisaBase()
        {
            AgruparCliente = false;
        }
    }



    public class PagEncomendaPesquisaRes : PagEncomendaPesquisaBase
    {
        public List<ViewModels.Encomendas.EncomendasLinha2> Encomendas { get; set; }


        public PagEncomendaPesquisaRes() : base()
        {

        }
    }

    public class PagEncomendaPesquisaProd : PagEncomendaPesquisaBase
    {
        public List<ViewModels.Encomendas.EncomendasLinhaProd> Encomendas { get; set; }


        public PagEncomendaPesquisaProd() : base()
        {

        }
    }

    public class PagEncomendaPesquisaAdmin : PagEncomendaPesquisaBase
    {
        public List<ViewModels.Encomendas.EncomendasLinhaAdmin> Encomendas { get; set; }


        public PagEncomendaPesquisaAdmin() : base()
        {

        }
    }


}