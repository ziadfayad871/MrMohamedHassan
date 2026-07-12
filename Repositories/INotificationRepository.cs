using MrMohamedHassan.Models;

namespace MrMohamedHassan.Repositories;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<IEnumerable<Notification>> GetNotificationsForUserAsync(string userId);
    Task<int> GetUnreadCountAsync(string userId);
    Task MarkAsReadAsync(int id);
}
