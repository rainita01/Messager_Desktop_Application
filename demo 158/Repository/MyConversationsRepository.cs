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
        

        public Action<List<ConversationModel>> SuccessReceiveConversations { get; set; }
        public Action<int,string> SuccessCreatedConversation { get; set; }
        public Action<int> SuccessDeletedConversation { get; set; }
        public MyConversationsRepository(ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        public async Task StartAsync()
        {
            await ReceiveConversations();
            await GetConversationId();
            await DeleteConversation();

        }


        public async Task ReceiveConversations()
        {
            await _connectionManager.OnAsync<List<ConversationModel>>("ReceiveConversations", (ConversationsList =>
            {
                try
                {
                    SuccessReceiveConversations.Invoke(ConversationsList);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something Wrong abt adding to ConversationList");
                    throw;
                }

            }));
        }

        public async Task GetConversationId()
        {
            await _connectionManager.OnAsync<int, string>("GetConversationId", ((conversationId, user) =>
            {
                SuccessCreatedConversation.Invoke(conversationId,user);
            }));
        }

        public async Task DeleteConversation()
        {
            await _connectionManager.OnAsync<int>("DeleteConversation", (conversationId =>
            {
                SuccessDeletedConversation.Invoke(conversationId);
            }));

        }
    }
}
