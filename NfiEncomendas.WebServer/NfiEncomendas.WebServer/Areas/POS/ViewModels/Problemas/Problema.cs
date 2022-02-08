using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace NfiEncomendas.WebServer.Areas.POS.ViewModels.Problemas
{
    public class Problema
    {

        public int IdProblema { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string DescricaoCausa { get; set; }

        public string Acompanhamento { get; set; }

        public string AcaoImplementar { get; set; }

        public DateTime DataCriacao { get; set; }

        public int? Eficacia { get; set; }

        public string AvaliacaoEficacia { get; set; }

        public bool Fechado { get; set; }

        public int idDepartamento { get; set; }

        public int numDepartamento { get; set; }

        public string nomeDepartamento { get; set; }

        public DateTime? DataAvaliacao { get; set; }

        public int? IdAnterior { get; set; }

        public Problema()
        {
        }


        public void ProblemaParaVM(Models.Problemas item)
        {
            this.IdProblema = item.IdProblema;
            this.Nome = item.Nome;
            this.Descricao = item.Descricao;
            this.DescricaoCausa = item.DescricaoCausa;
            this.Acompanhamento = item.Acompanhamento;
            this.AcaoImplementar = item.AcaoImplementar;
            this.Eficacia = item.Eficacia;
            this.AvaliacaoEficacia = item.AvaliacaoEficacia;
            this.Fechado = item.Fechado;
            this.DataCriacao = item.DataCriacao;
            this.DataAvaliacao = item.DataAvaliacao;
            this.IdAnterior = item.IdAnterior;

            if (item.Departamento != null)
            {
                this.idDepartamento = item.Departamento.IdDepartamentoSav;
                this.numDepartamento = item.Departamento.NumDepartamentoSav;
                this.nomeDepartamento = item.Departamento.NomeDepartamentoSav;
            }

        }


        public Models.Problemas ToModel(Models.AppDbContext db)
        {
            Models.Problemas res = (new NfiEncomendas.WebServer.BusinessLogic.ProblemaBL(db)).LerProblema(this.IdProblema);
            BusinessLogic.DepartamentoSavsBL depBl = new BusinessLogic.DepartamentoSavsBL(db);

            if (res.Departamento == null || res.Departamento.IdDepartamentoSav != this.idDepartamento)
            {
                res.Departamento = depBl.LerDepartamentoSav(this.idDepartamento);
            }

            res.Nome = this.Nome;
            res.Descricao = this.Descricao;
            res.DescricaoCausa = this.DescricaoCausa;
            res.Acompanhamento = this.Acompanhamento;
            res.AcaoImplementar = this.AcaoImplementar;
            res.Eficacia = this.Eficacia;
            res.DataCriacao = this.DataCriacao;
            res.AvaliacaoEficacia = this.AvaliacaoEficacia;
            res.Fechado = this.Fechado;
            res.IdAnterior = this.IdAnterior;

            return res;
        }

    }

    public class Departamento
    {
        public int IdDepartamento { get; set; }

        public int NumDepartamento { get; set; }

        public string NomeDepartamento { get; set; }

    }

    public class PagGestaoProblemas
    {

        public Problema Problema { get; set; }

        public List<Departamento> Departamentos { get; set; }

        public int idProx { get; set; }
        public int idAnt { get; set; }

        public PagGestaoProblemas()
        {
            Problema = new Problema();
        }


    }
}