using demo_158.Base;
using demo_158.MVVM.Model;
using demo_158.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WebSocketSharp;
using Color = System.Drawing.Color;

namespace demo_158.MVVM.ViewModel
{
    public class SignViewModel : ViewModelBase
    {
        private string _email;
        private string _username;
        private string _password;
        private string _verifyPassword;
        private ICommand signCommand;
        private string _messageText;
        private SolidColorBrush _messageTextSuccess1;
        private WebSocket _ws;
        public WebSocket ws
        {
            get => _ws;
            set => SetField(ref _ws, value);
        }


        public SolidColorBrush MessageTextSuccess
        {
            get => _messageTextSuccess1;
            set => SetField(ref _messageTextSuccess1, value);
        }
        public string MessageText
        {
            get => _messageText;
            set => SetField(ref _messageText, value);
        }

        public Visibility PasswordTextBlockVisibility => CollectionUtilities.IsNullOrEmpty(Password) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility VerifyPasswordTextBlockVisibility => CollectionUtilities.IsNullOrEmpty(VerifyPassword) ? Visibility.Visible : Visibility.Collapsed;
        public string Email 
        {
            get => _email;
            set => PropertyChange(ref _email, value);
        }
        public string Username
        {
            get => _username;
            set => PropertyChange(ref _username, value);
        }

        public string Password  
        {
            get => _password;
            set
            {
                if (SetField(ref _password,value))
                {
                    OnPropertyChanged(nameof(PasswordTextBlockVisibility));
                }
            }
        }

        public string VerifyPassword
        {
            get => _verifyPassword;
            set
            {
                if (SetField(ref _verifyPassword,value))
                {
                    OnPropertyChanged(nameof(VerifyPasswordTextBlockVisibility));
                }
            }
        }

  

        public SignViewModel()
        {
            ws = new WebSocket($"ws://localhost:7482/Sign");
            ws.Connect();
        }

     
        public ICommand SignCommand => signCommand ?? new GeneralCommand((SignExecuteAction),SignCanExecute);

        private async void SignExecuteAction()
        {
            if (!(await SignInValidation()))
            {
                return;
            }
            var user = new SendUser()
            {
                Username = Username,
                Password = Password,
                Email = Email,
            };
            var userDeserializer = JsonSerializer.Serialize(user);
            ws.Send(userDeserializer);
            
            


        }

        private async void MbOnOnMessage(object? sender, MessageEventArgs e)
        {
            if (e.Data == "Error")
            {
                MessageTextSuccess = Brushes.Red;
                MessageText = "Username Exist";
                return;
            }
                

            if (e.Data == "SuccessFull")
            {
                MessageText = "Account Created Successfully.";
                MessageTextSuccess = Brushes.LawnGreen;
            }
            
        }

        private bool SignCanExecute()
        {
            string[] fields = { Email, Username, Password, VerifyPassword };
            return fields.All(f => !string.IsNullOrEmpty(f));
        }

        private async Task<bool> SignInValidation()
        {
            
            if (Password != VerifyPassword)
            {
                MessageText= "Password and VerifyPassword are not same";
                MessageTextSuccess = Brushes.Red;
                return false;
            }

            if (Password.Length <8 )
            {
                MessageText = "Password Is Weak!";
                MessageTextSuccess = Brushes.Red;
                return false;
            }
            return true;

        }
    }
}
