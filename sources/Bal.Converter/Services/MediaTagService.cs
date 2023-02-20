using System.Net.Mime;
using Bal.Converter.Common.Media;
using TagLib;
using TagLib.Id3v2;
using File = System.IO.File;

namespace Bal.Converter.Services;

public class MediaTagService : IMediaTagService
{
    public void SetInformation(string path, MediaTags? tags)
    {
        if (tags == null)
        {
            return;
        }

        using var file = TagLib.File.Create(path);

        file.Tag.Title = tags.Title;
        file.Tag.Album = tags.Album;
        file.Tag.Comment = tags.Comment;
        file.Tag.Copyright = tags.Copyright;
        file.Tag.AlbumArtists = tags.AlbumArtists?.Split(";", StringSplitOptions.TrimEntries | StringSplitOptions.TrimEntries);
        file.Tag.Performers = tags.Performers?.Split(";", StringSplitOptions.TrimEntries | StringSplitOptions.TrimEntries);
        file.Tag.Composers = tags.Composers?.Split(";", StringSplitOptions.TrimEntries | StringSplitOptions.TrimEntries);
        file.Tag.Genres = tags.Genres?.Split(";", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        file.Save();
    }

    public void SetPicture(string path, string? imagePath)
    {
        if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
        {
            return;
        }

        using var file = TagLib.File.Create(path);

        byte[] data = File.ReadAllBytes(imagePath);

        file.Tag.Pictures = new IPicture[]
        {
            new AttachedPictureFrame
            {
                Data = new ByteVector(data),
                MimeType = MediaTypeNames.Image.Jpeg,
                Type = PictureType.FrontCover,
                TextEncoding = StringType.UTF16
            }
        };

        file.Save();
    }
}