using Microsoft.IdentityModel.Tokens;

namespace FristList.Service.PublicApi.Configuration;

public record JwtTokenGeneratorConfiguration(
    SymmetricSecurityKey SecurityKey, 
    string SecurityAlgorithm,
    TimeSpan ExpiresPeriod);