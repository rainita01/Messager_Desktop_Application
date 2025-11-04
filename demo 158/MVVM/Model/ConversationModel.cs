using demo_158.Base;
using demo_158.Hubs;
using System.Collections.ObjectModel;
using System.Windows.Input;
using demo_158.Services.Interfaces;



namespace demo_158.MVVM.Model
{

    public class ConversationModel : ViewModelBase
    {
        private readonly ConnectionManager _connection;
        private readonly IMessageServices _messagesServices;
        private UserModelFromServer _userModelFromServer;
        private ContactUserInfo _contactUserInfo;
        private ICommand _showUserContentCommand;
        private ICommand _sendMessageCommand;
        private int _id;
        private string _text;
        private MessagesModel? _lastMessage;

        public int Id
        {
            get => _id;
            set => SetField(ref _id, value);
        }
        public UserModelFromServer UserModelFromServer
        {
            get => _userModelFromServer;
            set => SetField(ref _userModelFromServer, value);
        }

        public ObservableCollection<MessagesModel> Messages { get; set; }
       
        public ContactUserInfo ContactUserInfo
        {
            get => _contactUserInfo;
            set => SetField(ref _contactUserInfo, value);
        }

        public MessagesModel? LastMessage  
        {
            get => _lastMessage;
            set => SetField(ref _lastMessage, value);
        }

        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }
        public ConversationModel(ConnectionManager connection,
            IMessageServices messagesServices
            )
        {
            _connection = connection;
            _messagesServices = messagesServices;
            
        }
        public ICommand ShowUserContentCommand => _showUserContentCommand ?? new GeneralCommand((ShowUserContentAction));
        public ICommand SendMessageCommand => _sendMessageCommand ??= new GeneralCommand(async () => await SendTextMessageExecuteAction());
        private async Task SendTextMessageExecuteAction()
        {
            if (string.IsNullOrEmpty(Text))
                throw new Exception();

            var messageFromUser = new MessageModelFromUser()
            {
                MessageType = MessageTypes.Text,
                ConversationId = this.Id,
                Text = this.Text,
                Object = null,
                UserId = UserModelFromServer.UserId,
                Username = UserModelFromServer.Username,
            };
            var result = 
                _messagesServices.MessageModelMapping(messageFromUser, UserModelFromServer.Username,this.Messages.OrderBy(e=>e.SentTime).LastOrDefault()?.SenderName);
            await _connection.SendAsync("SendMessageToPrivate", ContactUserInfo.ContactUsername, messageFromUser);
            Messages.Add(result);
            LastMessage = Messages.LastOrDefault();
            Text = String.Empty;
        }
        private void ShowUserContentAction()
        {
            var profile = new ProfileEditModel()
            {
                Username = ContactUserInfo.ContactUsername,
            };
        }
    }

    public class ConversationModelFromServer        
    {
        public int Id { get; set; }
        public bool IsConversationPrivateChat { get; set; }
        public DateTime CreatedTime { get; set; }
        public ContactUserInfo ContactUserInfo { get; set; }
        
    }
}
