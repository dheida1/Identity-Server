using IdentityServer.Core.Interfaces;
using IdentityServer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;
        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new GenericRepository<TEntity>(context);
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
