
using System.Windows;
using System.Windows.Input;
using demo_158.Base;
using demo_158.Hubs;
using demo_158.MVVM.Model;
using demo_158.MVVM.View;

using Microsoft.Extensions.DependencyInjection;

namespace demo_158.MVVM.ViewModel
{
    public class MessageViewModel : ViewModelBase
    {
        private readonly ConnectionManager _connectionManager;
        private readonly IServiceProvider _service;
        private ICommand editMessageCommand;
        private ICommand deleteMessageCommand;
        private ICommand copyTextCommand;
        private MessagesModel _message;
        private ICommand openProfile;

        public ContactUserModel ContactUser { get; set; }
        public MessagesModel Message
        {
            get => _message;
            set => SetField(ref _message, value);
        }

        public string Username { get; set; }
       public MessageViewModel(ConnectionManager connectionManager,IServiceProvider service)
       {
           _connectionManager = connectionManager;
           _service = service;
       }



       public ICommand EditMessageCommand => editMessageCommand ?? new GeneralCommand(() =>
       {
           if (Message.SenderName != Username)
           {
               MessageBox.Show("U cannot Edit another person message");
               return;
           }
           var editMessageView = _service.GetRequiredService<EditMessageView>();
           editMessageView.ContactUser = ContactUser;
          editMessageView.Text.Text = Message.Text;
          editMessageView.MessageId = Message.Id;
          editMessageView.Username = Username;
          editMessageView.ShowDialog();
       });
       public  ICommand DeleteMessageCommand => deleteMessageCommand ?? new GeneralCommand(() =>
       {
           if (Message.SenderName != Username )
           {
               MessageBox.Show("U cannot delete another person message");
               return;
           }

           var result =  MessageBox.Show("Are you sure ?", "Delete", MessageBoxButton.YesNo);
           if (result == MessageBoxResult.No)
           {
               return;
           }
           _connectionManager.SendAsync("DeleteMessage", Message.Id,Username,ContactUser.ContactUsername);
       });
       public ICommand CopyTextCommand => copyTextCommand ?? new GeneralCommand(() =>
       {
           Clipboard.SetDataObject(Message.Text);
           MessageBox.Show("copy");
       });

       public ICommand OpenProfile => openProfile ?? new GeneralCommand(ShowUserProfile);

       public void ShowUserProfile()
       {
           var profile = _service.GetRequiredService<ContactProfileVeiw>();
           profile.Profile = ContactUser;
           profile.ShowDialog();

       }
    }
}
