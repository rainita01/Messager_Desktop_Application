using CommunityToolkit.Mvvm.Messaging;
using demo_158.Base;
using demo_158.Hubs;
using demo_158.MVVM.Model;
using demo_158.MVVM.View;
using demo_158.MVVM.View.Model;
using demo_158.Repository;
using demo_158.Services.Enums;
using demo_158.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using demo_158.EventsPublish;

namespace demo_158.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        
        private object _currentView;
        private readonly IServiceProvider _service;
        private readonly ConnectionManager _connection;
        private readonly MyInformationRepository _myInformationRepository;
        private readonly MyConversationsRepository _conversationsRepository;
        private readonly MyMessagesRepository _myMessagesRepository;
        private readonly IMessageServices _messagesServices;
        private readonly IConversationServices _conversationServices;
        private ICommand moveAndDrugCommand;
        private ICommand openProfileCommand;
      
        private ConversationViewModel _conversationViewModel;
        private UserModelFromServer _userModelFromServer;
        private SolidColorBrush _cycleFillerBrush;
        private ICommand addConversationCommand;

        public ObservableCollection<ConversationViewModel>? Conversations { get; set; } = new();


        public SolidColorBrush CycleFillerBrush
        {
            get => _cycleFillerBrush;
            set => SetField(ref _cycleFillerBrush, value);
        }

        public ConversationViewModel ConversationViewModel 
        {
            get => _conversationViewModel;
            set
            {
                if (SetField(ref _conversationViewModel, value))
                {
                    OnSelectedConversationChangedAsync();
                }
            }
        }

        public UserModelFromServer UserModelFromServer
        {
            get => _userModelFromServer;
            set
            {
                if (SetField(ref _userModelFromServer, value)) OnPropertyChanged(nameof(OpenProfileCommand));
            }
        }
        public object CurrentView
        {
            get => _currentView;
            set => SetField(ref _currentView, value);
        }

            public MainViewModel( 
                
                ConnectionManager connection,
                MyInformationRepository myInformationRepository,
                MyConversationsRepository conversationsRepository,
                MyMessagesRepository myMessagesRepository,
                IMessageServices messagesServices,
                IConversationServices conversationServices,
                IServiceProvider service
                )
            {
                _connection = connection;
                _myInformationRepository = myInformationRepository;
                _conversationsRepository = conversationsRepository;
                _myMessagesRepository = myMessagesRepository;
                _messagesServices = messagesServices;
                _conversationServices = conversationServices;
                _service = service;

                var defaultMessageView = service.GetService<DefaultMessageView>();
                CurrentView = defaultMessageView;

                Application.Current.Dispatcher.Invoke(() =>
                {

                    switch (_connection.ConnectionState)
                    {
                        case HubConnectionState.Connecting:
                            CycleFillerBrush = new SolidColorBrush(Colors.LightGoldenrodYellow);
                            break;
                        case HubConnectionState.Connected:
                            CycleFillerBrush = new SolidColorBrush(Colors.Chartreuse);
                            break;
                        case HubConnectionState.Disconnected:
                            CycleFillerBrush = new SolidColorBrush(Colors.OrangeRed);
                            break;
                        case HubConnectionState.Reconnecting:
                            CycleFillerBrush = new SolidColorBrush(Colors.LightGoldenrodYellow);
                            
                            break;
                    }

                });
                CheckUsersState();
                UserModelFromServer = _myInformationRepository.MyUserInfo;
                // این برای وقتیه که میخوایم مکالمه بریم حالا جدید یا قدیمیش
                WeakReferenceMessenger.Default.Register<CreateNewConversationEvent>(this, (r, m) => {
                var value = m.Value;
                if (CurrentView is DefaultMessageView)
                {
                    var conversationView = _service.GetRequiredService<ConversationView>();
                    CurrentView = conversationView;
                }
                var conversation =
                    Conversations?.FirstOrDefault(e => e.ContactUserModel.ContactUsername == value.ContactUsername);
                if (conversation == null)
                {
                    conversation = new ConversationViewModel(_connection, _messagesServices, _service)
                    {
                        ContactUserModel = value,
                        UserModelFromServer = UserModelFromServer,
                        ContactState = "OffLine",
                        Messages = new ObservableCollection<MessageViewModel>()
                    };
                    Conversations?.Add(conversation);
                    
                }

                ConversationViewModel = conversation;
            });

                _conversationsRepository.SuccessReceiveConversations += SuccessReceiveConversations;
                _myMessagesRepository.MessageReceivedEvent +=  SuccessReceiveMessages;
                _myInformationRepository.ImageChanged += ImageChanged; 
                _connection.OnStateChanged += OnStateChanged;
                _myMessagesRepository.ContactDeletedMessageEvent += ContactDeletedMessageEvent;
                _myMessagesRepository.ContactEditedMessageEvent += ContactEditedMessageEvent;
                _myInformationRepository.ImageChanged -= ImageChanged;
                _connection.OnStateChanged -= OnStateChanged;
        }

        private async void ContactEditedMessageEvent(EditMessageModel newMessage)
        {
            await Task.Run((() =>
            {
                var conversation = Conversations?.FirstOrDefault(e => e.ContactUserModel.ContactUsername == newMessage.SenderUsername);
                var message = conversation?.Messages?.FirstOrDefault(i => i.Message.Id == newMessage.MessageId);
                if (message == null)
                    return;

                message.Message.Text = newMessage.NewText;

            }));
        }

        private  void ContactDeletedMessageEvent(int messageId, string contactName)
        {
           var conversation =  Conversations?.FirstOrDefault(e => e.ContactUserModel.ContactUsername == contactName);
            if (conversation == null)
            {
                return;
            }

            var message = conversation.Messages?.FirstOrDefault(i => i.Message.Id == messageId);
            if (message == null)
            {
                return;
            }

            conversation.Messages?.Remove(message);
        }


        private void ImageChanged(byte[] obj)
            {
                this.UserModelFromServer.Image = _myInformationRepository.MyUserInfo.Image;
            }


            public ICommand OpenProfileCommand => openProfileCommand ?? new GeneralCommand((() =>
             {
                 var profileView = _service.GetService<ProfileView>();

                 profileView.ShowDialog();
          
              }));

              public ICommand AddConversationCommand => addConversationCommand ?? new GeneralCommand((async () =>
              {
                  var addView = _service.GetRequiredService<AddNewConversationView>();
                  await _connection.SendAsync("AskUsers", UserModelFromServer.Id);
                  addView.ShowDialog();
              }));

        public ICommand MoveAndDrugCommand => moveAndDrugCommand ?? new GeneralCommand((() =>
        {
            Application.Current.Windows.OfType<MainView>().FirstOrDefault()?.DragMove();
        }));
        private void OnStateChanged(HubConnectionState obj)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {

                switch (obj)
                {
                    case HubConnectionState.Connecting:
                        CycleFillerBrush = new SolidColorBrush(Colors.LightGoldenrodYellow);
                        break;
                    case HubConnectionState.Connected:
                        CycleFillerBrush = new SolidColorBrush(Colors.Chartreuse);
                        break;
                    case HubConnectionState.Disconnected:
                        CycleFillerBrush = new SolidColorBrush(Colors.OrangeRed);
                        break;
                    case HubConnectionState.Reconnecting:
                        CycleFillerBrush = new SolidColorBrush(Colors.LightGoldenrodYellow);
                        break;
                }

            });
          
        }
        // این خط برای وقتی هست که ی مکالمه رو انتخاب میکنیم
        private void  OnSelectedConversationChangedAsync()
        {
            var messageView = _service.GetRequiredService<ConversationView>();
            messageView.DataContext = ConversationViewModel;
            CurrentView = messageView;
        }
        // این کد برای اضافه کردن پیام ها بعد دریافته 
        private async void SuccessReceiveMessages(MessageModelFromServer obj)
        {
            MessagesModel lastmessage;
            MessagesModel message;
            var conversation = Conversations?.FirstOrDefault(e => e.Id == obj.ConversationId);
            if (conversation is null)
            {
                var getContactUser = await _connection.InvokeAskDataAsync<ContactUserModel, string>("AskNewMessageUser", obj.Username);
                    conversation = new ConversationViewModel(_connection, _messagesServices, _service)
                    {

                        Id = obj.ConversationId,
                        ContactUserModel = getContactUser,
                        UserModelFromServer = UserModelFromServer,
                        Messages = new ObservableCollection<MessageViewModel>(),

                    };
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        Conversations?.Add(conversation);
                    });
                    
            }
            if (conversation.Messages?.Count > 0)
            {
                lastmessage = conversation.Messages.Last().Message;
                message = _messagesServices.MessageModelMapping(obj, UserModelFromServer.Username, lastmessage.SenderName);

            }
            else
            {
                message = _messagesServices.MessageModelMapping(obj, UserModelFromServer.Username, null);
            }

            if (message.SenderName == conversation.ContactUserModel.ContactUsername)
            {
                message.Image = conversation.ContactUserModel.ContactImage;
            }
            else
            {
                message.Image = _userModelFromServer.Image;
            }

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                AddToConversation(message, conversation);
            });
        }
        // این دو  متد هم برای ضافه کردن مکالمه ها بعد از دریافته 
        private void SuccessReceiveConversations(List<ConversationModelFromServer> obj)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                foreach (var conversationModelFromServer in obj)
                {
                    var conversation = _conversationServices.ConversationModelMapping(conversationModelFromServer);
                    if (conversationModelFromServer.ContactUserModel.State == State.Online)
                    {
                        conversation.ContactState = "Online";
                    }
                    else
                    {
                        conversation.ContactState =
                            conversationModelFromServer.ContactUserModel.LastActiveTime.ToString("MM-dd HH:mm");
                    }
                    Conversations?.Add(conversation);
                    _connection?.SendAsync("ReceiveMessages", conversationModelFromServer.Id);
                }

            });
        }
        private void AddToConversation(MessagesModel messageModel, ConversationViewModel conversationViewModel)
        {
            if (conversationViewModel == null)
            {
                throw new Exception("Conversation not found");
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                var messageViewModel = new MessageViewModel(_connection,_service)
                {
                    Message = messageModel,
                    Username = _userModelFromServer.Username,
                    ContactUser = conversationViewModel.ContactUserModel
                };
                conversationViewModel.Messages.Add(messageViewModel);
                conversationViewModel.LastMessage = conversationViewModel.Messages.LastOrDefault()?.Message;
            });
        }

        private void CheckUsersState()
        {
            _connection.On<State, string>("CheckUsersState", (state, username) =>
            {
                var conversation = Conversations
                    .FirstOrDefault(e => e.ContactUserModel.ContactUsername == username);

                if (conversation == null)
                    return;

                switch (state)
                {
                    case State.Online:
                        conversation.ContactState = "Online";
                        break;

                    case State.Offline:
                        conversation.ContactState = DateTime.Now.ToString("MM-dd HH:mm");
                        break;
                }

            });
       
        }
    }
}
