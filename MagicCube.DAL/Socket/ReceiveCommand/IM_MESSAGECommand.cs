using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicCube.DAL
{
    public class IM_MESSAGECommand : IReceiveCommand<PushPackageInfo>
    {
        public async Task CommandCallback(PushPackageInfo package)
        {
            Console.WriteLine("IM_MESSAGECommand");
            if(package!=null)
            {
                if (SocketEventHandel.PushMessageEventHandle != null)
                {
                    ViewModel.HttpSocketBaseModel model = DAL.JsonHelper.ToObject<ViewModel.HttpSocketBaseModel>(package.Body);
                    await SocketEventHandel.PushMessageEventHandle(model.data.ToString());
                }
                    
            }
        }
    }
}
