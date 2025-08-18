using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_158.MVVM.Model
{
    public class UserConversationsData
    {
        public ReceiveUser User { get; set; }
        public List<ConversationReceive> Conversations { get; set; }
    }
}
