using NfiEncomendas.WebServer.BusinessLogic;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace NfiEncomendas.WebServer.Areas.POS.Controllers
{
    [Authorize]
    public class ClientesController : ApiController
    {
        [HttpGet]
        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Clientes.Cliente> TabelaClientes()
        {
            var res = from c in (new ClientesBL()).ClientesListaAnulados()
                      select new ViewModels.Clientes.Cliente
                      {
                          Nome = c.NomeCliente,
                          NumCliente = c.NumCliente,
                          Anulado = c.Anulado
                      };
            return res.OrderBy(x => x.NumCliente).ToList();
        }

        [HttpGet]
        public ViewModels.Clientes.PagGestaoClienteesEdit EditCliente(int id)
        {
            BusinessLogic.ClientesBL bl = new BusinessLogic.ClientesBL();
            ViewModels.Clientes.PagGestaoClienteesEdit res = new ViewModels.Clientes.PagGestaoClienteesEdit();
            res.ClienteParaVM(bl.LerCliente(id));
            KeyValuePair<int, int> antProx = bl.IdsProximos(id);
            res.idAnt = antProx.Key;
            res.idProx = antProx.Value;

            return res;
        }

        [HttpPost]
        public string AtualizaCliente(NfiEncomendas.WebServer.Areas.POS.ViewModels.Clientes.Cliente cliente)
        {
            PermissoesBL.CheckPermissao(PermissoesBL.Permissoes.Clientes_Editar);

            BusinessLogic.ClientesBL bl = new BusinessLogic.ClientesBL();
            Models.Clientes c = cliente.ToModel();
            bl.AtualizaCliente(c);

            return "sucesso";
        }
    }
}
