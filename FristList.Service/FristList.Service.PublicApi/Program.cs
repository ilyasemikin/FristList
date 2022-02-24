using System.Text;
using AutoMapper;
using FluentValidation.AspNetCore;
using FristList.Service.Data;
using FristList.Service.Data.Models.Account;
using FristList.Service.Data.Models.Activities;
using FristList.Service.Data.Models.Categories;
using FristList.Service.PublicApi.Configuration;
using FristList.Service.PublicApi.Data.Activities;
using FristList.Service.PublicApi.Data.Categories;
using FristList.Service.PublicApi.Data.Users;
using FristList.Service.PublicApi.Filters;
using FristList.Service.PublicApi.Services;
using FristList.Service.PublicApi.Services.Abstractions;
using FristList.Service.PublicApi.Services.Implementations;
using FristList.Service.PublicApi.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ApiContext");
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(connectionString, b =>
    {
        b.MigrationsAssembly("FristList.Service.PublicApi");
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

var mapperConfig = new MapperConfiguration(config =>
{
    config.CreateMap<PersonalCategory, ApiCategory>()
        .ForMember(c => c.Id, opt => opt.MapFrom(c => c.Id))
        .ForMember(c => c.Name, opt => opt.MapFrom(c => c.Name));

    config.CreateMap<Activity, ApiActivity>()
        .ForMember(a => a.Id, opt => opt.MapFrom(a => a.Id))
        .ForMember(a => a.BeginAt, opt => opt.MapFrom(a => a.BeginAt))
        .ForMember(a => a.EndAt, opt => opt.MapFrom(a => a.EndAt))
        .ForMember(a => a.Categories, opt => opt.MapFrom(a => a.Categories.Select(c => c.Category)));

    config.CreateMap<CurrentActivity, ApiCurrentActivity>()
        .ForMember(a => a.BeginAt, opt => opt.MapFrom(a => a.BeginAt))
        .ForMember(a => a.Categories, opt => opt.MapFrom(a => a.Categories));
    
    config.CreateMap<User, ApiUser>()
        .ForMember(u => u.UserName, opt => opt.MapFrom(u => u.UserName));
});
builder.Services.AddSingleton(_ => mapperConfig.CreateMapper());

builder.Services.AddModelServices();

builder.Services.AddControllers(config =>
    {
        config.Filters.Add<RequestContextActionFilter>();
    })
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssembly(typeof(Program).Assembly);
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
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
    });
   
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    options.OperationFilter<AuthorizationOperationFilter>();
});

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
