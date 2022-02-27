using Dapper.FluentMap.Mapping;
using FristList.Models;

namespace FristList.Services.PostgreSql.Mapping;

internal class RefreshTokenMap : EntityMap<RefreshToken>
{
    internal RefreshTokenMap()
    {
        Map(rt => rt.Id).ToColumn("RefreshTokenId");
        Map(rt => rt.Token).ToColumn("RefreshTokenValue");
        Map(rt => rt.Expires).ToColumn("RefreshTokenExpires");
        Map(rt => rt.UserId).ToColumn("RefreshTokenUserId");
    }
}