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
using demo_158.Hubs;
using demo_158.MVVM.Model;
using demo_158.MVVM.ViewModel;

namespace demo_158.MVVM.View
{
    /// <summary>
    /// Interaction logic for EditMessageView.xaml
    /// </summary>
    public partial class EditMessageView : Window
    {
        public int MessageId { get; set; }
        private readonly EditMessageViewModel _viewModel;
        private readonly ConnectionManager _connectionManager;
        public ContactUserModel ContactUser { get; set; }
        public string Username { get; set; }
        public EditMessageView(EditMessageViewModel viewModel,ConnectionManager connectionManager)
        {
            _viewModel = viewModel;
            _connectionManager = connectionManager;
            DataContext = _viewModel;
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            Text.Focus();

        }

        private async  void SaveChanges(object sender, RoutedEventArgs e)
        {
            var editedMessage = new EditMessageModel()
            {
                SenderUsername   = Username,
                ContactUsername = ContactUser.ContactUsername,
                MessageId = MessageId,
                NewText = Text.Text,
                IsEdited = true

            };
           await _connectionManager.SendAsync("EditMessage",editedMessage);
            this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
