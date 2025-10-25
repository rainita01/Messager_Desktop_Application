using demo_158.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using demo_158.Hubs;
using demo_158.Midleware;
using demo_158.MVVM.Model;
using demo_158.MVVM.View;
using demo_158.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using WebSocketSharp;

namespace demo_158.MVVM.ViewModel
{
   public class MessageAndTalkViewModel : ViewModelBase
    {
        private readonly MessageAndTalkServices _messageAndTalkServices;
        private readonly ConnectionManager _connection;
        private readonly MessageReceiveController _messageReceive;
        private ObservableCollection<MessagesModel> _messages;
        private ICommand sendMessageCommand;
        private ICommand showUserContentCommand;
        private string _text;
        private ConversationModel _conversation;
        private UserModelFromServer _userModelFromServer;

        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }
        
        public ObservableCollection<MessagesModel> Messages
        {
            get => _messages;
            set => SetField(ref _messages, value);
        }


        public ConversationModel Conversation
        {
            get => _conversation;
            set => SetField(ref _conversation, value);
        }

        public UserModelFromServer UserModelFromServer
        {
            get => _userModelFromServer;
            set => SetField(ref _userModelFromServer, value);
        }

        public MessageAndTalkViewModel(MessageAndTalkServices messageAndTalkServices,ConnectionManager connection,MessageReceiveController messageReceive)
        {
            _messageAndTalkServices = messageAndTalkServices;
            _connection = connection;
            _messageReceive = messageReceive;
            _messageReceive.MessageReceived += OnMessageReceive;
        }


        private void OnMessageReceive(MessageModelFromServer msg)
        {
            var messageModel = new MessagesModel()
            {
                SenderName = msg.Username,
                SentTime = DateTime.Now,
                Text = msg.Text,
                SenderImage = Conversation.ContactImage,
                HorizontalAlignmentMessage = HorizontalAlignment.Left,
                FlowDirectionMessage = FlowDirection.LeftToRight,
                BackgroundColorBrush = Brushes.LightGray,
                FirstMessage = Messages.Count == 0 || _messageAndTalkServices.SetFirstMessage(UserModelFromServer.Username, Messages.Last().SenderName)
            };
                Messages.Add(messageModel);
        }
        public ICommand ShowUserContentCommand => showUserContentCommand ?? new GeneralCommand((ShowUserContentAction));

        private void ShowUserContentAction()
        {
            var profile = new ProfileEditModel()
            {
                Username = Conversation.ContactUsername,
                Type = "ContactProfileInfo"
            };
        }

        public ICommand SendMessageCommand => sendMessageCommand ??= new GeneralCommand(async()=> await  SendTextMessageExecuteAction());
      

        private async Task  SendTextMessageExecuteAction()
        {
            if (string.IsNullOrEmpty(Text))
                throw new Exception();

            var messageFromUser = new MessageModelFromUser()
            {
                MessageType = MessageTypes.Text,
                ConversationId = Conversation.Id,
                Text = this.Text,
                Object = null,
                UserId = UserModelFromServer.UserId,
                Username = UserModelFromServer.Username
            };
            var messageModel = new MessagesModel()
            {
                SenderName = UserModelFromServer.Username,
                SentTime = DateTime.Now,
                Text = Text,
                SenderImage = Conversation.ContactImage,
                HorizontalAlignmentMessage = HorizontalAlignment.Right,
                FlowDirectionMessage = FlowDirection.RightToLeft,
                BackgroundColorBrush = Brushes.LightSkyBlue,
                FirstMessage = Messages.Count == 0 || _messageAndTalkServices.SetFirstMessage(UserModelFromServer.Username, Messages.Last().SenderName)
            }; 

            await _connection.SendAsync("SendMessageToPrivate", Conversation.ContactUsername, messageFromUser);
            Messages.Add(messageModel);
            Text = String.Empty;
        }

    }
}
