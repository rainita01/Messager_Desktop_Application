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
using demo_158.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using WebSocketSharp;

namespace demo_158.MVVM.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IServiceProvider _service;
        private readonly ConnectionManager _connection;
        private string _password;
        private string _username;
        private ICommand loginCommand;
        public Visibility PasswordTextBlockVisibility => string.IsNullOrEmpty(Password) ? Visibility.Visible : Visibility.Collapsed;
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


    
        public LoginViewModel(IServiceProvider service,ConnectionManager connection)
        {
            _service = service;
            _connection = connection;
            ReceiveUser();
            ExceptionMessage();
       
        }
   
        public ICommand LoginCommand => loginCommand ?? new GeneralCommand(async () => await LoginCommandExecuteActionAsync(),LoginCanExecute);

        private async Task LoginCommandExecuteActionAsync()
        {
            var user = new UserModelFromUser()
            {
                Username = Username,
                Password = Password
            };

            await _connection.SendAsync("RegisterRequest", user);
            SharingDataViewModel.Instance.CurrenViewChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ReceiveUser()  
        {
           _connection.On<UserModelFromServer,List<ConversationModel>>("ReceiveUser", (user,conversations) =>
            {
                var mainView = _service.GetService<MainView>();
                mainView.UserModelFromServer = user;
                mainView.Conversations = conversations;
                
                Application.Current.Windows.OfType<MainLoginSignView>().FirstOrDefault()?.Close();
                mainView.Show();
            });

        }

        private void ExceptionMessage()
        {
            _connection.On<string>("ExceptionMessage", (message) =>
            {
                MessageBox.Show(message, "Register Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                SharingDataViewModel.Instance.CurrentViewErrorChanged.Invoke(this,EventArgs.Empty);


            });
        }
        private bool LoginCanExecute()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }
    }
}
