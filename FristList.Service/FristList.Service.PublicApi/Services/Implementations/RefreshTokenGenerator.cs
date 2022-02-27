using FristList.Service.Data.Models.Account;
using FristList.Service.PublicApi.Services.Abstractions;

namespace FristList.Service.PublicApi.Services.Implementations;

public class RefreshTokenGenerator : ITokenGenerator
{
    private readonly int _bufferCount;
    
    public RefreshTokenGenerator(int bufferCount)
    {
        if (bufferCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(bufferCount));
        _bufferCount = bufferCount;
    }

    public string Generate(User user)
    {
        var bytes = new byte[_bufferCount];
        Random.Shared.NextBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}