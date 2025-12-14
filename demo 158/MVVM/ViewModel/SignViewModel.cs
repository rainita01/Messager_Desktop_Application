using demo_158.Base;
using demo_158.Hubs;
using demo_158.MVVM.Model;
using demo_158.MVVM.View;
using demo_158.Repository;
using demo_158.Services;
using demo_158.Services.Enums;
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
using Microsoft.Extensions.DependencyInjection;
using WebSocketSharp;
using Color = System.Drawing.Color;

namespace demo_158.MVVM.ViewModel
{
    public class SignViewModel : ViewModelBase
    {
        private readonly ConnectionManager _connection;
        private readonly MyInformationRepository _myInformationRepository;
        private readonly IServiceProvider _service;
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

        public SignViewModel(ConnectionManager connection,MyInformationRepository myInformationRepository,IServiceProvider service)
        {
            _connection = connection;
            _myInformationRepository = myInformationRepository;
            _service = service;
            _myInformationRepository.SuccessSignInAction += SuccessLoginAction;
            InvalidSignUp();
        }

        private void SuccessLoginAction(UserModelFromServer obj)
        {

            var mainView = _service.GetService<MainView>();
            mainView?.Show();
             Application.Current.Windows.OfType<MainLoginSignView>().FirstOrDefault()?.Close();
        }

        public ICommand SignCommand => signCommand ?? new GeneralCommand((SignExecuteAction),SignCanExecute);

        private async void SignExecuteAction()
        {
            if  (!SignInValidation())
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
            SharingDataViewModel.Instance.CurrenViewChanged.Invoke(this,EventArgs.Empty);
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

        private bool SignInValidation()
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
