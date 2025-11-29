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
           

        }

        private void ExitButon(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
