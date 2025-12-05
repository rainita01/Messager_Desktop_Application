using demo_158.Hubs;
using demo_158.MVVM.Model;
using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
namespace demo_158.Repository
{
    public class MyMessagesRepository
    {
        private readonly ConnectionManager _connection;
        private ConcurrentQueue<MessageModelFromServer> _queue = new();
        public Action<MessageModelFromServer> MessageReceivedEvent;
        public Action<int, string> ContactDeletedMessageEvent;
        public Action<EditMessageModel> ContactEditedMessageEvent;
        public MyMessagesRepository(ConnectionManager connection)
        {
            _connection = connection;
        }   

        public async Task StartAsync()
        {

                await ReceivePrivateMessage();
                await ReceiveMessages();
                await ContactDeletedMessage();
                await ContactEditedMessage();

        }
        public async Task ReceiveMessages()
        {
            await _connection.OnAsync<List<MessageModelFromServer>>("ReceiveMessages", async messages =>
            {
                foreach (var messageModelFromServer in messages)
                {
                    _queue.Enqueue(messageModelFromServer);    
                    DequeueMessages();
                }


            });

        }

        public async Task ReceivePrivateMessage()
        {
            await _connection.OnAsync<MessageModelFromServer?>("ReceivePrivateMessage", async message =>
            {
                if (message == null)
                {
                    return;
                }
                _queue.Enqueue(message);
                
                DequeueMessages();
            });

        }

        public async Task ContactDeletedMessage()
        {
            await _connection.OnAsync<int, string>("ContactDeletedMessage", ((messageId, contactName) =>
            {
                ContactDeletedMessageEvent.Invoke(messageId,contactName);
            }));
        }
        public async Task ContactEditedMessage()
        {
            await _connection.OnAsync<EditMessageModel>("ContactEditedMessage", ((newMessage) =>
            {

                ContactEditedMessageEvent.Invoke(newMessage);
            }));

        }
        public void DequeueMessages()
        {
                while (_queue.TryDequeue(out var msg))
                {
                    MessageReceivedEvent?.Invoke(msg);
                }
        }

      
  
    }
}
