using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MagicCube.Common
{
    public static class TimeHelper
    {
        public static string RecentChatTime(DateTime pDateTime)
        {
            if (DateTime.Now.ToString("d") == pDateTime.ToString("d"))
            {
                return pDateTime.ToString("HH:mm");
            }
            else
            {
                if (DateTime.Now.Year != pDateTime.Year)
                {
                    return pDateTime.ToString("M/d");
                }
                if (DateTime.Now.Year == pDateTime.Year && DateTime.Now.Month == pDateTime.Month && DateTime.Now.Day - pDateTime.Day == 1)
                {
                    return "昨天";
                }
                else
                {
                    if (DateTime.Now.Year == pDateTime.Year && DateTime.Now.Month == pDateTime.Month && DateTime.Now.Day - pDateTime.Day < 8)
                    {
                        string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
                        string week = weekdays[Convert.ToInt32(pDateTime.DayOfWeek)];
                        return week;
                    }
                    else
                    {
                        return pDateTime.ToString("M/d");
                    }
                }
            }
        }

        public static string CurrentChatTime(DateTime pDateTime)
        {
            if (DateTime.Now.ToString("d") == pDateTime.ToString("d"))
            {
                return pDateTime.ToString("HH:mm");
            }
            else
            {
                if (DateTime.Now.Year != pDateTime.Year)
                {
                    return pDateTime.ToString("yyyy年M月d日 HH:mm");
                }
                if (DateTime.Now.Year == pDateTime.Year && DateTime.Now.Month == pDateTime.Month && DateTime.Now.Day - pDateTime.Day == 1)
                {
                    return "昨天" + pDateTime.ToString(" HH:mm");
                }
                else
                {
                    if (DateTime.Now.Year == pDateTime.Year && DateTime.Now.Month == pDateTime.Month && DateTime.Now.Day - pDateTime.Day < 8)
                    {
                        string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
                        string week = weekdays[Convert.ToInt32(pDateTime.DayOfWeek)];
                        return week + pDateTime.ToString(" HH:mm");
                    }
                    else
                    {
                        return pDateTime.ToString("M月d日 HH:mm");
                    }
                }
            }
        }
        public static string ResumeTableTime(DateTime pDateTime)
        {
            if (DateTime.Now.ToString("d") == pDateTime.ToString("d"))
            {
                return pDateTime.ToString("今天");
            }
            else
            {
                if (DateTime.Now.Year == pDateTime.Year && DateTime.Now.Month == pDateTime.Month && DateTime.Now.Day - pDateTime.Day == 1)
                {
                    return "昨天";
                }
                if (DateTime.Now.Year == pDateTime.Year && DateTime.Now.Month == pDateTime.Month && DateTime.Now.Day - pDateTime.Day == 2)
                {
                    return "前天";
                }
                return pDateTime.ToString("yyyy/MM/dd");
            }
        }
        public static string getAge(string dob)
        {
            string strAge = string.Empty;
            if (dob != null)
            {
                if (dob.Length > 4)
                {
                    string str = dob.Substring(0, 4);
                    int t = 0;
                    if (int.TryParse(str, out t))
                    {
                        strAge = (DateTime.Now.Year - Convert.ToInt32(str)).ToString();
                    }
                }
            }

            return strAge;
        }

        /// <summary>  
        /// 时间戳Timestamp  
        /// </summary>  
        /// <returns></returns>  
        public static long GetCreatetime()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

       
        /// <summary>  
        /// 时间戳转为C#格式时间  
        /// </summary>  
        /// <param name=”timeStamp”></param>  
        /// <returns></returns>  
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dateTimeStart.Add(toNow);
        }


        public static DateTime GetBeiJingTime()
        {
            /// //<?xml version="1.0" encoding="GB2312" ?>
            //- <ntsc>
            //- <time>
            // <year>2013</year>
            // <month>8</month>
            // <day>29</day> 
            // <Weekday /> 
            // <hour>16</hour>
            // <minite>29</minite>
            // <second>12</second> 
            // <Millisecond />
            // </time> 
            // </ntsc> 
            DateTime dt;
            WebRequest wrt = null;
            WebResponse wrp = null;
            try
            {
                wrt = WebRequest.Create("http://www.time.ac.cn/timeflash.asp?user=flash");
                wrt.Credentials = CredentialCache.DefaultCredentials;
                wrp = wrt.GetResponse();
                StreamReader sr = new StreamReader(wrp.GetResponseStream(), Encoding.UTF8);
                string html = sr.ReadToEnd();
                sr.Close();
                wrp.Close();
                int yearIndex = html.IndexOf("<year>") + 6;
                int monthIndex = html.IndexOf("<month>") + 7;
                int dayIndex = html.IndexOf("<day>") + 5;
                int hourIndex = html.IndexOf("<hour>") + 6;
                int miniteIndex = html.IndexOf("<minite>") + 8;
                int secondIndex = html.IndexOf("<second>") + 8;
                string year = html.Substring(yearIndex, html.IndexOf("</year>") - yearIndex);
                string month = html.Substring(monthIndex, html.IndexOf("</month>") - monthIndex);
                string day = html.Substring(dayIndex, html.IndexOf("</day>") - dayIndex);
                string hour = html.Substring(hourIndex, html.IndexOf("</hour>") - hourIndex);
                string minite = html.Substring(miniteIndex, html.IndexOf("</minite>") - miniteIndex);
                string second = html.Substring(secondIndex, html.IndexOf("</second>") - secondIndex);
                dt = DateTime.Parse(year + "-" + month + "-" + day + " " + hour + ":" + minite + ":" + second);
            }
            catch (WebException)
            {
                return DateTime.Parse("2013-1-1");
            }
            catch (Exception)
            {
                return DateTime.Parse("2013-1-1");
            }
            finally
            {
                if (wrp != null)
                    wrp.Close();
                if (wrt != null)
                    wrt.Abort();
            }
            return dt;
        }






    }
}
