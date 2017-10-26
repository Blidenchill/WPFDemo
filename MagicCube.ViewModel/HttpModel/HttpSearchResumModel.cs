using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpSearchResumModel
    {
        public long userID { get; set; }
        public string appKey { get; set; }
        public int page { get; set; }
        public int size { get; set; }
        public string cityCode { get; set; }

        public string words { get; set; }
        public string salary { get; set; }

        public int workExpType { get; set; }
        public string workExp { get; set; }
        public string companyName { get; set; }
        public bool onlyLastCompany { get; set; }
        public string thirdJobCode { get; set; }
        public string industry { get; set; }
        public string age { get; set; }
        public string education { get; set; }
        public string address { get; set; }
        public string gender { get; set; }
        public string updateTime { get; set; }
        
        public string school { get; set; }

        public string major { get; set; }
        public string language { get; set; }
        public string status { get; set; }
        public int sort { get; set; }
        public string properties { get; set; }

        public string desc { get; set; }

        public bool group211 { get; set; }
        public bool group985 { get; set; }
        public bool fullTime { get; set; }
        public bool oversea { get; set; }

        public string viewedResume { get; set; }





        public string RangeConvert(string gt, string lt)
        {
            if (gt == "0")
                return null;
            
            if (lt == "0" || string.IsNullOrEmpty(lt))
                return string.Format("$gte:{0}", gt);


            return string.Format("$gte:{0},$lte:{1}", gt, lt);
        }
        public string INConvert(List<string> list)
        {
            if (list.Count == 0)
                return null;
            string temp = string.Empty;
            foreach(var item in list)
            {
                temp += item + ",";
            }
           
            temp = temp.TrimEnd(new char[] { ',' });
            if (string.IsNullOrEmpty(temp))
                return null;
            return string.Format("$in:[{0}]", temp);
        }

    }
}
