using System.Windows;
using System.Windows.Input;
using demo_158.Base;
using demo_158.Hubs;
using demo_158.MVVM.View;
using demo_158.Services.Enums;
using Microsoft.AspNetCore.SignalR.Client;

namespace demo_158.MVVM.ViewModel
{
    public class MainLoginSignViewModel : ViewModelBase
    {
        private readonly LoginView _loginView;
        private readonly SignView _signView;
        private readonly LoginLoadingPage _loadingPage;
        private readonly ConnectionManager _connectionManager;
        private ICommand _signViewSwitch;
        private State _state;
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => SetField(ref _currentView, value);
        }

        public State State
        {
            get => _state;
            set => SetField(ref _state, value);
        }

        public MainLoginSignViewModel(LoginView loginView,SignView signView,LoginLoadingPage loadingPage,ConnectionManager connectionManager)
        {
            
            _loginView = loginView;
            _signView = signView;
            _loadingPage = loadingPage;
            _connectionManager = connectionManager;
            _connectionManager.OnStateChanged += OnStateChanged;
            _currentView = _loginView;
            SharingDataViewModel.Instance.CurrenViewChanged += CurrenViewChanged;
            SharingDataViewModel.Instance.CurrentViewErrorChanged += CurrentViewErrorChanged;
        }

        private void OnStateChanged(HubConnectionState state)
        {
            switch (state)
            {
                case HubConnectionState.Connecting:
                    State = State.Connecting;
                    break;
                case HubConnectionState.Connected:
                    State = State.Online;
                    break;
                case HubConnectionState.Disconnected:
                    State = State.Offline;
                    break;
                case HubConnectionState.Reconnecting:
                    State = State.Connecting;
                    break;
            }
        }



        private void CurrentViewErrorChanged(object? sender, EventArgs e)
        {
           
            CurrentView = _loginView;
        }

        private void CurrenViewChanged(object? sender, EventArgs e)
        {
            CurrentView = _loadingPage;
        }

        private ICommand _loginViewSwitch;
        public ICommand LoginViewSwitch => _loginViewSwitch = new GeneralCommand(LoginViewExecute);

        private void LoginViewExecute()
        {

            CurrentView = _loginView;
        }

      
        public ICommand SignViewSwitch => _signViewSwitch = new GeneralCommand(SignViewExecute);

        private void SignViewExecute()
        {
            CurrentView = _signView;
        }
    }
}
