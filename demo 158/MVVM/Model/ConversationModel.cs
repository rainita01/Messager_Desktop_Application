using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace demo_158.MVVM.Model
{
    public class ConversationModel
    {
        public int Id { get; set; }
        public string ContactUsername { get; set; }
        public string ContactImage { get; set; }
        public string LastMessage { get; set; }
        public bool IsMessageSeen { get; set; }
        public string LastOnline { get; set; }
        public ObservableCollection<ReceiveUser> Users { get; set; }
    }

    public class ConversationReceive
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public bool IsConversationPrivateChat { get; set; }
        public DateTime CreatedTime { get; set; }
        public string ContactUsername { get; set; }
        public string? LastMessage { get; set; }
        public string ContactImage { get; set; }
 
    }
}
