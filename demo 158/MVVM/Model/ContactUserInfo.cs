using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_158.MVVM.Model
{ public class ContactUserInfo
    {
        public int Id { get; set; }
        public string ContactUsername { get; set; }
        public byte[] ContactImage { get; set; }
        public List<int> UsersId { get; set; }
    }
}
