using demo_158.MVVM.Model;
using demo_158.MVVM.View;
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
using demo_158.MVVM.ViewModel;
using Newtonsoft.Json.Linq;
using WebSocketSharp;

namespace demo_158.Services
{
    public class MainViewServices
    {

        public void GetDataAndSend(MessageEventArgs e,IServiceProvider serviceProvider,MainViewModel viewModel,ConversationReceive? receive )
        {
            try
            {
                var jsonObject = JObject.Parse(e.Data);
                string type =(string) jsonObject["Type"];

                if (type != "mainView")
                {
                    return;
                }

                var messageDeserialize = JsonSerializer.Deserialize<ResiveConversationModel>(e.Data);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var messageView = serviceProvider.GetService<MessageAndTalkView>();
                    messageView.SuccessEventMessage += SuccessEventMessage;
                    messageView.Messages = new ObservableCollection<MessagesModel>(new ObservableCollection<MessagesModel>(messageDeserialize.Messages));
                    messageView.Conversation = receive;
                    messageView.Username = viewModel.Username;
                    viewModel.CurrentView = messageView;
                });
            }
            catch (Exception exception)
            {

                throw;
            }
          
        }
        private void SuccessEventMessage(object? sender, EventArgs e)
        {
            MessageBox.Show("Hello success");
        }
    }
}
