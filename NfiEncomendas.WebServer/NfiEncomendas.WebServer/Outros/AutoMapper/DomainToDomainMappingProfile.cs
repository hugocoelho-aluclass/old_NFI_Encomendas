using AutoMapper;


namespace NfiEncomendas.WebServer.Outros.AutoMapper
{
    public class DomainToDomainMappingProfile : Profile
    {
        public string ProfileName
        {
            get { return "DomainToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Models.Savs, Models.Savs>()
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.CriadoPor, o => o.Ignore())
                .ForMember(x => x.CriadoData, o => o.Ignore())
                .ForMember(x => x.EditadoPor, o => o.Ignore())
                .ForMember(x => x.EditadoData, o => o.Ignore())
                .ForMember(x => x.Anexos, o => o.Ignore())
                .ForMember(x => x.Recolha, o => o.Ignore())
                .ForMember(x => x.Setor, o => o.Ignore());
            ;
        }
    }
}