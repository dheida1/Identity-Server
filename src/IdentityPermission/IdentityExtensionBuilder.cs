using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Identity.ExtensionStore.IdentityPermission
{
    public class IdentityExtensionBuilder : IdentityBuilder
    {

        public IdentityExtensionBuilder(Type user, Type role, Type permission, IServiceCollection services) : base(user, role, services)
        {
            PermissionType = permission;
        }

        public Type PermissionType { get; private set; }

        private IdentityExtensionBuilder AddScoped(Type serviceType, Type concreteType)
        {
            Services.AddScoped(serviceType, concreteType);
            return this;
        }

        public virtual IdentityExtensionBuilder AddPermissionStore<TStore>() where TStore : class
        {
            if (PermissionType == null)
            {
                throw new InvalidOperationException("No Permissions Type");
            }
            return AddScoped(typeof(IPermissionStore<>).MakeGenericType(PermissionType), typeof(TStore));
        }
    }
}
