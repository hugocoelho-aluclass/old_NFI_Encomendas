namespace NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Clientes
{
    public class Cliente
    {
        public int NumCliente { get; set; }
        public string Nome { get; set; }
        public bool Anulado { get; set; }

        public Cliente()
        {
        }

        public Models.Clientes ToModel()
        {
            Models.Clientes res = (new NVidrosEncomendas.WebServer.BusinessLogic.ClientesBL()).LerCliente(this.NumCliente); //new Models.Operadores();
            res.NomeCliente = this.Nome;
            res.Anulado = this.Anulado;
            res.NumCliente = this.NumCliente;

            return res;
        }
    }


    public class PagGestaoClienteesEdit
    {
        public Cliente Cliente { get; set; }
        public int idProx { get; set; }
        public int idAnt { get; set; }

        public PagGestaoClienteesEdit()
        {
            Cliente = new Cliente();
            idProx = 999;
            idAnt = 0;
        }

        public void ClienteParaVM(Models.Clientes item)
        {
            Cliente.NumCliente = item.NumCliente;
            Cliente.Nome = item.NomeCliente;
            Cliente.Anulado = item.Anulado;
        }
    }
}