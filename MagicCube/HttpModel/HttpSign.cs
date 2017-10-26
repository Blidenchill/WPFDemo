using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class HttpSign
    {

        public string imName
        {
            get;
            set;
        }
        public string imPassword
        {
            get;
            set;
        }
        public string integral
        {
            get;
            set;
        }
        public string realName
        {
            get;
            set;
        }

        public string companyName
        {
            get;
            set;
        }

        public string MBBalance
        {
            get;
            set;
        }

        public string email
        {
            get;
            set;
        }
        public string avatarUrl
        {
            get;
            set;
        }
        public string areaCode
        {
            get;
            set;
        }
        public string officePhone
        {
            get;
            set;
        }
        public string extensionNumber
        {
            get;
            set;
        }
        public string briefName
        {
            get;
            set;
        }

        public int id { get; set; }

        public string nonce
        {
            get;
            set;
        }
        public int? companyId { get; set; }

        public int? awardAmount
        {
            get;
            set;
        }

        public bool firstLogin
        {
            get;
            set;
        }
        public bool awardAction
        {
            get;
            set;
        }

        public int? continuousSignIn
        {
            get;
            set;
        }

        public string validStatus
        {
            get;
            set;
        }
        public string position
        {
            get;
            set;
        }

    }
    public class HttpSignAward
    {
        public int? continuousSignIn
        {
            get;
            set;
        }

        public string validStatus
        {
            get;
            set;
        }
    }

    public class HttpSignFirst
    {
        public int? awardAmount
        {
            get;
            set;
        }

        public bool firstLogin
        {
            get;
            set;
        }
        public bool awardAction
        {
            get;
            set;
        }
    }
}
