using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demo_158.Base;
using demo_158.MVVM.Model;

namespace demo_158.MVVM.ViewModel
{
    public class ContactProfileViewModel :ViewModelBase
    {
        private ContactUserModel _profile;

        public ContactUserModel Profile     
        {
            get => _profile;
            set => SetField(ref _profile, value);
        }
    }
}
