
using System.Windows.Input;
using demo_158.Base;
using demo_158.Hubs;
using demo_158.MVVM.Model;
using WebSocketSharp;


namespace demo_158.MVVM.ViewModel
{
   public class ProfileViewModel :ViewModelBase
    {
        private readonly ConnectionManager _connectionManager;

        private string _username;
        private string _email;
        private string? _bio;
        private int _id;
        public EventHandler ProfileSuccessChange;
        
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
        public ProfileViewModel(ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
            ChangeProfile();
        }

        public ICommand SaveChanges => saveChanges ?? new GeneralCommand(async ()=>await ExecuteAction());

       private async Task ExecuteAction()
       {
           var model = new ProfileEditModel()
           {
               Bio = Bio,
               Id = Id,
               Email = Email,
               Username = Username,
           };
           await _connectionManager.SendAsync("ChangeProfile", model);
       }

       private void  ChangeProfile()
       {
            _connectionManager.OnAsync<string>("ChangeProfile", submit =>
            {
                        ProfileSuccessChange.Invoke(this,EventArgs.Empty);
            });

       }
    }
}
