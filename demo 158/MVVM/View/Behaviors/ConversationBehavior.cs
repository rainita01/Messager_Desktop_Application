using demo_158.MVVM.Model;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using demo_158.MVVM.ViewModel;

namespace demo_158.MVVM.View.Behaviors;

public class ConversationBehavior : Behavior<ConversationView>
{
    private ScrollViewer _scroll;
    protected override void OnAttached()
    {
        
        base.OnAttached();
        AssociatedObject.Loaded += (s, e) =>
        {
            _scroll = FindScrollViewer(AssociatedObject);
            if (_scroll != null)
            {
                _scroll.ScrollChanged += ScrollChanged;
                CheckVisible();
            }
        };

    }

    private void ScrollChanged(object sender, ScrollChangedEventArgs e)
    {

        CheckVisible();
    }

    private void CheckVisible()
    {
        for (int i = 0; i < AssociatedObject.MessagesListView.Items.Count; i++)
        {
            var item = AssociatedObject.MessagesListView.ItemContainerGenerator.ContainerFromIndex(i) as ListViewItem;
            if (item == null) continue;

            if (IsVisibleInScroll(item))
            {
                if (AssociatedObject.MessagesListView.Items[i] is MessageViewModel msg)
                {
                    msg.Message.IsSeen = true;
                }
            }
        }
    }

    private bool IsVisibleInScroll(FrameworkElement element)
    {

        var container = _scroll;
        var transform = element.TransformToAncestor(container);
        var rect = transform.TransformBounds(new Rect(0, 0, element.ActualWidth, element.ActualHeight));

        return rect.Bottom > 0 && rect.Top < container.ViewportHeight;
    }

    private ScrollViewer FindScrollViewer(DependencyObject obj)
    {

        if (obj is ScrollViewer sc) return sc;

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            var result = FindScrollViewer(VisualTreeHelper.GetChild(obj, i));
            if (result != null) return result;
        }
        return null;
    }
}