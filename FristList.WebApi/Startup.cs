using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FristList.Models;
using FristList.Services;
using FristList.Services.AbstractFactories;
using FristList.Services.PostgreSql;
using FristList.WebApi.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace FristList.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRepositoryAbstractFactory, PostgreSqlRepositoryAbstractFactory>();

            services.AddTransient(provider =>
                provider.GetRequiredService<IRepositoryAbstractFactory>().CreateStorageInitializer());
            services.AddTransient(provider =>
                provider.GetRequiredService<IRepositoryAbstractFactory>().CreateUserRepository());
            services.AddTransient(provider =>
                provider.GetRequiredService<IRepositoryAbstractFactory>().CreateActionRepository());
            services.AddTransient(provider =>
                provider.GetRequiredService<IRepositoryAbstractFactory>().CreateCategoryRepository());
            services.AddTransient(provider =>
                provider.GetRequiredService<IRepositoryAbstractFactory>().CreateTaskRepository());
            services.AddTransient(provider =>
                provider.GetRequiredService<IRepositoryAbstractFactory>().CreateProjectRepository());

            services.AddTransient<IActionManager, PostgreSqlActionManager>();
            
            services.AddIdentityCore<AppUser>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            var secret = Encoding.UTF8.GetBytes("VeryVeryLongSecret");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = false,
                        ValidateLifetime = true
                    };
                });

            services.AddTransient<IJwtTokenProvider>(_ => new JwtTokenDefaultProvider(new SymmetricSecurityKey(secret)));
            services.AddTransient<ITokenGenerator, RNGCryptoTokenGenerator>();
            services.AddTransient<IRefreshTokenProvider, PostgreSqlRefreshTokenProvider>();

            services.AddMediatR(typeof(Startup));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var storageInitializer = app.ApplicationServices.GetRequiredService<IStorageInitializer>();
            storageInitializer.InitializeAsync()
                .Wait();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
