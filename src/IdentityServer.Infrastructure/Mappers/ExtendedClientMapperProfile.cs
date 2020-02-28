using AutoMapper;

namespace IdentityServer.Infrastructure.Mappers
{
    public class ExtClientMapperProfile : Profile
    {
        public ExtClientMapperProfile()
        {
            CreateMap<IdentityServer4.Models.Client, IdentityServer4.EntityFramework.Entities.Client>()
                    .IncludeAllDerived();

            CreateMap<Dto.ExtClient, Core.Entities.ExtClient>()
                .ForPath(dest => dest.ExtendedClient.ClientType, opts => opts.MapFrom(src => src.ExtendedClient.ClientType))
                .ForPath(dest => dest.ExtendedClient.RawCertData, opts => opts.MapFrom(src => src.ExtendedClient.RawCertData))
                .ForPath(dest => dest.ExtendedClient.RequireJwe, opts => opts.MapFrom(src => src.ExtendedClient.RequireJwe))
                .ReverseMap();
        }
    }
}
