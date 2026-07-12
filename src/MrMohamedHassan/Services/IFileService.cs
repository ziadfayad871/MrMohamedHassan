namespace MrMohamedHassan.Services;

public interface IFileService
{
    Task<string> UploadImageAsync(IFormFile file, string folder = "images");
    Task<string> UploadFileAsync(IFormFile file, string folder = "files");
    void DeleteFile(string filePath);
}
