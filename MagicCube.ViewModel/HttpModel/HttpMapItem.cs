using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class MappingItem
    {
        public string simpleName { get; set; }
        public string fullName { get; set; }
        public string value { get; set; }
        public string defaultName { get; set; }
    }
    public class MappingRoot
    {
        /// <summary>
        /// Root
        /// </summary>
        public List<MappingItem> root { get; set; }
    }
}
