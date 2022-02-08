using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas
{
    public class Encomendas
    {
        public int IdEncomenda { get; set; }
        public string NomeSerie { get; set; }
        public int NumDoc { get; set; }

        public string NomeCliente { get; set; }
        public int NumCliente { get; set; }

        public int NumTipoEncomenda { get; set; }
        public string NomeTipoEncomenda { get; set; }

        public string NomeArtigo { get; set; }

        public string Cor { get; set; }

        public string Painel { get; set; }
        public string Producao { get; set; }

        public int SemanaEntrega { get; set; }
        public DateTime DataPedido { get; set; }
        public string DataPedidoString { get; set; }

        public DateTime? DataAprovacao { get; set; }
        public string DataAprovacaoString { get; set; }
        public string DataExpedido { get; set; }

        public string Fatura { get; set; }
        public string Notas { get; set; }

        public bool Anulada { get; set; }

        public int NumVaos { get; set; }
        public int Estado { get; set; }

        public bool MaterialEncomendado { get; set; }
        public string MaterialEncomendadoDetalhe { get; set; }

        public bool ComprasOK { get; set; }

        public bool NovaEncomenda { get; set; }
        public string NumSerie { get; set; }

        public int EstadoRow { get; set; }

        public List<Compras> Compras { get; set; }

        public int? AnoEntrega { get; set; }

        public DateTime? DataProduzido { get; set; }


        public Encomendas()
        {
            DataPedido = DateTime.Now.Date;
            //DataExpedido = DateTime.Now.AddDays(14);
            Compras = new List<Compras>();

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
            this.NomeArtigo = enc.NomeArtigo;
            this.DataPedido = enc.DataPedido;
            this.DataAprovacao = enc.DataAprovacao;

            if (enc.TipoEncomenda != null)
            {
                this.NumTipoEncomenda = enc.TipoEncomenda.NumTipoEncomenda;
                this.NomeTipoEncomenda = enc.TipoEncomenda.NomeTipoEncomenda;
            }
            this.NumVaos = enc.NumVaos;
            this.Cor = enc.Cor;
            this.Painel = enc.Painel;

            this.Producao = enc.Producao;
            this.DataExpedido = enc.DataExpedidoString;
            this.Fatura = enc.Fatura;
            this.Notas = enc.Notas;
            this.Estado = enc.Estado;
            this.SemanaEntrega = enc.SemanaEntrega;
            this.Anulada = enc.Anulada;
            this.NumSerie = enc.NumSerieEncomenda;
            this.Compras = (from c in enc.EncomendasCompras
                            select new Compras
                            {
                                DataEntrega = c.DataEntrega,
                                DataPedido = c.DataPedido,
                                IdCompraEncomendas = c.IdCompraEncomendas,
                                LinhaCompra = c.LinhaCompra,
                                NotasFornecedor = c.NotasFornecedor,
                                Material = c.Material
                            }).ToList();

            while (this.Compras.Count() < 5)
            {

                Compras.Add(new Compras()
                {
                    LinhaCompra = this.Compras.Count()
                });

            }
            
            this.NumSerie = enc.NumSerieEncomenda;
            this.NovaEncomenda = false;
            this.AnoEntrega = enc.AnoEntrega;

        }

        public Models.Encomendas ToModel(Models.AppDbContext db)
        {
            BusinessLogic.EncomendasBL encBl = new NfiEncomendas.WebServer.BusinessLogic.EncomendasBL(db);
            BusinessLogic.SeriesBL serBl = new BusinessLogic.SeriesBL(db);
            BusinessLogic.TipoEncomendasBL tencBl = new BusinessLogic.TipoEncomendasBL(db);
            BusinessLogic.ClientesBL clBl = new BusinessLogic.ClientesBL(db);
            if (this.NovaEncomenda) this.IdEncomenda = -1;

            Models.Encomendas res = encBl.LerEncomenda(this.IdEncomenda).Key; //new Models.Operadores();


            if (res.SerieDoc == null || res.SerieDoc.NumSerie != this.NomeSerie)
            {
                res.SerieDoc = serBl.ProcuraSerieOuDefeito(this.NomeSerie);
            }
            res.NumDoc = this.NumDoc;
            if (res.Cliente == null || res.Cliente.NumCliente != this.NumCliente)
            {
                res.Cliente = clBl.LerCliente(this.NumCliente);
            }
            if (res.TipoEncomenda == null || res.TipoEncomenda.NumTipoEncomenda != this.NumTipoEncomenda)
            {
                res.TipoEncomenda = tencBl.LerTipoEncomenda(this.NumTipoEncomenda);
            }

            res.NomeArtigo = this.NomeArtigo;
            res.Cor = this.Cor;
            res.Painel = this.Painel;
            res.Producao = this.Producao;
            res.SemanaEntrega = this.SemanaEntrega;
            res.DataPedido = this.DataPedido;
            res.DataAprovacao = this.DataAprovacao;

            res.SemanaEntrega = this.SemanaEntrega;
            res.DataExpedidoString = this.DataExpedido;
            res.Fatura = this.Fatura;
            res.Notas = this.Notas;
            res.Anulada = this.Anulada;
            res.NumVaos = this.NumVaos;
            res.Estado = this.Estado;
            res.NumSerieEncomenda = this.NumSerie;
         

            for (int i = 0; i < this.Compras.Count(); i++)
            {
                if (this.Compras[i].Material == "")
                {
                    this.Compras[i].NotasFornecedor = "";
                    this.Compras[i].DataEntrega = null;
                    this.Compras[i].DataPedido = null;
                }
            }
            res.EncomendasCompras = (from c in this.Compras
                                     select new Models.EncomendasCompras
                                     {
                                         DataEntrega = c.DataEntrega,
                                         DataPedido = c.DataPedido,
                                         IdCompraEncomendas = c.IdCompraEncomendas,
                                         LinhaCompra = c.LinhaCompra,
                                         NotasFornecedor = c.NotasFornecedor,
                                         Material = c.Material
                                     }).ToList();

            var tempWeek = GetIso8601WeekOfYear(this.DataPedido);

            if (tempWeek <= this.SemanaEntrega)
            {
                res.AnoEntrega = this.DataPedido.Year;
            }
            else
            {
                res.AnoEntrega = this.DataPedido.Year + 1;
            }

            return res;
        }

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
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

    public class RelatorioDados
    {

        public List<TipoEncomenda> TipoEncomenda { get; set; }

        public List<Serie> Series { get; set; }
        public List<SetorEncomenda> SetoresEncomenda { get; set; }


        public RelatorioDados()
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

        public string Fatura { get; set; }
        public int MostrarFaturados { get; set; }
        public string NomeObra { get; set; }

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
        public string NomeSerie { get; set; }
        public string SerieNumEncomenda { get; set; }
        [JsonIgnore]
        public int NumDoc { get; set; }
        public string NomeCliente { get; set; }
        public int SemanaEntrega { get; set; }

        public string NomeArtigo { get; set; }

        private int estado = 0;

        public int NumVaos { get; set; } = 0;

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

        public string DataExpedido { get; set; }


        public string Fatura { get; set; }

        public override string ClasseEstado
        {
            get
            {
                {
                    if (Estado == 2 && Fatura == "")
                    {
                        return "estado-blue";
                    }
                    //else if (Estado == 4)
                    //{
                    //    return "estado-e" + Estado;
                    //}
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