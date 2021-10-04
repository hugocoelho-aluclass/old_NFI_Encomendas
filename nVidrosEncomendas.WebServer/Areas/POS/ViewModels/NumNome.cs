namespace NVidrosEncomendas.WebServer.Areas.POS.ViewModels
{
    public class IdNome
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    public class IdNomePreSelect : IdNome
    {
        public bool PreSeleccionado { get; set; }
    }

    public class IdNomeExtra : IdNome
    {
        public bool Extra { get; set; }
    }
    public class IdNomeText : IdNome
    {
        public string Text { get; set; }
    }
    public class IdNumNome : IdNome
    {
        public int Num { get; set; }
    }
}