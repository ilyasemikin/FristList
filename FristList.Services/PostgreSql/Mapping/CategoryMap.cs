using Dapper.FluentMap.Mapping;
using FristList.Models;

namespace FristList.Services.PostgreSql.Mapping
{
    public class CategoryMap : EntityMap<Category>
    {
        internal CategoryMap()
        {
            Map(c => c.Id).ToColumn("CategoryId");
            Map(c => c.UserId).ToColumn("CategoryUserId");
            Map(c => c.Name).ToColumn("CategoryName");
        }
    }
}