using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FristList.Service.Data.Models.Account;
using FristList.Service.PublicApi.Configuration;
using FristList.Service.PublicApi.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace FristList.Service.PublicApi.Services.Implementations;

public class JwtTokenGenerator : ITokenGenerator
{
    private readonly JwtTokenGeneratorConfiguration _configuration;

    public JwtTokenGenerator(JwtTokenGeneratorConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Generate(User user)
    {
        var claims = new[] {new Claim(ClaimTypes.Name, user.NormalizedUserName)};
        var credentials = new SigningCredentials(_configuration.SecurityKey, _configuration.SecurityAlgorithm);
        var expires = DateTime.UtcNow.Add(_configuration.ExpiresPeriod);
        
        var token = new JwtSecurityToken(claims: claims, signingCredentials: credentials, expires: expires);
        var tokenValue = new JwtSecurityTokenHandler()
            .WriteToken(token);
        return tokenValue;
    }
}