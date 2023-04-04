namespace Bal.Converter.YouTubeDl;

public interface IYouTubeDl
{
    Task<Video> GetVideo(string url);

    Task<Playlist> GetPlaylist(string url);

    Task Download(string url,
                  DownloadOptions options,
                  Action<float, string> progressReport = null,
                  CancellationToken ct = default);
}