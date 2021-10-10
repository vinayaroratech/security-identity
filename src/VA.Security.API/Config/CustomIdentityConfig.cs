using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VA.Security.Identity;
using VA.Security.Identity.Jwt;

namespace VA.Security.API.Config
{
    public static class CustomIdentityConfig
    {
        public static void AddCustomIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Your own EF Identity configuration - Use when you have another database like postgres

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<MyIdentityContext>(options =>
                    options.UseInMemoryDatabase("VASecurityDb"));
            }

            if (configuration.GetValue<string>("DBProvider").ToLower().Equals("mssql")
                && !configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<MyIdentityContext>(options =>
                     options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                            b => b.MigrationsAssembly(typeof(Identity.Abstractions).Assembly.FullName)));
            }
            else if (configuration.GetValue<string>("DBProvider").ToLower().Equals("postgresql")
                && !configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<MyIdentityContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(Identity.Abstractions).Assembly.FullName)));
            }


            // Your own Identity configuration
            services.AddCustomIdentity<MyIdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddCustomRoles<MyIdentityRoles>()
            .AddCustomEntityFrameworkStores<MyIdentityContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager()
            .AddUserManager<UserManager<MyIdentityUser>>();

            // Ours JWT configuration
            services.AddJwtConfiguration(configuration, "AppSettings");
        }
    }

    public class MyIdentityUser : IdentityUser
    {

    }

    public class MyIdentityRoles : IdentityRole
    {

    }

    public class MyIdentityContext : IdentityDbContext<MyIdentityUser, MyIdentityRoles, string>
    {
        public MyIdentityContext(DbContextOptions<MyIdentityContext> options) : base(options) { }
    }
}