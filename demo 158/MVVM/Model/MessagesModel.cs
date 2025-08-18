

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace demo_158.MVVM.Model
{
   public class MessagesModel
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string SenderImage { get; set; }
        public string Text { get; set; }
        public DateTime SentTime { get; set; }
        public HorizontalAlignment HorizontalAlignmentMessage { get; set; }
        public FlowDirection FlowDirectionMessage { get; set; }
        public ConversationModel Conversation { get; set; }
        public SolidColorBrush BackgroundColorBrush { get; set; }
        public bool FirstMessage { get; set; }
    }
    public class MessageSendToServerModel
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string SenderName { get; set; }
        public string? SendToUser { get; set; }
        public string SenderImage { get; set; }
        public string Text { get; set; }
        public DateTime SentTime { get; set; }
        public int ConversationId { get; set; }
    }
    public class ResieveConversationModel
    {
        public string Type { get; set; }
        public List< MessagesModel> Messages { get; set; }

    }
 
}
