using CommunityToolkit.Mvvm.Messaging.Messages;

namespace demo_158.EventsPublish;

public class SuccessDeletedConversation : ValueChangedMessage<int>
{
    public SuccessDeletedConversation(int value) : base(value)
    {
    }
}