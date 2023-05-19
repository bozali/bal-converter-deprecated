using Bal.Converter.Modules.Downloads;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Bal.Converter.Messages;

public class DownloadAddedMessage : ValueChangedMessage<DownloadJob>
{
    public DownloadAddedMessage(DownloadJob value) : base(value)
    {
    }
}