using FristList.Data.Models;

namespace FristList.WebApi.Services
{
    public interface IJwtTokenProvider
    {
        string CreateToken(AppUser user);
    }
}