using CommunityToolkit.Mvvm.Messaging.Messages;

namespace demo_158.EventsPublish;

public class SuccessDeletedMessage : ValueChangedMessage<bool>
{
    public SuccessDeletedMessage(bool value) : base(value)
    {
    }
}