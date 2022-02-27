namespace FristList.Service.PublicApi.Contracts.RequestModels.Activities;

public class SearchActivitiesModel : SearchModel
{
    public IList<Guid> CategoryIds { get; set; } = new List<Guid>();
}