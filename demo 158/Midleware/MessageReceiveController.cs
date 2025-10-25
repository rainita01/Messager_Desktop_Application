using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demo_158.Hubs;
using demo_158.MVVM.Model;
using Microsoft.Identity.Client;

namespace demo_158.Midleware
{
    public class MessageReceiveController 
    {
        private readonly ConnectionManager _connectionManager;
        public event Action<MessageModelFromServer> MessageReceived;
        private ConcurrentQueue<MessageModelFromServer> _queue = new();
        private bool _processing;

        public MessageReceiveController(ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
            ReceivePrivateMessage();
        }
        public  void  ReceivePrivateMessage()
        {
            _connectionManager.On<MessageModelFromServer?>("ReceivePrivateMessage", (message =>
            {
                if (message == null)
                {
                    return;
                }
                _queue.Enqueue(message);
                QueueController();
            }));
        }

        public async Task QueueController()
        {
            if (_processing)
                return; 
            _processing = true;

            while (_queue.TryDequeue(out var msg))
            {
                MessageReceived?.Invoke(msg);
                await Task.Yield();
            }
            _processing = false;
            
        }
    }
}
