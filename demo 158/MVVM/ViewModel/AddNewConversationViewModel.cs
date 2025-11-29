using System.Collections.ObjectModel;
using demo_158.Base;
using demo_158.Hubs;
using demo_158.MVVM.Model;
using Microsoft.EntityFrameworkCore;

namespace demo_158.MVVM.ViewModel;

public class AddNewConversationViewModel :ViewModelBase
{
    private readonly ConnectionManager _connection;
    public ObservableCollection<ContactUserModel> Users { get; set; } = new();

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