
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using demo_158.Base;
using demo_158.Hubs;
using demo_158.MVVM.Model;
using demo_158.Repository;
using demo_158.Services;
using demo_158.Services.Enums;
using demo_158.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using WebSocketSharp;


namespace demo_158.MVVM.ViewModel
{
   public class ProfileViewModel :ViewModelBase
    {
        private readonly ConnectionManager _connectionManager;
        private readonly MyInformationRepository _myInfoRepo;
        private ICommand changeProfileImage;
        private readonly ICommand saveChanges;
        private byte[] _image;
        private UserModelFromServer? _myUserInfo;

        public EventHandler ProfileSuccessChange;

        public byte[] Image
        {
            get => _image;
            set => SetField(ref _image, value);
        }

        public UserModelFromServer? MyUserInfo
        {
            get => _myUserInfo;
            set => SetField(ref _myUserInfo, value);
        }

        public ProfileViewModel(ConnectionManager connectionManager,MyInformationRepository myInfoRepo)
        {
            _connectionManager = connectionManager;
            _myInfoRepo = myInfoRepo;
            MyUserInfo = _myInfoRepo.MyUserInfo;
            Image = _myInfoRepo.MyUserInfo.Image;
            ChangeProfile();
        }

        public ICommand SaveChanges => saveChanges ?? new GeneralCommand(async ()=>await SaveChangesExecuteAction());

        public ICommand ChangeProfileImage => changeProfileImage ?? new GeneralCommand(async () => await ChangeProfileImageExecute());

        public async Task ChangeProfileImageExecute()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images (*.jpg;*.png)|*.jpg;*.png";
            openFileDialog.Title = "Profile Image";
            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var imageBytes = await File.ReadAllBytesAsync(filePath);

                // ارسال تصویر به سرور
                var imageSend = await _connectionManager.InvokeAsync("UploadProfileImage", imageBytes, MyUserInfo.Id);

                if (imageSend == ServerAnswer.bad)
                    return;
                _myInfoRepo.MyUserInfo.Image = imageBytes;
                Image = imageBytes;
            }

        }

        private async Task SaveChangesExecuteAction()
       {
           var model = new ProfileEditModel()
           {
               Bio = MyUserInfo?.BioCaption,
               Id = MyUserInfo.Id,
               Email = MyUserInfo.Email,
               Username = MyUserInfo.Username,
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
