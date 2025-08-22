using demo_158.Base;
using demo_158.MVVM.Model;
using demo_158.MVVM.View;
using demo_158.MVVM.View.Model;
using demo_158.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WebSocketSharp;

namespace demo_158.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private object _currentView;
        private readonly IServiceProvider _service;
        private readonly MessageAndTalkView _messageAndTalkView;
        private ICommand moveAndDrugCommand;
        private ICommand openProfileCommand;
        private List<ConversationModel>? _conversations;
        private ConversationModel? _conversationModel;
        private string _username;
        private ObservableCollection<ConversationModel>? _conversations1;

        public ConversationModel? ConversationModel 
        {
            get => _conversationModel;
            set => SetField(ref _conversationModel, value);
        }

        public ReceiveUser User { get; set; }
        public string Username
        {
            get => _username;
            set => SetField(ref _username, value);
        }

        public ObservableCollection<ConversationModel>? Conversations
        {
            get => _conversations1;
            set => SetField(ref _conversations1, value);
        }

        public object CurrentView
        {
            get => _currentView;
            set => SetField(ref _currentView, value);
        }

        public MainViewModel(IServiceProvider service,MessageAndTalkView messageAndTalkView)
        {
            
            _service = service;
            _messageAndTalkView = messageAndTalkView;
            var userView = service.GetService<DefaultMessageView>();
            CurrentView = userView;
            var ws = SocketManager.Instance.GetConnection("/MainView", Username);
            _messageAndTalkView.SuccessEventMessage += (sender, e) =>
            {
                Conversations.First().LastMessage = _messageAndTalkView.Messages.Last().Text;
                OnPropertyChanged();
            };
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
            var jsonObject = JObject.Parse(e.Data);
            string type = (string)jsonObject["Type"];

            if (type != "mainView")
            {
                return;
            }

            var messageDeserialize = JsonSerializer.Deserialize<ResieveConversationModel>(e.Data);
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                _messageAndTalkView.Messages = new ObservableCollection<MessagesModel>(messageDeserialize.Messages);
                _messageAndTalkView.Conversation = this.ConversationModel;
                _messageAndTalkView.Username = Username;
                CurrentView =  _messageAndTalkView;
            });

        }

    }
}
