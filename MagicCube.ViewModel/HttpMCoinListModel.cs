using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpMCoinListModel
    {
        public int total { get; set; }
        public List<HttpMCoinItem> result { get; set; }
    }

    public class HttpMCoinItem
    {
        public string serviceSign { get; set; }
        public string remark { get; set; }
        public string triggerName { get; set; }
        public string triggerParams { get; set; }
        public object conditionEndTime { get; set; }
        public int userID { get; set; }
        public object conditionBeginTime { get; set; }
        public string triggerCode { get; set; }
        public string serviceName { get; set; }
        public int serviceAction { get; set; }
        public int serviceQuantity { get; set; }
        public string @operator { get; set; }
        public int userServiceLogID { get; set; }
        public string createTime { get; set; }
    }
}
