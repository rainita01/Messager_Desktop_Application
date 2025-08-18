using demo_158.MVVM.Model;
using demo_158.MVVM.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using demo_158.Services;
using Microsoft.Extensions.DependencyInjection;
using WebSocketSharp;

namespace demo_158.MVVM.View
{
    /// <summary>
    /// Interaction logic for MessageAndTalkView.xaml
    /// </summary>
    public partial class MessageAndTalkView : UserControl
    {
        private readonly MessageAndTalkViewModel _viewModel;
        
        private readonly UsersConversationsServices _services;
        public ObservableCollection<MessagesModel> Messages { get; set; }
        public ConversationReceive Conversation { get; set; }
        public string Username { get; set; }
        public EventHandler SuccessEventMessage;
        public MessageAndTalkView(MessageAndTalkViewModel viewModel,UsersConversationsServices services)
        {
            _viewModel = viewModel;
            _services = services;
           ;

            DataContext = _viewModel;

            InitializeComponent();
            Loaded += MessageAndTalkView_OnLoaded;
            Loaded -= MessageAndTalkView_OnLoaded;

           _viewModel.SuccessMessageSend += SuccessMessageSendEvent;
            _viewModel.SuccessMessageReceived += SuccessMessageReceived;
        }

        private void SuccessMessageReceived(object? sender, EventArgs e)
        {

            Messages.Add(_viewModel.Messages.Last());
            MessagesListView.ScrollIntoView(_viewModel.Messages.Last());
        
        }

        private void SuccessMessageSendEvent(object? sender, EventArgs e)
        {

            MessagesListView.ScrollIntoView(_viewModel.Messages.Last());
            Messages.Add(_viewModel.Messages.Last());
            SuccessEventMessage.Invoke(this,EventArgs.Empty);
        
        }



        private void MessageAndTalkView_OnLoaded(object sender, RoutedEventArgs e)
        {

            _viewModel.ContactUsername = Conversation.ContactUsername;
            _viewModel.Username = Username; 
            _viewModel.Id = Conversation.Id;
            _viewModel.Image = Conversation.ContactImage;

            _viewModel.Messages = new ObservableCollection<MessagesModel>(Messages.Select((i, index) => new MessagesModel()
            {
                Id = i.Id,
                SenderImage = _viewModel.Image,
                SenderName = i.SenderName,
                SentTime = i.SentTime,
                Text = i.Text,
                HorizontalAlignmentMessage = _services.SetHorizontalAlignment(_viewModel.ContactUsername, i.SenderName),
                FlowDirectionMessage = _services.SetFlowDirectionMessage(_viewModel.ContactUsername, i.SenderName),
                FirstMessage = index == 0 || _services.SetFirstMessage(i.SenderName, Messages?[index - 1].SenderName),
                BackgroundColorBrush = _services.SetBackGroundBrush(_viewModel.ContactUsername, i.SenderName)

            }));

            _viewModel.Conversation = Conversation;
            if (_viewModel.Messages != null)
            {
                MessagesListView.ScrollIntoView(_viewModel.Messages.Last());
            }

            MessageTextBox.Focus();
        }

        
    }
}
