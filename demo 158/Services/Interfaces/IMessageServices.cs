using demo_158.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_158.Services.Interfaces
{
    public  interface IMessageServices
    {
        MessagesModel MessageModelMapping(MessageModelFromServer msg, string? username, string lastMessageUsername);
        MessagesModel MessageModelMapping(MessageModelFromUser msg, string? username, string lastMessageUsername);
        ObservableCollection<MessagesModel> MessagesModelMapping(List<MessageModelFromServer> messages, string username);
    }
}
