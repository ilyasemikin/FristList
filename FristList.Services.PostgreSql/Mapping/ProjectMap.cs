using Dapper.FluentMap.Mapping;
using FristList.Models;

namespace FristList.Services.PostgreSql.Mapping;

internal class ProjectMap : EntityMap<Project>
{
    internal ProjectMap()
    {
        Map(p => p.Id).ToColumn("ProjectId");
        Map(p => p.Name).ToColumn("ProjectName");
        Map(p => p.Description).ToColumn("ProjectDescription");
        Map(p => p.IsCompleted).ToColumn("ProjectIsCompleted");
        Map(p => p.AuthorId).ToColumn("ProjectUserId");
    }
}