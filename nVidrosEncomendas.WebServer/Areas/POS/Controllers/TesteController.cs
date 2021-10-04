using System.Collections.Generic;
using System.Web.Http;


namespace NVidrosEncomendas.WebServer.Areas.POS.Controllers
{
    [AllowAnonymous]
    [Route("api/teste")]
    public class TesteController : ApiController
    {
        // GET: api/Teste

        [HttpGet]
        //[Route("")]
        public IEnumerable<string> Get()
        {
            return new string[] { "TesteOK", "value1", "value2" };
        }

        //[HttpGet]
        //public string CriarUserAdmin()
        //{
        //    UtilizadoresGestaoBL utzBL = new UtilizadoresGestaoBL();
        //    string a = utzBL.Novo("admin", "admin@email.com", "admin");
        //    return "Criado Admin";
        //}

        //  [HttpGet]      
        //public IHttpActionResult CriarUserAdmin()
        //  {
        //     AdminUtilizadoresBL utzBL = new AdminUtilizadoresBL();
        //     string a = utzBL.Novo("admin", "admin@email.com", "admin");
        //     return Ok("ok");
        //  }

        //// GET: api/Teste/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Teste
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Teste/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Teste/5
        //public void Delete(int id)
        //{
        //}
    }
}
