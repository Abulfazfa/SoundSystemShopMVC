namespace SoundSystemShop.Helper
{
    public class DeleteHelper
    {
        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
