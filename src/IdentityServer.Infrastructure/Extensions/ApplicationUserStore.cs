//using IdentityServer.Infrastructure.Data;
//using IdentityServer.Infrastructure.Entities;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using System.Threading;
//using System.Threading.Tasks;

//namespace IdentityServer.Infrastructure.Extensions
//{
//    public class ApplicationUserStore : UserStore<ApplicationUser>, IUserStore<ApplicationUser>
//    {
//        private readonly ApplicationDbContext _context;
//        public ApplicationUserStore(ApplicationDbContext context) : base(context)
//        {
//            _context = context;
//        }

//        public override async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
//        {
//            return await base.CreateAsync(user, cancellationToken);
//        }

//        public override async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
//        {
//            return await base.DeleteAsync(user, cancellationToken);
//        }

//        public override async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
//        {
//            return await base.FindByIdAsync(userId, cancellationToken);
//        }

//        public override async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
//        {
//            return await base.FindByNameAsync(normalizedUserName, cancellationToken);
//        }

//        public override async Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
//        {
//            return await base.GetNormalizedUserNameAsync(user, cancellationToken);
//        }

//        public override async Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
//        {
//            return await base.GetUserIdAsync(user, cancellationToken);
//        }

//        public override async Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
//        {
//            return await base.GetUserNameAsync(user, cancellationToken);
//        }

//        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
//        {
//            throw new System.NotImplementedException();
//        }

//        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
//        {
//            throw new System.NotImplementedException();
//        }

//        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}
