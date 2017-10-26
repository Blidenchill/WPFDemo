using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class HttpUploadFileModel
    {
        public string url { get; set; }
        public string path { get; set; }
        public string filesize { get; set; }
        public string originalFileName { get; set; }

    }
}
