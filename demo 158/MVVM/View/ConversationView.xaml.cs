using demo_158.MVVM.Model;
using demo_158.MVVM.ViewModel;
using Microsoft.EntityFrameworkCore;

using System.Windows;
using System.Windows.Controls;


namespace demo_158.MVVM.View
{
    /// <summary>
    /// Interaction logic for ConversationView.xaml
    /// </summary>
    public partial class ConversationView : UserControl
    {
        private readonly MainViewModel _viewModel;
        
        public ConversationView(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel.ConversationViewModel;
          
            Loaded += MessageAndTalkView_OnLoaded;
            Loaded -= MessageAndTalkView_OnLoaded;
        }
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("EVENT Fired");
        }
        private void MessageAndTalkView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_viewModel.ConversationViewModel.Messages is { Count: > 0 })
            {
                MessagesListView.ScrollIntoView(_viewModel.ConversationViewModel.Messages.Last());
                MessageTextBox.Focus();
            }
        }
    }
}
