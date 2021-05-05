using FlowerShop.Infrastructure;
using FlowerShop.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Reflection;
using FlowerShop.Infrastructure.Repositories;
using FlowerShop.Infrastructure.Repositories.Interfaces;
using FlowerShop.Infrastructure.Services.Interfaces;

namespace FlowerShop.Web
{
    public class Startup
    {

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext(Configuration.GetConnectionString("DefaultConnection"));

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            //Register repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            //Register Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IAddressService, AddressService>();

            //Configure swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
                c.EnableAnnotations();

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }


        public void Configure(IApplicationBuilder app)
        {

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseCookiePolicy();

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
