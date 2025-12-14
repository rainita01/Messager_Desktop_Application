using demo_158.MVVM.Model;
using demo_158.MVVM.ViewModel;
using Microsoft.EntityFrameworkCore;

using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Messaging;
using demo_158.EventsPublish;


namespace demo_158.MVVM.View
{
    /// <summary>
    /// Interaction logic for ConversationsView.xaml
    /// </summary>
    public partial class ConversationView : UserControl
    {
        private readonly MainViewModel _viewModel;
        
        public ConversationView(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel.ConversationViewModel;
            Loaded -= MessageAndTalkView_OnLoaded;
            Loaded += MessageAndTalkView_OnLoaded;
            WeakReferenceMessenger.Default.Register<MessageSendedSuccessEvent>(this, MessageSendSuccess);
        
        }

        private void MessageSendSuccess(object recipient, MessageSendedSuccessEvent message)
        {
            MessagesListView.ScrollIntoView(_viewModel.ConversationViewModel.Messages.Last());
        }
        private void MessageAndTalkView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_viewModel.ConversationViewModel?.Messages is { Count: > 0 })
            {
                MessagesListView.ScrollIntoView(_viewModel.ConversationViewModel.Messages.Last());
                MessageTextBox.Focus();
            }
        }

        private void MessagesListView_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
                var listView = sender as ListView;

                for (int i = 0; i < listView?.Items.Count; i++)
                {
                    var item = listView.ItemContainerGenerator.ContainerFromIndex(i) as ListViewItem;
                    if (item == null)
                        continue;

                    if (IsElementVisible(item, listView))
                    {
                        if (item.DataContext is MessageModelFromServer msg && !msg.IsSeen)
                        {
                            msg.IsSeen = true;
                        }
                    }
                }

        }
        private bool IsElementVisible(FrameworkElement element, FrameworkElement container)
        {
            var bounds = element.TransformToAncestor(container)
                .TransformBounds(new Rect(0, 0, element.ActualWidth, element.ActualHeight));

            var containerRect = new Rect(0, 0, container.ActualWidth, container.ActualHeight);
            return containerRect.IntersectsWith(bounds);
        }

        private void MessagesListView_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
