using MagicCube.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class HttpChat
    {
        public HttpContactRecord contactRecord
        {
            get;
            set;
        }
        public HttpChatResume resume
        {
            get;
            set;
        }
        public string jobName
        {
            get;
            set;
        }
    }

    public class HttpChatResume
    {
        public string workingExp
        {
            get;
            set;
        }
        public string name
        {
            get;
            set;
        }

        public string degree
        {
            get;
            set;
        }


        public string avatarUrl
        {
            get;
            set;
        }

        public string _age;
        public string age
        {
            get
            {
                return TimeHelper.getAge(dob);
            }
            set
            {
                _age = value;
            }
        }

        public string dob
        {
            get;
            set;
        }
        public string gender
        {
            get;
            set;
        }

        
    }

    public class HttpToken
    {
        public string token
        {
            get;
            set;
        }

    }
}
