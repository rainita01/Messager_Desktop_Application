using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demo_158.Base;


namespace demo_158.MVVM.Model
{

    public class ConversationModel : ViewModelBase
    {
        private string? _lastMessage;
        public int Id { get; set; }
        public string Type { get; set; }
        public string ContactUsername { get; set; }

        public string? LastMessage
        {
            get => _lastMessage;
            set => SetField(ref _lastMessage, value);
        }

        public string ContactImage { get; set; }
 
    }
}
