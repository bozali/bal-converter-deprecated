using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Bal.Converter.Messages ;

public class DownloadRemovedMessage : ValueChangedMessage<int>
{
    public DownloadRemovedMessage(int value) : base(value)
    {
    }
}