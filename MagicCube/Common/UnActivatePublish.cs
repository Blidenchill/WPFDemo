using MagicCube.HttpModel;
using MagicCube.TemplateUC;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MagicCube.Common
{
    public static class UnActivatePublishHelper
    {

        public async static Task<int> UnActivatePublish()
        {
            List<string> key = new List<string> { "userID", "companyID", "isHRAuth", "isSimilarAuth" };
            List<string> value = new List<string> { MagicGlobal.UserInfo.Id.ToString(), MagicGlobal.UserInfo.CompanyId.ToString(), MagicGlobal.UserInfo.IsHRAuth.ToString(), MagicGlobal.UserInfo.IsSimilarAuth.ToString() };
            string std = DAL.JsonHelper.JsonParamsToString(key, value);
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobLimit, MagicGlobal.UserInfo.Version, std));
            MagicCube.ViewModel.BaseHttpModel model = DAL.JsonHelper.ToObject<MagicCube.ViewModel.BaseHttpModel>(jsonResult);
            Console.WriteLine(jsonResult);
            if (model != null)
            {
                return model.code;
            }
            return 0;
        }
    }



}
