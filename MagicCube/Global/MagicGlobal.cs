using MagicCube.Common;
using MagicCube.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using MagicCube.HttpModel;
using System.ComponentModel;
using System.IO;

namespace MagicCube
{
    public static class MagicGlobal
    {
        public static bool isResumeJudege = false;
        public static bool isJobDetialEdit = false;
        public static bool isJobTableEdit = false;
        /// <summary>
        /// 账号是否认证
        /// </summary>
        public static bool isHRAuth = false;
        public static bool isCompanyInfoComplete = false;
        public static bool HasJobPublish = false;
        public static bool HasJobOpen = false;
        public static bool isAccountBindZhilian = false;
        public static string GNonce = string.Empty;
        public static UserSetting UserInfo;
        public static string token;
        public static ObservableCollection<SaveSearchCondition> saveSearchConditionList = new ObservableCollection<SaveSearchCondition>();
        public static CurrentUserInfo currentUserInfo;
        public static ObservableCollection<TagsManagerModel> TagsManagerList = new ObservableCollection<TagsManagerModel>();
        public static ObservableCollection<ContentCommonReply> ContentCommonReplyList = new ObservableCollection<ContentCommonReply>();
        public static DateTime trackDateTime;
        public static List<string> GLimit = new List<string>();
        public static void SaveSeting()
        {
            if(!UserInfo.RememberPassword)
            {
                UserInfo.Password = null;
            }
            XmlClassData.WriteDataToXml(UserInfo, DAL.ConfUtil.LocalHomePath + "userinfo.db");
            if (!Directory.Exists(DAL.ConfUtil.LocalHomePath + MagicGlobal.UserInfo.UserAccount))
            {
                Directory.CreateDirectory(DAL.ConfUtil.LocalHomePath + MagicGlobal.UserInfo.UserAccount);
            }
            XmlClassData.WriteDataToXml(currentUserInfo, DAL.ConfUtil.LocalHomePath + MagicGlobal.UserInfo.UserAccount + "//" + "CurrentUserInfo.xml");
                
        }



    }
}
