namespace Bal.Converter.Domain;

public class FileDownloadResponse
{
    public byte[] Data { get; set; }

    public string DownloadPath { get; set; }
}