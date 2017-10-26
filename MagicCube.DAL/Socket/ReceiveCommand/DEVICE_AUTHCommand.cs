using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicCube.DAL
{
    public class DEVICE_AUTHCommand : IReceiveCommand<PushPackageInfo>
    {
        public async  Task CommandCallback(PushPackageInfo data)
        {
            Console.WriteLine("DEVICE_AUTHCommand");
            string str = data.Body;
            ViewModel.HttpSocketBaseModel<ViewModel.HttpSocketRegistModel> model = JsonHelper.ToObject<ViewModel.HttpSocketBaseModel<ViewModel.HttpSocketRegistModel>>(str);
            if(model != null)
            {
                if(model.status != 200)
                {
                    SocketHelper.Instance.DeviceID = string.Empty;
                }
            }
        }
    }
}
