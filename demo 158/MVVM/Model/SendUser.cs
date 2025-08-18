using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_158.MVVM.Model
{
    public class SendUser
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string BioCaption { get; set; }
        public string Image { get; set; }

    }
    public class ReceiveUser
    {
        public int Id { get; set; } 
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime RegisterDate { get; set; }
        public string? BioCaption { get; set; }
        public string Image { get; set; }

    }
}
