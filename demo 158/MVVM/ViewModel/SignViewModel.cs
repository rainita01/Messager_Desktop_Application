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
using demo_158.Hubs;
using WebSocketSharp;
using Color = System.Drawing.Color;

namespace demo_158.MVVM.ViewModel
{
    public class SignViewModel : ViewModelBase
    {
        private readonly ConnectionManager _connection;
        private string _email;
        private string _username;
        private string _password;
        private string _verifyPassword;
        private ICommand signCommand;
        private string _messageText;
        private SolidColorBrush _messageTextSuccess1;

        public Visibility PasswordTextBlockVisibility => CollectionUtilities.IsNullOrEmpty(Password) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility VerifyPasswordTextBlockVisibility => CollectionUtilities.IsNullOrEmpty(VerifyPassword) ? Visibility.Visible : Visibility.Collapsed;


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

        public SignViewModel(ConnectionManager connection)
        {
            _connection = connection;
            InvalidSignUp();
            SuccessSignUp();
        }
     
        public ICommand SignCommand => signCommand ?? new GeneralCommand((SignExecuteAction),SignCanExecute);

        private async void SignExecuteAction()
        {
            if (!(await SignInValidation()))
            {
                return;
            }
            var user = new UserModelFromUser()
            {
                Username = Username,
                Password = Password,
                 Email = Email,
            };

           await _connection.SendAsync("SignUp", user);

        }

        private  void SuccessSignUp()
        {
            _connection.OnAsync<string>("SuccessSignUp", data =>
            {
                MessageBox.Show("Success Sign Up!", data, MessageBoxButton.OKCancel);
            });
        }

        private void InvalidSignUp()
        {
            _connection.OnAsync<string>("InvalidSignUp", data =>
            {
                MessageBox.Show("InvalidOperator", data + "!!!", MessageBoxButton.OK);
            });
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
