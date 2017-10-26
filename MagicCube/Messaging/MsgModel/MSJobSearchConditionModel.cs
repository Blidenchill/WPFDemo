using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.Messaging
{
    public class MSJobSearchConditionModel
    {
        public string ConditionHeadShow { get; set; }
        public ViewModel.HttpSearchResumModel model { get; set; }
    }
}
