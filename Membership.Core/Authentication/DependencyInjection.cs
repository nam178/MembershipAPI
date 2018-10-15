using System;
using Membership.Core.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Membership.Core.Authentication
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Add OAuth authentication for our tenant API.
        /// </summary>
        /// <see cref="SchemeName"/>
        public static void AddMembershipOAuth(this IServiceCollection services)
        {
            services
                .AddAuthentication(SchemeName.Member)
                // This adds Basic HTTP authentication,
                // allowing 3rd party tenant applications to request
                // authentication tokens.
                .AddScheme<AuthenticationSchemeOptions, TenantAppsAuthenticationHandler>(
                    SchemeName.TenantApps, null)
                // This adds Token authentication,
                // only allow clients with valid Bearer token to execute
                // API requests
                .AddScheme<AuthenticationSchemeOptions, MemberAuthenticationHandler>(
                    SchemeName.Member, null)
                ;

            services
                .AddTransient<ITenantAppsAuthenticationService, TenantAppsAuthenticationService>()
                .AddTransient<IMemberAuthenticationService, MemberAuthenticationService>()
                .AddTransient<IOAuthPasswordGrantService, OAuthPasswordGrantService>()
                .AddTransient<IDeactivationService, DeactivationService>()
                .AddTransient<IDeactivationServiceReadOnly, DeactivationService>()
                .AddScoped<ICurrentUserService, CurrentUserService>()
                ;
        }
    }
}
