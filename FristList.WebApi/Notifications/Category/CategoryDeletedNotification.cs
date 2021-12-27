using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.Category;

public class CategoryDeletedNotification : INotification
{
    public AppUser User { get; init; }
    public int Id { get; init; }
}