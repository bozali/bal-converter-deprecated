using System.Text;

using Newtonsoft.Json;

namespace Bal.Converter.Services;

public class FileSystemService : IFileSystemService
{
    public async Task<T?> ReadJsonAsync<T>(string folder, string fileName)
    {
        return await Task.Run(() => this.ReadJson<T>(folder, fileName));
    }

    public T? ReadJson<T>(string folder, string fileName)
    {
        string path = Path.Combine(folder, fileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }

        return default;
    }

    public async Task WriteJsonAsync<T>(string folderPath, string fileName, T content)
    {
        await Task.Run(() => this.WriteJson(folderPath, fileName, content));
    }

    public void WriteJson<T>(string folderPath, string fileName, T content)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string? fileContent = JsonConvert.SerializeObject(content);
        File.WriteAllText(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
    }
}