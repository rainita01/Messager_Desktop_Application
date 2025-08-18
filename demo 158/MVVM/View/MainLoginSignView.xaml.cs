using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using demo_158.Base;
using demo_158.MVVM.View;
using demo_158.MVVM.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace demo_158
{
    /// <summary>
    /// Interaction logic for MainLoginSignView.xaml
    /// </summary>
    public partial class MainLoginSignView : Window
    {
        private readonly MainLoginSignViewModel _viewModel;
       

        public MainLoginSignView(MainLoginSignViewModel viewModel)
        {
            _viewModel = viewModel;
           
            DataContext = _viewModel;
            InitializeComponent();
            SharingDataViewModel.Instance.CurrenViewChanged += CurrenViewChanged;
            SharingDataViewModel.Instance.CurrentViewErrorChanged += CurrentViewErrorChanged;
        }

        private void CurrentViewErrorChanged(object? sender, EventArgs e)
        {
            LogInButoon.Visibility = Visibility.Visible;
            SignInButoon.Visibility = Visibility.Visible;
        }

        private void CurrenViewChanged(object? sender, EventArgs e)
        {
            LogInButoon.Visibility = Visibility.Hidden;
            SignInButoon.Visibility = Visibility.Hidden;
        }


        private void ExitClickMethod(object sender, RoutedEventArgs e)
        {
                Application.Current.Shutdown();
        }

        private void LoginView_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            

        }

        private void HideClickMethod(object sender, RoutedEventArgs e)
        {
                
                this.WindowState = WindowState.Minimized;
        }
    }
}