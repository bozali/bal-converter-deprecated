namespace Bal.Converter.Common.Services;

public interface IFileSystemService
{
    void DeleteFile(string? path);

    void MoveFile(string source, string destination);

    public T? ReadJson<T>(string path);

    void WriteJson<T>(string path, T content);
}