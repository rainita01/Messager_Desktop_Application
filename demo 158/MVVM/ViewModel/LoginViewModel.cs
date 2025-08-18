using demo_158.Base;
using demo_158.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using demo_158.MVVM.Model;
using demo_158.MVVM.View;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebSocketSharp;

namespace demo_158.MVVM.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IServiceProvider _service;
        private string _password;
        private string _username;
        private ICommand loginCommand;
        private Visibility _passwordTextBlockVisibility;
        private UserConversationsData? _userConversationsData;
       

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
            var wb = SocketManager.Instance.GetConnection("/Login");
            wb.OnMessage += WbOnOnMessage;

        }
   
        public ICommand LoginCommand => loginCommand ?? new GeneralCommand((LoginCommandExecuteAction),(LoginCanExecute));

        private async void LoginCommandExecuteAction()
        {
            var user = new SendUser()
            {
                
                Username = Username,
                Password = Password
            };
            var loadingPage = _service.GetService<LoginLoadingPage>();
            SharingDataViewModel.Instance.CurrentView = loadingPage;
            SharingDataViewModel.Instance.CurrenViewChanged?.Invoke(this, EventArgs.Empty);
            SocketManager.Instance.Send("/Login",user);

        }


        private void WbOnOnMessage(object? sender, MessageEventArgs e)
        {
            if (e.Data == "error")
            {
                Application.Current.Dispatcher.Invoke(() =>
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
            Application.Current.Dispatcher.Invoke(() =>
            {

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
