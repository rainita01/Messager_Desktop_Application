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
        public Action<MessageModelFromServer> MessageReceived;
        public MyMessagesRepository(ConnectionManager connection)
        {
            _connection = connection;
        }

        public async Task StartAsync()
        {

                await ReceivePrivateMessage();
                await ReceiveMessages();
                
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

        public void DequeueMessages()
        {
                while (_queue.TryDequeue(out var msg))
                {
                    MessageReceived?.Invoke(msg);
                }
        }

      
  
    }
}
