using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class HttpSelfInfo
    {
        public string realName { get; set; }
        public string avatarUrl { get; set; }
        public string officePhone { get; set; }
        public string extensionNumber { get; set; }
        public string areaCode { get; set; }
        public string email { get; set; }
        public string position { get; set; }
        public List<ThirdAccount> thirdAccounts { get; set; }
        public bool isAccountActivate { get; set; }
    }

    public class ThirdAccount
    {
        public string sourceType { get; set; }
        public string accountName { get; set; }
        public string password { get; set; }
    }

    public class HttpReturnUploadPictureUrl
    {
        public string error { get; set; }
        public int status { get; set; }
        public string url { get; set; }
    }



    public class ContactTimeInterval
    {
        public string to { get; set; }
        public string from { get; set; }
        public List<int> days { get; set; }
    }

   

    public class SelfInfoRetunResult
    {
        public ContactTimeInterval contactTimeInterval { get; set; }
        public int successiveLoginDays { get; set; }
        public string ownerType { get; set; }
        public bool activeStatus { get; set; }
        public bool applyStatus { get; set; }
        public string lastLoginDate { get; set; }
        public int owner { get; set; }
        public int continuousSignIn { get; set; }
        public int id { get; set; }
        public bool notificationEnabled { get; set; }
        public string realName { get; set; }
        public int companyId { get; set; }
        public string areaCode { get; set; }
        public string extensionNumber { get; set; }
        public int version { get; set; }
        public string activeType { get; set; }
        public int totalSignIn { get; set; }
        public string email { get; set; }
        public string officePhone { get; set; }
        public string imName { get; set; }
        public List<ThirdAccount> thirdAccounts { get; set; }
        public bool deleted { get; set; }
        public string emailSuffix { get; set; }
        public bool signInStatus { get; set; }
        public string password { get; set; }
        public List<string> signInDates { get; set; }
        public string updatedTime { get; set; }
        public string mobile { get; set; }
        public bool emailVerified { get; set; }
        public string avatarUrl { get; set; }
        public string signInDate { get; set; }
        public string createdTime { get; set; }
        public string imPassword { get; set; }
    }

    public class HttpSelfInfoReturn
    {
        public int status { get; set; }
        public SelfInfoRetunResult result { get; set; }
    }

}
