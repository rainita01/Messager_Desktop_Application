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
using demo_158.MVVM.Model;
using demo_158.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WebSocketSharp;

namespace demo_158.MVVM.ViewModel
{
   public class MessageAndTalkViewModel : ViewModelBase
    {
        private readonly MessageAndTalkServices _services;
        private int _id;
        private string _contactUsername;
        private ObservableCollection<MessagesModel> _messages;
        private string _image;
        private ICommand sendMessageCommand;
        private string _text;
        private ConversationModel _conversation;
        private string _username;
        private WebSocket _ws;
        public EventHandler SuccessMessageEvent { get; set; }
   
        public int Id
        {
            get => _id;
            set => SetField(ref _id, value);
        }

        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }
            
        public string Username
        {
            get => _username;
            set => SetField(ref _username, value);
        }

        public string ContactUsername
        {
            get => _contactUsername;
            set => SetField(ref _contactUsername, value);
        }

        public ObservableCollection<MessagesModel> Messages
        {
            get => _messages;
            set => SetField(ref _messages, value);
        }

        public string Image
        {
            get => _image;
            set => SetField(ref _image, value);
        }

        public ConversationModel Conversation
        {
            get => _conversation;
            set => SetField(ref _conversation, value);
        }

        public MessageAndTalkViewModel(MessageAndTalkServices services)
        {
            SocketManager.Instance.SuccessMessageReceive += OnMessage;
          _services = services; 
        }

     

        private void OnMessage(string obj)
        {
            if (obj == "Successfully")
            {
                return;
            }
            JObject jObject = JObject.Parse(obj);
            string type = (string)jObject["Type"];

            if (type != "message")
            {
                return;
            }
            var deserialize = JsonSerializer.Deserialize<MessageSendToServerModel>(obj);
            if (deserialize == null)
            {
                return;
            }
            var messageModel = new MessagesModel()
            {
                Id = Id,
                SenderName = deserialize.SenderName,
                SentTime = deserialize.SentTime,
                Text = deserialize.Text,
                Conversation = Conversation,
                SenderImage = deserialize.SenderImage,
                HorizontalAlignmentMessage = HorizontalAlignment.Left,
                FlowDirectionMessage = FlowDirection.LeftToRight,
                BackgroundColorBrush = Brushes.LightGray,
                FirstMessage = Messages.Count == 0 || _services.SetFirstMessage(deserialize.SenderName, Messages.Last().SenderName)
            };

            Application.Current.Dispatcher.Invoke(() =>
            {
                Messages.Add(messageModel);
            });

            SuccessMessageEvent.Invoke(this, EventArgs.Empty);
        }

        public ICommand SendMessageCommand => sendMessageCommand ?? new GeneralCommand((SendMessageExecuteActionAsync));

        private async void SendMessageExecuteActionAsync()
        {
            if (string.IsNullOrEmpty(Text))
                return;

            var message = new MessageSendToServerModel()
            {
                Type = "message",
                ConversationId = Conversation.Id,
                Text = Text,
                SenderName = Username,
                SendToUser = ContactUsername,
                SenderImage = Image,
                SentTime = DateTime.Now

            };

            SocketManager.Instance.Send(message);
            var messageModel = new MessagesModel()
            {
                Id = Id,
                SenderName = message.SenderName,
                SentTime = message.SentTime,
                Text = Text,
                Conversation = Conversation,
                SenderImage = Image,
                HorizontalAlignmentMessage = HorizontalAlignment.Right,
                FlowDirectionMessage = FlowDirection.RightToLeft,
                BackgroundColorBrush = Brushes.LightSkyBlue,
                FirstMessage = Messages.Count == 0 || _services.SetFirstMessage(message.SenderName, Messages.Last().SenderName)
            };
            Messages.Add(messageModel);
            Text = String.Empty;
            SuccessMessageEvent.Invoke(this, EventArgs.Empty);

          
        }

        private void WsOnOnMessage(object? sender, MessageEventArgs e)
        {
       
        }
    }
}
