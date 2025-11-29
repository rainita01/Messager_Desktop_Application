using demo_158.Base;
using demo_158.Hubs;
using demo_158.MVVM.View;
using demo_158.Services.Enums;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;

namespace demo_158.MVVM.ViewModel
{
    public class MainLoginSignViewModel : ViewModelBase
    {
        private readonly LoginView _loginView;
        private readonly SignView _signView;
        private readonly LoginLoadingPageView _loadingPageView;
        private readonly ConnectionManager _connectionManager;
        private readonly IServiceProvider _service;
        private ICommand _signViewSwitch;
        private ICommand _loginViewSwitch;
        private SolidColorBrush _cycleFillerBrush;
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => SetField(ref _currentView, value);
        }

        public SolidColorBrush CycleFillerBrush
        {
            get => _cycleFillerBrush;
            set => SetField(ref _cycleFillerBrush, value);
        }

        public MainLoginSignViewModel(LoginView loginView,SignView signView,LoginLoadingPageView loadingPageView,ConnectionManager connectionManager,IServiceProvider service)
        {
            
            _loginView = loginView;
            _signView = signView;
            _loadingPageView = loadingPageView;
            _connectionManager = connectionManager;
            _service = service;
            _connectionManager.OnStateChanged += OnStateChanged;
            _currentView = _loginView;
            OnErrorLogin();
            SharingDataViewModel.Instance.CurrenViewChanged += CurrenViewChanged;
            SharingDataViewModel.Instance.CurrentViewErrorChanged += CurrentViewErrorChanged;
        }

        private void OnStateChanged(HubConnectionState state)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                
                switch (state)
                {
                    case HubConnectionState.Connecting:
                        CycleFillerBrush = new SolidColorBrush(Colors.LightGoldenrodYellow);
                        break;
                    case HubConnectionState.Connected:
                        CycleFillerBrush = new SolidColorBrush(Colors.Chartreuse);
                        break;
                    case HubConnectionState.Disconnected:
                        CycleFillerBrush = new SolidColorBrush(Colors.OrangeRed);
                        break;
                    case HubConnectionState.Reconnecting:
                        CycleFillerBrush = new SolidColorBrush(Colors.LightGoldenrodYellow);
                        break;
                }
            });

        }



        private void CurrentViewErrorChanged(object? sender, EventArgs e)
        {
           
            CurrentView = _loginView;
        }

        private void CurrenViewChanged(object? sender, EventArgs e)
        {
            CurrentView = _loadingPageView;
        }

       
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
        public void OnErrorLogin()
        {

            _connectionManager.On<string>("OnErrorLogin", (methodError) =>
            {
                SharingDataViewModel.Instance.CurrentViewErrorChanged.Invoke(this,EventArgs.Empty);
                MessageBox.Show($"Could not Login {methodError}");
            });
        }
    }
}
