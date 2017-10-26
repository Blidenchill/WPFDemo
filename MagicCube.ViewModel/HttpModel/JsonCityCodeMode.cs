using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class JsonCityCodeMode
    {
        public CityCodes CityCodes { get; set; }
    }


    public class CityCodeItem
    {
        public string simpleName { get; set; }
        public string fullName { get; set; }
        public string value { get; set; }
        public string defaultName { get; set; }
    }

    public class CityCodes
    {
        public List<CityCodeItem> CityCode { get; set; }
    }


}
