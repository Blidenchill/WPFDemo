using MagicCube.HttpModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MagicCube.Common
{
    public static class JudgeHelper
    {
        public async static Task<string> ResumeJudge(int id, bool type)
        {
            string strtype = null;
            if (type)
                strtype = "3";
            else
                strtype = "4";
            string std = DAL.JsonHelper.JsonParamsToString(new string[] {"userID", "deliveryID", "type" }, new string[] {MagicGlobal.UserInfo.Id.ToString(), id.ToString(), strtype });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrResumeConfirm, MagicGlobal.UserInfo.Version, std));
            MagicCube.ViewModel.BaseHttpModel model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel>(jsonResult);
            if (model == null)
                return DAL.ConfUtil.netError;
            if (model.code != 200)
                return model.msg;       
            return DAL.ConfUtil.judgeSucess;
        }

        public async static Task<bool> ResumeSign(int id,int sign)
        {

            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "deliveryID", "userSignID" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), id.ToString(),sign.ToString() });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrResumeSign, MagicGlobal.UserInfo.Version, std));
            MagicCube.ViewModel.BaseHttpModel model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel>(jsonResult);
            if (model != null)
            {
                if (model.code == 200)
                    return true;
            }
            return false;
        }
        public async static Task<bool> ResumeUnSign(int id, int sign)
        {

            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "deliveryID", "userSignID" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), id.ToString(), sign.ToString() });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrResumeUnSign, MagicGlobal.UserInfo.Version, std));
            MagicCube.ViewModel.BaseHttpModel model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel>(jsonResult);
            if (model != null)
            {
                if (model.code == 200)
                    return true;
            }
            return false;
        }
    }
}
