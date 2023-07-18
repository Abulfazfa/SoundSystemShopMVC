namespace SoundSystemShop.Helper
{
    public class DeleteHelper
    {
        public static void DeleteFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}
