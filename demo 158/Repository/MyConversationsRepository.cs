using System.Collections.ObjectModel;
using System.Windows;
using demo_158.Hubs;
using demo_158.MVVM.Model;
using demo_158.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace demo_158.Repository
{
    public class MyConversationsRepository 
    {
        private readonly ConnectionManager _connectionManager;
        private readonly IConversationServices _conversationServices;

        public Action<List<ConversationModel>> SuccessReceiveConversations { get; set; }
        public List<ConversationModel> Conversations { get; set; } = new();
        public MyConversationsRepository(ConnectionManager connectionManager,IConversationServices conversationServices)
        {
            _connectionManager = connectionManager;
            _conversationServices = conversationServices;
        }
        public async Task StartAsync()
        {
            await ReceiveConversations();
           
        }


        public async Task ReceiveConversations()
        {
            await _connectionManager.OnAsync<List<ConversationModelFromServer>>("ReceiveConversations", (ConversationsList =>
            {
                try
                {
                    foreach (var conversation in ConversationsList)
                    {
                        Conversations.Add(_conversationServices.ConversationModelMapping(conversation));
                    }

                    SuccessReceiveConversations.Invoke(Conversations);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something Wrong abt adding to ConversationList");
                    throw;
                }

            }));
        }

    
    }
}
