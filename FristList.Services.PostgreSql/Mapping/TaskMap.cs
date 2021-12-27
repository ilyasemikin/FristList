using Dapper.FluentMap.Mapping;
using Task = FristList.Models.Task;

namespace FristList.Services.PostgreSql.Mapping;

internal class TaskMap : EntityMap<Task>
{
    internal TaskMap()
    {
        Map(t => t.Id).ToColumn("TaskId");
        Map(t => t.Name).ToColumn("TaskName");
        Map(t => t.IsCompleted).ToColumn("TaskIsCompleted");
        Map(t => t.ProjectId).ToColumn("TaskProjectId");
        Map(t => t.UserId).ToColumn("TaskUserId");
    }
}