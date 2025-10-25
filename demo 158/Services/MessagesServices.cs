using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demo_158.MVVM.Model;

namespace demo_158.Services
{
    public class MessagesServices(MessageAndTalkServices messageAndTalkServices)
    {
        public ObservableCollection<MessagesModel> ConvertMessagesFromServerToMessageModel(
            List<MessageModelFromServer> messages, string username)
        {
            var messageModel = new ObservableCollection<MessagesModel>();
            MessagesModel? lastMsg = null;

            foreach (var msg in messages)
            {

                messageModel.Add(new MessagesModel
                {
                    SenderName = msg.Username,
                    SentTime = msg.SendDate,
                    HorizontalAlignmentMessage = messageAndTalkServices.SetHorizontalAlignment(username, msg.Username),
                    FlowDirectionMessage = messageAndTalkServices.SetFlowDirectionMessage(username, msg.Username),
                    BackgroundColorBrush = messageAndTalkServices.SetBackGroundBrush(username, msg.Username),
                    FirstMessage = messageAndTalkServices.SetFirstMessage(lastMsg?.SenderName, msg.Username),
                    MessageType = msg.MessageType,
                    Text = msg.Text,
                    Object = msg.Object
                });
                lastMsg = messageModel.Last();
            }

            return messageModel;
        }
    }
}
