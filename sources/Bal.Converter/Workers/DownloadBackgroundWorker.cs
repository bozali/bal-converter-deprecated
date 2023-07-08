using System.Diagnostics;
using Bal.Converter.Common.Extensions;
using Bal.Converter.Common.Services;
using Bal.Converter.Events;
using Bal.Converter.Modules.Downloads;
using Bal.Converter.Services;
using Bal.Converter.YouTubeDl;

using Microsoft.Extensions.Logging;
using Microsoft.UI.Dispatching;
// ReSharper disable AccessToDisposedClosure

namespace Bal.Converter.Workers;

public class DownloadBackgroundWorker
{
    private readonly ILogger<DownloadBackgroundWorker> logger;
    private readonly IDownloadsRegistryService downloadsRegistry;
    private readonly ILocalSettingsService localSettingsService;
    private readonly IMediaTagService mediaTagService;
    private readonly IFileSystemService fileSystemService;
    private readonly IYouTubeDl youtubeDl;

    public DownloadBackgroundWorker(ILogger<DownloadBackgroundWorker> logger,
                                    IDownloadsRegistryService downloadsRegistry,
                                    ILocalSettingsService localSettingsService,
                                    IMediaTagService mediaTagService,
                                    IFileSystemService fileSystemService,
                                    IYouTubeDl youtubeDl)
    {
        this.logger = logger;
        this.downloadsRegistry = downloadsRegistry;
        this.localSettingsService = localSettingsService;
        this.mediaTagService = mediaTagService;
        this.fileSystemService = fileSystemService;
        this.youtubeDl = youtubeDl;
    }

    public async Task Process(CancellationToken ct = default)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var job = await this.downloadsRegistry.NextDownloadJob();

                if (job == null)
                {
                    continue;
                }

                using var cts = new CancellationTokenSource();

                // Create a local function to call the cancellation token if cancel is requested.
                void OnStateChanged(object s, DownloadStateChangedEventArgs e)
                {
                    if (e.State == DownloadState.Cancelled && cts is { IsCancellationRequested: false })
                    {
                        this.logger.LogInformation($"Requesting cancellation for {job.Id}");
                        cts?.Cancel();
                    }
                }

                job.StateChanged += OnStateChanged;

                string configDownloadPath = await this.localSettingsService.ReadSettingsAsync<string>(ILocalSettingsService.DownloadDirectoryKey)
                                            ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));

                int bandwidth = await this.localSettingsService.ReadSettingsAsync<int>(ILocalSettingsService.BandwidthKey);

                string format = job.TargetFormat.ToString().ToLowerInvariant();

                string downloadPathPattern = Path.Combine(ILocalSettingsService.TempPath, $"{job.Id}.%(ext)s");
                string downloadPath = downloadPathPattern.Replace("%(ext)s", format);
                string destinationPath = Path.Combine(configDownloadPath, job.Tags?.Title.RemoveIllegalChars() + "." + format);

                this.logger.LogDebug("Downloading parameters");
                this.logger.LogDebug($"Pattern Path: {downloadPathPattern}");
                this.logger.LogDebug($"Download Path: {downloadPath}");
                this.logger.LogDebug($"Destination Path: {destinationPath}");

                this.fileSystemService.DeleteFile(destinationPath);

                var options = new DownloadOptions { DownloadBandwidth = bandwidth, Destination = downloadPathPattern, DownloadExtension = job.TargetFormat, };

                // Register a callback that removes the job from the registry and also removes
                // the part files if cancellation is requested.
                await using var ctr = cts.Token.Register(() =>
                {
                    job.ProgressText = job.State.ToString("G");

                    var file = new FileInfo(downloadPath + ".part");

                    if (file.Exists && file.Wait())
                    {
                        file.SafeDelete();
                    }

                    file = new FileInfo(downloadPath);

                    if (file.Exists && file.Wait())
                    {
                        file.SafeDelete();
                    }
                });

                await this.youtubeDl.Download(job.Url, options, (f, s) =>
                {
                    App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, () =>
                    {
                        job.Progress = f;
                        job.ProgressText = s;
                    });
                }, ctr.Token);

                cts.Token.ThrowIfCancellationRequested();

                this.mediaTagService.SetInformation(downloadPath, job.Tags);
                this.mediaTagService.SetPicture(downloadPath, job.ThumbnailPath);

                this.fileSystemService.DeleteFile(job.ThumbnailPath);
                this.fileSystemService.MoveFile(downloadPath, destinationPath);

                this.downloadsRegistry.UpdateState(job, DownloadState.Done);

                job.StateChanged -= OnStateChanged;
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                this.logger.LogError("Failed to download", e);
            }
        }
    }
}
