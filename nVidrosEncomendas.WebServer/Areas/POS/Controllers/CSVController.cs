using NVidrosEncomendas.WebServer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace NVidrosEncomendas.WebServer.Areas.POS.Controllers
{
    public class CSVController : ApiController
    {

        [HttpPost]
        public string CsvClientes()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + ("csv/clientes.csv");
            StreamReader reader = new StreamReader(path, Encoding.GetEncoding("iso-8859-1"));


            string a = reader.ReadToEnd();
            string[] linhas = a.Split('\n');
            ClientesBL cbl = new ClientesBL();
            foreach (var item in linhas)
            {
                string[] dados = item.Split(';');
                if (dados.Count() < 2) continue;

                int num = 0;
                bool numParse = Int32.TryParse(dados[0], out num);
                if (!numParse) continue;


                Models.Clientes c = cbl.LerCliente(num);
                c.NomeCliente = dados[1];
                cbl.AtualizaCliente(c);
            }

            return a;
        }

        [HttpPost]
        public string CsvTiposEncomenda()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + ("csv/tiposEncomenda.csv");
            StreamReader reader = new StreamReader(path, Encoding.GetEncoding("iso-8859-1"));


            string a = reader.ReadToEnd();
            string[] linhas = a.Split('\n');
            TipoEncomendasBL cbl = new TipoEncomendasBL();
            foreach (var item in linhas)
            {
                string[] dados = item.Split(';');
                if (dados.Count() < 2) continue;

                int num = 0;
                bool numParse = Int32.TryParse(dados[0], out num);
                if (!numParse) continue;


                Models.TipoEncomendas c = cbl.LerTipoEncomenda(num);
                c.NomeTipoEncomenda = dados[1];
                cbl.AtualizaTipoEncomenda(c);
            }

            return a;
        }

        [HttpPost]
        public string CsvEncomendas()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + ("csv/encomendas.csv");
            StreamReader reader = new StreamReader(path, Encoding.GetEncoding("iso-8859-1"));


            string a = reader.ReadToEnd();
            string[] linhas = a.Split('\n');
            EncomendasBL encBl = new EncomendasBL();
            ClientesBL cbl = new ClientesBL(encBl.DbContext);
            TipoEncomendasBL teBl = new TipoEncomendasBL(encBl.DbContext);
            foreach (var item in linhas)
            {
                string[] dados = item.Split(';');
                if (dados.Count() < 2) continue;

                int num = 0;
                bool numParse = Int32.TryParse(dados[0], out num);
                if (!numParse) continue;

                DateTime dataDoc = DateTime.Parse(dados[5]);
                int numDoc = Int32.Parse(dados[2]);

                string serie = dataDoc < new DateTime(2014, 01, 01) ? "2013" : "2014";

                Models.Encomendas c = encBl.LerEncomenda(serie, numDoc).Key;
                //c. = dados[1];
                c.NumDoc = numDoc;
                c.Cliente = cbl.LerCliente(Int32.Parse(dados[0]));
                c.RefObra = dados[3];
                int semanEntrega = 0;
                bool semanaEntregaParse = Int32.TryParse(dados[4], out semanEntrega);
                c.SemanaEntrega = semanEntrega;
                c.DataPedido = dataDoc;
               

                int numVaos = 0;
                bool numValosParse = Int32.TryParse(dados[8], out numVaos);
                c.NumVidros = numVaos;
               
                
                c.Producao = dados[11];
             
                c.GuiaRemessa = dados[13];
                c.Notas = dados[14];

                int estado = 0;
                bool estadoParse = Int32.TryParse(dados[15], out estado);
                c.Estado = estado;

             //   if (c.DataExpedidoString != "") c.Estado = 2;

                encBl.AtualizaEncomenda(c);
            }



            return a;
        }

        [HttpPost]
        public string CsvTiposAvaria()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + ("csv/tipoAvariaSAV.csv");
            StreamReader reader = new StreamReader(path, Encoding.GetEncoding("iso-8859-1"));


            string a = reader.ReadToEnd();
            string[] linhas = a.Split('\n');
            TipoAvariasBL bl = new TipoAvariasBL();
            List<Models.TipoAvarias> todos = bl.TipoAvariasListaAnulados().ToList();

            foreach (var item in linhas)
            {
                string dados = item.Trim();
                if (!todos.Any(x => x.NomeTipoAvaria.Trim() == dados))
                {
                    Models.TipoAvarias nItem = bl.LerTipoAvaria(0);
                    nItem.NomeTipoAvaria = dados;
                    bl.AtualizaTipoAvaria(nItem);
                }
            }

            return a;
        }
        [HttpPost]
        public string CsvProdutos()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + ("csv/produtosSAV.csv");
            StreamReader reader = new StreamReader(path, Encoding.GetEncoding("iso-8859-1"));


            string a = reader.ReadToEnd();
            string[] linhas = a.Split('\n');
            ProdutoSavsBL bl = new ProdutoSavsBL();
            var todos = bl.ProdutoSavsListaAnulados().ToList();

            foreach (var item in linhas)
            {
                string dados = item.Trim();
                if (!todos.Any(x => x.NomeProdutoSav.Trim() == dados))
                {
                    Models.ProdutoSav nItem = bl.LerProdutoSav(0);
                    nItem.NomeProdutoSav = dados;
                    bl.AtualizaProdutoSav(nItem);
                }
            }

            return a;
        }

        [HttpPost]
        public string CsvDepartamentos()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + ("csv/departamentosSav.csv");
            StreamReader reader = new StreamReader(path, Encoding.GetEncoding("iso-8859-1"));


            string a = reader.ReadToEnd();
            string[] linhas = a.Split('\n');
            DepartamentoSavsBL bl = new DepartamentoSavsBL();
            var todos = bl.DepartamentoSavsListaAnulados().ToList();

            foreach (var item in linhas)
            {
                string dados = item.Trim();
                if (!todos.Any(x => x.NomeDepartamentoSav.Trim() == dados))
                {
                    Models.DepartamentoSav nItem = bl.LerDepartamentoSav(0);
                    nItem.NomeDepartamentoSav = dados;
                    bl.AtualizaDepartamentoSav(nItem);
                }
            }

            return a;
        }
    }
}
