using System;
using System.Security.Cryptography;

namespace FristList.Services;

public class RandomBytesCryptoTokenGenerator : ITokenGenerator
{
    public int BytesCount { get; }

    public RandomBytesCryptoTokenGenerator(int bytesCount)
    {
        BytesCount = bytesCount;
    }
        
    public string Generate()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(BytesCount);;
        return Convert.ToBase64String(randomBytes);
    }
}