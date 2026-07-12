using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MrMohamedHassan.Models;
using MrMohamedHassan.Services;
using MrMohamedHassan.ViewModels;

namespace MrMohamedHassan.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFileService _fileService;

    public ProfileController(UserManager<ApplicationUser> userManager, IFileService fileService)
    {
        _userManager = userManager;
        _fileService = fileService;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var model = new ProfileViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber,
            JobTitle = user.JobTitle,
            ProfileImageUrl = user.ProfileImageUrl
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(ProfileViewModel model)
    {
        if (!ModelState.IsValid) return View("Index", model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        user.FullName = model.FullName;
        user.PhoneNumber = model.PhoneNumber;
        user.JobTitle = model.JobTitle;

        if (model.ProfileImage != null)
        {
            if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                _fileService.DeleteFile(user.ProfileImageUrl);
            user.ProfileImageUrl = await _fileService.UploadImageAsync(model.ProfileImage, "profiles");
        }

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            TempData["Success"] = "تم تحديث الملف الشخصي بنجاح";
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View("Index", model);
    }
}
