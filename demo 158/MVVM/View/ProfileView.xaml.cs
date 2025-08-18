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
        public ReceiveUser? User { get; set; }
        public ProfileView(ProfileViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.ProfileSuccessChange += ProfileSuccessChange;
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void ProfileSuccessChange(object? sender, EventArgs e)
        {
            User.Username = _viewModel.Username;
            User.BioCaption = _viewModel.Bio;
            User.Email = _viewModel.Email;
            Application.Current.Dispatcher.Invoke(() =>
            {

                SaveButton.Visibility = Visibility.Hidden;
            });
         

        }

        protected override void OnActivated(EventArgs e)
        {
            _viewModel.Id = User.Id;
            _viewModel.Username = User.Username;
            _viewModel.Bio = User.BioCaption;
            _viewModel.Email = User.Email;
            _viewModel.Image = User.Image;
            base.OnActivated(e);
        }
        private void ProfileViewExitButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void Bio_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SaveButton.Visibility = Visibility.Visible;

            if (Bio.Text == User.BioCaption)
            {
                SaveButton.Visibility = Visibility.Hidden;
            }
           
        }

        private void Email_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SaveButton.Visibility = Visibility.Visible;
            if ( Email.Text == User.Email)
            {
                SaveButton.Visibility = Visibility.Hidden;
            }
           
        }

        private void Username_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SaveButton.Visibility = Visibility.Visible;
            if (Username.Text == User.Username)
            {
                SaveButton.Visibility = Visibility.Hidden;
            }
            
        }
    }
}
