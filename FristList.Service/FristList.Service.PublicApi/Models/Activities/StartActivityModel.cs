namespace FristList.Service.PublicApi.Models.Activities;

public class StartActivityModel
{
    public IList<Guid> CategoryIds { get; set; } = new List<Guid>();
}