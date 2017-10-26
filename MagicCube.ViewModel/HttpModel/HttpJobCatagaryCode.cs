using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpJobCatagaryCodeRECORD
    {
        /// <summary>
        /// Parent_type_code
        /// </summary>
        public string parent_type_code { get; set; }
        /// <summary>
        /// 01
        /// </summary>
        public string type_code { get; set; }
        /// <summary>
        /// 1
        /// </summary>
        public string type_level { get; set; }
        /// <summary>
        /// 技术
        /// </summary>
        public string type_name { get; set; }
    }

    public class HttpJobCatagaryCodeRECORDS
    {
        /// <summary>
        /// RECORD
        /// </summary>
        public List<HttpJobCatagaryCodeRECORD> RECORD { get; set; }
    }

    public class HttpJobCatagaryCodeRoot
    {
        /// <summary>
        /// RECORDS
        /// </summary>
        public HttpJobCatagaryCodeRECORDS RECORDS { get; set; }
    }
}
