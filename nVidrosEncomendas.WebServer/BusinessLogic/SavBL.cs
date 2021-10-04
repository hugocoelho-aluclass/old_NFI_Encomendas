using AutoMapper;
using NVidrosEncomendas.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace NVidrosEncomendas.WebServer.BusinessLogic
{
    public class SavsBL
    {

        public KeyValuePair<Models.Savs, bool> LerSavResultado = new KeyValuePair<Savs, bool>();

        private AppDbContext _dbContext;
        public AppDbContext DbContext
        {
            get
            {
                if (_dbContext == null) _dbContext = new AppDbContext();
                return _dbContext;
            }
            set { _dbContext = value; }
        }

        private SessionObject _SessionObject { get; set; }
        public SavsBL()
        {
        }

        public SavsBL(AppDbContext db)
        {
            DbContext = db;
        }
        public SavsBL(SessionObject sessionObject)
        {
            _SessionObject = sessionObject;
        }
        public IEnumerable<Savs> SavsListaAnulados()
        {
            return DbContext.Savs.AsParallel().OrderBy(x => x.Id).ThenBy(x => x.Id);
        }

        public IEnumerable<Savs> SavsLista()
        {
            return DbContext.Savs.Include(x => x.Cliente).Where(x => x.Anulada == false).OrderBy(x => x.Id);

        }

        public KeyValuePair<Models.Savs, bool> LerSav(int id, SessionObject sessionObj = null)
        {
            SessionObject.GetMySessionObject(System.Web.HttpContext.Current);
            var res = DbContext.Savs
                .Include(x => x.SerieDoc)
                .Include(x => x.Cliente)
                .Include(x => x.Produto)
                .Include(x => x.TipoAvaria)
                .Include(x => x.Departamento)
                .Include(x => x.CriadoPor)
                .Include(x => x.Estado)
                .Include(x => x.Anexos)
                .Include(x => x.Recolha)
                .Include(x => x.Setor)
                .Where(x => x.Id == id).FirstOrDefault();

            bool nova = res == null;
            if (res == null)
            {
                res = new Savs();

                if (sessionObj != null)
                {
                    res.CriadoData = DateTime.Now;
                    res.CriadoPor = sessionObj.OperadorObject;
                }
            }

            return new KeyValuePair<Models.Savs, bool>(res, nova); ;
        }
        public KeyValuePair<Models.Savs, bool> LerSav(string serie, int numDoc, SessionObject sessionObj = null)
        {
            SessionObject.GetMySessionObject(System.Web.HttpContext.Current);
            var res = DbContext.Savs
                .Include(x => x.SerieDoc)
                .Include(x => x.Cliente)
                .Include(x => x.Produto)
                .Include(x => x.TipoAvaria)
                .Include(x => x.Departamento)
                .Include(x => x.CriadoPor)
                .Include(x => x.Estado)
                .Include(x => x.Anexos)
                .Include(x => x.Recolha)
                .Include(x => x.Recolha.EstadoRecolha)                
                .Include(x => x.Setor)
                .Where(x => x.SerieDoc.NumSerie == serie && x.NumDoc == numDoc).FirstOrDefault();
            bool nova = res == null;
            if (res == null)
            {
                res = new Savs();
                SeriesBL sbl = new SeriesBL(DbContext);
                res.SerieDoc = sbl.ProcuraSerieOuDefeito(serie);
                res.NumDoc = res.SerieDoc.UltimoDocSav + 1;


                if (sessionObj != null)
                {
                    res.CriadoData = DateTime.Now;
                    res.CriadoPor = sessionObj.OperadorObject;
                }
            }

            return new KeyValuePair<Models.Savs, bool>(res, nova); ;
        }


        public void AtualizaSav(Savs sav, SessionObject sessionObj)
        {
            try
            {
                Savs _sr = DbContext.Savs.Where(x => x.Id == sav.Id).FirstOrDefault();
                AnexosBL anexoBl = new AnexosBL(DbContext);
                bool novo = false;

                if (_sr == null)
                {
                    novo = true;
                    _sr = new Savs();
                    _sr.CriadoPor = DbContext.Operadores.First(x => x.UtilizadorId == sessionObj.OperadorObject.UtilizadorId);
                    _sr.CriadoData = DateTime.Now;
                }
                else
                {
                    _sr = sav;
                }
                if (_sr.Anexos == null) _sr.Anexos = new List<Models.Anexos>();

                List<Anexos> anexosTmp = sav.Anexos.ToList();
                _sr.EditadoPor = DbContext.Operadores.First(x => x.UtilizadorId == sessionObj.OperadorObject.UtilizadorId);
                _sr.EditadoData = DateTime.Now;
                Mapper.Map<Models.Savs, Models.Savs>(sav, _sr);

                if (_sr.Anexos == null) _sr.Anexos = new List<Models.Anexos>();
                _sr.Anexos.Clear();
                foreach (var item in anexosTmp)
                {
                    _sr.Anexos.Add(anexoBl.LerAnexo(item.Id));
                }
                if (novo)
                {
                    _sr.NumDoc = _sr.SerieDoc.UltimoDocSav + 1;
                    _sr.SerieDoc.UltimoDocSav = _sr.NumDoc;
                    DbContext.Savs.Add(_sr);
                }

                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }


        public void CheckSerie()
        {
            bool existemSavsSemSerie = this.DbContext.Savs.Any(x => x.SerieDoc == null);
            if (existemSavsSemSerie)
            {
                SeriesBL sbl = new SeriesBL(this.DbContext);
                var savsSemSerie = this.DbContext.Savs.Include(x => x.SerieDoc).Where(x => x.SerieDoc == null).OrderBy(x => x.CriadoData).ToList();
                for (int i = 0; i < savsSemSerie.Count(); i++)
                {
                    Savs sav = savsSemSerie[i];
                    Series serie = sbl.LerSerieAno(sav.CriadoData.Year);
                    serie.UltimoDocSav = serie.UltimoDocSav + 1;
                    sav.NumDoc = serie.UltimoDocSav;

                    sav.SerieDoc = serie;
                    AtualizaSav(sav, this._SessionObject);
                    DbContext.SaveChanges();
                }
            }

        }

        public NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.PagSavPesquisaRes PesqParamRes(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.PagSavPesquisaParam pesqParams)
        {
            CheckSerie();


            string[] allIncludes = new string[] { "Cliente", "SerieDoc", "Produto", "TipoAvaria", "Departamento", "CriadoPor", "Estado" };
            if (pesqParams.Recolha || true) { allIncludes = allIncludes.Concat<string>(new string[] { "Recolha.EstadoRecolha" }).ToArray(); }

            var encsQuery = this.DbContext.Savs.AsQueryable();

            foreach (string include in allIncludes)
            {
                encsQuery = encsQuery.Include(include);
            }

            IQueryable<Models.Savs> encs = encsQuery.AsNoTracking().Where(x => !x.Anulada);
            if (pesqParams.Recolha)
            {
                encs = encs.Where(x => x.TemRecolha == true);
            }

            if (pesqParams.Serie != "" && pesqParams.Serie != "--")
            {
                encs = encs.Where(x => x.SerieDoc.NumSerie == pesqParams.Serie);
            }

            if (pesqParams.NumDocDesdeBool && pesqParams.NumDocDesdeValue != 0)
            {
                encs = encs.Where(x => x.NumDoc >= pesqParams.NumDocDesdeValue);
            }

            if (pesqParams.NumDocAteBool && pesqParams.NumDocAteValue != 0)
            {
                encs = encs.Where(x => x.NumDoc <= pesqParams.NumDocAteValue);
            }
            if (pesqParams.Clientes.Any())
            {
                int[] ids = pesqParams.Clientes.Select(x => x.Id).ToArray();
                encs = encs.Where(x => ids.Any(z => z == x.Cliente.NumCliente));
            }
            if (pesqParams.Departamentos.Any())
            {
                int[] ids = pesqParams.Departamentos.Select(x => x.Id).ToArray();
                encs = encs.Where(x => ids.Any(z => z == x.Departamento.NumDepartamentoSav));
            }

            if (pesqParams.EstadoSav.Any())
            {
                int[] ids = pesqParams.EstadoSav.Select(x => x.Id).ToArray();
                encs = encs.Where(x => ids.Any(z => z == x.Estado.IdEstadoSav));
            }
            if (pesqParams.Produtos.Any())
            {
                int[] ids = pesqParams.Produtos.Select(x => x.Id).ToArray();
                encs = encs.Where(x => ids.Any(z => z == x.Produto.IdProdutoSav));
            }
            if (pesqParams.TipoAvaria.Any())
            {
                int[] ids = pesqParams.TipoAvaria.Select(x => x.Id).ToArray();
                encs = encs.Where(x => ids.Any(z => z == x.TipoAvaria.NumTipoAvaria));
            }


            DateTime now = DateTime.Now;
            if (pesqParams.NDiasDesdeBool && pesqParams.NDiasDesdeValue != 0)
            {
                encs = encs.Where(x => (System.Data.Entity.DbFunctions.DiffDays((x.MarcarResolvida ? x.DataResolvida : x.CriadoData), now) + 1) >= pesqParams.NDiasDesdeValue);//   ((x.MarcarResolvida ? x.DataResolvida : x.CriadoData) - now).TotalDays <= pesqParams.NDiasAteValue);

            }
            if (pesqParams.NDiasAteBool && pesqParams.NDiasAteValue != 0)
            {

                encs = encs.Where(x => (System.Data.Entity.DbFunctions.DiffDays((x.MarcarResolvida ? x.DataResolvida : x.CriadoData), now) + 1) <= pesqParams.NDiasAteValue);//   ((x.MarcarResolvida ? x.DataResolvida : x.CriadoData) - now).TotalDays <= pesqParams.NDiasAteValue);
            }


            if (pesqParams.DataResolvidoDesdeBool)
            {
                encs = encs.Where(x => DateTime.Compare(pesqParams.DataResolvidoDesdeValue, x.DataResolvida) < 0);
            }

            if (pesqParams.DataResolvidoAteBool)
            {
                encs = encs.Where(x => DateTime.Compare(pesqParams.DataResolvidoAteValue, x.DataResolvida) > 0);
            }

            if (pesqParams.DataEntradaDesdeBool)
            {
                encs = encs.Where(x => DateTime.Compare(pesqParams.DataEntradaDesdeValue, x.DataSav) < 0);
            }

            if (pesqParams.DataEntradaAteBool)
            {
                encs = encs.Where(x => DateTime.Compare(pesqParams.DataEntradaAteValue, x.DataSav) > 0);
            }

            if (!(pesqParams.Ref == null || pesqParams.Ref.Trim() == string.Empty))
            {
                string valorRef = pesqParams.Ref.ToLowerInvariant();
                encs = encs.Where(x => (x.Ref != null && x.Ref != "" && x.Ref.ToLower().IndexOf(valorRef) != -1));
            }


            if (pesqParams.SemanaEntregaDesdeBool || (pesqParams.SemanaEntregaAteBool))
            {
                //recheck max e min
                int max = Math.Max(pesqParams.SemanaEntregaAteValue, pesqParams.SemanaEntregaDesdeBool ? pesqParams.SemanaEntregaDesdeValue : -100);
                int min = Math.Min(pesqParams.SemanaEntregaDesdeValue, pesqParams.SemanaEntregaAteBool ? pesqParams.SemanaEntregaAteValue : 100);

                if (pesqParams.SemanaEntregaDesdeBool)
                {
                    encs = encs.Where(x => min <= x.SemanaEntrega);
                }

                if (pesqParams.SemanaEntregaAteBool)
                {
                    encs = encs.Where(x => max >= x.SemanaEntrega);
                }
            }


            if (pesqParams.Recolha && pesqParams.EstadoRecolha.Any())
            {
                int[] ids = pesqParams.EstadoRecolha.Select(x => x.Id).ToArray();
                encs = encs.Where(x => x.Recolha != null && ids.Any(z => z == x.Recolha.EstadoRecolha.Id));
            }


            List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.SavsLinha2> resLista = Mapper.Map<List<Models.Savs>, List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.SavsLinha2>>(encs.ToList());


            List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.SavsLinha2> aRemover = new List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.SavsLinha2>();

            foreach (var item in resLista)
            {
                // item.DataResolvidoString = item.DataResolvido.ToString(@"yyyy-MM-dd");
                if (pesqParams.DataEntradaAteBool || pesqParams.DataEntradaDesdeBool)
                {
                    DateTime dt = new DateTime();
                    bool dtParse = DateTime.TryParse(item.DataResolvido, out dt);
                    if (dtParse)
                    {
                        if (pesqParams.DataEntradaAteBool && !(DateTime.Compare(pesqParams.DataEntradaAteValue, dt) > 0))
                        {
                            aRemover.Add(item);
                        }

                        if (pesqParams.DataEntradaDesdeBool && !(DateTime.Compare(pesqParams.DataEntradaDesdeValue, dt) < 0))
                        {
                            aRemover.Add(item);
                        }
                    }
                    else
                    {
                        aRemover.Add(item);
                    }
                }
            }

            foreach (var item in aRemover)
            {
                resLista.Remove(item);
            }

            NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.PagSavPesquisaRes res = new NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.PagSavPesquisaRes();
            res.Savs = resLista;

            if (pesqParams.OrdemPesquisa == 0)
            {
                res.Savs = res.Savs.OrderByDescending(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 1)
            {
                res.Savs = res.Savs.OrderBy(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 2)
            {
                res.Savs = res.Savs.OrderBy(x => x.SerieNum).ThenBy(x => x.NumDoc).ToList();

            }
            else if (pesqParams.OrdemPesquisa == 3)
            {
                res.Savs = res.Savs.OrderByDescending(x => x.SerieNum).ThenByDescending(x => x.NumDoc).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 4)
            {
                res.Savs = res.Savs.OrderBy(x => x.NomeCliente).ThenByDescending(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 5)
            {
                res.Savs = res.Savs.OrderBy(x => x.NomeDepartamento).ThenByDescending(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 6)
            {
                res.Savs = res.Savs.OrderBy(x => x.NomeProduto).ThenByDescending(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 7)
            {
                res.Savs = res.Savs.OrderBy(x => x.SemanaEntrega).ThenByDescending(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 8)
            {
                res.Savs = res.Savs.OrderByDescending(x => x.SemanaEntrega).ThenByDescending(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 9)
            {
                res.Savs = res.Savs.OrderBy(x => x.NumDias).ThenByDescending(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 10)
            {
                res.Savs = res.Savs.OrderByDescending(x => x.NumDias).ThenByDescending(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 11)
            {
                res.Savs = res.Savs.OrderBy(x => x.NomeEstado).ThenByDescending(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 12) //agrupar cliente
            {
                res.AgruparCliente = true;
                res.Savs = res.Savs.OrderBy(x => x.NomeCliente).ToList();
                string lastNomeCliente = "";
                int listaCountFixo = res.Savs.Count();
                for (int i = 0; i < listaCountFixo; i++)
                {
                    if (res.Savs[i].NomeCliente != lastNomeCliente)
                    {
                        res.Savs.Add(new NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.SavsLinha2()
                        {
                            SeparadorTexto = "Cliente",
                            SeparadorTexto2 = res.Savs[i].NomeCliente,
                            NomeCliente = res.Savs[i].NomeCliente,
                            NomeSerie = "0",
                            NumDoc = 0,
                            Custos = res.Savs.Where(x => res.Savs[i].NomeCliente == x.NomeCliente).Sum(x => (x.Custos ?? 0)),
                            CustosTransporte = res.Savs.Where(x => res.Savs[i].NomeCliente == x.NomeCliente).Sum(x => (x.CustosTransporte ?? 0)),

                            Separador = true
                        });
                    }
                    lastNomeCliente = res.Savs[i].NomeCliente;
                }

                res.Savs = res.Savs.OrderBy(x => x.NomeCliente).ThenBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }

            else if (pesqParams.OrdemPesquisa == 13) //agrupar tipo avaria
            {
                res.AgruparTipoAvaria = true;
                res.Savs = res.Savs.OrderBy(x => x.NomeTipoAvaria).ToList();
                string lastNomeTipoAvaria = "";
                int listaCountFixo = res.Savs.Count();
                for (int i = 0; i < listaCountFixo; i++)
                {
                    if (res.Savs[i].NomeTipoAvaria != lastNomeTipoAvaria)
                    {
                        res.Savs.Add(new NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.SavsLinha2()
                        {
                            SeparadorTexto = "Tipo de Avaria",
                            SeparadorTexto2 = res.Savs[i].NomeTipoAvaria,
                            NomeTipoAvaria = res.Savs[i].NomeTipoAvaria,
                            NomeSerie = "0",
                            NumDoc = 0,
                            Custos = res.Savs.Where(x => res.Savs[i].NomeTipoAvaria == x.NomeTipoAvaria).Sum(x => (x.Custos ?? 0)),
                            CustosTransporte = res.Savs.Where(x => res.Savs[i].NomeTipoAvaria == x.NomeTipoAvaria).Sum(x => (x.CustosTransporte ?? 0)),

                            Separador = true,
                        });
                    }
                    lastNomeTipoAvaria = res.Savs[i].NomeTipoAvaria;
                }

                res.Savs = res.Savs.OrderBy(x => x.NomeTipoAvaria).ThenBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }

            else if (pesqParams.OrdemPesquisa == 14) //agrupar departamento
            {
                res.AgruparDepartamento = true;
                res.Savs = res.Savs.OrderBy(x => x.NomeDepartamento).ToList();
                string lastNomeDepartamento = "";
                int listaCountFixo = res.Savs.Count();
                for (int i = 0; i < listaCountFixo; i++)
                {
                    if (res.Savs[i].NomeDepartamento != lastNomeDepartamento)
                    {
                        res.Savs.Add(new NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.SavsLinha2()
                        {
                            SeparadorTexto = "Departamento",
                            SeparadorTexto2 = res.Savs[i].NomeDepartamento,
                            NomeDepartamento = res.Savs[i].NomeDepartamento,
                            NomeSerie = "0",
                            NumDoc = 0,
                            Separador = true,
                            Custos = Math.Round(res.Savs.Where(x => res.Savs[i].NomeDepartamento == x.NomeDepartamento).Sum(x => ((x.Custos.HasValue && x.Custos.Value > 0) ? x.Custos : 0)) ?? 0, 2),
                            CustosTransporte = res.Savs.Where(x => res.Savs[i].NomeDepartamento == x.NomeDepartamento).Sum(x => (x.CustosTransporte ?? 0))

                        }
                        );
                    }
                    lastNomeDepartamento = res.Savs[i].NomeDepartamento;
                }

                res.Savs = res.Savs.OrderBy(x => x.NomeDepartamento).ThenBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }

            else if (pesqParams.OrdemPesquisa == 15) //agrupar produto
            {
                res.AgruparProduto = true;
                res.Savs = res.Savs.OrderBy(x => x.NomeProduto).ToList();
                string lastNomeProduto = "";
                int listaCountFixo = res.Savs.Count();
                for (int i = 0; i < listaCountFixo; i++)
                {
                    if (res.Savs[i].NomeProduto != lastNomeProduto)
                    {
                        res.Savs.Add(new NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.SavsLinha2()
                        {
                            SeparadorTexto = "Produto",
                            SeparadorTexto2 = res.Savs[i].NomeProduto,
                            NomeProduto = res.Savs[i].NomeProduto,
                            NomeSerie = "0",
                            NumDoc = 0,
                            Separador = true,
                            Custos = res.Savs.Where(x => res.Savs[i].NomeProduto == x.NomeProduto).Sum(x => (x.Custos ?? 0)),
                            CustosTransporte = res.Savs.Where(x => res.Savs[i].NomeProduto == x.NomeProduto).Sum(x => (x.CustosTransporte ?? 0))

                        });
                    }
                    lastNomeProduto = res.Savs[i].NomeProduto;
                }

                res.Savs = res.Savs.OrderBy(x => x.NomeProduto).ThenBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }

            else if (pesqParams.OrdemPesquisa == 16) //agrupar produto
            {
                res.AgruparSemanaEntrega = true;
                res.Savs = res.Savs.OrderBy(x => x.SemanaEntrega).ToList();
                int last = -999999;
                int listaCountFixo = res.Savs.Count();
                for (int i = 0; i < listaCountFixo; i++)
                {
                    if (res.Savs[i].SemanaEntrega != last)
                    {
                        res.Savs.Add(new NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.SavsLinha2()
                        {
                            SeparadorTexto = "Semana de Entrega",
                            SeparadorTexto2 = res.Savs[i].SemanaEntrega.ToString(),
                            SemanaEntrega = res.Savs[i].SemanaEntrega,
                            NomeSerie = "0",
                            NumDoc = 0,
                            Custos = res.Savs.Where(x => res.Savs[i].SemanaEntrega == x.SemanaEntrega).Sum(x => (x.Custos ?? 0)),
                            CustosTransporte = res.Savs.Where(x => res.Savs[i].SemanaEntrega == x.SemanaEntrega).Sum(x => (x.CustosTransporte ?? 0)),

                            Separador = true
                        });
                    }
                    last = res.Savs[i].SemanaEntrega;
                }

                res.Savs = res.Savs.OrderBy(x => x.SemanaEntrega).ThenBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 17) //agrupar n dias
            {
                res.AgruparNumDias = true;
                res.Savs = res.Savs.OrderBy(x => x.NumDias).ToList();
                int last = -999999;
                int listaCountFixo = res.Savs.Count();
                for (int i = 0; i < listaCountFixo; i++)
                {
                    if (res.Savs[i].NumDias != last)
                    {
                        res.Savs.Add(new NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.SavsLinha2()
                        {
                            SeparadorTexto = "N Dias",
                            SeparadorTexto2 = res.Savs[i].NumDias.ToString(),
                            NumDias = res.Savs[i].NumDias,
                            NomeSerie = "0",
                            NumDoc = 0,
                            Custos = res.Savs.Where(x => res.Savs[i].SemanaEntrega == x.SemanaEntrega).Sum(x => (x.Custos ?? 0)),
                            CustosTransporte = res.Savs.Where(x => res.Savs[i].SemanaEntrega == x.SemanaEntrega).Sum(x => (x.CustosTransporte ?? 0)),
                            Separador = true

                        });
                    }
                    last = res.Savs[i].NumDias;
                }
                res.Savs = res.Savs.OrderBy(x => x.NumDias).ThenBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 18) //agrupar estado
            {
                res.AgruparEstado = true;
                res.Savs = res.Savs.OrderBy(x => x.NomeEstado).ToList();
                string last = "";
                int listaCountFixo = res.Savs.Count();
                for (int i = 0; i < listaCountFixo; i++)
                {
                    if (res.Savs[i].NomeEstado != last)
                    {
                        res.Savs.Add(new NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.SavsLinha2()
                        {
                            SeparadorTexto = "Estado",
                            SeparadorTexto2 = res.Savs[i].NomeEstado,
                            NomeEstado = res.Savs[i].NomeEstado,
                            NomeSerie = "0",
                            NumDoc = 0,
                            Custos = res.Savs.Where(x => res.Savs[i].NomeEstado == x.NomeEstado).Sum(x => (x.Custos ?? 0)),
                            CustosTransporte = res.Savs.Where(x => res.Savs[i].NomeEstado == x.NomeEstado).Sum(x => (x.CustosTransporte ?? 0)),

                            Separador = true
                        });
                    }
                    last = res.Savs[i].NomeEstado;
                }

                res.Savs = res.Savs.OrderBy(x => x.NomeEstado).ThenBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 19) //Custo asc
            {
                res.Savs = res.Savs.OrderBy(x => x.Custos).ThenByDescending(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 20) //Custo asc
            {
                res.Savs = res.Savs.OrderByDescending(x => x.Custos).ThenByDescending(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 21) //Recolha - Data Pedido Recolha
            {
                res.Savs = res.Savs.Where(x => x.Recolha != null).OrderBy(x => x.Recolha.DataPedidoRecolha).ThenByDescending(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 22) //Recolha - Data Pedido Recolha desc
            {
                res.Savs = res.Savs.Where(x => x.Recolha != null).OrderByDescending(x => x.Recolha.DataPedidoRecolha).ThenByDescending(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 23) //Recolha - Data Recolha
            {
                res.Savs = res.Savs.Where(x => x.Recolha != null).OrderBy(x => x.Recolha.DataRecolha).ThenByDescending(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 24) //Recolha - Data Recolha desc
            {
                res.Savs = res.Savs.Where(x => x.Recolha != null).OrderByDescending(x => x.Recolha.DataRecolha).ThenByDescending(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 25) //Recolha - Data Pedido Recolha
            {
                res.Savs = res.Savs.Where(x => x.Recolha != null).OrderBy(x => x.Recolha.DataPedidoRecolha).ThenByDescending(x => x.DataEntrada).ToList();
            }
            else if (pesqParams.OrdemPesquisa == 26) //Recolha - Data Pedido Recolha desc
            {
                res.Savs = res.Savs.Where(x => x.Recolha != null).OrderByDescending(x => x.Recolha.DataPedidoRecolha).ThenByDescending(x => x.DataEntrada).ToList();
            }
            //res.AgruparCliente = pesqParams.AgruparCliente;
            return res;
        }


    }
}