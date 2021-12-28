using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.Category;

public record CategoryDeletedNotification(AppUser User, int CategoryId) : INotification;