using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpRelation
    {
        
        public string remark { get; set; }
        
        public int sourceIdentity { get; set; }
        
        public int middleID { get; set; }
        
        public int targetID { get; set; }
        
        public DateTime updateTime { get; set; }
        
        public int sourceID { get; set; }
        
        public int relationID { get; set; }
        
        public string relationSign { get; set; }
        
        public DateTime createTime { get; set; }
    }

    public class HttpCollect
    {

        public HttpRelation relation { get; set; }
        public HttpResumeTable resume { get; set; }

        public HttpJobCmb job { get; set; }
        public HttpFlow flow { get; set; }
    }
    public class HttpCollectData
    {

        public List<HttpCollect> result { get; set; }

        public int total { get; set; }
    }
    public class HttpFlow
    {
        
        public string remark { get; set; }
        
        public int viewContact { get; set; }
        
        public int deliveryID { get; set; }
        
        public DateTime updateTime { get; set; }
        
        public int jobID { get; set; }

        public int hrUserID { get; set; }
        
        public List<HttpResumTags> userSignList { get; set; }
        
        public int jhUserID { get; set; }
        
        public int type { get; set; }
        
        public DateTime createTime { get; set; }
    }

    public class HttpCollectViewRed
    {
        public string collectJobReverse_redis_count { get; set; }
        public bool viewJobReverse { get; set; }
        public int collectJobReverse_count { get; set; }
        public int viewJobReverse_count { get; set; }
        public string viewJobReverse_redis_count { get; set; }
        public bool collectJobReverse { get; set; }
    }
}
