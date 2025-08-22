using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using demo_158.Base;
using demo_158.MVVM.View;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace demo_158.MVVM.ViewModel
{
    public class MainLoginSignViewModel : ViewModelBase
    {
        private readonly IServiceProvider _service;
        private readonly LoginView _loginView;
        private readonly SignView _signView;
        private readonly LoginLoadingPage _loadingPage;
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => SetField(ref _currentView, value);
        }

        public MainLoginSignViewModel(IServiceProvider service,LoginView loginView,SignView signView,LoginLoadingPage loadingPage)
        {
            _service = service;
            _loginView = loginView;
            _signView = signView;
            _loadingPage = loadingPage;
            _currentView = _loginView;
            SharingDataViewModel.Instance.CurrenViewChanged += CurrenViewChanged;
            SharingDataViewModel.Instance.CurrentViewErrorChanged += CurrentViewErrorChanged;
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

        private ICommand _signViewSwitch;
        public ICommand SignViewSwitch => _signViewSwitch = new GeneralCommand(SignViewExecute);

        private void SignViewExecute()
        {
            CurrentView = _signView;
        }
    }
}
