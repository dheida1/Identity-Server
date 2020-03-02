using AutoMapper;

namespace IdentityServer.Infrastructure.Mappers
{
    /// <summary>
    /// Extension methods to map to/from entity/model for clients.
    /// </summary>
    public static class ExtClientMappers
    {
        static ExtClientMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ExtClientMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static Core.Entities.ExtClient ToModel(this Dto.ExtClient dto)
        {
            return Mapper.Map<Core.Entities.ExtClient>(dto);
        }

        public static Dto.ExtClient ToEntity(this Core.Entities.ExtClient entity)
        {
            return Mapper.Map<Dto.ExtClient>(entity);
        }
    }
}