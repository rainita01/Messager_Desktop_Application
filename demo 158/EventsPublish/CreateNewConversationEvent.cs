using CommunityToolkit.Mvvm.Messaging.Messages;
using demo_158.MVVM.Model;

namespace demo_158.EventsPublish;

public class CreateNewConversationEvent : ValueChangedMessage<ContactUserModel>
{
    public CreateNewConversationEvent(ContactUserModel value) : base(value)
    {
    }
}