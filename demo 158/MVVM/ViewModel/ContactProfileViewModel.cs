using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demo_158.Base;

namespace demo_158.MVVM.ViewModel
{
    public class ContactProfileViewModel :ViewModelBase
    {
        private string _image;
        private string _username;
        private string _email;
        private string? _bio;

        public string Image
        {
            get => _image;
            set => SetField(ref _image, value);
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
    }
}
