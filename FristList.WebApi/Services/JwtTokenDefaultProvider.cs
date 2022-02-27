using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FristList.Models;
using FristList.WebApi.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FristList.WebApi.Services;

public class JwtTokenDefaultProvider : IJwtTokenProvider
{
    private readonly JwtGeneratorOptions _options;
    private readonly SymmetricSecurityKey _key;

    public JwtTokenDefaultProvider(IOptions<JwtGeneratorOptions> options, SymmetricSecurityKey key)
    {
        _options = options.Value;
        _key = key;
    }
        
    public string CreateToken(AppUser user)
    {
        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);
        Claim[] claims = { new Claim(ClaimTypes.Name, user.UserName) };

        var token = new JwtSecurityToken(claims: claims, signingCredentials: credentials,
            expires: DateTime.UtcNow.Add(_options.ExpirePeriod));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}