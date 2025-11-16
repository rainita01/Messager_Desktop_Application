using demo_158.MVVM.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using demo_158.Services.Interfaces;

namespace demo_158.Services
{
    public class MessagesServices : IMessageServices
    {
        public ObservableCollection<MessagesModel> MessagesModelMapping(List<MessageModelFromServer> messages, string username)
        {
            var messageModel = new ObservableCollection<MessagesModel>();
            var lastMessageUsername = string.Empty; 
            foreach (var msg in messages)
            {

                messageModel.Add(MessageModelMapping(msg,username,lastMessageUsername));
                lastMessageUsername = msg.Username;
            }

            return messageModel;
        }
        public MessagesModel MessageModelMapping(MessageModelFromServer msg,string? username,string? lastMessageUsername)
        {
           var messageModel = new MessagesModel()
            {
                Id = msg.Id,
                SenderName = msg.Username,
                SentTime = msg.SendDate,
                HorizontalAlignmentMessage = SetHorizontalAlignment(username, msg.Username),
                FlowDirectionMessage = SetFlowDirectionMessage(username, msg.Username),
                BackgroundColorBrush = SetBackGroundBrush(username, msg.Username),
                FirstMessage =SetFirstMessage(lastMessageUsername, msg.Username),
                MessageType = msg.MessageType,
                Text = msg.Text,
              
            };  
            return messageModel;
        }
        public MessagesModel MessageModelMapping(MessageModelFromUser msg, string username, string? lastMessageUsername)
        {
            var messageModel = new MessagesModel()
            {
                Id = msg.Id,
                SenderName = msg.Username,
                SentTime = DateTime.Now,
                HorizontalAlignmentMessage = SetHorizontalAlignment(username, msg.Username),
                FlowDirectionMessage =SetFlowDirectionMessage(username, msg.Username),
                BackgroundColorBrush = SetBackGroundBrush(username, msg.Username),
                FirstMessage = SetFirstMessage(lastMessageUsername, msg.Username),
                MessageType = msg.MessageType,
                Text = msg.Text,
              
            };
            return messageModel;
        }
        public HorizontalAlignment SetHorizontalAlignment(string username, string senderUsername)
        {
            if (username != senderUsername)
            {
                return HorizontalAlignment.Left;
            }

            return HorizontalAlignment.Right;
        }
        public FlowDirection SetFlowDirectionMessage(string username, string senderUsername)
        {

            if (username != senderUsername)
            {
                return FlowDirection.LeftToRight;
            }
            return FlowDirection.RightToLeft;
        }
        public SolidColorBrush SetBackGroundBrush(string? username, string? senderUsername)
        {
            if (username != senderUsername)
            {
                return Brushes.LightGray;
            }

            return Brushes.LightSkyBlue;
        }
        public bool SetFirstMessage(string? username, string? senderUsername)
        {
            if (username == senderUsername)
            {
                return false;
            }

            return true;

        }
    }
}
