using MagicCube.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MagicCube.Common
{
    public static class VideoHelper
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        public static string UserSig = string.Empty;
        public static string RelationId = string.Empty;
        public static void writeIni()
        {
            string fileCon = DAL.ConfUtil.LocalHomePath + "ConfigInfo.ini";
            if (File.Exists(fileCon))
            {
                File.Delete(fileCon);
            }
            WritePrivateProfileString("ConfigInfo", "AccountType", "8913", fileCon);
            WritePrivateProfileString("ConfigInfo", "AppIdAt3rd", "1400018838", fileCon);
            WritePrivateProfileString("ConfigInfo", "SdkAppId", "1400018838", fileCon);
            WritePrivateProfileString("ConfigInfo", "RelationId", RelationId, fileCon);
            WritePrivateProfileString("ConfigInfo", "Identifier", MagicGlobal.UserInfo.Id.ToString(), fileCon);
            WritePrivateProfileString("ConfigInfo", "UserSig", UserSig, fileCon);
        }

        public async static Task<bool> GetVideoSign()
        {
            string jsonStr = DAL.JsonHelper.JsonParamsToString(new string[] { "userID" }, new string[] { MagicGlobal.UserInfo.Id.ToString() });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrVideoSign, MagicGlobal.UserInfo.Version, jsonStr));
            BaseHttpModel<HttpVideoSig> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpVideoSig>>(jsonResult);
            if(model !=null)
            {
                if(model.code==200)
                {
                    UserSig = model.data.sig;
                    return true;
                }
            }
            return false;
        }
        public async static Task<bool> VideoInvite(long userID,string jobID = null)
        {
            HttpVideoInviteParams pParams = new HttpVideoInviteParams();
            pParams.fromUserID = MagicGlobal.UserInfo.Id;
            pParams.fromUserIdentity = 2;
            pParams.toUserID = userID;
            pParams.toUserIdentity = 1;
            pParams.jobID = jobID;
            string jsonStr = DAL.JsonHelper.ToJsonString(pParams);
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrVideoInvite, MagicGlobal.UserInfo.Version, jsonStr));
            BaseHttpModel<HttpVideoInvite> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpVideoInvite>>(jsonResult);
            if (model != null)
            {
                if (model.code == 200)
                {
                    RelationId = model.data.roomID;
                    return true;
                }
            }
            return false;
        }
        
    }
}
