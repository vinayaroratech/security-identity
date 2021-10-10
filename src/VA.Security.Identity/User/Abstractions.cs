using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace VA.Security.Identity.User
{
    public static class Abstractions
    {
        public static IServiceCollection AddCurrentUserContextConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICurrentUserContext, CurrentUserContext>();

            return services;
        }
    }
}