using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.Common
{
    public class DateTimeOperation
    {
        /// <summary>
        /// 两个日期相减 得到年、月、日字符串
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string DateDiff(DateTime start, DateTime end)
        {
            string dateDiff = string.Empty;
            try
            {
                DateTime end1;
                int date = (end.AddDays(1) - start).Days;//两个日期相距的天数
                int date1 = 0;
                //开始日期的天<=结束日期的天。
                if (start.Day <= end.Day)
                {
                    //自定义一个日期，结束日期。年月不变，天变为开始日期的天
                    end1 = new DateTime(end.Year, end.Month, start.Day);
                }
                //开始日期的天>结束日期的天。结束日期需要减小一个月
                else
                {
                    //自定义一个日期，结束日期。年不变，月份减1，天变为开始日期的天
                    end1 = new DateTime(end.Year, end.Month - 1, start.Day);
                }
                //自定义结束日期与开始日期相距的天数
                date1 = (end1 - start).Days;
                int year = 0;
                int month = (end1.Year - start.Year) * 12 + end1.Month - start.Month;
                int day = date - date1;
                if (month >= 12)
                {
                    year = (int)month / 12;
                    month = month % 12;
                }
                if (year > 0)
                {
                    dateDiff += year + "年";
                }
                if (month > 0)
                {
                    dateDiff += month + "个月";
                }
                //if (day > 0)
                //{
                //    dateDiff += day + "天";
                //}
                if(dateDiff == string.Empty)
                {
                    dateDiff = "无";
                }
            }
            catch
            {
                dateDiff = "无";
            }


            return dateDiff;
        }
    }
}
