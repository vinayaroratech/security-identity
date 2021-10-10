using Microsoft.Extensions.DependencyInjection;

namespace VA.Security.API.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            // Resolve another dependencies here!

            return services;
        }
    }
}
