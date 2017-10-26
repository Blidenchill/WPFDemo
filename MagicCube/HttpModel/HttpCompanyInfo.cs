using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class HttpCompanyInfo
    {
        public Company company { get; set; }
        public CompanyIntro companyIntro { get; set; }
    }


    public class Product
    {
        public string updatedTime { get; set; }
        public string description { get; set; }
        public int companyId { get; set; }
        public bool deleted { get; set; }
        public string ownerType { get; set; }
        public string productName { get; set; }
        public string createdTime { get; set; }
        public string productUrl { get; set; }
        public string intro { get; set; }
        public int owner { get; set; }
        public int id { get; set; }
        public List<object> productImageUrls { get; set; }
    }


    public class CompanyContact
    {
        public string updatedTime { get; set; }
        public string name { get; set; }
        public int companyId { get; set; }
        public bool deleted { get; set; }
        public string ownerType { get; set; }
        public string phone { get; set; }
        public string createdTime { get; set; }
        public int owner { get; set; }
        public int id { get; set; }
    }

}
