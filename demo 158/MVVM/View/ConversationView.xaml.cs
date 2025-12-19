using CommunityToolkit.Mvvm.Messaging;
using demo_158.EventsPublish;
using demo_158.MVVM.Model;
using demo_158.MVVM.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace demo_158.MVVM.View
{
    /// <summary>
    /// Interaction logic for ConversationsView.xaml
    /// </summary>
    public partial class ConversationView : UserControl
    {
        private readonly MainViewModel _viewModel;
        private ScrollViewer _scrollViewer;
        public ConversationView(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel.ConversationViewModel;
            Loaded -= MessageAndTalkView_OnLoaded;
            Loaded += MessageAndTalkView_OnLoaded;
            WeakReferenceMessenger.Default.Register<MessageSendedSuccessEvent>(this, MessageSendSuccess);
           
        }

        private void MessageAndTalkView_OnLoaded(object sender, RoutedEventArgs e)
        {

            var message = _viewModel.ConversationViewModel?.Messages
                .Where(m => m.Message.IsSeen == false)
                .Where(b => b.Message.IsMyMessage == false).FirstOrDefault();
            if (message != null)
            {
                MessagesListView.ScrollIntoView(message);
                MessageTextBox.Focus();
            }
            else if (_viewModel.ConversationViewModel?.Messages?.Count > 0)
            {
                MessagesListView.ScrollIntoView(_viewModel.ConversationViewModel.Messages.Last());
                MessageTextBox.Focus();
            }
            var sv = FindScrollViewer(MessagesListView);

            if (sv == null)
                return;

            // اگر اسکرول لازم نیست
            if (sv.ExtentHeight <= sv.ViewportHeight)
            {
                _viewModel.ConversationViewModel?.MarkAllVisibleAsSeenAsync();
                return;
            }

            sv.ScrollChanged += (_,__) =>
            {
                var last = _viewModel.ConversationViewModel.Messages.LastOrDefault(m => !m.Message.IsMyMessage);
                if (last != null)
                    _viewModel.ConversationViewModel.OnUserScrolledAsync(last.Id);
            };
        }
        private ScrollViewer FindScrollViewer(DependencyObject root)
        {
            if (root == null)
                return null;

            if (root is ScrollViewer scrollViewer)
                return scrollViewer;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                var result = FindScrollViewer(child);
                if (result != null)
                    return result;
            }

            return null;
        }
        private void MessageSendSuccess(object recipient, MessageSendedSuccessEvent message)
        {
            MessagesListView.ScrollIntoView(_viewModel.ConversationViewModel.Messages.Last());
        }

    }
}
