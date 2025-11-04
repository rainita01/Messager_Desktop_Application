using demo_158.Base;
using demo_158.MVVM.Model;
using demo_158.MVVM.View;
using demo_158.MVVM.View.Model;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using demo_158.Hubs;
using demo_158.Repository;
using demo_158.Services.Interfaces;

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
        private ICommand moveAndDrugCommand;
        private ICommand openProfileCommand;
        private ConversationModel _conversationModel;
        private UserModelFromServer _userModelFromServer;

        public ObservableCollection<ConversationModel>? Conversations { get; set; } = new();

        public ConversationModel ConversationModel 
        {
            get => _conversationModel;
            set
            {
                if (SetField(ref _conversationModel, value))
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
                IServiceProvider service
                )
            {
                _connection = connection;
                _myInformationRepository = myInformationRepository;
                _conversationsRepository = conversationsRepository;
                _myMessagesRepository = myMessagesRepository;
                _messagesServices = messagesServices;
                _service = service;

                var userView = service.GetService<DefaultMessageView>();
                CurrentView = userView;

                UserModelFromServer = _myInformationRepository.MyUserInfo;
                _conversationsRepository.SuccessReceiveConversations += SuccessReceiveConversations;
                _myMessagesRepository.MessageReceived += SuccessReceiveMessages;

            }

            public ICommand OpenProfileCommand => openProfileCommand ?? new GeneralCommand((() =>
        {
            var profileView = _service.GetService<ProfileView>();

            if (profileView.User == null)
            {
                profileView.User = UserModelFromServer;
            }
            profileView.ShowDialog();
          
        }));

        public ICommand MoveAndDrugCommand => moveAndDrugCommand ?? new GeneralCommand((() =>
        {
            Application.Current.Windows.OfType<MainView>().FirstOrDefault()?.DragMove();
        }));
        private void  OnSelectedConversationChangedAsync()
        {
            var messageView = _service.GetRequiredService<MessageAndTalkView>();
            messageView.DataContext = ConversationModel;
            CurrentView = messageView;
        }
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

        private void SuccessReceiveConversations(List<ConversationModel> obj)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                foreach (var conversationModel in obj)
                {

                    Conversations.Add(conversationModel);
                    _connection.SendAsync("ReceiveMessages", conversationModel);
                }

            });
        }

        private void AddToConversation(MessagesModel obj, ConversationModel conversation)
        {
            if (conversation == null)
            {
                throw new Exception("Conversation not found");
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                conversation.Messages.Add(obj);
                conversation.LastMessage = conversation.Messages.LastOrDefault();
            });
        }

        private void SortConversation()
        {
          
        }
    }
}
