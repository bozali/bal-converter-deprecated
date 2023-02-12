﻿using Bal.Converter.Domain;

using System.Net;
using ImageMagick;

namespace Bal.Converter.Services ;

public class FileDownloaderService : IFileDownloaderService
{
    public async Task<FileDownloadResponse> DownloadFileAsync(string url)
    {
        var client = new WebClient();

        byte[] thumbnailData = await client.DownloadDataTaskAsync(url);

        var thumbnailUri = new Uri(url);
        string file = thumbnailUri.Segments.Last();

        // ReSharper disable once PossibleNullReferenceException
        string extension = Path.GetExtension(file).Replace(".", string.Empty);

        var destinationThumbnailFile = new FileInfo(Path.Combine(ILocalSettingsService.TempPath, "Thumbnails", Guid.NewGuid() + ".jpg"));

        if (destinationThumbnailFile.Directory is { Exists: false })
        {
            destinationThumbnailFile.Directory.Create();
        }

        await using var stream = destinationThumbnailFile.OpenWrite();

        // If we have already jpg/jpeg we just write the file and don't recode it.
        if (string.Equals(extension, "jpeg", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(extension, "jpg", StringComparison.OrdinalIgnoreCase))
        {
            await stream.WriteAsync(thumbnailData);
        }
        else
        {
            using var image = new MagickImage(thumbnailData, Enum.Parse<MagickFormat>(extension, true));
            await image.WriteAsync(stream, MagickFormat.Jpeg);
        }

        return new FileDownloadResponse
        {
            Data = thumbnailData,
            DownloadPath = destinationThumbnailFile.FullName
        };
    }
}