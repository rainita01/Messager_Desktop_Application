using demo_158.Base;
using demo_158.MVVM.Model;
using demo_158.MVVM.View;
using demo_158.MVVM.View.Model;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using demo_158.Hubs;
using demo_158.Repository;
using demo_158.Services.Enums;
using demo_158.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

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

                var userView = service.GetService<DefaultMessageView>();
                CurrentView = userView;

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
                _conversationsRepository.SuccessReceiveConversations += SuccessReceiveConversations;
                _myMessagesRepository.MessageReceived += SuccessReceiveMessages;
                _connection.OnStateChanged += OnStateChanged;
            }

            

            public ICommand OpenProfileCommand => openProfileCommand ?? new GeneralCommand((() =>
             {
                 var profileView = _service.GetService<ProfileView>();

                 profileView.ShowDialog();
          
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
        private void SuccessReceiveMessages(MessageModelFromServer obj)
        {
            MessagesModel lastmessage;
            MessagesModel message;

            Application.Current.Dispatcher.Invoke(() =>
            {
                var conversation = Conversations?.FirstOrDefault(e => e.Id == obj.ConversationId);
                if (conversation.Messages.Count > 0)
                {
                   lastmessage = conversation.Messages.Last();
                     message = _messagesServices.MessageModelMapping(obj, UserModelFromServer.Username, lastmessage.SenderName);

                }
                else
                {
                     message = _messagesServices.MessageModelMapping(obj, UserModelFromServer.Username, null); 
                }
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
        private void AddToConversation(MessagesModel obj, ConversationViewModel conversationView)
        {
            if (conversationView == null)
            {
                throw new Exception("Conversation not found");
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                conversationView.Messages.Add(obj);
                conversationView.LastMessage = conversationView.Messages.LastOrDefault();
            });
        }

        private void CheckUsersState()
        {
            _connection.OnAsync<State, string>("CheckUsersState", (state, username) =>
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
