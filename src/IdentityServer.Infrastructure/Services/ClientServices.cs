using AutoMapper;
using IdentityServer.Core.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using System.Linq;

namespace IdentityServer.Infrastructure.Services
{
    public class ClientServices : IClientServices
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public ClientServices(IUnitOfWork uow,
            IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        public IQueryable<IdentityServer4.Models.Client> Get()
        {
            var clients = uow.GetRepository<IdentityServer4.EntityFramework.Entities.Client>()
                .Get();
            return mapper.Map<IQueryable<IdentityServer4.EntityFramework.Entities.Client>,
                IQueryable<IdentityServer4.Models.Client>>(clients);
        }

        public void Add(IdentityServer4.Models.Client client)
        {
            uow.GetRepository<IdentityServer4.EntityFramework.Entities.Client>().Add(client.ToEntity());
            uow.Commit();
        }

        public void Delete(IdentityServer4.Models.Client client)
        {
            uow.GetRepository<IdentityServer4.EntityFramework.Entities.Client>().Add(client.ToEntity());
            uow.Commit();
        }

        public void Update(IdentityServer4.Models.Client client)
        {
            uow.GetRepository<IdentityServer4.EntityFramework.Entities.Client>().Update(client.ToEntity());
            uow.Commit();
        }
    }
}
