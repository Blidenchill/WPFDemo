using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.Messaging
{
    public class MSChangeAccount
    {
        public string header = "ChangeAccount";
        /// <summary>
        /// 不谈提示直接切换
        /// </summary>
        public bool IsDirectChange;
    }
}
