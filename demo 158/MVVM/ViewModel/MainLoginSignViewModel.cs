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
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => SetField(ref _currentView, value);
        }

        public MainLoginSignViewModel(IServiceProvider service)
        {
            _service = service;
            LoginViewExecute();
            SharingDataViewModel.Instance.CurrenViewChanged += CurrenViewChanged;
            SharingDataViewModel.Instance.CurrentViewErrorChanged += CurrentViewErrorChanged;
        }

        private void CurrentViewErrorChanged(object? sender, EventArgs e)
        {
            var loginV = _service.GetService<LoginView>();
            CurrentView = loginV;
        }

        private void CurrenViewChanged(object? sender, EventArgs e)
        {
            CurrentView = SharingDataViewModel.Instance.CurrentView;
        }

        private ICommand _loginViewSwitch;
        public ICommand LoginViewSwitch => _loginViewSwitch = new GeneralCommand(LoginViewExecute);

        private void LoginViewExecute()
        {

            var loginV = _service.GetService<LoginView>();
            CurrentView = loginV;
        }

        private ICommand _signViewSwitch;
        public ICommand SignViewSwitch => _signViewSwitch = new GeneralCommand(SignViewExecute);

        private void SignViewExecute()
        {
            var signV = _service.GetService<SignView>();
            CurrentView = signV;
        }
    }
}
