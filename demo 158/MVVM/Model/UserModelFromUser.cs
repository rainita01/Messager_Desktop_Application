using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_158.MVVM.Model
{
    public class UserModelFromUser
    {
      
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

    }
    public class UserModelFromServer
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? BioCaption { get; set; }
        public byte[] Image { get; set; }
        public string Email { get; set; }

    }
}
