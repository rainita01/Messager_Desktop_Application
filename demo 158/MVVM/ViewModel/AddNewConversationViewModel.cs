using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using demo_158.Base;
using demo_158.EventsPublish;
using demo_158.Hubs;
using demo_158.MVVM.Model;
using demo_158.MVVM.View;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace demo_158.MVVM.ViewModel;

public class AddNewConversationViewModel :ViewModelBase
{
    private readonly ConnectionManager _connection;
    private readonly IServiceProvider _service;
    private ContactUserModel _selectedUser;
    private ICommand _creatGroupCommand;
    public event EventHandler RequestClose;
    public ObservableCollection<ContactUserModel> Users { get; set; } = new();

    public ContactUserModel SelectedUser    
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            OnPropertyChanged();
            WeakReferenceMessenger.Default.Send(new CreateNewConversationEvent(SelectedUser));
            RequestClose.Invoke(this,EventArgs.Empty);
        }
    }

    public AddNewConversationViewModel(ConnectionManager connection,IServiceProvider service)
    {
        _connection = connection;
        _service = service;
        GetUsersToTak();
    }


    public ICommand CreateGroupCommand => _creatGroupCommand ?? new GeneralCommand(CreateGroupExecute);
  

    private void CreateGroupExecute()
    {
        var createGroupView = _service.GetRequiredService<CreateGroupView>();   
        RequestClose.Invoke(this,EventArgs.Empty);
        createGroupView.ShowDialog();
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