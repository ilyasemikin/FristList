using Dapper.FluentMap.Mapping;
using FristList.Models;

namespace FristList.Services.PostgreSql.Mapping
{
    public class RunningActionMap : EntityMap<RunningAction>
    {
        internal RunningActionMap()
        {
            Map(ra => ra.StartTime).ToColumn("RunningActionStartTime");
            Map(ra => ra.UserId).ToColumn("RunningActionUserId");
        }
    }
}