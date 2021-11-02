using FristList.Models;

namespace FristList.WebApi.Services
{
    public interface IJwtTokenProvider
    {
        string CreateToken(AppUser user);
    }
}