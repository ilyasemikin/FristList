using Dapper.FluentMap.Mapping;
using Action = FristList.Data.Models.Action;

namespace FristList.Services.PostgreSql.Mapping;

internal class ActionMap : EntityMap<Action>
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