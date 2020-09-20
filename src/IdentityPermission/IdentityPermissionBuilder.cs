using Microsoft.Extensions.DependencyInjection;
using System;

namespace Identity.ExtensionStore.IdentityPermission
{
    public class IdentityPermissionBuilder
    {
        public IServiceCollection Services { get; private set; }
        public Type PermissionType { get; private set; }
        public IdentityPermissionBuilder(Type permission, IServiceCollection services)
        {
            Services = services;
            PermissionType = permission;
        }

        private IdentityPermissionBuilder AddScoped(Type serviceType, Type concreteType)
        {
            Services.AddScoped(serviceType, concreteType);
            return this;
        }

        public virtual IdentityPermissionBuilder AddPermissionStore<TStore>() where TStore : class
        {
            if (PermissionType == null)
            {
                throw new InvalidOperationException("No Permissions Type");
            }
            return AddScoped(typeof(IPermissionStore<>).MakeGenericType(PermissionType), typeof(TStore));
        }

        //TODO add permission IPermission, IPermissionValidator, PermissionManager
    }
}
