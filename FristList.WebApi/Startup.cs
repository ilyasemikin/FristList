using System;
using System.Text;
using FristList.Models;
using FristList.Models.Services;
using FristList.Services;
using FristList.Services.Abstractions;
using FristList.Services.PostgreSql;
using FristList.Services.PostgreSql.DependencyInjection;
using FristList.WebApi.Hubs;
using FristList.WebApi.Options;
using FristList.WebApi.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FristList.WebApi;

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
        services.AddIdentityCore<AppUser>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        });

        var connectionString = new PostgreSqlConnectionStringBuilder()
                .WithHost("postgres_image")
                .WithPort(5432)
                .WithDatabase("frist_list")
                .WithUsername("app")
                .WithPassword("pass")
            .Build();
        services.AddPostgreSqlStorage(connectionString);
        
        services.Configure<JwtGeneratorOptions>(_configuration.GetSection(nameof(JwtGeneratorOptions)));
        services.Configure<RefreshTokenGeneratorOptions>(
            _configuration.GetSection(nameof(RefreshTokenGeneratorOptions)));

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
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        
        services.AddTransient<TokenService>();
        services.AddTransient<IModelToDtoMapper, DefaultMapToDtoMapper>();
        services.AddTransient<IModelLinkPropertyAggregator, DefaultModelLinkPropertyAggregator>();
        services.AddTransient<IJwtTokenProvider>(provider =>
            new JwtTokenDefaultProvider(provider.GetRequiredService<IOptions<JwtGeneratorOptions>>(),
                new SymmetricSecurityKey(secret)));
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