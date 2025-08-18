using System;
using System.Collections.Generic;
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

namespace demo_158.MVVM.View
{
    /// <summary>
    /// Interaction logic for SignView.xaml
    /// </summary>
    public partial class SignView : UserControl
    {
        private readonly SignViewModel _viewModel;

        public SignView(SignViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void Password_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = ((PasswordBox)sender).Password;
        }

        private void VerifyPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.VerifyPassword = ((PasswordBox)sender).Password;
        }

        private void SignView_OnLoaded(object sender, RoutedEventArgs e)
        {
            Email.Focus();
        }
    }
}
