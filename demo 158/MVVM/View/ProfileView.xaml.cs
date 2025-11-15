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
using System.Windows.Shapes;

using demo_158.MVVM.Model;
using demo_158.MVVM.ViewModel;

namespace demo_158.MVVM.View
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : Window
    {
        private readonly ProfileViewModel _viewModel;
        public EventHandler OnSuccessChange;
        public ProfileView(ProfileViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.ProfileSuccessChange += ProfileSuccessChange;
       
            
        }

        private void ProfileSuccessChange(object? sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {

                SaveButton.Visibility = Visibility.Hidden;
            });
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
        }
        private void ProfileViewExitButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void Bio_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SaveButton.Visibility = Visibility.Visible;

            if (Bio.Text == _viewModel.MyUserInfo.BioCaption)
            {
                SaveButton.Visibility = Visibility.Hidden;
            }
           
        }

        private void Email_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SaveButton.Visibility = Visibility.Visible;
            if ( Email.Text == _viewModel.MyUserInfo.Email)
            {
                SaveButton.Visibility = Visibility.Hidden;
            }
           
        }

    }
}
