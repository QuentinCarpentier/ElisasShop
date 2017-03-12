using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ElisasShop.Models;
using Microsoft.Extensions.Configuration;

namespace ElisasShop
{
    public class Startup
    {
        // Added with Scaffolding
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // Add services to the container. IServiceCollection represents all registered services of our ASP.NET Core App
        // Dependencies can be added here aswell.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add to the service collection the MockPieRepository as an implementation of the IPieRepository interface.
            // AddTransient method results in a new instance of the MockPieRepository every time I'll ask for an IPieRepository 
            services.AddTransient<IPieRepository, MockPieRepository>();
            // Same here for the Category
            services.AddTransient<ICategoryRepository, MockCategoryRepository>();

            // Add Mvc support
            services.AddMvc();
        }

        // Configure the HTTP request pipeline. (middle-ware components)
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Support for showing exceptions in the browser when something goes wrong.
            app.UseDeveloperExceptionPage();
            // Handles status code between 400 and 600.
            app.UseStatusCodePages();
            // Serve static files.
            app.UseStaticFiles();
            // Support for a very simple and basic route.
            app.UseMvcWithDefaultRoute();


            // Added with Scaffolding
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
