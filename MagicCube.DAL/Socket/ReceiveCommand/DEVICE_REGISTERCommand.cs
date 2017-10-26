using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicCube.DAL
{
    public class DEVICE_REGISTERCommand : IReceiveCommand<PushPackageInfo>
    {
        public async Task CommandCallback(PushPackageInfo package)
        {
            Console.WriteLine("DEVICE_REGISTERCommand");

            ViewModel.HttpSocketBaseModel<ViewModel.HttpSocketRegistModel> model = DAL.JsonHelper.ToObject<ViewModel.HttpSocketBaseModel<ViewModel.HttpSocketRegistModel>>(package.Body);
            if(model != null)
            {
                if(model.status == 200)
                {
                    SocketHelper.Instance.DeviceID = model.data.deviceID;
                    SocketHelper.Instance.SecureKey = model.data.secureKey;
                }
            }
            
        }
    }
}
