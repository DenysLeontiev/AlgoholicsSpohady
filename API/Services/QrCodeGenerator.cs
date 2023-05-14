using API.Interfaces;
using System.Drawing;
using QRCoder;

namespace API.Services
{
    public class QrCodeGenerator : IQrCodeGenerator
    {
        public string GenerateQrCode(string qrCodeData)
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(qrCodeData, QRCodeGenerator.ECCLevel.Q);

            QRCode qRCode = new QRCode(qRCodeData);
            Bitmap qrCodeImage = qRCode.GetGraphic(20);
            // Bitmap qrCodeImage = qRCode.GetGraphic(20, Color.White, Color.Black, true);

            MemoryStream ms = new MemoryStream();
            qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            byte[] byteQrArray = ms.ToArray();
            string qrBase64 = Convert.ToBase64String(byteQrArray);

            return qrBase64;
        }
    }
}