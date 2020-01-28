//using AutoMapper;
//using IdentityServer.Core.Interfaces;
//using IdentityServer4.EntityFramework.DbContexts;
//using IdentityServer4.EntityFramework.Mappers;
//using IdentityServer4.Models;
//using System.Linq;

//namespace IdentityServer.Infrastructure.Repositories
//{
//    public class ClientRepository : IRepository<Client>
//    {
//        private readonly ConfigurationDbContext context;
//        private readonly IMapper mapper;

//        public ClientRepository(ConfigurationDbContext context,
//            IMapper mapper)
//        {
//            this.context = context;
//            this.mapper = mapper;
//        }
//        public IQueryable<Client> Get()
//        {
//            var clientEntities = context.Clients.AsQueryable();
//            return clientEntities;
//        }
//        public async void Add(Client clientModel)
//        {
//            context.Clients.Add(clientModel.ToEntity());
//            await context.SaveChangesAsync();
//            return;
//        }

//        public async void Remove(Client clientModel)
//        {
//            context.Remove(clientModel.ToEntity());
//            await context.SaveChangesAsync();
//        }

//        public async void Update(Client clientModel)
//        {
//            context.Update(clientModel.ToEntity());
//            await context.SaveChangesAsync();
//        }
//    }
//}
