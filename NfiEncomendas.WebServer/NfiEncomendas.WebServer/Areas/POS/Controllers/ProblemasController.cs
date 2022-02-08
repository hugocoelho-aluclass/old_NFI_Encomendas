using NfiEncomendas.WebServer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NfiEncomendas.WebServer.Areas.POS.Controllers
{
    public class ProblemasController : ApiController
    {
        [HttpGet]
        public ViewModels.Problemas.PagGestaoProblemas EditProblema(int id)
        {
            BusinessLogic.ProblemaBL bl = new BusinessLogic.ProblemaBL();
            BusinessLogic.DepartamentoSavsBL depBl = new BusinessLogic.DepartamentoSavsBL();
            ViewModels.Problemas.PagGestaoProblemas res = new ViewModels.Problemas.PagGestaoProblemas();
            res.Problema.ProblemaParaVM(bl.LerProblema(id));

            res.Departamentos = (from s in depBl.DepartamentoSavsLista()
                                 select new ViewModels.Problemas.Departamento
                                 {
                                     IdDepartamento = s.IdDepartamentoSav,
                                     NumDepartamento = s.NumDepartamentoSav,
                                     NomeDepartamento = s.NomeDepartamentoSav
                                 }).ToList();

            KeyValuePair<int, int> antProx = bl.IdsProximos(id);
            res.idAnt = antProx.Key;
            res.idProx = antProx.Value;

            return res;
        }

        [HttpPost]
        public int AtualizaProblemas(NfiEncomendas.WebServer.Areas.POS.ViewModels.Problemas.Problema Problema)
        {

            BusinessLogic.ProblemaBL bl = new BusinessLogic.ProblemaBL();
            //bl.AtualizaProblema(Problema.ToModel(bl.DbContext));

            return bl.AtualizaProblema(Problema.ToModel(bl.DbContext)); ;
        }


        [HttpGet]

        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Problemas.Problema> TabelaProblemas()
        {
            var res = from c in (new ProblemaBL()).ProblemasListaFechados()
                      select new ViewModels.Problemas.Problema
                      {

                          IdProblema = c.IdProblema,
                          Nome = c.Nome,
                          Descricao = c.Descricao,
                          DescricaoCausa = c.DescricaoCausa,
                          Acompanhamento = c.Acompanhamento != null ? c.Acompanhamento : "",
                          AcaoImplementar = c.AcaoImplementar != null ? c.AcaoImplementar : "",
                          DataCriacao = c.DataCriacao,
                          Eficacia = c.Eficacia,
                          AvaliacaoEficacia = c.AvaliacaoEficacia != null ? c.AvaliacaoEficacia : "",
                          Fechado = c.Fechado,
                          IdAnterior = c.IdAnterior,
                          idDepartamento = c.Departamento.IdDepartamentoSav,
                          numDepartamento = c.Departamento.NumDepartamentoSav,
                          nomeDepartamento = c.Departamento.NomeDepartamentoSav
                          
                      };

            return res.OrderBy(x => x.IdProblema).ToList();
        }



    }
}