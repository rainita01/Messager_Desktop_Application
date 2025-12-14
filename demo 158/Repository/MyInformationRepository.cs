using demo_158.Base;
using demo_158.Hubs;
using demo_158.MVVM.Model;
using demo_158.Services.Enums;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Hosting;

namespace demo_158.Repository
{
    public class MyInformationRepository :ViewModelBase 
    {
        private readonly ConnectionManager _connectionManager;
        private UserModelFromServer _myUserInfo;
        public Action<byte[]>? ImageChanged;
       
        public Action<UserModelFromServer> SuccessLoginAction { get; set; }
        public Action<UserModelFromServer> SuccessSignInAction { get; set; }

        public UserModelFromServer MyUserInfo
        {
            get => _myUserInfo;
            set
            {
                _myUserInfo = value;
                if (_myUserInfo?.Image != value?.Image)
                {
                    _myUserInfo = value;
                    ImageChanged?.Invoke(value.Image);
                }
                OnPropertyChanged();
            }

        }

        public MyInformationRepository( ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
       
       
        public  async Task StartAsync()
        {
                await ReceiveUser();
                await SignedUserReceived();
        }

        private async Task ReceiveUser()
        {
            await _connectionManager.OnAsync<UserModelFromServer>("ReceiveUser", (user) =>
            {
                MyUserInfo = user;
                SuccessLoginAction.Invoke(user);

            });
        }
        private async Task SignedUserReceived()
        {
            await _connectionManager.OnAsync<UserModelFromServer>("SignedUserReceived", (user) =>
            {
                MyUserInfo = user;
                SuccessSignInAction.Invoke(user);

            });
        }
    }
}
