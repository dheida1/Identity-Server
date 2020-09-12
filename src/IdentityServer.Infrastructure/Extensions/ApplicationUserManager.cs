//using IdentityServer.Infrastructure.Entities;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace IdentityServer.Infrastructure.Extensions
//{
//    public class ApplicationUserManager : UserManager<ApplicationUser>, IDisposable
//    {
//        private readonly IRoleStore<ApplicationRole> _roleStore;
//        private readonly IUserStore<ApplicationUser> _userStore;


//        public ApplicationUserManager(
//            IUserStore<ApplicationUser> userStore,
//            IRoleStore<ApplicationRole> roleStore,
//            IOptions<IdentityOptions> optionsAccessor,
//            IPasswordHasher<ApplicationUser> passwordHasher,
//            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
//            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
//            ILookupNormalizer keyNormalizer,
//            IdentityErrorDescriber errors,
//            IServiceProvider services,
//            ILogger<ApplicationUserManager> logger
//            )
//            : base(
//                  userStore,
//                  optionsAccessor,
//                  passwordHasher,
//                  userValidators,
//                  passwordValidators,
//                  keyNormalizer,
//                  errors,
//                  services,
//                  logger)
//        {
//            _roleStore = roleStore;
//            _userStore = userStore;
//        }

//        public async Task<List<Claim>> GetPermissions(ApplicationUser user)
//        {
//            var userRoles = await base.GetRolesAsync(user);
//        }

//    }
//}
