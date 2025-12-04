using demo_158.Base;
using demo_158.Services.Enums;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace demo_158.Hubs
{
    public class ConnectionManager :ViewModelBase
    {
        private readonly HubConnection _connection;
        private HubConnectionState _connectionState;
        public Action<HubConnectionState>? OnStateChanged;
        public HubConnectionState ConnectionState
        {
            get => _connectionState;
            set
            {
                if (SetField(ref _connectionState, value))
                {
                    OnStateChanged?.Invoke(ConnectionState);
                }
            }
        }
        public ConnectionManager(HubConnection connection) : base()
        {
            _connection = connection;
            _connection.Reconnecting += async (ex) =>
            {
                ConnectionState = HubConnectionState.Reconnecting;
                await Task.CompletedTask;
            };

            _connection.Reconnected += async (id) =>
            {
                ConnectionState = HubConnectionState.Connected;
                await Task.CompletedTask;
            };

            _connection.Closed += async (ex) =>
            {
                ConnectionState = HubConnectionState.Disconnected;
              
                await Task.CompletedTask;
            };
            
            
        }
        
        public async Task StartAsync()
        {
            if (_connection.State == HubConnectionState.Disconnected)
                await _connection.StartAsync();
            ConnectionState = _connection.State;
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
         public async Task OnAsync<T1, T2,T3>(string methodName, Action<T1, T2,T3> handler)
         {
             await Task.Run((() => {
                 _connection.On<T1, T2,T3>(methodName, (data1, data2,data3) =>
                 {
                     Application.Current.Dispatcher.Invoke(() =>
                     {
                         handler(data1, data2,data3);
                     });
                 });
             }));
            
         }

         public void On<T>(string methodName, Action<T> handler)
         {
             _connection.On<T>(methodName, (data) =>
             {
                 Application.Current.Dispatcher.Invoke(() =>
                 {
                     handler(data);
                 });
             });
         }

         public void On<T1, T2>(string methodName, Action<T1, T2> handler)
         {
             _connection.On<T1,T2>(methodName, (data1,data2) =>
             {
                 Application.Current.Dispatcher.Invoke(() =>
                 {
                     handler(data1,data2);
                 });
             });

        }
         public void On<T1, T2,T3>(string methodName, Action<T1, T2,T3> handler)
         {
             _connection.On<T1, T2,T3>(methodName, (data1, data2,data3) =>
             {
                 Application.Current.Dispatcher.Invoke(() =>
                 {
                     handler(data1, data2,data3);
                 });
             });

         }
        public async Task SendAsync<T>(string methodName,T obj)
         {

             await _connection.SendAsync(methodName, obj);
         }

        public async Task SendAsync<T1,T2>(string methodName, T1 obj1,T2 obj2)
        {

            await _connection.SendAsync(methodName, obj1,obj2);
        }
        public async Task SendAsync<T1, T2,T3>(string methodName, T1 obj1, T2 obj2,T3 obj3)
        {

            await _connection.SendAsync(methodName, obj1, obj2,obj3);
        }
        public async Task<ServerAnswer> InvokeAsync<T>(string methodName, T obj)
        {
          return await _connection.InvokeAsync<ServerAnswer>(methodName, obj);
           
        }

        public async Task<TResult> InvokeAskDataAsync<TResult,TPara>(string methodName, TPara obj)
        {
            return await _connection.InvokeAsync<TResult>(methodName,obj);
        }
        public async Task<ServerAnswer> InvokeAsync<T1,T2>(string methodName, T1 obj,T2 obj2)
        {
            return await _connection.InvokeAsync<ServerAnswer>(methodName, obj,obj2);

        }
        public async Task<T3> InvokeAsync<T1, T2,T3>(string methodName, T1 obj,T2 obj2)
        {
            return await _connection.InvokeAsync<T3>(methodName, obj, obj2);

        }
        public async Task StopAsync()
        {

            if (_connection.State == HubConnectionState.Connected )
            {
                await _connection.StopAsync();
            }

        }

      
        public void Close()
        {
            _connection.StopAsync();
        }

    }
}
