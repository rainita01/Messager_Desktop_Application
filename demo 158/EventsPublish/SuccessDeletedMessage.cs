using CommunityToolkit.Mvvm.Messaging.Messages;

namespace demo_158.EventsPublish;

public class SuccessDeletedMessage : ValueChangedMessage<int>
{
    public SuccessDeletedMessage(int id) : base(id)
    {
    }
}