using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Bal.Converter.Messages;

public class WindowStateChangedMessage : ValueChangedMessage<bool>
{
    public WindowStateChangedMessage(bool value) : base(value)
    {
    }
}