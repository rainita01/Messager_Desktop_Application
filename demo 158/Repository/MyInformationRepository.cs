using demo_158.Base;
using demo_158.Hubs;
using demo_158.MVVM.Model;
using Microsoft.Extensions.Hosting;

namespace demo_158.Repository
{
    public class MyInformationRepository 
    {
        private readonly ConnectionManager _connectionManager;
        public Action<UserModelFromServer> SuccessLoginAction { get; set; }
        public UserModelFromServer? MyUserInfo { get; set; }

        public MyInformationRepository( ConnectionManager connectionManager)
        {
           
            _connectionManager = connectionManager;

        }
       
       
        public  async Task StartAsync()
        {
                await ReceiveUser();
        }

        private async Task ReceiveUser()
        {
            await _connectionManager.OnAsync<UserModelFromServer>("ReceiveUser", (user) =>
            {
                MyUserInfo = user;
                SuccessLoginAction.Invoke(user);

            });

        }
    }
}
