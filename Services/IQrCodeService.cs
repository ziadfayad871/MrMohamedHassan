namespace MrMohamedHassan.Services;

public interface IQrCodeService
{
    byte[] GenerateQrCode(string content);
    string GetQrCodeAsBase64(string content);
}
