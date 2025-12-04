
using demo_158.Base;
using demo_158.MVVM.Model;
using demo_158.MVVM.View.Model;
using demo_158.MVVM.ViewModel;
using demo_158.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WebSocketSharp;

namespace demo_158.MVVM.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private readonly MainViewModel _viewModel;
        public MainView(MainViewModel viewModel)
        {
            _viewModel = viewModel;
       
            DataContext = _viewModel;
            InitializeComponent();
            
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            
        }

        // کلیک های دکمه های بالای صفحه برای بسته شدن و بزرگ وکوچک شدن صفخه
        private void TopHideButtonClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            
        }

        private void NormalizeMaximizeWindowClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {   
                this.WindowState = WindowState.Normal;
            }

            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void CloseTheAppClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

  
}
