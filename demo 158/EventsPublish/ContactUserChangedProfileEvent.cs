using CommunityToolkit.Mvvm.Messaging.Messages;
using demo_158.MVVM.Model;

namespace demo_158.EventsPublish;

public class ContactUserChangedProfileEvent  : ValueChangedMessage<ProfileEditModel>
{
    public ContactUserChangedProfileEvent(ProfileEditModel value) : base(value)
    {
    }
}