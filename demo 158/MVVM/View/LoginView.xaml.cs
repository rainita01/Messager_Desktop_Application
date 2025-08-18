using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using demo_158.MVVM.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace demo_158.MVVM.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        private readonly LoginViewModel _viewModel;
        private readonly IServiceProvider _service;
      
        public LoginView(LoginViewModel viewModel,IServiceProvider service)
        {
            _viewModel = viewModel;
            _service = service;
            DataContext = _viewModel;
            InitializeComponent();


        }


     
        private void Password_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = ((PasswordBox)sender).Password;
           
        }

        private void LoginView_OnLoaded(object sender, RoutedEventArgs e)
        {

            Username.Focus();

        }
    }
}
