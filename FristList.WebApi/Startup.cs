using System.Text;
using FristList.Data;
using FristList.Models;
using FristList.Models.Services;
using FristList.Services;
using FristList.Services.Abstractions;
using FristList.Services.PostgreSql.DependencyInjection;
using FristList.WebApi.Hubs;
using FristList.WebApi.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace FristList.WebApi;

public class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddIdentityCore<AppUser>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        });
        
        services.AddTransient<IDatabaseConfiguration, DatabaseConfiguration>();
        services.AddPostgreSqlStorage();

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

        services.AddTransient<IModelToDtoMapper, DefaultMapToDtoMapper>();
        services.AddTransient<IModelLinkPropertyAggregator, DefaultModelLinkPropertyAggregator>();
        services.AddTransient<IJwtTokenProvider>(_ => new JwtTokenDefaultProvider(new SymmetricSecurityKey(secret)));
        services.AddTransient<ITokenGenerator>(_ => new RandomBytesCryptoTokenGenerator(64));

        services.AddSingleton<IRealTimeClientsService, InMemoryRealTimeClientsService>();
        
        services.AddMediatR(typeof(Startup));

        services.AddControllers();

        services.AddSignalR();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        var storageInitializer = app.ApplicationServices.GetRequiredService<IRepositoryInitializer>();
        storageInitializer.InitializeAsync()
            .Wait();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<EventMessageHub>("/api/events");
            endpoints.MapControllers();
        });
    }
}