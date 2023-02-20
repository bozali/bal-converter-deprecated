using Bal.Converter.Common.Media;

namespace Bal.Converter.Services;

public interface IMediaTagService
{
    void SetInformation(string path, MediaTags? tags);

    void SetPicture(string path, string? imagePath);
}
