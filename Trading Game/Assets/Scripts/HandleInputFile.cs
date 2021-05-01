using System.IO;

public class HandleInputFile {

    public static string ReadString(string subpath, string fileName)
    {
        string path = $"Assets/Resources/{subpath}/{fileName}";
        StreamReader reader = new StreamReader(path);
        return reader.ReadToEnd();
    }
}
