using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class HttpModifyPassword
    {
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
