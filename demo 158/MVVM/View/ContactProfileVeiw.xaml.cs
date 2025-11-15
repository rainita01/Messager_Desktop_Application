using demo_158.MVVM.Model;
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
using demo_158.MVVM.ViewModel;

namespace demo_158.MVVM.View
{
    /// <summary>
    /// Interaction logic for ContactProfileVeiw.xaml
    /// </summary>
    public partial class ContactProfileVeiw : Window
    {
        private readonly ContactProfileViewModel _viewModel;
        public ContactUserModel Profile { get; set; }
        public ContactProfileVeiw(ContactProfileViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            _viewModel.Profile = Profile;
        }
        private void ProfileViewExitButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
