namespace FristList.Service.PublicApi.Contracts.RequestModels.Activities;

public class AddActivityModel
{
    public IList<Guid> CategoryIds { get; set; } = new List<Guid>();
    
    public DateTimeOffset BeginAt { get; set; }
    public DateTimeOffset EndAt { get; set; }
}