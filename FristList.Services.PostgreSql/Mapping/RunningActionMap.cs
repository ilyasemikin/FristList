using Dapper.FluentMap.Mapping;
using FristList.Models;

namespace FristList.Services.PostgreSql.Mapping;

internal class RunningActionMap : EntityMap<RunningAction>
{
    internal RunningActionMap()
    {
        Map(ra => ra.Id).ToColumn("RunningActionId");
        Map(ra => ra.UserId).ToColumn("RunningActionUserId");
        Map(ra => ra.TaskId).ToColumn("RunningActionTaskId");
        Map(ra => ra.StartTime).ToColumn("RunningActionStartTime");
    }
}