using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MrMohamedHassan.Repositories;
using MrMohamedHassan.Services;
using MrMohamedHassan.ViewModels;

namespace MrMohamedHassan.Controllers;

[Authorize(Roles = "Admin")]
public class SettingsController : Controller
{
    private readonly ISettingRepository _settingRepository;
    private readonly IFileService _fileService;

    public SettingsController(ISettingRepository settingRepository, IFileService fileService)
    {
        _settingRepository = settingRepository;
        _fileService = fileService;
    }

    public async Task<IActionResult> Index()
    {
        var settings = await _settingRepository.GetAllSettingsAsync();
        var model = new SettingsViewModel
        {
            CenterName = settings.GetValueOrDefault("CenterName"),
            CenterPhone = settings.GetValueOrDefault("CenterPhone"),
            CenterEmail = settings.GetValueOrDefault("CenterEmail"),
            CenterAddress = settings.GetValueOrDefault("CenterAddress"),
            AcademicYear = settings.GetValueOrDefault("AcademicYear"),
            CenterLogo = settings.GetValueOrDefault("CenterLogo"),
            Currency = settings.GetValueOrDefault("Currency")
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(SettingsViewModel model)
    {
        if (model.LogoFile != null)
        {
            var existingLogo = await _settingRepository.GetValueAsync("CenterLogo");
            if (!string.IsNullOrEmpty(existingLogo))
                _fileService.DeleteFile(existingLogo);

            var logoPath = await _fileService.UploadImageAsync(model.LogoFile, "logos");
            await _settingRepository.SetValueAsync("CenterLogo", logoPath);
        }

        await _settingRepository.SetValueAsync("CenterName", model.CenterName);
        await _settingRepository.SetValueAsync("CenterPhone", model.CenterPhone);
        await _settingRepository.SetValueAsync("CenterEmail", model.CenterEmail);
        await _settingRepository.SetValueAsync("CenterAddress", model.CenterAddress);
        await _settingRepository.SetValueAsync("AcademicYear", model.AcademicYear);
        await _settingRepository.SetValueAsync("Currency", model.Currency);

        TempData["Success"] = "تم حفظ الإعدادات بنجاح";
        return RedirectToAction(nameof(Index));
    }
}
