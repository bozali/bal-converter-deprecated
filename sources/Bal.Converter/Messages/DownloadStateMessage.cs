using Bal.Converter.Modules.Downloads;

using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Bal.Converter.Messages;

public class DownloadStateMessage : ValueChangedMessage<DownloadJob>
{
    public DownloadStateMessage(DownloadJob value) : base(value)
    {
    }
}