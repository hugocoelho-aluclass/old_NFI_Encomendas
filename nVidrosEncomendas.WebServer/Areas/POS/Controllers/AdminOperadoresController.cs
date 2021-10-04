using AutoMapper;
using NVidrosEncomendas.WebServer.BusinessLogic;
using NVidrosEncomendas.WebServer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NVidrosEncomendas.WebServer.Areas.POS.Controllers
{
    public class AdminOperadoresController : ApiController
    {
        [HttpGet]
        [Authorize]
        public List<ViewModels.Admin.Operador> TabelaOperadores()
        {
            if (!SessionObject.GetMySessionObject().RoleAdmin)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }

            BusinessLogic.AdminUtilizadoresBL bl = new BusinessLogic.AdminUtilizadoresBL();
            ViewModels.Admin.PagGestaoOperadores vm = new ViewModels.Admin.PagGestaoOperadores();
            vm.OperadoresParaVM(bl.OperadoresLista().ToList());
            return vm.Operadores;
        }

        //   [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public string AtualizaOperador(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Admin.Operador op)
        {
            PermissoesBL.CheckPermissao(PermissoesBL.Permissoes.AdminOperadores_Editar);

            BusinessLogic.AdminUtilizadoresBL bl = new BusinessLogic.AdminUtilizadoresBL();
            bl.AtualizaOperador(op.ToModel());

            return "sucesso";
        }


        //   [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public ViewModels.Admin.PagGestaoOperadoresEdit EditarOperador(int id)
        {

            BusinessLogic.AdminUtilizadoresBL bl = new BusinessLogic.AdminUtilizadoresBL();
            ViewModels.Admin.PagGestaoOperadoresEdit res = new ViewModels.Admin.PagGestaoOperadoresEdit();
            BusinessLogic.DepartamentoSavsBL dep = new BusinessLogic.DepartamentoSavsBL(bl.DbContext);
            res.Departamentos = Mapper.Map<List<Models.DepartamentoSav>, List<ViewModels.IdNome>>(dep.DepartamentoSavsLista().ToList());
            res.OperadorParaVM(bl.LerOperador(id));

            KeyValuePair<int, int> antProx = bl.IdsProximos(id);
            res.idAnt = antProx.Key;
            res.idProx = antProx.Value;
            return res;
        }

        [HttpPost]
        public string AtualizaOperadorPassword(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Admin.PagGestaoOperadoresAtualizaPassword op)
        {
            BusinessLogic.AdminUtilizadoresBL bl = new BusinessLogic.AdminUtilizadoresBL();
            bl.AtualizarPassword(op.OperadorId, op.Md5Password);
            return "sucesso";
        }
    }
}
