using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using demo_158.Base;
using demo_158.Hubs;
using demo_158.MVVM.Model;
using demo_158.MVVM.View;
using demo_158.Services.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace demo_158.MVVM.ViewModel
{
    public class MessageViewModel : ViewModelBase
    {
        private readonly ConnectionManager _connectionManager;
        private readonly IServiceProvider _service;
        private ICommand editMessageCommand;
        private ICommand deleteMessageCommand;
        private ICommand copyTextCommand;
        public MessagesModel Message { get; set; }
        public string Username { get; set; }
       public MessageViewModel(ConnectionManager connectionManager,IServiceProvider service)
       {
           _connectionManager = connectionManager;
           _service = service;
       }



       public ICommand EditMessageCommand => editMessageCommand ?? new GeneralCommand(() =>
       {
           if (Message.SenderName != Username)
           {
               MessageBox.Show("U cannot delete another person message");
               return;
           }
           var edit = _service.GetService<EditMessageView>();
          edit.Text.Text = Message.Text;
          edit.MessageId = Message.Id;
          edit.ShowDialog();
       });
       public  ICommand DeleteMessageCommand => deleteMessageCommand ?? new GeneralCommand(() =>
       {
           if (Message.SenderName != Username )
           {
               MessageBox.Show("U cannot delete another person message");
               return;
           }

           var result =  MessageBox.Show("Are you sure ?", "Delete", MessageBoxButton.YesNo);
           if (result == MessageBoxResult.No)
           {
               return;
           }
           _connectionManager.SendAsync("DeleteMessage", Message.Id);
       });
       public ICommand CopyTextCommand => copyTextCommand ?? new GeneralCommand(() =>
       {
           Clipboard.SetDataObject(Message.Text);
           MessageBox.Show("copy");
       });
    }
}
