using AutoMapper;

namespace NVidrosEncomendas.WebServer.Outros.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {

            Mapper.CreateMap<Areas.POS.ViewModels.Savs.Savs, Models.Savs>()
                .ForMember(x => x.Anexos, o => o.Ignore());

            Mapper.CreateMap<Areas.POS.ViewModels.Recolhas.Recolha, Models.Recolhas>();
            //Mapper.CreateMap<Areas.POS.ViewModels.Setores.Setor, Models.Setor>()
            //     .ForMember(x => x.IdSetor, o => o.MapFrom(u => u.Id))       
            //;

        }
    }
}