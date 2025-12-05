using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using demo_158.Base;
using demo_158.EventsPublish;
using demo_158.Hubs;
using demo_158.MVVM.Model;
using Microsoft.EntityFrameworkCore;

namespace demo_158.MVVM.ViewModel;

public class AddNewConversationViewModel :ViewModelBase
{
    private readonly ConnectionManager _connection;
    private ContactUserModel _selectedUser;
    public ObservableCollection<ContactUserModel> Users { get; set; } = new();

    public ContactUserModel SelectedUser    
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            OnPropertyChanged();
            WeakReferenceMessenger.Default.Send(new CreateNewConversationEvent(SelectedUser));
        }
    }

    public AddNewConversationViewModel(ConnectionManager connection)
    {
        _connection = connection;
        GetUsersToTak();
    }

    public void GetUsersToTak()
    {
        _connection.On<List<ContactUserModel>>("GetUsersToTalk", (users =>
        {
            foreach (var user in users)
            {
                    Users.Add(user);
            }

        }));
    }
}