using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;
using MrMohamedHassan.Repositories;

namespace MrMohamedHassan.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ApplicationDbContext _context;

    public NotificationService(INotificationRepository notificationRepository, ApplicationDbContext context)
    {
        _notificationRepository = notificationRepository;
        _context = context;
    }

    public async Task<Notification> CreateAsync(Notification notification)
    {
        notification.CreatedAt = DateTime.UtcNow;
        return await _notificationRepository.AddAsync(notification);
    }

    public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        => await _notificationRepository.GetNotificationsForUserAsync(userId);

    public async Task<int> GetUnreadCountAsync(string userId)
        => await _notificationRepository.GetUnreadCountAsync(userId);

    public async Task MarkAsReadAsync(int id) => await _notificationRepository.MarkAsReadAsync(id);

    public async Task SendToAllAsync(string title, string message, NotificationType type)
    {
        var users = await _context.Users.ToListAsync();
        foreach (var user in users)
        {
            await _notificationRepository.AddAsync(new Notification
            {
                Title = title,
                Message = message,
                Type = type,
                TargetUserId = user.Id,
                CreatedAt = DateTime.UtcNow
            });
        }
    }
}
