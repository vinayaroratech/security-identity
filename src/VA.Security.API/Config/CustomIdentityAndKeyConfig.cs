using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VA.Security.Identity;
using VA.Security.Identity.Jwt;

namespace VA.Security.API.Config
{
    public static class CustomIdentityAndKeyConfig
    {
        public static void AddCustomIdentityAndKeyConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<MyIntIdentityContext>(options =>
                    options.UseInMemoryDatabase("VASecurityDb"));
            }

            if (configuration.GetValue<string>("DBProvider").ToLower().Equals("mssql")
                && !configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<MyIntIdentityContext>(options =>
                     options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                            b => b.MigrationsAssembly(typeof(Identity.Abstractions).Assembly.FullName)));
            }
            else if (configuration.GetValue<string>("DBProvider").ToLower().Equals("postgresql")
                && !configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<MyIntIdentityContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(Identity.Abstractions).Assembly.FullName)));
            }

            // Your own Identity configuration
            services.AddCustomIdentity<MyIntIdentityUser, int>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddCustomRoles<MyIntIdentityRoles, int>()
            .AddCustomEntityFrameworkStores<MyIntIdentityContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager()
            .AddUserManager<UserManager<MyIntIdentityUser>>();

            // Ours JWT configuration
            services.AddJwtConfiguration(configuration, "AppSettings");
        }
    }

    public class MyIntIdentityUser : IdentityUser<int>
    {

    }

    public class MyIntIdentityRoles : IdentityRole<int>
    {

    }

    public class MyIntIdentityContext : IdentityDbContext<MyIntIdentityUser, MyIntIdentityRoles, int>
    {
        public MyIntIdentityContext(DbContextOptions<MyIntIdentityContext> options) : base(options) { }
    }
}