using System;
using System.Security.Cryptography;
using FristList.Models;

namespace FristList.Services.PostgreSql
{
    public class RNGCryptoTokenGenerator : ITokenGenerator
    {
        public string Generate()
        {
            using var provider = new RNGCryptoServiceProvider();

            var randomBytes = new byte[64];
            provider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}