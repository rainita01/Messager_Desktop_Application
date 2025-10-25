using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demo_158.Base;
using Server_API_With_SignalR_For_Messager_01.Models;


namespace demo_158.MVVM.Model
{

    public class ConversationModel : ViewModelBase
    {
        private LastMessageModel _lastMessage;
       
        public int Id { get; set; }
        public string ContactUsername { get; set; }
        public byte[] ContactImage { get; set; }
        public List<int> UsersId { get; set; }


        public LastMessageModel LastMessage
        {
            get => _lastMessage;
            set => SetField(ref _lastMessage, value);
        }
    }
}
