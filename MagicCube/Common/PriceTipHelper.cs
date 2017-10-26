using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.Common
{
    public static class PriceTipHelper
    {
        public static string PriceConvert(string target)
        {
            
            string price = "10";
            if (string.IsNullOrWhiteSpace(target))
                return price;
            switch (target)
            {
                case "2000元/月及以下":
                    price = "4";
                    break;
                case "2001-4000元/月":
                    price = "4";
                    break;
                case "4001-6000元/月":
                    price = "8";
                    break;
                case "6001-8000元/月":
                    price = "12";
                    break;
                case "8001-10000元/月":
                    price = "16";
                    break;
                case "10001-15000元/月":
                    price = "20";
                    break;
                case "15001-25000元/月":
                    price = "30";
                    break;
                case "25000元/月以上":
                    price = "50";
                    break;
            }
            return price;
        }

        public static string CostConvert(string target)
        {

            string price = "50";
            if (string.IsNullOrWhiteSpace(target))
                return price;
            switch (target)
            {
                case "2000元/月及以下":
                    price = "20";
                    break;
                case "2001-4000元/月":
                    price = "20";
                    break;
                case "4001-6000元/月":
                    price = "40";
                    break;
                case "6001-8000元/月":
                    price = "60";
                    break;
                case "8001-10000元/月":
                    price = "80";
                    break;
                case "10001-15000元/月":
                    price = "100";
                    break;
                case "15001-25000元/月":
                    price = "150";
                    break;
                case "25000元/月以上":
                    price = "250";
                    break;
            }
            return price;
        }

    }
}
