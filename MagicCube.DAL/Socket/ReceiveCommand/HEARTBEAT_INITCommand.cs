using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicCube.DAL
{
    public class HEARTBEAT_INITCommand: IReceiveCommand<PushPackageInfo>
    {
        public async Task CommandCallback(PushPackageInfo package)
        {
            Console.WriteLine("HEARTBEAT_INITCommand");
            string str = package.Body;
            ViewModel.HttpSocketBaseModel<ViewModel.HttpSocketHeartbeatModel> model = JsonHelper.ToObject<ViewModel.HttpSocketBaseModel<ViewModel.HttpSocketHeartbeatModel>>(str);
            if(model != null)
            {
                int beat = Convert.ToInt32(model.data.heartBeat);
                if (SocketHelper.Instance.heartbeatInterval == beat)
                    return;
                SocketHelper.Instance.heartbeatInterval = beat;
                //启动心跳
                SocketHelper.Instance.timeAlive = new System.Timers.Timer(SocketHelper.Instance.heartbeatInterval * 1000);
                SocketHelper.Instance.timeAlive.Elapsed += SocketHelper.Instance.timeAlive_Elapsed;
                SocketHelper.Instance.timeAlive.Enabled = true;
            }

         
        }
    }
}
