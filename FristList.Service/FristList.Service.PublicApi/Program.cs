using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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

app.UseSwagger();
app.UseSwaggerUI();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
