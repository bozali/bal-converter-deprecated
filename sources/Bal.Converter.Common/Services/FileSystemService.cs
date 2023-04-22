using System.Text;
using Newtonsoft.Json;

namespace Bal.Converter.Common.Services;

public class FileSystemService : IFileSystemService
{
    public void DeleteFile(string? path)
    {
        if (File.Exists(path))
        {
            // TODO Maybe check if the file is used by another process?
            File.Delete(path);
        }
    }

    public void MoveFile(string source, string destination)
    {
        if (File.Exists(source))
        {
            File.Move(source, destination);
        }
    }

    public T? ReadJson<T>(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }

        return default;
    }

    public void WriteJson<T>(string path, T content)
    {
        string? directory = Path.GetDirectoryName(path);

        if (string.IsNullOrEmpty(directory))
        {
            throw new DirectoryNotFoundException($"The provided path {path} has no directory.");
        }

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string? json = JsonConvert.SerializeObject(content);
        File.WriteAllText(path, json, Encoding.UTF8);
    }
}