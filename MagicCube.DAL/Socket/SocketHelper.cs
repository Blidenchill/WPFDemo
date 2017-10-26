using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.ProtoBase;
using SuperSocket.ClientEngine;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography;
using System.Reflection;

namespace MagicCube.DAL
{
    public class SocketHelper
    {
        #region "单例"
        private static SocketHelper instance;
        private static object objLock = new object();
        public static SocketHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objLock)
                    {
                        if (instance == null)
                        {
                            instance = new SocketHelper();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region "变量"
        private EasyClient<PushPackageInfo> client;
        private string ipAddress = "moxin.service.mofanghr.com";
        //private string ipAddress = "10.0.3.203";
        private int port = 6211;
        private bool connected = false;

        public int heartbeatInterval = 30;
        public System.Timers.Timer timeAlive;
        private double interval ;
        private object objLockConnect = new object();

        public Action LoginAction;
        #endregion

        #region "属性"
        private string deviceID = string.Empty;
        public string DeviceID
        {
            get { return deviceID; }
            set { deviceID = value; }
        }

        private string secureKey = string.Empty;
        public string SecureKey
        {
            get { return secureKey; }
            set { secureKey = value; }
        }
        
        #endregion

        #region "构造函数"
        private SocketHelper()
        {
            client = new EasyClient<PushPackageInfo>();
            client.Initialize(new PushReceiveFilter());
            client.Connected += OnClientConnected;
            client.NewPackageReceived += OnPackageReceived;
            client.Error += OnClientError;
            client.Closed += OnClientClosed;
        }

        #endregion

        #region "方法"
        public  async Task Start()
        {
            if (!connected)
                Console.WriteLine("Socket发起一次连接");
            IPHostEntry ip = Dns.GetHostEntry(ipAddress);
            if (ip.AddressList.Length < 1)
                return;
            connected = await client.ConnectAsync(new IPEndPoint(ip.AddressList[0], port));

            //connected = await client.ConnectAsync(new IPEndPoint(IPAddress.Parse(ipAddress), port));




            if (!connected)
            {
                await TaskEx.Delay(5000);
                await Start();
            }
            else
            {
                this.interval = this.heartbeatInterval * 1000;
                //启动心跳
                timeAlive = new System.Timers.Timer(this.interval);
                timeAlive.Elapsed += timeAlive_Elapsed;
                timeAlive.Enabled = true;
                //发送心跳初始化请求
                string heartBeatStr = JsonHelper.JsonParamsToString(new string[] { "cmd", "data" }, new string[] { "HEARTBEAT_INIT", "" });
                this.Send(heartBeatStr);

                if (string.IsNullOrEmpty(this.deviceID))
                {
                    //获取网卡Id
                    //获取网卡Mac
                    string IMEI = string.Empty;
                    System.Management.ManagementObjectSearcher query = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
                    System.Management.ManagementObjectCollection queryCollection = query.Get();
                    foreach (System.Management.ManagementObject mo in queryCollection)
                    {
                        if (mo["IPEnabled"].ToString() == "True")
                            IMEI = mo["MacAddress"].ToString();
                    }

                    //注册
                    ViewModel.HttpSocketBaseModel<ViewModel.HttpSocketRegistModel> model = new ViewModel.HttpSocketBaseModel<ViewModel.HttpSocketRegistModel>();
                    model.data = new ViewModel.HttpSocketRegistModel();
                    model.cmd = "DEVICE_REGISTER";
                    model.data.IMEI = IMEI;
                    string str = DAL.JsonHelper.ToJsonString(model);
                    this.Send(str);
                }
                else
                {
                    //设备认证授权
                    ViewModel.HttpSocketBaseModel<ViewModel.HttpSocketRegistModel> model = new ViewModel.HttpSocketBaseModel<ViewModel.HttpSocketRegistModel>();
                    model.data = new ViewModel.HttpSocketRegistModel();
                    model.cmd = "DEVICE_AUTH";
                    model.data.secureKey = this.secureKey;
                    model.data.deviceID = this.deviceID;
                    string str = DAL.JsonHelper.ToJsonString(model);

                    this.Send(str);
                }

            }
        }

        public async Task ReStart()
        {
            //await client.Close();
        
            client = new EasyClient<PushPackageInfo>();
            client.Initialize(new PushReceiveFilter());
            client.Connected += OnClientConnected;
            client.NewPackageReceived += OnPackageReceived;
            client.Error += OnClientError;
            client.Closed += OnClientClosed;

            if (connected)
            {
                return;
            }
            IPHostEntry ip = Dns.GetHostEntry(ipAddress);
            if (ip.AddressList.Length < 1)
                return;
            connected = await client.ConnectAsync(new IPEndPoint(ip.AddressList[0], port));     
            //connected = await client.ConnectAsync(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            if (!connected)
            {
                await TaskEx.Delay(5000);
                await ReStart();
            }
            else
            {
                this.interval = this.heartbeatInterval * 1000;
                //启动心跳
                timeAlive = new System.Timers.Timer(this.interval);
                timeAlive.Elapsed += timeAlive_Elapsed;
                timeAlive.Enabled = true;
                //发送心跳初始化请求
                string heartBeatStr = JsonHelper.JsonParamsToString(new string[] { "cmd", "data" }, new string[] { "HEARTBEAT_INIT", "" });
                this.Send(heartBeatStr);
                await TaskEx.Delay(2000);
                if (string.IsNullOrEmpty(this.deviceID))
                {
                    //获取网卡Id
                    //获取网卡Mac
                    string IMEI = string.Empty;
                    System.Management.ManagementObjectSearcher query = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
                    System.Management.ManagementObjectCollection queryCollection = query.Get();
                    foreach (System.Management.ManagementObject mo in queryCollection)
                    {
                        if (mo["IPEnabled"].ToString() == "True")
                            IMEI = mo["MacAddress"].ToString();
                    }

                    //注册
                    ViewModel.HttpSocketBaseModel<ViewModel.HttpSocketRegistModel> model = new ViewModel.HttpSocketBaseModel<ViewModel.HttpSocketRegistModel>();
                    model.data = new ViewModel.HttpSocketRegistModel();
                    model.cmd = "DEVICE_REGISTER";
                    model.data.IMEI = IMEI;
                    string str = DAL.JsonHelper.ToJsonString(model);
                    this.Send(str);
                }
                else
                {
                    //设备认证授权
                    ViewModel.HttpSocketBaseModel<ViewModel.HttpSocketRegistModel> model = new ViewModel.HttpSocketBaseModel<ViewModel.HttpSocketRegistModel>();
                    model.data = new ViewModel.HttpSocketRegistModel();
                    model.cmd = "DEVICE_AUTH";
                    model.data.secureKey = this.secureKey;
                    model.data.deviceID = this.deviceID;
                    string str = DAL.JsonHelper.ToJsonString(model);

                    this.Send(str);

                }
                await TaskEx.Delay(5000);
                //login登录
                if (this.LoginAction != null)
                    this.LoginAction();
            }
        }

        public void Send(string jsonStr)
        {
            if (!this.connected)
                return;
            try
            {
                byte[] body = Encoding.UTF8.GetBytes(jsonStr);
                int len = body.Length;
                byte[] header = BitConverter.GetBytes((uint)len);
                byte[] data = new byte[header.Length + body.Length];
                for (int i = 0; i < header.Length; i++)
                {
                    data[i] = header[header.Length - 1 - i];
                }
                body.CopyTo(data, 4);
                client.Send(data);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Socket发送异常：" + ex.Message);
            }
          
        }

        public void Close()
        {
            this.client.Close();
        }

        #endregion

        #region "回调函数"
        private async void OnPackageReceived(object sender, PackageEventArgs<PushPackageInfo> e)
        {
            try
            {
                ViewModel.HttpSocketBaseModel model = DAL.JsonHelper.ToObject<ViewModel.HttpSocketBaseModel>(e.Package.Body);
                if (model == null)
                    return;
                string temp = model.cmd;
                Console.WriteLine("Socket接收：" +e.Package.Body);
                string assemblyStr = "MagicCube.DAL." + temp + "Command";
                Type type = Type.GetType(assemblyStr);
                var obj = type.Assembly.CreateInstance(assemblyStr) as IReceiveCommand<PushPackageInfo>;
                await obj.CommandCallback(e.Package);
            
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
          
        }

        private  void OnClientConnected(object sender, EventArgs e)
        {
            Console.WriteLine("已连接到服务器...");
        }

        private  async void OnClientClosed(object sender, EventArgs e)
        {
            Console.WriteLine("客户端已关闭...");
            if (this.connected == false)
                return;
            this.connected = false;
            this.timeAlive.Enabled = false;
            
            await client.Close();
            await TaskEx.Delay(10000);
            await ReStart();


        }

        private async  void OnClientError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("客户端错误：" + e.Exception.Message);
            if (this.connected == false)
                return;
            this.connected = false;
            this.timeAlive.Enabled = false;
            await TaskEx.Delay(10000);
            await ReStart();

        }


        public void timeAlive_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            string str = JsonHelper.JsonParamsToString(new string[] { "cmd", "data" }, new string[] { "HEARTBEAT", "" });
            this.Send(str);
        }
        #endregion


    }
}
