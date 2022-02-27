namespace FristList.Service.PublicApi.Contracts.RequestModels.Activities;

public class StartActivityModel
{
    public IList<Guid> CategoryIds { get; set; } = new List<Guid>();
}