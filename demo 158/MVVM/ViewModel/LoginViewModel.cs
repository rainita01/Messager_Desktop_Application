using demo_158.Base;
using demo_158.MVVM.Model;
using demo_158.MVVM.View;
using demo_158.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WebSocketSharp;

namespace demo_158.MVVM.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IServiceProvider _service;
        private string _password;
        private string _username;
        private ICommand loginCommand;
        private UserConversationsData? _userConversationsData;
        private WebSocket _ws;

        public WebSocket Ws
        {
            get => _ws;
            set => SetField(ref _ws, value);
        }

        public Visibility PasswordTextBlockVisibility => string.IsNullOrEmpty(Password) ? Visibility.Visible : Visibility.Collapsed;

        public UserConversationsData? UserConversationsData
        {
            get => _userConversationsData;
            set => SetField(ref _userConversationsData, value);
        }

        public string Username
        {
            get => _username;
            set 
            {
                if (SetField(ref _username, value))
                {
                    OnPropertyChanged(nameof(Username));

                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (SetField(ref _password, value))
                {
                    OnPropertyChanged(nameof(PasswordTextBlockVisibility));
                    
                }
            }
        }

        public LoginViewModel(IServiceProvider service)
        {

            _service = service;
            Ws = new WebSocket("ws://localhost:7482/Login");
            Ws.Connect();
            Ws.OnMessage += WbOnOnMessage;

        }
   
        public ICommand LoginCommand => loginCommand ?? new GeneralCommand((LoginCommandExecuteAction),(LoginCanExecute));

        private async void LoginCommandExecuteAction()
        {
            var user = new SendUser()
            {
                
                Username = Username,
                Password = Password
            };
            SharingDataViewModel.Instance.CurrenViewChanged?.Invoke(this, EventArgs.Empty);
            var userSerialize = JsonSerializer.Serialize(user);
            Ws.Send(userSerialize);
        }


        private void WbOnOnMessage(object? sender, MessageEventArgs e)
        {
            if (e.Data == "error")
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    SharingDataViewModel.Instance.CurrentViewErrorChanged?.Invoke(this, EventArgs.Empty);
                    MessageBox.Show("Incorrect Username or Password");
                });
                return;
            }

            UserConversationsData = JsonSerializer.Deserialize<UserConversationsData>(e.Data);
            SuccessLoginEventExecute();
        }
        private void SuccessLoginEventExecute()
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                SocketManager.Instance.Connect("/MainView",Username);
                var mainView = _service.GetService<MainView>();
                mainView.User = UserConversationsData.User;
                mainView.Conversations = UserConversationsData.Conversations;
                Application.Current.Windows.OfType<MainLoginSignView>().FirstOrDefault()?.Close();
                mainView.Show();
            });
        
        }
        

        private bool LoginCanExecute()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }
    }
}
