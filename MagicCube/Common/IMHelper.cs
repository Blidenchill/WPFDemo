using MagicCube.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MagicCube.Common
{
    public static class IMHelper
    {

        public async static Task<BaseHttpModel<HttpMessage>> SendMessage(long UserID, string message,string type = "1")
        {
            HttpSendMessageParams pParams = new HttpSendMessageParams();
            pParams.fromUserID = MagicGlobal.UserInfo.Id;
            pParams.fromUserIdentity = "2"; 
            pParams.toUserID = UserID;
            //pParams.toUserID = 287;
            pParams.toUserIdentity = "1";
            pParams.platform = DAL.ConfUtil.platform;
            pParams.content = message;
            pParams.clientVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            pParams.pathID = "1";
            pParams.type = type;
            pParams.ip = GetIP();
            string jsonStr = DAL.JsonHelper.ToJsonString(pParams);
            string jsonResult = await DAL.HttpHelper.Instance.HttpPostAsync(string.Format(DAL.ConfUtil.AddrSendMessage, MagicGlobal.UserInfo.Version), jsonStr);
            BaseHttpModel<HttpMessage> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpMessage>>(jsonResult);
            return model;
        }
        public  static HttpSendMessageParams GetMessageModel(long UserID,long jobID,string content)
        {
            HttpSendMessageParams pParams = new HttpSendMessageParams();
            pParams.fromUserID = MagicGlobal.UserInfo.Id;
            pParams.toUserID = UserID;
            pParams.pathID = "1";
            pParams.content = content;
            pParams.jobID = jobID.ToString();
            return pParams;
        }
        public async static Task<BaseHttpModel> BatchSend(List<HttpSendMessageParams> lstParams)
        {

            string jsonStr = DAL.JsonHelper.ToJsonString(lstParams);
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrBatchInvite, MagicGlobal.UserInfo.Version, jsonStr));
            BaseHttpModel model = DAL.JsonHelper.ToObject<BaseHttpModel>(jsonResult);
            return model;
        }

        public async static Task<BaseHttpModel<HttpRecentList>> GetSesionInfo(long UserID,string jobID = null)
        {
            HttpGetSessionParams pParams = new HttpGetSessionParams();
            pParams.userID = MagicGlobal.UserInfo.Id;
            pParams.userIdentity = 2;
            pParams.anotherUserID = UserID;
            pParams.anotherUserIdentity = 1;
            pParams.jobID = jobID;
            string jsonStr = DAL.JsonHelper.ToJsonString(pParams);
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrMessageSessionGet, MagicGlobal.UserInfo.Version, jsonStr));
            BaseHttpModel<HttpRecentList> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpRecentList>>(jsonResult);
            return model;
        }
        public  static string GetIP()
        {
            string AddressIP = string.Empty;
            foreach (System.Net.IPAddress _IPAddress in System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }

        public async static Task<BaseHttpModel<HttpMessageRoot>> GetIMRecord(long UserID,long jobID,string sessionID=null)
        {
            HttpMessageListParams pParams = new HttpMessageListParams();
            pParams.userID = MagicGlobal.UserInfo.Id;
            pParams.userIdentity = 2;
            pParams.sessionID = sessionID;
            pParams.start = null;
            pParams.size = 50;
            pParams.type = null;
            pParams.anotherUserID = UserID;
            pParams.anotherUserIdentity = 1;
            pParams.jobID = jobID;
            string jsonStr = DAL.JsonHelper.ToJsonString(pParams);
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrMessageList,  MagicGlobal.UserInfo.Version, jsonStr));
            MagicCube.ViewModel.BaseHttpModel<HttpMessageRoot> model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel<HttpMessageRoot>>(jsonResult);
            return model;
        }

        public async static Task<BaseHttpModel> CancelCollectResume(long UserID)
        {
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "sourceID", "targetID", "middleID" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), MagicGlobal.UserInfo.Id.ToString(), UserID.ToString(), "0" });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrCancelCollectResume, MagicGlobal.UserInfo.Version, std));
            MagicCube.ViewModel.BaseHttpModel model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel>(jsonResult);
            return model;
        }

        public async static Task<BaseHttpModel<HttpCollectionCount>> CollectResume(long UserID)
        {
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "sourceID", "targetID", "middleID" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), MagicGlobal.UserInfo.Id.ToString(), UserID.ToString(), "0" });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrCollectResume, MagicGlobal.UserInfo.Version, std));
            MagicCube.ViewModel.BaseHttpModel<HttpCollectionCount> model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel<HttpCollectionCount>>(jsonResult);
            return model;
        }

        public async static Task<BaseHttpModel<HttpPhoneRoot>> FreePhone(long UserID)
        {
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "fromUserID", "toUserID", "purposeCode", "toUserIdentity" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), UserID.ToString(), "1000","1" });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrFreePhone, MagicGlobal.UserInfo.Version, std));
            BaseHttpModel<HttpPhoneRoot> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpPhoneRoot>>(jsonResult);
            return model;

        }
        public async static Task<BaseHttpModel<HttpMobileRoot>> ViewContact(long UserID)
        {
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID","sourceID", "targetID", "middleID", "relationSign", "sourceIdentity" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), MagicGlobal.UserInfo.Id.ToString(), UserID.ToString(), MagicGlobal.UserInfo.CompanyId.ToString(), "viewContact","2" });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrViewCantect, MagicGlobal.UserInfo.Version, std));
            BaseHttpModel<HttpMobileRoot> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpMobileRoot>>(jsonResult);
            return model;

        }


        public async static Task<BaseHttpModel<HttpResumeDetial>> GetDetialReusme(string UserID)
        {
            string propertys = ModelTools.SetHttpPropertys<HttpResumeDetial>();
            string jsonStr = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "sourceID", "resumeID", "targetID", "properties", "desc", "companyID" },
            new string[] { MagicGlobal.UserInfo.Id.ToString(), MagicGlobal.UserInfo.Id.ToString(), UserID, UserID, propertys, "fullName", MagicGlobal.UserInfo.CompanyId.ToString() });

            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrGetResumeDetialInfo, MagicGlobal.UserInfo.Version, jsonStr));
            BaseHttpModel<HttpResumeDetial> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpResumeDetial>>(resultStr);
            return model;

        }
       
    }
}
