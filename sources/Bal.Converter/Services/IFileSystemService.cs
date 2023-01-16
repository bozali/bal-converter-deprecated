namespace Bal.Converter.Services;

public interface IFileSystemService
{
    Task<T?> ReadJsonAsync<T>(string folder, string fileName);
    
    T? ReadJson<T>(string folder, string fileName);

    Task WriteJsonAsync<T>(string folderPath, string fileName, T content);

    void WriteJson<T>(string folderPath, string fileName, T content);
}