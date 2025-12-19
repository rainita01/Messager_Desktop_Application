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
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using demo_158.EventsPublish;
using Microsoft.Extensions.Logging;
using demo_158.MVVM.ViewModel.ConversationViewModels;

namespace demo_158.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private object _currentView;
        private readonly IServiceProvider _service;
        private readonly DefaultMessageView _defaultMessageView;
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
    
        public ICollectionView? ConversationsView { get; set; }
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
                IServiceProvider service,
                DefaultMessageView defaultMessageView
                )
            {
                _connection = connection;
                _myInformationRepository = myInformationRepository;
                _conversationsRepository = conversationsRepository;
                _myMessagesRepository = myMessagesRepository;
                _messagesServices = messagesServices;
                _conversationServices = conversationServices;
                _service = service;
                _defaultMessageView = defaultMessageView;
                CurrentView = defaultMessageView;
              
                OnStateChanged(_connection.ConnectionState);
                CheckUsersState();
                UserModelFromServer = _myInformationRepository.MyUserInfo;
                EventCallsHandler();
                WeakReferenceMessenger.Default.Register<MessageSendedSuccessEvent>(this, (r, m) =>
                {
                    ConversationsView?.Refresh();
                });
                ConversationsView = CollectionViewSource.GetDefaultView(Conversations);
                ConversationsView.SortDescriptions.Add(new SortDescription("LastMessageDateTime", ListSortDirection.Descending));
            }

             // commands
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
        //----------------------------------------------------------------------------
        // فراخوان های اونت ها
        private void EventCallsHandler()
        {
            WeakReferenceMessenger.Default.Register<CreateNewConversationEvent>(this, NewConversationCreated);

            _connection.OnStateChanged -= OnStateChanged;
            _connection.OnStateChanged += OnStateChanged;

            _conversationsRepository.SuccessReceiveConversations += SuccessReceiveConversations;
            _conversationsRepository.SuccessCreatedConversation += SuccessCreatedConversation;
            _conversationsRepository.SuccessDeletedConversation += ContactDeletedConversation;
            _myInformationRepository.ImageChanged -= ImageChanged;
            _myInformationRepository.ImageChanged += ImageChanged;


            _myMessagesRepository.ContactDeletedMessageEvent += ContactDeletedMessageEvent;
            _myMessagesRepository.ContactEditedMessageEvent += ContactEditedMessageEvent;
           
            WeakReferenceMessenger.Default.Register<SuccessMessageReceivedEvent>(this, SuccessMessageReceived );
            WeakReferenceMessenger.Default.Register<SuccessDeletedConversation>(this, SuccessDeleteConversationHandler);
            WeakReferenceMessenger.Default.Register<SuccessDeletedMessage>(this, (recipient, message) =>
            {
                ConversationsView?.Refresh();
            });

            WeakReferenceMessenger.Default.Register<ContactUserChangedProfileEvent>(this, ContactEditedProfileHandler);
        }

        private void ContactDeletedConversation(int obj)
        {
            var conversaiton = Conversations?.FirstOrDefault(i => i.Id == obj);
            if (conversaiton != null)
            {
                Conversations?.Remove(conversaiton);
                ConversationsView?.Refresh();
            }
           
        }

        private void ContactEditedProfileHandler(object recipient, ContactUserChangedProfileEvent message)
        {
            var contactUser =
                Conversations?.FirstOrDefault(e => e.ContactUserModel.ContactUsername == message.Value.Username);
            if (contactUser != null)
            {
                contactUser.ContactUserModel.Bio = message.Value.Bio;
                contactUser.ContactUserModel.Email = message.Value.Email;
            }
        }

        private void SuccessDeleteConversationHandler(object recipient, SuccessDeletedConversation message)
        {
            var conversation = Conversations?.FirstOrDefault(e => e.Id == message.Value);
            if (ConversationViewModel == conversation)
            {

                CurrentView = _defaultMessageView;
            }

            Conversations?.Remove(conversation);
        }

        private void NewConversationCreated(object recipient, CreateNewConversationEvent m)
        {
            var value = m.Value;

            var conversation = Conversations?.FirstOrDefault(e => e.ContactUserModel.ContactUsername == value.ContactUsername);
            if (conversation == null)
            {
                conversation = new ConversationViewModel(_connection, _messagesServices, _service)
                {

                    ContactUserModel = value,
                    UserModelFromServer = UserModelFromServer,
                    ContactState = value.State.ToString(),
                    Messages = new ObservableCollection<MessageViewModel>()
                };
                Conversations?.Add(conversation);

            }
            ConversationViewModel = conversation;
            if (CurrentView is DefaultMessageView)
            {
                var conversationView = _service.GetRequiredService<ConversationView>();
                CurrentView = conversationView;
            }
        }


        //----------------------------------------------------
        // عوض کردن رنگ state 
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
        //---------------------------------------------------
        // این خط برای وقتی هست که ی مکالمه رو انتخاب میکنیم
        private void  OnSelectedConversationChangedAsync()
        {
            var messageView = _service.GetRequiredService<ConversationView>();
            messageView.DataContext = ConversationViewModel;
          
            CurrentView = messageView;
        }
        //-----------------------------------------------
        // این کد برای اضافه کردن پیام ها بعد دریافته 
        private async void SuccessMessageReceived(object recipient, SuccessMessageReceivedEvent obj)    
        {
            MessagesModel lastmessage;
            MessagesModel message;
            var conversation = Conversations?.FirstOrDefault(e => e.Id == obj.Value.ConversationId);
            if (conversation == null)
            {
                var getContactUser = await _connection.InvokeAskDataAsync<ContactUserModel, string>("AskNewMessageUser", obj.Value.Username);
                conversation = new ConversationViewModel(_connection, _messagesServices, _service)
                {
                    Id = obj.Value.ConversationId,
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
                message = _messagesServices.MessageModelMapping(obj.Value, UserModelFromServer.Username, lastmessage.SenderName);
            }
            else
                message = _messagesServices.MessageModelMapping(obj.Value, UserModelFromServer.Username, null);

            if (message.IsSeen == false && message.SenderName != UserModelFromServer.Username && conversation != ConversationViewModel)
                conversation.UnreadCount++;

            if (conversation == ConversationViewModel)
                message.IsSeen = true;

            if (message.SenderName == conversation.ContactUserModel.ContactUsername)
                message.Image = conversation.ContactUserModel.ContactImage;

            else
            {
                message.IsMyMessage = true;
                message.Image = _userModelFromServer.Image;
            }


            Application.Current.Dispatcher.Invoke(() =>
            {
                AddToConversation(message, conversation);
                ConversationsView?.Refresh();
            });
          
        }
        //-------------------------------------------------------------------------------
        // این متدها هم برای مکالمه هاست 
        private void SuccessCreatedConversation(int conversationId, string contactUsername)
        {
            var conversation =
                Conversations?.FirstOrDefault(e => e.ContactUserModel.ContactUsername == contactUsername);
            if (conversation != null)
            {
                conversation.Id = conversationId;
            }
        }
        private void SuccessReceiveConversations(List<ConversationModel> obj)
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
                    ContactUser = conversationViewModel.ContactUserModel,
                    Id = 1 + conversationViewModel.Messages.Count,
                };
                conversationViewModel.Messages?.Add(messageViewModel);
                conversationViewModel.LastMessage = conversationViewModel.Messages.LastOrDefault()?.Message;
                conversationViewModel.LastMessageDateTime = conversationViewModel.LastMessage?.SentTime;
              
            });
        }

        //--------------------------------------------------------------------------------------------------
        // وقتی پیام ها از طریق کسی که داریم صحبت میکنیم حذف یا ادیت میشه
        private async void ContactEditedMessageEvent(EditMessageModel newMessage)
        {
            await Task.Run((() =>
            {
                var conversation = Conversations?.FirstOrDefault(e => e.ContactUserModel.ContactUsername == newMessage.SenderUsername);
                var message = conversation?.Messages?.FirstOrDefault(i => i.Message.Id == newMessage.MessageId);
                if (message == null)
                    return;

                message.Message.Text = newMessage.NewText;
                message.Message.IsMessageEdited = newMessage.IsEdited;

            }));
        }

        private void ContactDeletedMessageEvent(int messageId,string contactName)
        {
            var conversation = Conversations?.FirstOrDefault(e => e.ContactUserModel.ContactUsername == contactName);
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
            var lastMessage = conversation.LastMessage;
            conversation.LastMessageDateTime  = lastMessage?.SentTime;
            ConversationsView?.Refresh();
        }
        //----------------------------------------------------------------------------------------------
        // این برای وقتیه یکی از یوزر ها افلاین یا انلاین شده باشه
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
        //------------------------------------------------------------------------
        // این کدم وقتی عکستو عوض کردی فراخوانی میشه
        private void ImageChanged(byte[] obj)
        {
            this.UserModelFromServer.Image = _myInformationRepository.MyUserInfo.Image;
        }
    }
}
