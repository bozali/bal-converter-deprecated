using Bal.Converter.Common.Extensions;
using Bal.Converter.Modules.Downloads;
using Bal.Converter.Services;
using Bal.Converter.YouTubeDl;

namespace Bal.Converter.Workers;

public class DownloadBackgroundWorker
{
    private readonly IDownloadsRegistryService downloadsRegistry;
    private readonly ILocalSettingsService localSettingsService;
    private readonly IYouTubeDl youtubeDl;

    public DownloadBackgroundWorker(IDownloadsRegistryService downloadsRegistry, ILocalSettingsService localSettingsService, IYouTubeDl youtubeDl)
    {
        this.downloadsRegistry = downloadsRegistry;
        this.localSettingsService = localSettingsService;
        this.youtubeDl = youtubeDl;
    }

    public async Task Process(CancellationToken ct = default)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var job = await this.downloadsRegistry.NextDownloadJob();

                using var cts = new CancellationTokenSource();

                string configDownloadPath = await this.localSettingsService.ReadSettingsAsync<string>(ILocalSettingsService.DownloadDirectoryKey)
                                            ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));

                int bandwidth = await this.localSettingsService.ReadSettingsAsync<int>(ILocalSettingsService.BandwidthKey);

                string format = job.TargetFormat.ToString().ToLowerInvariant();

                string downloadPathPattern = Path.Combine(ILocalSettingsService.TempPath, $"{job.Id}.%(ext)s");
                string downloadPath = downloadPathPattern.Replace("%(ext)s", format);
                string destinationPath = Path.Combine(configDownloadPath, job.Tags?.Title.RemoveIllegalChars() + "." + format);


                var options = new DownloadOptions
                {
                    DownloadBandwidth = bandwidth,
                    Destination = downloadPathPattern,
                    DownloadExtension = job.TargetFormat,
                };

                await this.youtubeDl.Download(job.Url, options, (f, s) => { }, cts.Token);

                cts.Token.ThrowIfCancellationRequested();

                File.Move(downloadPath, destinationPath);

                job.State = DownloadState.Done;
            }
            catch (Exception e)
            {
            }
        }
    }
}
