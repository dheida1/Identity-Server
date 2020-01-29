using AutoMapper;
using IdentityServer.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IdentityServer.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IUnitOfWork<DbContext> unitOfWork;
        private readonly IMapper mapper;

        public Repository(DbContext context)
        {
            //UnitOfWork<DbContext> uow = new UnitOfWork(context);
            //this.unitOfWork = uow;
        }

        public IQueryable<T> Get()
        {
            return unitOfWork.Context.Set<T>().AsQueryable<T>();
        }

        public void Add(T entity)
        {
            unitOfWork.Context.Add(entity);
            return;
        }

        public void Delete(T entity)
        {
            unitOfWork.Context.Remove(entity);
            return;
        }
        public void Update(T entity)
        {
            unitOfWork.Context.Update(entity);
            return;
        }
    }
}
