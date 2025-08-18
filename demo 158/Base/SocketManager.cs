using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebSocketSharp;

namespace demo_158.Base
{
    public class SocketManager :IDisposable
    {
      
        private Dictionary<string,WebSocket> _connections = new Dictionary<string,WebSocket>();

        private static SocketManager? instance;

        public string SessionId { get; set; }
        public static SocketManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SocketManager();
                }
                return instance;
            }
        }
        private SocketManager()
        {

        }

        public WebSocket GetConnection(string endpoint, string username = null)
        {
            if (_connections.TryGetValue(endpoint, out var ws) &&
                ws.ReadyState == WebSocketState.Open)
            {
                return ws;
            }

            var newWs = new WebSocket($"ws://localhost:7482{endpoint}?Username={username}");
            newWs.Connect();

            newWs.OnError += (s, e) =>
                Debug.WriteLine($"WebSocket error on {endpoint}: {e.Message}");

            _connections[endpoint] = newWs;
            return newWs;
        }

        public void Send(string endpoint, object message)
        {
            var ws = GetConnection(endpoint);
            if (ws.ReadyState == WebSocketState.Open)
            {
                ws.Send(JsonSerializer.Serialize(message));
            }
        }
        public void Dispose()
        {
            foreach (var connection in _connections.Values)
            {
                connection?.Close();
            }
            _connections.Clear();
        }
    }
}
