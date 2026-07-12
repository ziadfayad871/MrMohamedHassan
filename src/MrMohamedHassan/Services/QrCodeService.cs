using QRCoder;

namespace MrMohamedHassan.Services;

public class QrCodeService : IQrCodeService
{
    public byte[] GenerateQrCode(string content)
    {
        using var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }

    public string GetQrCodeAsBase64(string content)
    {
        var bytes = GenerateQrCode(content);
        return $"data:image/png;base64,{Convert.ToBase64String(bytes)}";
    }
}
