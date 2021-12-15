using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FristList.Data.Models;
using Microsoft.IdentityModel.Tokens;

namespace FristList.WebApi.Services
{
    public class JwtTokenDefaultProvider : IJwtTokenProvider
    {
        private readonly SymmetricSecurityKey _key;

        public JwtTokenDefaultProvider(SymmetricSecurityKey key)
        {
            _key = key;
        }
        
        public string CreateToken(AppUser user)
        {
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);
            Claim[] claims = { new Claim(ClaimTypes.Name, user.UserName) };

            var token = new JwtSecurityToken(claims: claims, signingCredentials: credentials, expires: DateTime.UtcNow.AddHours(2));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}