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
    /// Interaction logic for CreateGroupView.xaml
    /// </summary>
    public partial class CreateGroupView : Window
    {
        private readonly CreateGroupViewModel _viewModel;

        public CreateGroupView(CreateGroupViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
           
            _viewModel.RequestClose += (s, e) => this.Close();
            Loaded -= CreateGroupView_Loaded;
            Loaded += CreateGroupView_Loaded;   
        }

        private void CreateGroupView_Loaded(object sender, RoutedEventArgs e)
        {
            NameTextBox.Focus();
        }
    }
}
