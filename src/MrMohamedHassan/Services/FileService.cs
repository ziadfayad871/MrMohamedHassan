namespace MrMohamedHassan.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;

    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> UploadImageAsync(IFormFile file, string folder = "images")
    {
        return await UploadFileAsync(file, folder);
    }

    public async Task<string> UploadFileAsync(IFormFile file, string folder = "files")
    {
        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", folder);
        Directory.CreateDirectory(uploadsFolder);

        var uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, uniqueName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/{folder}/{uniqueName}";
    }

    public void DeleteFile(string filePath)
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
