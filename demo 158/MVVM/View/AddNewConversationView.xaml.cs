using CommunityToolkit.Mvvm.Messaging;
using demo_158.EventsPublish;
using demo_158.MVVM.ViewModel;
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

namespace demo_158.MVVM.View
{
    /// <summary>
    /// Interaction logic for AddNewConversationView.xaml
    /// </summary>
    public partial class AddNewConversationView : Window
    {
        private readonly AddNewConversationViewModel _viewModel;

        public AddNewConversationView(AddNewConversationViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            WeakReferenceMessenger.Default.Register<CreateNewConversationEvent>(this, (r, m) =>
            {
                Close();
            });

        }

        private void ExitButon(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
