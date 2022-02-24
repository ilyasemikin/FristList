namespace FristList.Service.PublicApi.Models.Activities;

public class SearchActivitiesModel
{
    public IList<Guid> CategoryIds { get; set; } = new List<Guid>();
}