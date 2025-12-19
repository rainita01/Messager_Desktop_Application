using CommunityToolkit.Mvvm.Messaging;
using demo_158.Base;
using demo_158.EventsPublish;
using demo_158.Hubs;
using demo_158.MVVM.View;
using demo_158.MVVM.ViewModel;
using demo_158.Services.Enums;
using demo_158.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using CommunityToolkit.Mvvm.Input;


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
        private ICommand openProfileCommand;
        private ICommand _isSeenCommand;
        private string _text;
        private MessagesModel? _lastMessage;
        private string _contactState;
        private int _id;
        private DateTime? _lastMessageDateTime;
        private int _unreadCount;
        private Visibility _newMessageVisibility = Visibility.Hidden;

        private readonly HashSet<long> _seenMessageIds = new();

        private long _lastReadMessageId = 0;
        public long LastReadMessageId
        {
            get => _lastReadMessageId;
            private set => _lastReadMessageId = value;
        }
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
        public ObservableCollection<MessageViewModel>? Messages { get; set; } = new();
        public EventHandler? MessageAdded { get; set; }
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

        public DateTime? LastMessageDateTime
        {
            get => _lastMessageDateTime;
            set => SetField(ref _lastMessageDateTime, value);
        }

        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }

        public int UnreadCount    
        {
            get => _unreadCount;
            set 
            {
                if (value > 0)
                {
                    NewMessageVisibility = Visibility.Visible;
                }
                else
                {
                    NewMessageVisibility = Visibility.Hidden;
                }

                _unreadCount = value;
                OnPropertyChanged();
            }
        }

        public Visibility NewMessageVisibility
        {
            get => _newMessageVisibility;
            set => SetField(ref _newMessageVisibility, value);
        }

        public ConversationViewModel(ConnectionManager connection,
            IMessageServices messagesServices,
            IServiceProvider service
            )
        {
          
            _connection = connection;
            _messagesServices = messagesServices;
            _service = service;
           
            MessageEdited();
            WeakReferenceMessenger.Default.Register<SuccessDeletedMessage>(this, (recipient, message) =>
            {
                var messageToRemove = Messages.FirstOrDefault(e => e.Message.Id == message.Value);
                if (messageToRemove is null)
                {
                    return;
                }
                Messages.Remove(messageToRemove);
                LastMessage = Messages.LastOrDefault()?.Message;
                LastMessageDateTime = LastMessage?.SentTime;

            });

           
        }

        public ICommand ShowUserContentCommand => _showUserContentCommand ??= new GeneralCommand((ShowUserContentAction));
        public ICommand SendMessageCommand => _sendMessageCommand ??= new MyRelayCommand(async () => await SendTextMessageExecuteAction());

        public ICommand DeleteConversation => _deleteConversation ??= new MyRelayCommand(async () =>
        {
          var result = await _connection.InvokeAsync<int,string,ServerAnswer>("DeleteConversation", Id, ContactUserModel.ContactUsername);

          if (result is ServerAnswer.ok)
          {
              WeakReferenceMessenger.Default.Send(new SuccessDeletedConversation(Id));
          }
        });
        public async Task MarkAllVisibleAsSeenAsync()
        {
            var lastIncoming = Messages
                .Where(m => !m.Message.IsMyMessage)
                .OrderBy(m => m.Id)
                .LastOrDefault();

            if (lastIncoming != null)
               await OnUserScrolledAsync(lastIncoming.Id);
        }
        public async Task OnUserScrolledAsync(long lastVisibleId)
        {
            if (lastVisibleId <= LastReadMessageId)
                return;

            LastReadMessageId = lastVisibleId;
           await ApplySeenAsync();
        }
  
     
        private async Task ApplySeenAsync()
        {
            var messagesId = new List<int>(Messages
                .Where(m => !m.Message.IsMyMessage)
                .Where(e => !e.Message.IsSeen)
                .Select(i => i.Message.Id));
            if (messagesId.Count > 0)
            {
                await _connection.SendAsync("SendSeenMessages", messagesId);
            }
            foreach (var msg in Messages)
            {
                msg.Message.IsSeen =
                    !msg.Message.IsMyMessage &&
                    msg.Id <= LastReadMessageId;
            }

            UnreadCount = Messages.Count(m =>
                !m.Message.IsMyMessage &&
                m.Id > LastReadMessageId
            );

          

        }
        private async Task SendTextMessageExecuteAction()
        {
            if (string.IsNullOrEmpty(Text))
               return;

            var messageFromUser = new MessageModelFromUser()
            {
                MessageType = MessageTypes.Text,
                ConversationId = this.Id,
                Text = this.Text,
                Object = null,
                UserId = UserModelFromServer.Id,
                Username = UserModelFromServer.Username,
                
            };
            var messageModel = _messagesServices.MessageModelMapping(messageFromUser, UserModelFromServer.Username,Messages.OrderBy(e=>e.Message.SentTime).LastOrDefault()?.Message.SenderName);
            messageModel.Image = _userModelFromServer.Image;
            var messageViewModel = new MessageViewModel(_connection, _service)
            {
                Message = messageModel,
                Username = UserModelFromServer.Username,
                ContactUser = ContactUserModel
            };
            LastMessageDateTime = DateTime.Now;
            
            messageViewModel.Message.Id = await _connection.InvokeAsync<string, MessageModelFromUser, int>("SendMessageToPrivate", ContactUserModel.ContactUsername, messageFromUser);
         
            Messages.Add(messageViewModel);
            LastMessage = Messages.LastOrDefault()?.Message;
            WeakReferenceMessenger.Default.Send(new MessageSendedSuccessEvent(true));
            Text = String.Empty;
        }

        public void MessageEdited()
        {
             _connection.On<ServerAnswer,string,int>("MessageEdited", ((answer, Text, id) =>
            {
                if (answer == ServerAnswer.ok)
                {
                    var message = Messages.FirstOrDefault(e => e.Message.Id == id);
                    if (message != null)
                    {
                        message.Message.Text = Text;
                        message.Message.IsMessageEdited = true;
                    }
                }
            }));
        }
        private void ShowUserContentAction()
        {
            var contactProfile = _service.GetRequiredService<ContactProfileVeiw>();
            contactProfile.Profile = ContactUserModel;
            contactProfile.ShowDialog();
        }
    }

    
}
