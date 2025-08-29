using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebSocketSharp;

namespace demo_158.Base
{
    public class SocketManager : ViewModelBase
    {
      
        //private Dictionary<string,WebSocket> _connections = new Dictionary<string,WebSocket>();
        public Action<string> SuccessMessageReceive;
        private WebSocket _ws;
        private static SocketManager? instance;

        public WebSocket Ws 
        {
            get => _ws;
            set => SetField(ref _ws, value);
        }

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
        public void Connect(string endpoint,string username)
        {
            Ws = new WebSocket($"ws://localhost:7482{endpoint}?Username={username}");
            Ws.Connect();
            Ws.OnMessage += (e, s) => SuccessMessageReceive.Invoke(s.Data);

        }
        public void Send(object message)
        {
            if (Ws.ReadyState == WebSocketState.Open)
            {
                Ws.Send(JsonSerializer.Serialize(message));
            }
        }
    }
}
