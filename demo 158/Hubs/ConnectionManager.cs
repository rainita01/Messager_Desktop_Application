using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace demo_158.Hubs
{
    public class ConnectionManager 
    {
        private readonly HubConnection _connection;


        public ConnectionManager(HubConnection connection) : base()
        {
            _connection = connection;

        }

        public HubConnectionState ConnectionState => _connection.State;
        public async Task StartAsync()
        {
            if (_connection.State == HubConnectionState.Disconnected)
                await _connection.StartAsync();
        }

      
         public async Task OnAsync<T>(string methodName, Action<T> handler)
         {
             await Task.Run((() => {
                 _connection.On<T>(methodName, data =>
                 {
                     Application.Current.Dispatcher.Invoke(() =>
                     {
                         handler(data);
                     });
                 });
             }));
        }
         public async Task OnAsync<T1, T2>(string methodName, Action<T1, T2> handler)
         {
             await Task.Run((() => {
                 _connection.On<T1, T2>(methodName, (data1, data2) =>
                 {
                     Application.Current.Dispatcher.Invoke(() =>
                     {
                         handler(data1, data2);
                     });
                 });
             }));
             
             
             

         }

        public async Task SendAsync<T>(string methodName,T obj)
         {

             await _connection.SendAsync(methodName, obj);
         }

        public async Task SendAsync<T1,T2>(string methodName, T1 obj1,T2 obj2)
        {

            await _connection.SendAsync(methodName, obj1,obj2);
        }

        public async Task StopAsync()
        {

            if (_connection.State == HubConnectionState.Connected )
            {
                await _connection.StopAsync();
            }

        }

      
    }
}
