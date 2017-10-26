using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace MagicCube.Common
{
    public static class TrackHelper2
    {
        #region "变量"
        private static string TrackUpload = @"http://t.mofanghr.com/?";

        private static string originSpm = string.Empty;

        private static string accessID = string.Empty;
        private static string version = string.Empty;
        private static string uuid = string.Empty;

        public static string UserId;
        public static string UniqueKey;
        #endregion

        #region "对内函数"
        public static string MD5Encrypt(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(strText));
            string temp = string.Empty;
            for (int i = 0; i < result.Length; i++)
            {
                temp += result[i].ToString("x2");
            }
            return temp;
        }

        private static long GetTimestamp()
        {
            DateTime dt = DateTime.Now;
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (dt.Ticks - dt1970.Ticks) / 10000/1000;
        }
        #endregion

        #region "对外方法"

        public static void FirstTrackOperation(string eventType)
        {
            //Console.WriteLine("埋点" + eventType);
            //accessID生成
            string updateTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            Random random = new Random();
            int x = random.Next(0, 999999);
            accessID = updateTime + x.ToString();
            version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            try
            {
                //获取网卡Mac
                System.Management.ManagementObjectSearcher query = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
                System.Management.ManagementObjectCollection queryCollection = query.Get();
                foreach (System.Management.ManagementObject mo in queryCollection)
                {
                    if (mo["IPEnabled"].ToString() == "True")
                        uuid = mo["MacAddress"].ToString();
                }


                //System.Management.ManagementClass mc = new System.Management.ManagementClass("Win32_NetworkAdapterConfiguration");
                //System.Management.ManagementObjectCollection moc = mc.GetInstances();
                //foreach (System.Management.ManagementObject mo in moc)
                //{
                //    uuid = mo.Properties["Model"].Value.ToString();
                //    if ((bool)mo["IPEnabled"] == true)
                //    {
                //        uuid = mo["MacAddress"].ToString();
                //        break;
                //    }
                //}
            }
            catch
            {
                //Console.WriteLine(ex.Message);
            }
            
            string url = TrackUpload;
            url += "channel=" + "";
            url += "&accessID=" + accessID;
            url += "&spm=5"; 
            url += "&eventType=" + eventType;
            url += "&version=" + version;
            url += "&deviceID=" + uuid;
            url += "&osVersion=" + SystemInfoHelper.GetSystemInfo();
            url += "&clientTimestamp=" + GetTimestamp().ToString();
            Action method = delegate
            {
                //string temp = MagicCube.Common.HttpHelper.HttpGet(url);
                MagicCube.Common.HttpHelper.TrackHttp(url);
            };
            method.BeginInvoke(null, null);
        }

        public static void TrackOperation(string spm, string eventType)
        {
            string url = TrackUpload;
            url += "channel=" + "";
            if (!string.IsNullOrEmpty(UserId))
            {
                url += "&userID=" + UserId;
            }
            if (!string.IsNullOrEmpty(UniqueKey))
            {
                url += "&uniqueKey=" + UniqueKey;
            }
            url += "&accessID=" + accessID;
            url += "&eventType=" + eventType;
            url += "&spm=" + spm;
            url += "&version=" + version;
            url += "&deviceID=" + uuid;
            url += "&osVersion=" + SystemInfoHelper.GetSystemInfo();
            url += "&clientTimestamp=" + GetTimestamp().ToString();
            if (!string.IsNullOrEmpty(originSpm))
            {
                url += "&origin=" + originSpm;
            }
            originSpm = spm;
            Console.WriteLine("SPM:" + url);
            Action method = delegate
            {
                //string temp = MagicCube.Common.HttpHelper.HttpGet(url);
                MagicCube.Common.HttpHelper.TrackHttp(url);
            };
            method.BeginInvoke(null, null);
        }


        public static void TrackOperation(string spm, string eventType, string eventDate)
        {
            string url = TrackUpload;
            url += "channel=" + "";
            if(!string.IsNullOrEmpty(UserId))
            {
                url += "&userID=" + UserId;
            }
            if(!string.IsNullOrEmpty(UniqueKey))
            {
                url += "&uniqueKey=" + UniqueKey;
            }
          
            url += "&accessID=" + accessID;
            url += "&eventType=" + eventType;
            url += "&eventData=" + eventDate;
            url += "&spm=" + spm;
            url += "&version=" + version;
            url += "&deviceID=" + uuid;
            url += "&osVersion=" + SystemInfoHelper.GetSystemInfo();
            url += "&clientTimestamp=" + GetTimestamp().ToString();
            if (!string.IsNullOrEmpty(originSpm))
            {
                url += "&origin=" + originSpm;
            }
            originSpm = spm;
            //Console.WriteLine("SPM:" + url);
            Action method = delegate
            {
                //string temp = MagicCube.Common.HttpHelper.HttpGet(url);
                MagicCube.Common.HttpHelper.TrackHttp(url);
            };
            method.BeginInvoke(null, null);
        }

        public static void TrackOperation(string eventType)
        {
            Console.WriteLine("埋点：唤醒成功一次");
            string url = TrackUpload;
            url += "channel=" + "";
            url += "&accessID=" + accessID;
            url += "&spm=5";
            url += "&eventType=" + eventType;
            url += "&version=" + version;
            url += "&deviceID=" + uuid;
            url += "&osVersion=" + SystemInfoHelper.GetSystemInfo();
            url += "&clientTimestamp=" + GetTimestamp().ToString();
            if (!string.IsNullOrEmpty(originSpm))
            {
                url += "&origin=" + originSpm;
            }
            Action method = delegate
            {
                //string temp = MagicCube.Common.HttpHelper.HttpGet(url);
                MagicCube.Common.HttpHelper.TrackHttp(url);
            };
            method.BeginInvoke(null, null);
        }

        #endregion



    }
}
