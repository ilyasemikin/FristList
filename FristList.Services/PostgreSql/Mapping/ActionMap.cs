using Dapper.FluentMap.Mapping;
using FristList.Models;

namespace FristList.Services.PostgreSql.Mapping
{
    public class ActionMap : EntityMap<Action>
    {
        internal ActionMap()
        {
            Map(a => a.Id).ToColumn("ActionId");
            Map(a => a.StartTime).ToColumn("ActionStartTime");
            Map(a => a.EndTime).ToColumn("ActionEndTime");
            Map(a => a.Description).ToColumn("ActionDescription");
            Map(a => a.UserId).ToColumn("ActionUserId");
        }
    }
}