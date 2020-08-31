using IdentityServer.Infrastructure.Dto;
using IdentityServer.Infrastructure.Interfaces;
using IdentityServer.Infrastructure.Mappers;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Infrastructure.Services
{
    public class ExtendedClientStore : IClientStore
    {
        /// <summary>
        /// The DbContext.
        /// </summary>
        protected readonly IExtendedConfigurationDbContext Context;

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<ExtendedClientStore> Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientStore"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public ExtendedClientStore(IExtendedConfigurationDbContext context, ILogger<ExtendedClientStore> logger)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = logger;
        }

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<Core.Entities.ExtendedClient> FindExtClientByIdAsync(int clientId)
        {
            IQueryable<ExtendedClient> baseQuery = Context.ExtendedClient
                .Where(x => x.ClientId == clientId)
                .Take(1);

            var client = await baseQuery.FirstOrDefaultAsync();
            if (client == null) return null;

            var model = client.ToModel();

            Logger.LogDebug("{clientId} found in extended client table: {clientIdFound}", clientId, model != null);

            return model;
        }
    }
}
