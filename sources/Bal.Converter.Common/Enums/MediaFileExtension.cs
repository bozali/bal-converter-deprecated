namespace Bal.Converter.Common.Enums;

public enum MediaFileExtension
{
    MP3,
    MP4,
    Wav,
    M4a,
    Opus,
    Vorbis,
    Aac,
    Flac,
    Flv,
    Ogg,
    Webm,
    Mkv,
    Avi
}

public static class FileTypeExtensions
{
    public static bool IsAudioOnly(this MediaFileExtension type)
    {
        return type == MediaFileExtension.Aac ||
               type == MediaFileExtension.Flac ||
               type == MediaFileExtension.MP3 ||
               type == MediaFileExtension.M4a ||
               type == MediaFileExtension.Opus ||
               type == MediaFileExtension.Vorbis ||
               type == MediaFileExtension.Wav;
    }
}