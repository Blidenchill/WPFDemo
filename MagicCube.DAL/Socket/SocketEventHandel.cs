using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicCube.DAL
{
    public static class SocketEventHandel
    {
        public delegate Task SocketEventDelegate(string data);

        public static SocketEventDelegate PushMessageEventHandle;
    }
}
