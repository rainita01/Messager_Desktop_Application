using demo_158.Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_158.MVVM.Model
{ public class ContactUserModel
    {
        public int Id { get; set; }
        public string ContactUsername { get; set; }
        public byte[] ContactImage { get; set; }
        public string? Bio { get; set; }
        public string Email { get; set; }
        public List<int> UsersId { get; set; }
        public State State { get; set; }
        public DateTime LastActiveTime { get; set; }
    }
}
