using AutoMapper;


namespace NfiEncomendas.WebServer.Outros.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToViewModelMappingProfile>();
                x.AddProfile<ViewModelToDomainMappingProfile>();
                x.AddProfile<DomainToDomainMappingProfile>();
            });
        }
    }
}