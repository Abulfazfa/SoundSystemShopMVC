using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;

namespace SoundSystemShop.Services
{
    public class GenerateQRCode
    {
        public string GenerateQR(string json)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(json, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrBitmap = qrCode.GetGraphic(60);

            byte[] bitmapBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                qrBitmap.Save(ms, ImageFormat.Png);
                bitmapBytes = ms.ToArray();
            }

            return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(bitmapBytes));
        }
    }
}
