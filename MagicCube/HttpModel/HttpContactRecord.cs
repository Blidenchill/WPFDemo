using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{

    public class HttpContactRecord
    {
        //{"updatedTime": "2016-03-24T02:16:32Z", "initiator": "MOFANG_USER", "enterpriseAccountId": 93, "deleted": false, 
        //"userId": 30109, "systemFiltrateStatus": "\u673a\u7b5b\u901a\u8fc7", "ownerType": "ENTERPRISE_USER", "jobId": 1856, 
        //"createdTime": "2016-03-22T11:23:31Z", "resumeId": 26752, "owner": 93, "userDeleted": false, "evaluation": "\u5408\u9002", "id": 19724}

        //简历来源
        public string initiator
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

        public int id
        {
            get;
            set;
        }

        public string evaluation
        {
            get;
            set;
        }


        public string userMobile
        {
            get;
            set;
        }

        public string uniqueKey
        {
            get;
            set;
        }

        public string userId
        {
            get;
            set;
        }
        public string createdTime
        {
            get;
            set;
        }
    }
}
