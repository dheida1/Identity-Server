//using AutoMapper;

//namespace IdentityServer.Infrastructure.Mappers
//{
//    public static class ApplicationRoleMappers
//    {
//        static ApplicationRoleMappers()
//        {
//            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationRoleMapperProfile>())
//                .CreateMapper();
//        }

//        internal static IMapper Mapper { get; }

//        public static Core.Entities.ApplicationRole ToModel(this Dto.ApplicationRole dto)
//        {
//            return Mapper.Map<Core.Entities.ApplicationRole>(dto);
//        }

//        public static Dto.ApplicationRole ToEntity(this Core.Entities.ApplicationRole entity)
//        {
//            return Mapper.Map<Dto.ApplicationRole>(entity);
//        }
//    }
//}
