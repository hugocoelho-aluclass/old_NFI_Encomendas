using NVidrosEncomendas.WebServer.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NVidrosEncomendas.WebServer.BusinessLogic
{

    public class PermissoesBL
    {
        public enum Permissoes
        {
            AdminOperadores_Editar,
            Clientes_Editar,

            DepartamentosSav_Editar,

            EstadosSav_Editar,
            EstadosRecolha_Editar,

            ProdutosSav_Editar,

            Encomendas_Editar,
            Encomendas_Ver,
            SAVS_Editar,
            SAVS_Ver,

            Setores_Editar,
            SetoresEncomenda_Editar,

            Series_Editar,
            TiposAvaria_Editar,

            TiposEncomenda_Editar,
            VidrosExtra_Editar

        }
        public static bool CheckPermissao(Operadores op, Permissoes permissao)
        {
            var sj = SessionObject.GetMySessionObject(System.Web.HttpContext.Current);

            if (op.Comercial &&
               (permissao == Permissoes.Encomendas_Editar || permissao == Permissoes.SAVS_Editar))
            {
                return false;
            }
            return true;
        }
        public static void CheckPermissao(Permissoes permissao)
        {
            var sj = SessionObject.GetMySessionObject(System.Web.HttpContext.Current);
            var r = CheckPermissao(sj.OperadorObject, permissao);
            if (!r)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "" };
                throw new HttpResponseException(msg);
            }
        }
    }
}