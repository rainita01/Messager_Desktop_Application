using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using demo_158.Base;
using demo_158.MVVM.Model;
using demo_158.MVVM.View;
using demo_158.MVVM.View.Model;
using demo_158.Services;
using Microsoft.Extensions.DependencyInjection;
using WebSocketSharp;

namespace demo_158.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        
        private readonly IServiceProvider _service;
        private readonly MessageAndTalkViewModel _messageViewModel;
        private readonly MainViewServices _services;
        private ICommand moveAndDrugCommand;
        private ICommand messageShowCommand;
        private ICommand openProfileCommand;
        private ObservableCollection<ConversationReceive>? _conversations;
        private object _currentView;
        private ConversationReceive? _receive;
        private string _username;
        public ConversationReceive? Receive 
        {
            get => _receive;
            set => SetField(ref _receive, value);
        }

        public ReceiveUser User { get; set; }
        public string Username
        {
            get => _username;
            set => SetField(ref _username, value);
        }
        public ObservableCollection<ConversationReceive>? Conversations
        {
            get => _conversations;
            set => SetField(ref _conversations, value);
        }

        public object CurrentView
        {
            get => _currentView;
            set => SetField(ref _currentView, value);
        }

        public MainViewModel(IServiceProvider service,MessageAndTalkViewModel messageViewModel,MainViewServices services)
        {
            
            _service = service;
            _messageViewModel = messageViewModel;
            _services = services;
            var userView = service.GetService<DefaultMessageView>();
            CurrentView = userView;
            var ws = SocketManager.Instance.GetConnection("/MainView", Username);
            ws.OnMessage += WsOnOnMessage;

        }

        


        public ICommand OpenProfileCommand => openProfileCommand ?? new GeneralCommand((() =>
        {
            var profileView = _service.GetService<ProfileView>();

            if (profileView.User == null)
            {
                profileView.User = User;
            }
            profileView.ShowDialog();
        }));

        public ICommand MoveAndDrugCommand => moveAndDrugCommand ?? new GeneralCommand((() =>
        {
            Application.Current.Windows.OfType<MainView>().FirstOrDefault()?.DragMove();
        }));
        private void WsOnOnMessage(object? sender, MessageEventArgs e)
        {
            if (e.Data == "Successfully")
            {
                return;
            }
            _services.GetDataAndSend(e, _service, this, this.Receive);
        }
    }
}
