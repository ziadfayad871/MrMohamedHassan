using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MrMohamedHassan.Models;
using MrMohamedHassan.Services;
using MrMohamedHassan.ViewModels;
using System.Security.Claims;

namespace MrMohamedHassan.Controllers;

[Authorize]
public class NotificationsController : Controller
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!;
        var notifications = await _notificationService.GetUserNotificationsAsync(userId);

        var viewModel = notifications.Select(n => new NotificationListViewModel
        {
            Id = n.Id,
            Title = n.Title,
            Message = n.Message,
            Type = n.Type,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt
        }).ToList();

        return View(viewModel);
    }

    public async Task<IActionResult> Create()
    {
        return View(new NotificationCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NotificationCreateViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!;

        var notification = new Notification
        {
            Title = model.Title,
            Message = model.Message,
            Type = model.Type,
            TargetUserId = model.TargetUserId,
            SenderId = userId
        };

        if (string.IsNullOrEmpty(model.TargetUserId))
            await _notificationService.SendToAllAsync(model.Title, model.Message, model.Type);
        else
            await _notificationService.CreateAsync(notification);

        TempData["Success"] = "تم إرسال الإشعار بنجاح";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _notificationService.MarkAsReadAsync(id);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetUnreadCount()
    {
        var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!;
        var count = await _notificationService.GetUnreadCountAsync(userId);
        return Json(count);
    }
}
