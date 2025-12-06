using CommunityToolkit.Mvvm.Messaging.Messages;
using demo_158.MVVM.Model;

namespace demo_158.EventsPublish;

public class MessageSendedSuccessEvent : ValueChangedMessage<bool>
{
    public MessageSendedSuccessEvent(bool value) : base(value)
    {
    }
}