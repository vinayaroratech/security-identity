using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using VA.Security.API.Config;
using VA.Security.Identity.User;
using VA.Security.Identity;

namespace VA.Security.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // When you just want the default configuration
            services.AddDefaultIdentityConfiguration(Configuration);


            // When you have specifics configurations (see inside this method)
            //services.AddCustomIdentityConfiguration(Configuration);

            // When you have specifics configurations (with Key type [see inside this method])
            //services.AddCustomIdentityAndKeyConfiguration(Configuration);

            // Setting the interactive AspNetUser (logged in)
            services.AddCurrentUserContextConfiguration();

            services.AddSwaggerConfiguration();

            services.AddDependencyInjectionConfiguration();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerConfiguration();

            app.UseHttpsRedirection();

            app.UseRouting();

            // Custom VA.Security abstraction here!
            app.UseAuthConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}