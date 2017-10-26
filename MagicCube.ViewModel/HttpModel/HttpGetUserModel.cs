using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpGetUserModel
    {
        public string lastLoginIdentity { get; set; }
        public string account { get; set; }
        public string hrLastLoginMethod { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string userID { get; set; }
        public string hrLastLoginPlatform { get; set; }
        public string isSimilarAuth { get; set; }
        public string hrLastLoginTime { get; set; }
        public string accountStatus { get; set; }
        public string lastLoginPlatform { get; set; }
        public string updateTime { get; set; }
        public string regTime { get; set; }
        public bool existPwd { get; set; }
        public string lastLoginMethod { get; set; }
        public string lastLoginTime { get; set; }
        public string createTime { get; set; }
    }
}
