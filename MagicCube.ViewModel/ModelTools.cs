using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public static class ModelTools
    {
        /// <summary>
        /// URL中反射类中属性名，获取参数。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string SetHttpPropertys<T>()
        {
            try
            {
          
                string temp = string.Empty;
                Type tp = typeof(T);
                System.Reflection.PropertyInfo[] properties = tp.GetProperties();
                foreach (var item in properties)
                {
                    temp += item.Name + "$$";
                }
                temp = temp.TrimEnd(new char[] { '$' });
                return temp;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }
        public static string SetListPropertys(List<string> lstStr)
        {
            try
            {

                string temp = string.Empty;
                foreach (var item in lstStr)
                {
                    temp += item + "$$";
                }
                temp = temp.TrimEnd(new char[] { '$' });
                return temp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public static string GetLongYearFromDate(string dateTime)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(dateTime);
                DateTime now = DateTime.Now;
                int ages = now.Year - dt.Year;
                if (now < dt.AddYears(ages))
                {
                    ages--;
                }
                return ages.ToString();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
            

        }
        public static string GetWorkYearFromDate(string dateTime)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dateTime))
                    return "无";
                //年月,特殊年0000年表示在读，0001年表示应届
                DateTime dt = Convert.ToDateTime(dateTime);
                if (dt.Year == 0)
                    return "在读学生";
                if (dt.Year == 1)
                    return "应届毕业生";
                DateTime now = DateTime.Now;
                TimeSpan ages = (now- dt);
                int mouth = Convert.ToInt16(ages.TotalDays / 30);
                if (mouth < 6)
                {
                    return "无工作经验";
                }

                return ages.ToString() + "年";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }


        }
        /// <summary>
        /// $$分割的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> StringConvertToList(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            List<string> list = new List<string>();
            string[] tempList = str.Split(new string[] { "$$"}, StringSplitOptions.RemoveEmptyEntries);
            list.AddRange(tempList);
            return list;
        }
    }
}
