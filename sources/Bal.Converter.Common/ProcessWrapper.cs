using System.Diagnostics;

namespace Bal.Converter.Common ;

public class ProcessWrapper : IDisposable
{
    private readonly Process process;
    private readonly string exePath;

    public ProcessWrapper(string exePath)
    {
        this.process = new Process();
        this.exePath = exePath;
    }

    public event DataReceivedEventHandler OutputDataReceived
    {
        add => this.process.OutputDataReceived += value;
        remove => this.process.OutputDataReceived -= value;
    }

    public event DataReceivedEventHandler ErrorDataReceived
    {
        add => this.process.ErrorDataReceived += value;
        remove => this.process.ErrorDataReceived -= value;
    }

    public async Task Execute(string arguments, CancellationToken ct = default)
    {
        var startInfo = new ProcessStartInfo(this.exePath)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            UseShellExecute = false,
            Arguments = arguments
        };

        this.process.StartInfo = startInfo;
        this.process.EnableRaisingEvents = true;

        var tcs = new TaskCompletionSource();

        void Exited(object s, EventArgs e) => tcs.SetResult();

        try
        {
            await using var ctr = ct.Register(() =>
            {
                if (!this.process.HasExited)
                {
                    this.process.CloseMainWindow();
                    this.process.Kill();
                }
            });

            this.process.Exited += Exited;

            this.process.Start();

            this.process.BeginOutputReadLine();
            this.process.BeginErrorReadLine();

            await tcs.Task;

            ct.ThrowIfCancellationRequested();
        }
        catch (Exception)
        {
            // ignored
        }
        finally
        {
        }
    }

    public void Dispose()
    {
        this.process?.Dispose();
    }
}