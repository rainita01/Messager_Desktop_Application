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
        private readonly MainViewModel _viewModel;
        
        public MessageAndTalkView(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel.ConversationModel;
            InitializeComponent();
            Loaded += MessageAndTalkView_OnLoaded;
            Loaded -= MessageAndTalkView_OnLoaded;
        }

        private void MessageAndTalkView_OnLoaded(object sender, RoutedEventArgs e)
        {
           
            MessagesListView.ScrollIntoView(_viewModel.ConversationModel.Messages.Last());
            MessageTextBox.Focus();
        }


    }
}
