using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace NVidrosEncomendas.WebServer.Models
{
    public class Savs
    {
        [Key]
        public int Id { get; set; }

        public DateTime DataSav { get; set; } = DateTime.Now;

        public Clientes Cliente { get; set; }
        public ProdutoSav Produto { get; set; }
        public TipoAvarias TipoAvaria { get; set; }
        public string TipoAvariaExtra { get; set; }
        public DepartamentoSav Departamento { get; set; }
        public string DescricaoSav { get; set; }
        public string Causa { get; set; }
        public string AcaoImplementar { get; set; }


        public Series SerieDoc { get; set; }
        public int NumDoc { get; set; }

        // public DateTime DataEntrada { get; set; }
        public DateTime DataEstado { get; set; } = DateTime.Now;
        public EstadoSav Estado { get; set; }
        public string NotasAdicionais { get; set; }

        public bool MarcarResolvida { get; set; }
        public DateTime DataResolvida { get; set; } = DateTime.Now;
        public string NotasResolvida { get; set; }

        public bool MarcarRespostaAoCliente { get; set; }
        public DateTime? DataRespostaAoCliente { get; set; } = DateTime.Now;
        public DateTime? DataExpedicao { get; set; }

        public string RespostaAoCliente { get; set; }
        public bool DireitoNaoConformidade { get; set; }

        public virtual Operadores CriadoPor { get; set; }
        public DateTime CriadoData { get; set; }

        public virtual Operadores EditadoPor { get; set; }
        public DateTime EditadoData { get; set; }

        public bool Anulada { get; set; }

        public List<Anexos> Anexos { get; set; } = new List<Anexos>();

        public string Ref { get; set; } = "";

        public int SemanaEntrega { get; set; } = 0;

        public double? Custos { get; set; }
        public double? CustosTransporte { get; set; }
        public string CustosDescricao { get; set; }

        public bool TemRecolha { get; set; } = false;
        public Recolhas Recolha { get; set; }

        public Setor Setor { get; set; }

        public Savs()
        {
        }

        public string NumDocStr()
        {
            return NumDoc.ToString().PadLeft(10, '0');
        }
    }
}