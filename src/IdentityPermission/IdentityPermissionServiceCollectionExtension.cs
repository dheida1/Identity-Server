using Microsoft.Extensions.DependencyInjection;

namespace Identity.IdentityPermission
{
    public static class IdentityPermissionServiceCollectionExtensions
    {

        public static IdentityPermissionBuilder AddIdentityPermission<TPermission>(
            this IServiceCollection services)
            where TPermission : class
        {
            return new IdentityPermissionBuilder(typeof(TPermission), services);
        }
    }
}
