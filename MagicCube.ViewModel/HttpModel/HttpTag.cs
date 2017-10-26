using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpTag
    {
        //ownerID	是	long	标签的所有者id。公司拥有的标签是公司ID，HR拥有的标签是HR的ID，应聘者拥有的ID是应聘者ID，系统拥有的标签为固定值
        //group	是	string	标签的分组，如薪资福利，更多福利等。可选值见附件
        //text	是	string	
        public long ownerID
        {
            get;
            set;
        }
        public string group
        {
            get;
            set;
        }
        public string text
        {
            get;
            set;
        }
        public string isDel
        {
            get;
            set;
        }
    }
    public class HttpTagAttractionJob
    {
        public List<HttpTag> attractionJob
        {
            get;
            set;
        }
        public List<HttpTag> userDefinedJob
        {
            get;
            set;
        }
    }
    
}
