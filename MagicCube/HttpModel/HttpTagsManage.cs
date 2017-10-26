using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class HttpTagsManage
    {
        public int status { get; set; }
        public List<HttpTagsItem> result { get; set; }
    }
    public class HttpTagsItem
    {
        public string color { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }
    public class HttpTagsItemResult
    {
        public int status { get; set; }
        public HttpTagsItem result { get; set; }
    }

    public class TagV
    {
        public string color { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    //public class TagsHashtable
    //{
    //    //tagsId
    //    public string __invalid_name__3 { get; set; }
    //    //忽略
    //    public string __invalid_name__4 { get; set; }
    //}

    public class HttpResumeTagsResult
    {
        public List<TagV> tagVs { get; set; }
        public string updatedTime { get; set; }
        public string type { get; set; }
        public int id { get; set; }
        public Dictionary<string, string> tags { get; set; }
        public string flag { get; set; }
    }

    public class HttpResumeTags
    {
        public int status { get; set; }
        public HttpResumeTagsResult result { get; set; }
    }

    public class HttpResumeGetTags
    {
        public string uniqueKey { get; set; }
        public int resumeId { get; set; }
        public Dictionary<string, string> tags { get; set; }
        public string flag { get; set; }
    }


}
