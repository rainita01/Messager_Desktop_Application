using CommunityToolkit.Mvvm.Messaging;
using demo_158.EventsPublish;
using demo_158.Hubs;
using demo_158.MVVM.Model;

namespace demo_158.Repository;

public class UserChangesRepository 
{
    private readonly ConnectionManager _connectionManager;

    public UserChangesRepository(ConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public async Task StartAsync()
    {
        await UserChangedProfile();
    }

    public async Task UserChangedProfile()
    {
        await _connectionManager.OnAsync<ProfileEditModel>("UserChangedProfile", model =>
        {

            WeakReferenceMessenger.Default.Send(new ContactUserChangedProfileEvent(model));
        });
    }
}