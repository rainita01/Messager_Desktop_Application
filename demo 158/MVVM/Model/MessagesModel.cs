

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using demo_158.Base;
using Color = System.Drawing.Color;

namespace demo_158.MVVM.Model
{
   public class MessagesModel : ViewModelBase 
    {
        private string _senderName;
        private byte[] _senderImage;
        private string? _text;
        private byte[]? _o;
        public int Id { get; set; }

        public string SenderName
        {
            get => _senderName;
            set => SetField(ref _senderName, value);
        }

        public Byte[] SenderImage
        {
            get => _senderImage;
            set => SetField(ref _senderImage, value);
        }

        public string? Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }

        public byte[]? Object
        {
            get => _o;
            set => SetField(ref _o, value);
        }

        public MessageTypes MessageType { get; set; }
        public DateTime SentTime { get; set; }
        public HorizontalAlignment HorizontalAlignmentMessage { get; set; }
        public FlowDirection FlowDirectionMessage { get; set; }
        public SolidColorBrush BackgroundColorBrush { get; set; }
        public bool FirstMessage { get; set; }
    }
    public class MessageModelFromUser
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public MessageTypes MessageType { get; set; }
        public byte[]? Object { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int ConversationId { get; set; }
    }
    public class MessageModelFromServer
    {
        public int Id { get; set; }
        public MessageTypes MessageType { get; set; }
        public string? Text { get; set; }
        public byte[]? Object { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public DateTime SendDate { get; set; }
        public int ConversationId { get; set; }
    }

}
