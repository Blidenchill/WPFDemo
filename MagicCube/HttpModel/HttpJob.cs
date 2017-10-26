using MagicCube.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{

    public class HttpJob
    {
        public int id
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

    public class HttpJobResume
    {
        public int recordId
        {
            get;
            set;
        }
        public int jobId
        {
            get;
            set;
        }
        public string jobName
        {
            get;
            set;
        }

        public string resumeId
        {
            get;
            set;
        }

        public string name
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


        public string workingExp
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

    }
}
