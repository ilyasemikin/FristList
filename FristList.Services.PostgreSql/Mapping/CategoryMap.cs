using Dapper.FluentMap.Mapping;
using FristList.Models;

namespace FristList.Services.PostgreSql.Mapping;

internal class CategoryMap : EntityMap<Category>
{
    internal CategoryMap()
    {
        Map(c => c.Id).ToColumn("CategoryId");
        Map(c => c.Name).ToColumn("CategoryName");
        Map(c => c.UserId).ToColumn("CategoryUserId");
    }
}