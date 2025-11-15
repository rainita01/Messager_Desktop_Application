

using demo_158.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Color = System.Drawing.Color;

namespace demo_158.MVVM.Model
{
   public class MessagesModel : ViewModelBase 
    {
        private string _senderName;
        private string? _text;
        private byte[]? _o;
        private bool _isSeen;
        private ICommand editMessageCommand;
        private ICommand copyTextCommand;
        private ICommand deleteMessageCommand;
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public string SenderName
        {
            get => _senderName;
            set => SetField(ref _senderName, value);
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
        public bool IsSeen
        {
            get => _isSeen;
            set => SetField(ref _isSeen, value);
        }
        public MessageTypes MessageType { get; set; }
        public DateTime SentTime { get; set; }
        public HorizontalAlignment HorizontalAlignmentMessage { get; set; }
        public FlowDirection FlowDirectionMessage { get; set; }
        public SolidColorBrush BackgroundColorBrush { get; set; }
        public bool FirstMessage { get; set; }

        public ICommand EditMessageCommand => editMessageCommand ?? new GeneralCommand(() =>
        {
            
        });
        public ICommand DeleteMessageCommand => deleteMessageCommand ?? new GeneralCommand(() =>
        {


        });
        public ICommand CopyTextCommand => copyTextCommand ?? new GeneralCommand(() =>
        {
            Clipboard.SetDataObject(this.Text);
        });
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
        public bool IsSeen { get; set; }= false;
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
        public  bool IsSeen { get; set; }
    }

}
