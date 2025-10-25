using demo_158.MVVM.Model;
using demo_158.MVVM.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using demo_158.Services;
using Microsoft.Extensions.DependencyInjection;
using WebSocketSharp;

namespace demo_158.MVVM.View
{
    /// <summary>
    /// Interaction logic for MessageAndTalkView.xaml
    /// </summary>
    public partial class MessageAndTalkView : UserControl
    {
        private readonly MessageAndTalkViewModel _viewModel;
       
        public ObservableCollection<MessagesModel> Messages { get; set; }
        public ConversationModel Conversation { get; set; }
        public UserModelFromServer UserModelFromServer { get; set; }
   

        public MessageAndTalkView(MessageAndTalkViewModel viewModel)
        {
            _viewModel = viewModel;
            
            DataContext = _viewModel;
            InitializeComponent();
            Loaded += MessageAndTalkView_OnLoaded;
            Loaded -= MessageAndTalkView_OnLoaded;
        }
        private void MessageAndTalkView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Conversation = Conversation;
            _viewModel.UserModelFromServer = UserModelFromServer; 
            _viewModel.Messages = Messages;

            _viewModel.Conversation = Conversation;
            if (_viewModel.Messages != null)
            {
                MessagesListView.ScrollIntoView(_viewModel.Messages.Last());
            }

            MessageTextBox.Focus();
        }


    }
}
