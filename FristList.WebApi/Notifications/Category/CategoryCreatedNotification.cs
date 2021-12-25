using MediatR;

namespace FristList.WebApi.Notifications.Category;

public class CategoryCreatedNotification : INotification
{
    public Data.Models.AppUser User { get; init; }
    public Data.Models.Category Category { get; init; }
}