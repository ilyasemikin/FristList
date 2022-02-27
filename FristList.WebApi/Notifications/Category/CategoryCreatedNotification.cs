using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.Category;

public record CategoryCreatedNotification(AppUser User, Models.Category Category) : INotification;