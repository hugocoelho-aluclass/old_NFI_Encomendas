using System;
using System.Collections.Generic;

namespace NfiEncomendas.WebServer.Areas.POS.ViewModels.Estatisticas
{
    public class Estatisticas
    {
        public int EncomendasTotal { get; set; }
        public List<NomeTotalLista> EncomendasSetor { get; set; }
        public List<NomeTotalLista> EncomendasCliente { get; set; }
        public List<NomeTotalLista> EncomendasEstado { get; set; }
        public List<NomeTotalLista> EncomendasTipoEncomenda { get; set; }



        public int SavsTotal { get; set; }

        public List<NomeTotalLista> Savs { get; set; }

        //agrupar por cliente

        //agrupar por produto

        //agrupar tipo avaria

        //agrupar departamento

        //agrupar setor


        public List<NomeTotalLista> SavsCliente { get; set; }
        public List<NomeTotalLista> SavsProduto { get; set; }
        public List<NomeTotalLista> SavsTipoAvaria { get; set; }
        public List<NomeTotalLista> SavsDepartamento { get; set; }
        public List<NomeTotalLista> SavsSetor { get; set; }
    }

    public class NomeTotal
    {
        public string Nome { get; set; }
        public int Total { get; set; }

    }

    public class NomeTotalLista
    {
        public string Nome { get; set; }
        public int Total { get; set; }
        public NomeTotalLista[] Totais { get; set; }
    }

    public class PesquisaEstatisticas
    {
        public DateTime DataPesquisaDesde { get; set; }
        public DateTime DataPesquisaAte { get; set; }


    }
}