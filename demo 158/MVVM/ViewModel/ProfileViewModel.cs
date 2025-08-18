using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using demo_158.Base;
using demo_158.MVVM.Model;
using WebSocketSharp;


namespace demo_158.MVVM.ViewModel
{
   public class ProfileViewModel :ViewModelBase
    {

        private string _username;
        private string _email;
        private string? _bio;
        private string _image;
        private ICommand _profileBioOkCommand;
        private int _id;
        public EventHandler ProfileSuccessChange;
        public EventHandler ProfileCancelChange;
       
        private readonly ICommand saveChanges;

        public int Id
        {
            get => _id;
            set => SetField(ref _id, value);
        }

        public string Username
        {
            get => _username;
            set => SetField(ref _username, value);
        }

        public string Email
        {
            get => _email;
            set => SetField(ref _email, value);
        }

        public string? Bio
        {
            get => _bio;
            set => SetField(ref _bio, value);
        }

        public string Image
        {
            get => _image;
            set => SetField(ref _image, value);
        }

        public ProfileViewModel()
        {
            var ws = SocketManager.Instance.GetConnection("/MainView");
            ws.Connect();
            ws.OnMessage += WsOnOnMessage;
        }

        private void WsOnOnMessage(object? sender, MessageEventArgs e)
        {
            if (e.Data == "Successfully")   
            {
                ProfileSuccessChange.Invoke(this,EventArgs.Empty);
            }

        }

        public ICommand SaveChanges => saveChanges ?? new GeneralCommand((ExecuteAction));

       private void ExecuteAction()
       {
           var model = new ProfileEditModel()
           {
               Type = "Profile",
               Bio = Bio,
               Id = Id,
               Email = Email,
               Username = Username,
           };
          SocketManager.Instance.Send("/MainView", model);
       }

       
    }
}
