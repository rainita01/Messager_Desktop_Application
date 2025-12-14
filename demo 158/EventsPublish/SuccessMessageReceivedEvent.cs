using CommunityToolkit.Mvvm.Messaging.Messages;
using demo_158.MVVM.Model;

namespace demo_158.EventsPublish;

public class SuccessMessageReceivedEvent : ValueChangedMessage<MessageModelFromServer>
{
    public SuccessMessageReceivedEvent(MessageModelFromServer value) : base(value)
    {   
    }
}