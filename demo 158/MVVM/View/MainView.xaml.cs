
using demo_158.MVVM.Model;
using demo_158.MVVM.View.Model;
using demo_158.MVVM.ViewModel;
using demo_158.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
using demo_158.Base;
using WebSocketSharp;

namespace demo_158.MVVM.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private readonly MainViewModel _viewModel;
 
        private readonly IServiceProvider _serviceProvider;
        private readonly MainViewServices _services;
        private readonly MessageAndTalkView _messageAndTalkView;
        public ReceiveUser User { get; set; } = new();
        public List<ConversationReceive>? Conversations { get; set; }
      
        public MainView(MainViewModel viewModel,IServiceProvider serviceProvider,MainViewServices services,MessageAndTalkView messageAndTalkView)
        {
            _viewModel = viewModel;
          
            _serviceProvider = serviceProvider;
            _services = services;
            _messageAndTalkView = messageAndTalkView;
            DataContext = _viewModel;
            InitializeComponent();
            
        }

   

        protected override void OnActivated(EventArgs e)
        {
         
            _viewModel.User = User;
            var ws = SocketManager.Instance.GetConnection("/MainView",User.Username);
            ws.OnMessage += WsOnOnMessage;
            _viewModel.Username = User.Username;
            _viewModel.Conversations = new ObservableCollection<ConversationReceive>(Conversations);
            base.OnActivated(e);
            
        }

        
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (ContactsList.SelectedItem != null)
            {
                _viewModel.Receive = ContactsList.SelectedItem as ConversationReceive;
                _viewModel.Receive.Type = "mainView";
                SocketManager.Instance.Send("/MainView",_viewModel.Receive);
                
            }
        }

        private void WsOnOnMessage(object? sender, MessageEventArgs e)
        {
            if (e.Data == "Successfully")
            {
                return;
            }
            _services.GetDataAndSend(e, _serviceProvider, _viewModel,_viewModel.Receive);
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
