using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demo_158.MVVM.Model;

namespace demo_158.Services.Interfaces
{
    public interface IConversationServices
    {
        ConversationViewModel ConversationModelMapping(ConversationModelFromServer obj); 
       
    }
}
