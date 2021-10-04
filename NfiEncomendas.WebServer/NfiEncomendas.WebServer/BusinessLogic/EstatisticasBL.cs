using NfiEncomendas.WebServer.Areas.POS.ViewModels.Estatisticas;
using NfiEncomendas.WebServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace NfiEncomendas.WebServer.BusinessLogic
{
    public class EstatisticasBL
    {
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

        public EstatisticasBL()
        {
        }
        public EstatisticasBL(AppDbContext db)
        {
            DbContext = db;
        }


        string semNomeDefault = "[outro/sem nome] ";
        public Estatisticas GetEstatisticas(PesquisaEstatisticas pesq)
        {
            Estatisticas res = new Estatisticas();

            List<Encomendas> enc = DbContext.Encomendas.Include("TipoEncomenda.SetorEncomenda")
                .Include("Cliente").Include("TipoEncomenda")
                .Where(x => !x.Anulada && x.DataPedido > pesq.DataPesquisaDesde && x.DataPedido < pesq.DataPesquisaAte).ToList();
            res.EncomendasTotal = enc.Count;
            //encomendas

            //agrupar por setor
            res.EncomendasSetor = enc.GroupBy(x => x.TipoEncomenda.SetorEncomenda).Select(n => new NomeTotalLista
            {
                Nome = n.Key != null ? n.Key.Nome.ToString() : semNomeDefault,
                Total = n.Count(),
                Totais = new[]
                {
                    new NomeTotalLista()
                    {
                        Nome = "Por Cliente",
                        Totais = n.GroupBy(y => y.Cliente.NomeCliente).Select(j => new NomeTotalLista
                            {
                                Nome = j.Key != null ? j.Key.ToString() :semNomeDefault,
                                Total = j.Count()
                            }).ToArray()
                    },
                    new NomeTotalLista()
                    {
                        Nome = "Por Tipo de encomenda",
                        Totais = n.GroupBy(y => y.TipoEncomenda.NomeTipoEncomenda).Select(j => new NomeTotalLista
                            {
                                Nome = j.Key != null ? j.Key.ToString() :semNomeDefault,
                                Total = j.Count()
                            }).ToArray()
                    }
                }
            }).ToList();
            //agrupar por cliente
            res.EncomendasCliente = enc.GroupBy(x => x.Cliente.NomeCliente).Select(n => new NomeTotalLista
            {
                Nome = n.Key != null ? n.Key.ToString() : semNomeDefault,
                Total = n.Count(),
                Totais = new[]
                {
                    new NomeTotalLista()
                    {
                        Nome = "Por Setor",
                        Totais = n.GroupBy(y => y.TipoEncomenda.SetorEncomenda).Select(j => new NomeTotalLista
                            {
                                Nome = j.Key != null ? j.Key.Nome.ToString() :semNomeDefault,
                                Total = j.Count()
                            }).ToArray()
                    },
                    new NomeTotalLista()
                    {
                        Nome = "Por Tipo de encomenda",
                        Totais = n.GroupBy(y => y.TipoEncomenda.NomeTipoEncomenda).Select(j => new NomeTotalLista
                            {
                                Nome = j.Key != null ? j.Key.ToString() :semNomeDefault,
                                Total = j.Count()
                            }).ToArray()
                    }
                }
            }).ToList();

            //agrupar estado
            res.EncomendasEstado = enc.GroupBy(x => x.Estado).Select(n => new NomeTotalLista
            {
                Nome = GetEstadoEncomenda(n.Key),
                Total = n.Count()
            }).ToList();
            //agrupar tipo encomenda
            res.EncomendasTipoEncomenda = enc.GroupBy(x => x.TipoEncomenda.NomeTipoEncomenda).Select(n => new NomeTotalLista
            {
                Nome = n.Key != null ? n.Key.ToString() : semNomeDefault,
                Total = n.Count(),
                Totais = new[]
                {
                    new NomeTotalLista()
                    {
                        Nome = "Por Setor",
                        Totais = n.GroupBy(y => y.TipoEncomenda.SetorEncomenda).Select(j => new NomeTotalLista
                            {
                                Nome = j.Key != null ? j.Key.Nome.ToString() :semNomeDefault,
                                Total = j.Count()
                            }).ToArray()
                    },
                    new NomeTotalLista()
                    {
                        Nome = "Por Cliente",
                        Totais = n.GroupBy(y =>y.Cliente.NomeCliente).Select(j => new NomeTotalLista
                            {
                                Nome = j.Key != null ? j.Key.ToString() :semNomeDefault,
                                Total = j.Count()
                            }).ToArray()
                    }
                }
            }).ToList();


            List<Savs> savs = DbContext.Savs.
                Include("Cliente").Include("Produto").Include("TipoAvaria").Include("Departamento").Include("Setor")
                .Where(x => !x.Anulada && x.DataSav > pesq.DataPesquisaDesde && x.DataSav < pesq.DataPesquisaAte).ToList();
            res.SavsTotal = savs.Count;


            //agrupar por cliente
            res.SavsCliente = savs.GroupBy(x => x.Cliente).Select(n => new NomeTotalLista
            {
                Nome = n.Key != null ? n.Key.NomeCliente.ToString() : semNomeDefault,
                Total = n.Count()
            }).ToList();
            //agrupar por produto
            res.SavsProduto = savs.GroupBy(x => x.Produto).Select(n => new NomeTotalLista
            {
                Nome = n.Key != null ? n.Key.NomeProdutoSav.ToString() : semNomeDefault,
                Total = n.Count()
            }).ToList();

            //agrupar tipo avaria
            res.SavsTipoAvaria = savs.GroupBy(x => x.TipoAvaria).Select(n => new NomeTotalLista
            {
                Nome = n.Key != null ? n.Key.NomeTipoAvaria.ToString() : semNomeDefault,
                Total = n.Count()
            }).ToList();

            //agrupar departamento
            res.SavsDepartamento = savs.GroupBy(x => x.Departamento).Select(n => new NomeTotalLista
            {
                Nome = n.Key != null ? n.Key.NomeDepartamentoSav.ToString() : semNomeDefault,
                Total = n.Count()
            }).ToList();
            //agrupar setor
            res.SavsSetor = savs.GroupBy(x => x.Setor).Select(n => new NomeTotalLista
            {
                Nome = n.Key != null ? n.Key.Nome.ToString() : semNomeDefault,
                Total = n.Count()
            }).ToList();



            return res;

        }

        public string GetEstadoEncomenda(int num)
        {
            if (num == 0)
            {
                return "Pendente";
            }
            else if (num == 1)
            {
                return "Em Produção";
            }
            else if (num == 2)
            {
                return "Entregue";
            }
            else if (num == 3)
            {
                return "Cancelada";
            }
            else if (num == 4)
            {
                return "Pronta";
            }
            return semNomeDefault;
        }

    }
}