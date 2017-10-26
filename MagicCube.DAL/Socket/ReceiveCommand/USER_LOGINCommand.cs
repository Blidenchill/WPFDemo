using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicCube.DAL
{
    public class USER_LOGINCommand : IReceiveCommand<PushPackageInfo>
    {
        public async Task CommandCallback(PushPackageInfo package)
        {
            Console.WriteLine("USER_LOGINCommand");
        }
    }
}
