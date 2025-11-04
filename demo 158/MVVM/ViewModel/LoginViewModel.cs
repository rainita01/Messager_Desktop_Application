using demo_158.Base;
using demo_158.MVVM.Model;
using demo_158.MVVM.View;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using demo_158.Hubs;
using demo_158.Repository;

namespace demo_158.MVVM.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly MyInformationRepository _myInformationRepository;
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
        public LoginViewModel(MyInformationRepository myInformationRepository,IServiceProvider service,ConnectionManager connection)
        {
            _myInformationRepository = myInformationRepository;
            _service = service;
            _connection = connection;
            _myInformationRepository.SuccessLoginAction += SuccessLoginAction;
       
        }

        public ICommand LoginCommand => loginCommand ?? new GeneralCommand(async () => await LoginCommandExecuteActionAsync(),LoginCanExecute);

        private void SuccessLoginAction(UserModelFromServer obj)
        {
            var mainView = _service.GetService<MainView>();
            mainView?.Show();
            ReceiveConversations(obj.UserId);
            Application.Current.Windows.OfType<MainLoginSignView>().FirstOrDefault()?.Close();

        }
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

        private bool LoginCanExecute()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }
        public async Task ReceiveConversations(int userId)
        {
            await _connection.SendAsync("ReceiveConversations", userId);
        
        }
    }
}
