using demo_158.Base;
using demo_158.Hubs;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using demo_158.MVVM.View;
using demo_158.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;


namespace demo_158.MVVM.Model
{

    public class ConversationViewModel : ViewModelBase
    {
        private readonly ConnectionManager _connection;
        private readonly IMessageServices _messagesServices;
        private readonly IServiceProvider _service;
        private UserModelFromServer _userModelFromServer;
        private ContactUserModel _contactUserModel;
        private ICommand _showUserContentCommand;
        private ICommand _sendMessageCommand;
        private ICommand _deleteConversation;
        private int _id;
        private string _text;
        private MessagesModel? _lastMessage;
        private string _contactState;

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

        public ContactUserModel ContactUserModel
        {
            get => _contactUserModel;
            set => SetField(ref _contactUserModel, value);
        }

        public string ContactState  
        {
            get => _contactState;
            set => SetField(ref _contactState, value);
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
        public ConversationViewModel(ConnectionManager connection,
            IMessageServices messagesServices,
            IServiceProvider service
            )
        {
            _connection = connection;
            _messagesServices = messagesServices;
            _service = service;
        }
        public ICommand ShowUserContentCommand => _showUserContentCommand ?? new GeneralCommand((ShowUserContentAction));
        public ICommand SendMessageCommand => _sendMessageCommand ??= new GeneralCommand(async () => await SendTextMessageExecuteAction());

        public ICommand DeleteConversation => _deleteConversation ?? new GeneralCommand(() =>
        {
           
        });
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
                UserId = UserModelFromServer.Id,
                Username = UserModelFromServer.Username,
            };
            var result = 
                _messagesServices.MessageModelMapping(messageFromUser, UserModelFromServer.Username,this.Messages.OrderBy(e=>e.SentTime).LastOrDefault()?.SenderName);
            await _connection.SendAsync("SendMessageToPrivate", ContactUserModel.ContactUsername, messageFromUser);
            Messages.Add(result);
            LastMessage = Messages.LastOrDefault();
            Text = String.Empty;
        }
        private void ShowUserContentAction()
        {
            var contactProfile = _service.GetRequiredService<ContactProfileVeiw>();
            contactProfile.Profile = ContactUserModel;
            contactProfile.ShowDialog();
        }
    }

    
}
