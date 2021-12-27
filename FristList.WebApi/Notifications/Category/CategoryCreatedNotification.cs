using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.Category;

public class CategoryCreatedNotification : INotification
{
    public AppUser User { get; init; }
    public Models.Category Category { get; init; }
}