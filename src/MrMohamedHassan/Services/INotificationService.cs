using MrMohamedHassan.Models;

namespace MrMohamedHassan.Services;

public interface INotificationService
{
    Task<Notification> CreateAsync(Notification notification);
    Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);
    Task<int> GetUnreadCountAsync(string userId);
    Task MarkAsReadAsync(int id);
    Task SendToAllAsync(string title, string message, NotificationType type);
}
