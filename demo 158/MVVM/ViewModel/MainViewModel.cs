using demo_158.Base;
using demo_158.MVVM.Model;
using demo_158.MVVM.View;
using demo_158.MVVM.View.Model;
using demo_158.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using demo_158.Hubs;
using WebSocketSharp;

namespace demo_158.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private object _currentView;
        private readonly IServiceProvider _service;
        private readonly ConnectionManager _connection;
        private readonly MessageAndTalkServices _messageAndTalkServices;
        private ICommand moveAndDrugCommand;
        private ICommand openProfileCommand;
        private ConversationModel _conversationModel;
      
        private ObservableCollection<ConversationModel>? _conversations;
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
        public UserModelFromServer UserModelFromServer { get; set; }
     

        public ObservableCollection<ConversationModel>? Conversations
        {
            get => _conversations;
            set => SetField(ref _conversations, value);
        }

        public object CurrentView
        {
            get => _currentView;
            set => SetField(ref _currentView, value);
        }

        public MainViewModel(IServiceProvider service,ConnectionManager connection,MessageAndTalkServices messageAndTalkServices)
        {
          
            _service = service;
          
            _connection = connection;
            _messageAndTalkServices = messageAndTalkServices;
            var userView = service.GetService<DefaultMessageView>();
            CurrentView = userView;
            ReceiveConversation();
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
        private async Task OnSelectedConversationChangedAsync()
        {
            await _connection.SendAsync("ConversationSender", ConversationModel);
        }

        private void ReceiveConversation()
        {
            _connection.On<List<MessageModelFromServer>,UserModelFromServer>("ReceiveConversation", (messages, user) =>
            {
                var messageView = _service.GetService<MessageAndTalkView>();
                messageView.UserModelFromServer = UserModelFromServer;
                messageView.Conversation = ConversationModel;
                messageView.Messages ??= new ObservableCollection<MessagesModel>();

                foreach (var msg in messages)
                {
                    messageView.Messages.Add(new MessagesModel
                    {
                        SenderName = msg.Username,
                        SenderImage = user.Image,
                        SentTime = msg.SendDate,
                        HorizontalAlignmentMessage = _messageAndTalkServices.SetHorizontalAlignment(UserModelFromServer.Username, msg.Username),
                        FlowDirectionMessage = _messageAndTalkServices.SetFlowDirectionMessage(UserModelFromServer.Username, msg.Username),
                        BackgroundColorBrush = _messageAndTalkServices.SetBackGroundBrush(UserModelFromServer.Username, msg.Username),
                        MessageType = msg.MessageType,
                        Text = msg.Text,
                        Object = msg.Object
                    });
                }
                CurrentView = messageView;
            });
        }
    }
}
