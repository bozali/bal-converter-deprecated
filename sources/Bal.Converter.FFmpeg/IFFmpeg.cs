namespace Bal.Converter.FFmpeg;

public interface IFFmpeg
{
    Task Convert(string path, string destination, ConversionOptions options);
}