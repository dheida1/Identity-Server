//using IdentityServer.Infrastructure.Data;
//using IdentityServer.Infrastructure.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//namespace IdentityServer.Api.Configurations
//{
//    public static class DatabaseConfigs
//    {
//        public static IServiceCollection AddDatabase(
//          this IServiceCollection services,
//          IConfiguration configuration)
//        {
//            var connectionString = configuration.GetConnectionString("DefaultConnection");

//            services.AddDbContext<ApplicationDbContext>(builder =>
//                 builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

//            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
//                {
//                    options.SignIn.RequireConfirmedEmail = true;
//                })
//                .AddEntityFrameworkStores<ApplicationDbContext>();

//            return services;
//        }
//    }
//}
