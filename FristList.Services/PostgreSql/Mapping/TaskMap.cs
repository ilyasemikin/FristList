using Dapper.FluentMap.Mapping;
using FristList.Models;

namespace FristList.Services.PostgreSql.Mapping
{
    public class TaskMap : EntityMap<Task>
    {
        internal TaskMap()
        {
            Map(t => t.Id).ToColumn("TaskId");
            Map(t => t.Name).ToColumn("TaskName");
            Map(t => t.UserId).ToColumn("TaskUserId");
            Map(t => t.ProjectId).ToColumn("TaskProjectId");
        }
    }
}