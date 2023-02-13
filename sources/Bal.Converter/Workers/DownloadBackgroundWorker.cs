using Bal.Converter.Common.Extensions;
using Bal.Converter.Modules.Downloads;
using Bal.Converter.Services;
using Bal.Converter.YouTubeDl;
using Microsoft.UI.Dispatching;

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

                await this.youtubeDl.Download(job.Url, options, (f, s) =>
                {
                    App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, () =>
                    {
                        job.Progress = f;
                        job.ProgressText = s;
                    });
                }, cts.Token);

                cts.Token.ThrowIfCancellationRequested();

                File.Move(downloadPath, destinationPath);

                this.downloadsRegistry.UpdateState(job, DownloadState.Done);
            }
            catch (Exception e)
            {
            }
        }
    }
}
