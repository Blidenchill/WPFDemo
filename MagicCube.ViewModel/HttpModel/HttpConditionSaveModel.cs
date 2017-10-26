using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpConditionSaveModel
    {
        public string conditionID { get; set; }
        public string conditionName { get; set; }
        public string conditionContent { get; set; }
        public string userID { get; set; }
    }

    public class HttpConditionSaveListModel
    {
        public List<HttpConditionSaveModel> conditionList { get; set; }
    }
}
