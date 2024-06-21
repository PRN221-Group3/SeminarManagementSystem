using BusinessObject.Models;
using QRCoder;

namespace SeminarManagement_PRN221.Common
{
    public static class QrCodeGenerator
    {
        public static void GenerateQRCode(Event @event)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode($"Event: {@event.EventName}\nStart Date: {@event.StartDate}\nEnd Date: {@event.EndDate}", QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);

            using (var ms = new System.IO.MemoryStream())
            {
                qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                var qrCodeBase64 = Convert.ToBase64String(ms.ToArray());
                @event.QrCode = $"data:image/png;base64,{qrCodeBase64}";
            }
        }
    }
}
