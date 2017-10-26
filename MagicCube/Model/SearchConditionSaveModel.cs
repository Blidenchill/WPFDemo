using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.Model
{
    public class SearchConditionSaveModel
    {
        public string words { get; set; }
        public string companyName { get; set; }
        public bool onlyLastCompany { get; set; }
        public List<CodeModel> jobType { get; set; }
        public List<CodeModel> industry { get; set; }
        public Age age { get; set; }
        public WorkExp workExp { get; set; }
        public Degree degree { get; set; }
        public Salary salary { get; set; }
        public List<CodeModel> expCity { get; set; }
        public List<CodeModel> address { get; set; }
        public string gender { get; set; }
        public string updateTime { get; set; }
        public string major { get; set; }
        public string school { get; set; }
        public string language { get; set; }
        public string status { get; set; }

        public string conditionHeadShow { get; set; }
        public bool school985 { get; set; }
        public bool school211 { get; set; }
        public bool schoolAll { get; set; }
        public bool schoolOverSea { get; set; }
    }

    public class Age
    {
        public string minAge { get; set; }
        public string maxAge { get; set; }
    }


    public class WorkExp
    {
        public CodeModel maxWorkExp { get; set; }
        public CodeModel minWorkExp { get; set; }
    }
    public class Degree
    {
        public string minDegreeCode { get; set; }
        public string maxDegreeCode { get; set; }
        public List<CodeModel> expand { get; set; }
    }

    public class Salary
    {
        public string maxSalaryCode { get; set; }
        public string minSalaryCode { get; set; }
    }

   

}
