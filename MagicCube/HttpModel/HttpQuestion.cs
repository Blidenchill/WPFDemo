using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class HttpQuestion
    {
        public List<string> answer { get; set; }
        public string question { get; set; }
        public string type { get; set; }
    }

    public class HttpQuestionRoot
    {
        public List<HttpQuestion> question { get; set; }
    }
}
