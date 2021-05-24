using FlowerShop.Infrastructure;
using FlowerShop.Infrastructure.Data;
using FlowerShop.Infrastructure.Repositories;
using FlowerShop.Infrastructure.Repositories.Interfaces;
using FlowerShop.Infrastructure.Services;
using FlowerShop.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Reflection;
using System.Text;

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

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:57678")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                        builder.WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddDbContext(Configuration.GetConnectionString("DefaultConnection"));

            services.Configure<JwtConfig>(Configuration.GetSection(nameof(JwtConfig)));
            services.Configure<ImageConfig>(Configuration.GetSection(nameof(ImageConfig)));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });
            services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            //Register repositories
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(s =>
            {
                var memoryCache = s.GetService<IMemoryCache>();
                var dbContext = s.GetService<AppDbContext>();

                IProductRepository concreteService = new ProductRepository(dbContext);
                IProductRepository cachingDecorator = new ProductRepositoryCachingDecorator(concreteService, memoryCache);

                return cachingDecorator;
            });

            //Register Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICartItemService, CartItemService>();

            services.AddScoped<IOrderCreatorServiceFactory, OrderCreatorServiceFactory>();

            services.AddScoped<IOrderCreatorStrategy, AnonymousOrderCreatorStrategy>();

            services.AddScoped<IOrderCreatorStrategy, RegularOrderCreatorStrategy>();

            //Used by nlog to get user identity
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Configure swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
                c.EnableAnnotations();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
               {
                 new OpenApiSecurityScheme
                 {
                   Reference = new OpenApiReference
                   {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                   }
                  },
                  Array.Empty<string>()
                }
              });

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
            app.UseCors();

            app.UseHttpsRedirection();

            app.UseCookiePolicy();

            app.UseSwagger();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}