using FristList.Services;
using Microsoft.Extensions.Configuration;

namespace FristList.WebApi.Services;

public class DatabaseConfiguration : IDatabaseConfiguration
{
    private readonly IConfiguration _configuration;

    public DatabaseConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetConnectionString()
        => _configuration.GetConnectionString("DefaultConnection");
}