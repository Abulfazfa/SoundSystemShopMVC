namespace SoundSystemShop.Services;

public class FileService
{
    public string ReadFile(string path, string body)
    {
        using(StreamReader stream = new StreamReader(path))
        {
            body = stream.ReadToEnd();
        }
        return body;
    }
}