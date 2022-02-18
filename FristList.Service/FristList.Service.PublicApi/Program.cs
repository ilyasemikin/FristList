using System.Text;
using FluentValidation.AspNetCore;
using FristList.Service.Data;
using FristList.Service.Data.Models.Account;
using FristList.Service.PublicApi.Configuration;
using FristList.Service.PublicApi.Services.Abstractions;
using FristList.Service.PublicApi.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ApiContext");
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(connectionString, b =>
    {
        b.MigrationsAssembly("FristList.Service.PostgreSqlMigrations");
    }));

builder.Services.AddIdentityCore<User>(options =>
    {
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 3;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<AppDbContext>();

// TODO: delegate configuration to configuration infrastructure
var userTokensManagerConfiguration = new UserTokensManagerConfiguration
{
    RefreshTokenExpiresTimePeriod = TimeSpan.FromDays(15)
};
var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Very very very very secret string"));
var jwtTokenGeneratorConfiguration =
    new JwtTokenGeneratorConfiguration(secret, SecurityAlgorithms.HmacSha512, TimeSpan.FromHours(2));
builder.Services.AddTransient<IUserTokensManager, UserTokensManager>(provider =>
    new UserTokensManager(userTokensManagerConfiguration, provider.GetRequiredService<AppDbContext>(),
        new JwtTokenGenerator(jwtTokenGeneratorConfiguration), new RefreshTokenGenerator(128)));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = secret,
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssembly(typeof(Program).Assembly);
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "FristList API",
        Description = "An Web API for managing time and tasks",
        Contact = new OpenApiContact
        {
            Name = "Ilya Semikin",
            Email = "iasemikin@gmail.com"
        }
    }));

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
