
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Threading;

//namespace IdentityServer.Core.Entities
//{
//    public class ApplicationRoleManager : RoleManager<ApplicationRole>, IDisposable
//    {
//        private readonly CancellationToken _cancel;
//        public ApplicationRoleManager(IRoleStore<ApplicationRole> store,
//            IEnumerable<IRoleValidator<ApplicationRole>> roleValidators,
//            ILookupNormalizer keyNormalizer,
//            IdentityErrorDescriber errors,
//            ILogger<RoleManager<ApplicationRole>> logger,
//            IHttpContextAccessor contextAccessor)
//            : base(store, roleValidators, keyNormalizer, errors, logger)
//        {
//            _cancel = contextAccessor?.HttpContext?.RequestAborted ?? CancellationToken.None;
//        }
//        protected override CancellationToken CancellationToken => _cancel;
//        //public virtual async Task<IList<Permission>> GetPermissionsAsync(ApplicationRole role,
//        //        CancellationToken cancellationToken = default(CancellationToken))
//        //{
//        //    ThrowIfDisposed();
//        //    if (role == null)
//        //    {
//        //        throw new ArgumentNullException(nameof(role));
//        //    }
//        //    var roleStore = await Store.FindByIdAsync(role.Id, _cancel);
//        //    return roleStore.Permissions;
//        //}
//    }
//}

