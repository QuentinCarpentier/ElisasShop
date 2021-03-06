﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ElisasShop.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace ElisasShop
{
    public class Startup
    {
        // Entry point to the Configuration data
        private IConfigurationRoot _configurationRoot;

        // Hosting environment is used to know where the application is running
        public Startup(IHostingEnvironment hostingEnvironment)
        {
            // Set the root path and the json file (for the ConnectionString)to our new ConfigurationBuilder
            _configurationRoot = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build(); // Build the configuration
        }

        // Add services to the container. IServiceCollection represents all registered services of our ASP.NET Core App
        // Dependencies can be added here aswell. (used for dependencies injection)
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the DbContext and get the ConnectionString from appsettings.json
            services.AddDbContext<AppDbContext>(options => 
                                        options.UseSqlServer(_configurationRoot.GetConnectionString("DefaultConnection")));

            // Add to the service collection the PieRepository as an implementation of the IPieRepository interface.
            // AddTransient method results in a new instance of the PieRepository every time I'll ask for an IPieRepository 
            services.AddTransient<IPieRepository, PieRepository>();
            // Same here for the Category
            services.AddTransient<ICategoryRepository, CategoryRepository>();

            // Allow to access to the context
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // AddScoped will create an object that is associated with the request  
            // It means that when I'll browse to the site, I'll get my own unique instance to the ShoppingCart
            services.AddScoped<ShoppingCart>(sp => ShoppingCart.GetCart(sp));

            // Add Mvc support
            services.AddMvc();

            // Allow to access to the session
            services.AddMemoryCache();
            services.AddSession();
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
            // Middleware to work with session (have to be before the DefaultRoute method)
            app.UseSession();
            // Support for a very simple and basic route.
            app.UseMvcWithDefaultRoute();

            DbInitializer.Seed(app);

            // Added with Scaffolding
            loggerFactory.AddConsole(_configurationRoot.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
