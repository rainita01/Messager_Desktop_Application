

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
using demo_158.Hubs;
using Microsoft.EntityFrameworkCore;
using Color = System.Drawing.Color;

namespace demo_158.MVVM.Model
{
   public class MessagesModel : ViewModelBase 
    {
        private string _senderName;
        private string? _text;
        private byte[]? _image;
        private bool _isSeen;
        private DateTime _sentTime;

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

        public byte[]? Image
        {   
            get => _image;
            set => SetField(ref _image, value);
        }
        public bool IsSeen
        {
            get => _isSeen;
            set => SetField(ref _isSeen, value);
        }
        public MessageTypes MessageType { get; set; }

        public DateTime SentTime
        {
            get => _sentTime;
            set => SetField(ref _sentTime, value);
        }
        public bool FirstMessage { get; set; }
        public bool IsMyMessage { get; set; }
        public bool IsMessageEdited { get; set; }

     
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
    public class EditMessageModel
    {
        public int MessageId { get; set; }
        public string SenderUsername { get; set; }
        public string ContactUsername { get; set; }
        public string NewText { get; set; }
    }
}
