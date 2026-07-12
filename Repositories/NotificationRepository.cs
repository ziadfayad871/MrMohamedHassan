using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.Repositories;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Notification>> GetNotificationsForUserAsync(string userId)
    {
        return await _dbSet
            .Where(n => n.TargetUserId == userId || n.TargetUserId == null)
            .OrderByDescending(n => n.CreatedAt)
            .Take(50)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(string userId)
    {
        return await _dbSet.CountAsync(n => (n.TargetUserId == userId || n.TargetUserId == null) && !n.IsRead);
    }

    public async Task MarkAsReadAsync(int id)
    {
        var notification = await _dbSet.FindAsync(id);
        if (notification != null)
        {
            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }
}
