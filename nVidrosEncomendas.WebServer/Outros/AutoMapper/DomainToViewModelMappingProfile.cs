using AutoMapper;
using NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Recolhas;
using System;


namespace NVidrosEncomendas.WebServer.Outros.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public new string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            //EXEMPLOS
            //clientes
            // Mapper.CreateMap<Entities.Clientes, ClientesArea.ClientesEdit>().ForMember(x => x.Morada, o => o.MapFrom(u => u));
            //Mapper.CreateMap<Entities.Clientes, ClientesArea.Morada>();
            // Mapper.CreateMap<Models.Operadores, Areas.POS.ViewModels.Savs.Operadores>()
            //     .ForMember(x => x.NomeOperador, o => o.MapFrom( u => u.NomeCompleto))
            //     .ForMember(x => x.NumOperador, o => o.MapFrom(u => u.UtilizadorId));

            //Mapper.CreateMap<Models.EstadoSav, Areas.POS.ViewModels.Savs.EstadoEncomenda>()
            //     .ForMember(x => x.NomeEstadoSav, o => o.MapFrom( u => u.NomeEstadoSav))
            //     .ForMember(x => x.NumEstadoSav, o => o.MapFrom(u => u.IdEstadoSav));


            //IEnumerable<Models.EstadoSav>, IEnumerable<IdNome>
            Mapper.CreateMap<Models.Clientes, NVidrosEncomendas.WebServer.Areas.POS.ViewModels.IdNome>()
            .ForMember(x => x.Nome, o => o.MapFrom(u => u.NomeCliente))
            .ForMember(x => x.Id, o => o.MapFrom(u => u.NumCliente));

            //IEnumerable<Models.EstadoSav>, IEnumerable<IdNome>
            Mapper.CreateMap<Models.EstadoRecolha, NVidrosEncomendas.WebServer.Areas.POS.ViewModels.IdNome>()
            .ForMember(x => x.Nome, o => o.MapFrom(u => u.NomeEstado))
            .ForMember(x => x.Id, o => o.MapFrom(u => u.Id));


            Mapper.CreateMap<Models.EstadoSav, NVidrosEncomendas.WebServer.Areas.POS.ViewModels.IdNome>()
            .ForMember(x => x.Nome, o => o.MapFrom(u => u.NomeEstadoSav))
            .ForMember(x => x.Id, o => o.MapFrom(u => u.IdEstadoSav));

            Mapper.CreateMap<Models.EstadoSav, NVidrosEncomendas.WebServer.Areas.POS.ViewModels.IdNomePreSelect>()
            .ForMember(x => x.Nome, o => o.MapFrom(u => u.NomeEstadoSav))
            .ForMember(x => x.Id, o => o.MapFrom(u => u.IdEstadoSav))
            .ForMember(x => x.PreSeleccionado, o => o.MapFrom(u => u.PreSeleccionadoNaPesquisa));

            Mapper.CreateMap<Models.TipoAvarias, NVidrosEncomendas.WebServer.Areas.POS.ViewModels.IdNome>()
            .ForMember(x => x.Nome, o => o.MapFrom(u => u.NomeTipoAvaria))
            .ForMember(x => x.Id, o => o.MapFrom(u => u.NumTipoAvaria));

            Mapper.CreateMap<Models.TipoAvarias, NVidrosEncomendas.WebServer.Areas.POS.ViewModels.IdNomeExtra>()
            .ForMember(x => x.Nome, o => o.MapFrom(u => u.NomeTipoAvaria))
            .ForMember(x => x.Id, o => o.MapFrom(u => u.NumTipoAvaria))
            .ForMember(x => x.Extra, o => o.MapFrom(u => u.InfoExtra));

            Mapper.CreateMap<Models.ProdutoSav, NVidrosEncomendas.WebServer.Areas.POS.ViewModels.IdNome>()
            .ForMember(x => x.Nome, o => o.MapFrom(u => u.NomeProdutoSav))
            .ForMember(x => x.Id, o => o.MapFrom(u => u.NumProdutoSav));

            Mapper.CreateMap<Models.DepartamentoSav, NVidrosEncomendas.WebServer.Areas.POS.ViewModels.IdNome>()
            .ForMember(x => x.Nome, o => o.MapFrom(u => u.NomeDepartamentoSav))
            .ForMember(x => x.Id, o => o.MapFrom(u => u.NumDepartamentoSav));


            Mapper.CreateMap<Models.DepartamentoSav, NVidrosEncomendas.WebServer.Areas.POS.ViewModels.IdNomePreSelect>()
            .ForMember(x => x.Nome, o => o.MapFrom(u => u.NomeDepartamentoSav))
            .ForMember(x => x.Id, o => o.MapFrom(u => u.NumDepartamentoSav));

            Mapper.CreateMap<Models.Savs, NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.SavGet>()
            .ForMember(x => x.NomeSerie, o => o.MapFrom(u => u.SerieDoc.NumSerie))
            .ForMember(x => x.NumCliente, o => o.MapFrom(u => u.Cliente.NumCliente))
            .ForMember(x => x.DepartamentoNum, o => o.MapFrom(u => u.Departamento.IdDepartamentoSav))
            .ForMember(x => x.EstadoSavNum, o => o.MapFrom(u => u.Estado.IdEstadoSav))
            .ForMember(x => x.ProdutoNum, o => o.MapFrom(u => u.Produto.IdProdutoSav))
            .ForMember(x => x.TipoAvariaNum, o => o.MapFrom(u => u.TipoAvaria.IdTipoAvaria))
            .ForMember(x => x.CriadoPor, o => o.MapFrom(u => u.CriadoPor.NomeCompleto))
            .ForMember(x => x.DataResolvida, o => o.MapFrom(u => u.DataResolvida.Date))
            .ForMember(x => x.DataEstado, o => o.MapFrom(u => u.DataEstado.Date))
            .ForMember(x => x.DataRespostaAoCliente, o => o.MapFrom(u => u.DataRespostaAoCliente.HasValue ? u.DataRespostaAoCliente.Value.Date : u.DataRespostaAoCliente))
            .ForMember(x => x.Recolha, o => o.MapFrom(u => u.Recolha))
            .ForMember(x => x.SetorId, o => o.MapFrom(u => u.Setor.IdSetor))
            .ForMember(x => x.Recolha, o => o.NullSubstitute(new Models.Recolhas()));


            Mapper.CreateMap<Models.Savs, NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Savs.SavsLinha2>()
            .ForMember(x => x.QuemColocou, o => o.MapFrom(u => u.CriadoPor.NomeCompleto))
            .ForMember(x => x.NomeCliente, o => o.MapFrom(u => u.Cliente.NomeCliente))
            .ForMember(x => x.NomeDepartamento, o => o.MapFrom(u => u.Departamento.NomeDepartamentoSav))
            .ForMember(x => x.NomeProduto, o => o.MapFrom(u => u.Produto.NomeProdutoSav))
            .ForMember(x => x.NomeTipoAvaria, o => o.MapFrom(u => u.TipoAvaria.InfoExtra ? (u.TipoAvaria.NomeTipoAvaria + " - " + u.TipoAvariaExtra) : u.TipoAvaria.NomeTipoAvaria))
            .ForMember(x => x.NomeEstado, o => o.MapFrom(u => u.Estado.NomeEstadoSav))
            .ForMember(x => x.CssEstado, o => o.MapFrom(u => u.Estado != null ? u.Estado.SubEstado.ToString() : ""))
            .ForMember(x => x.DataEntrada, o => o.MapFrom(u => u.DataSav.ToString("yyyy-MM-dd")))
            .ForMember(x => x.DataResposta, o => o.MapFrom(u => u.DataRespostaAoCliente.HasValue ? u.DataRespostaAoCliente.Value.ToString("yyyy-MM-dd") : ""))
            .ForMember(x => x.DataExpedicao, o => o.MapFrom(u => u.DataExpedicao.HasValue ? u.DataExpedicao.Value.ToString("yyyy-MM-dd") : ""))
            .ForMember(x => x.DataResolvido, o => o.MapFrom(u => u.DataResolvida.ToString("yyyy-MM-dd")))
            .ForMember(x => x.EstadoResolvido, o => o.MapFrom(u => u.Estado.MarcaEncerrado))
            .ForMember(x => x.DireitoNC, o => o.MapFrom(u => u.DireitoNaoConformidade))
            .ForMember(x => x.NomeSerie, o => o.MapFrom(u => u.SerieDoc.NumSerie))
            .ForMember(x => x.NumDias, o => o.MapFrom(u => (((u.Estado.MarcaEncerrado ? u.DataResolvida.Date : DateTime.Now.Date) - u.DataSav.Date).TotalDays) + 1));


            Mapper.CreateMap<Models.Recolhas, RecolhaLinha>()
            .ForMember(x => x.DataChegadaPrevista, o => o.MapFrom(u => u.DataChegadaPrevista.ToString("yyyy-MM-dd")))
            .ForMember(x => x.DataPedidoRecolha, o => o.MapFrom(u => u.DataPedidoRecolha.ToString("yyyy-MM-dd")))
            .ForMember(x => x.DataRecolha, o => o.MapFrom(u => u.DataRecolha.ToString("yyyy-MM-dd")))
            .ForMember(x => x.EstadoRecolha, o => o.MapFrom(u => u.EstadoRecolha.NomeEstado))
            .ForMember(x => x.EstadoRecolhaCor, o => o.MapFrom(u => u.EstadoRecolha.Cor))
            .AfterMap((x, y) => y.AfterMap());


            //.ForMember(x => x.DataResposta, o => o.MapFrom(u => u.DataRespostaAoCliente.HasValue ? u.DataRespostaAoCliente.Value.ToString("yyyy-MM-dd") : ""))

            Mapper.CreateMap<Models.Anexos, NVidrosEncomendas.WebServer.Areas.POS.ViewModels.IdNome>()
            .ForMember(x => x.Nome, o => o.MapFrom(u => u.NomeFicheiro))
            .ForMember(x => x.Id, o => o.MapFrom(u => u.Id));

            Mapper.CreateMap<Models.EstadoRecolha, NVidrosEncomendas.WebServer.Areas.POS.ViewModels.IdNomeText>()
            .ForMember(x => x.Nome, o => o.MapFrom(u => u.NomeEstado))
            .ForMember(x => x.Text, o => o.MapFrom(u => u.Cor))
            .ForMember(x => x.Id, o => o.MapFrom(u => u.Id));

            Mapper.CreateMap<Models.Recolhas, Areas.POS.ViewModels.Recolhas.Recolha>();


            Mapper.CreateMap<Models.Setor, NVidrosEncomendas.WebServer.Areas.POS.ViewModels.IdNome>()
                    .ForMember(x => x.Nome, o => o.MapFrom(u => u.Nome))
                    .ForMember(x => x.Id, o => o.MapFrom(u => u.IdSetor));

            Mapper.CreateMap<Models.Setor, NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Setores.Setor>()
                     .ForMember(x => x.Nome, o => o.MapFrom(u => u.Nome))
                     .ForMember(x => x.Id, o => o.MapFrom(u => u.IdSetor));

        }
    }
}