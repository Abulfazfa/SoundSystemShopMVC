using System.Drawing.Imaging;
using System.Drawing;

namespace SoundSystemShop.Helper;

public static class Extention
{
    public static bool CheckFileType(this IFormFile file)
    {
        return file.ContentType.Contains("image");
    }
    public static string SaveImage(this IFormFile file, IWebHostEnvironment _webHostEnvironment, string folder)
    {
        string fileName = Guid.NewGuid() + file.FileName;
        string path = Path.Combine(_webHostEnvironment.WebRootPath, folder, fileName);

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            file.CopyTo(stream);
        }
        return fileName;
    }

    public static byte[] BitmapToByteArray(this Bitmap bitmap)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            bitmap.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }
    }
}