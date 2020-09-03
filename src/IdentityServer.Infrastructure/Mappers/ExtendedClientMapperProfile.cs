//using IdentityServer4.EntityFramework.Mappers;

//namespace IdentityServer.Infrastructure.Mappers
//{
//    public class ExtClientMapperProfile : ClientMapperProfile
//    {
//        public ExtClientMapperProfile()
//        {
//            CreateMap<Dto.ExtendedClient, Core.Entities.ExtendedClient>()
//                .ReverseMap();

//            CreateMap<Core.Entities.ExtClient, Dto.ExtClient>()
//                .IncludeBase<IdentityServer4.Models.Client, IdentityServer4.EntityFramework.Entities.Client>();

//            CreateMap<Dto.ExtClient, Core.Entities.ExtClient>()
//                .IncludeBase<IdentityServer4.EntityFramework.Entities.Client, IdentityServer4.Models.Client>();
//        }
//    }
//}