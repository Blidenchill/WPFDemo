using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpSetPasswordModel
    {
        public string userID { get; set; }

        public string password { get; set; }
        public string type { get; set; }
        public string oldPassword { get; set; }
    }
}
