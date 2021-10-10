using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VA.Security.Identity;
using VA.Security.Identity.Jwt;

namespace VA.Security.API.Config
{
    public static class DefaultIdentityConfig
    {
        public static void AddDefaultIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Default EF Context for Identity (inside of the VA.Security.Identity)
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddIdentityEntityFrameworkContextConfiguration(options =>
                    options.UseInMemoryDatabase("VASecurityDb"));
            }

            if (configuration.GetValue<string>("DBProvider").ToLower().Equals("mssql")
                && !configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddIdentityEntityFrameworkContextConfiguration(options =>
                     options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                            b => b.MigrationsAssembly(typeof(Identity.Abstractions).Assembly.FullName)));
            }
            else if (configuration.GetValue<string>("DBProvider").ToLower().Equals("postgresql")
                && !configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddIdentityEntityFrameworkContextConfiguration(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(Identity.Abstractions).Assembly.FullName)));
            }

            // Default Identity configuration
            services.AddIdentityConfiguration()
                .AddSignInManager()
                .AddUserManager<UserManager<IdentityUser>>();  // <== This extension returns IdentityBuilder to extends configuration

            // Default JWT configuration
            services.AddJwtConfiguration(configuration, "AppSettings");
        }
    }
}