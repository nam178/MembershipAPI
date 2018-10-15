using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Membership.Core.Membership
{
    public static class DependencyInjection
    {
        public static void AddMembershipServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddTransient<ICreateUserService, CreateUserService>()
                .AddTransient<IPasswordPolicy, PasswordPolicySimple>()
                .AddTransient<IUpdatePasswordService, UpdatePasswordService>()
                ;
        }
    }
}
