using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Bal.Converter.Messages;

public class DisableInteractionChangeMessage : ValueChangedMessage<bool>
{
    public DisableInteractionChangeMessage(bool value) : base(value)
    {
    }
}