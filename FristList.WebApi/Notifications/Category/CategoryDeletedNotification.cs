using MediatR;

namespace FristList.WebApi.Notifications.Category;

public class CategoryDeletedNotification : INotification
{
    public Data.Models.AppUser User { get; init; }
    public int Id { get; init; }
}