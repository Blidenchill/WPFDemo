using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MagicCube.Common
{
    public static class CommonValidationMethod
    {

        /// <summary>
        /// 验证电子邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            String strExp = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex r = new Regex(strExp);
            Match m = r.Match(email);
            return m.Success;
        }

        public static bool IsUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            if(!url.StartsWith("http://"))
            {
                url = "http://" + url;
            }
            string strExp = @"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$";
            Regex r = new Regex(strExp);
            Match m = r.Match(url);
            return m.Success;
        }

        public static bool IsWebSite(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            string strExp = @"((http|ftp|https):\/\/)?[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";
            Regex r = new Regex(strExp);
            Match m = r.Match(url);
            return m.Success;
        }

        /// <summary>
        /// 验证手机号
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidTel(string strIn)
        {
            bool flag = Regex.IsMatch(strIn, @"^[1]+\d{10}");
            return flag;
        }

        public static bool UserNameValidate(string userName)
        {
            //去除（）()括号
            userName = userName.Replace("(", "");
            userName = userName.Replace(")", "");
            userName = userName.Replace("（", "");
            userName = userName.Replace("）", "");
            userName = userName.Trim();
            if(string.IsNullOrEmpty(userName))
            {
                return true;
            }
            String strExp = @"^[a-zA-Z\u4e00-\u9fa5]+$"; 
            Regex r = new Regex(strExp);
            Match m = r.Match(userName);
         
            return m.Success;
        }

        public static bool UserPositionValidate(string userName)
        {
            //去除（）()括号
            userName = userName.Replace("(", "");
            userName = userName.Replace(")", "");
            userName = userName.Replace("（", "");
            userName = userName.Replace("）", "");
            userName = userName.Trim();
            if (string.IsNullOrEmpty(userName))
            {
                return true;
            }
            String strExp = @"^[0-9a-zA-Z\u4e00-\u9fa5]+$";
            Regex r = new Regex(strExp);
            Match m = r.Match(userName);

            return m.Success;
        }

        public static bool CompanyNameValidate(string companyName)
        {
            //去除（）()括号
            companyName = companyName.Replace("(", "");
            companyName = companyName.Replace(")", "");
            companyName = companyName.Replace("（", "");
            companyName = companyName.Replace("）", "");
            companyName = companyName.Replace("_", "");
            companyName = companyName.Replace("-", "");
            companyName = companyName.Replace("—", "");
            companyName = companyName.Trim();
            if (string.IsNullOrEmpty(companyName))
            {
                return true;
            }
            String strExp = @"^[0-9a-zA-Z\u4e00-\u9fa5]+$";
            Regex r = new Regex(strExp);
            Match m = r.Match(companyName);

            return m.Success;
        }

        public static bool CompanyShortNameValidate(string userName)
        {
            //去除（）()括号
            userName = userName.Replace("(", "");
            userName = userName.Replace(")", "");
            userName = userName.Replace("（", "");
            userName = userName.Replace("）", "");
            userName = userName.Replace("-", "");
            userName = userName.Replace("_", "");
            userName = userName.Replace("—", "");
            userName = userName.Trim();
            if (string.IsNullOrEmpty(userName))
            {
                return true;
            }
            String strExp = @"^[0-9a-zA-Z\u4e00-\u9fa5]+$";
            Regex r = new Regex(strExp);
            Match m = r.Match(userName);

            return m.Success;
        }

        public static bool IsContainTel(string strIn)
        {
            //if (string.IsNullOrEmpty(strIn))
            //    return false;
            //String strExp = @"$1(3[4-9]|5[012789]|8[78])\d{8}$";
            //Regex r = new Regex(strExp);
            //Match m = r.Match(strIn);
            //return m.Success;

            bool flag = Regex.IsMatch(strIn, @"/1[3,5,7,8,9]{1}[0-9]{9}|[0-9]{6,}/g");
            return flag;
        }
        public static bool IsContainEmail(string strIn)
        {
            if (string.IsNullOrEmpty(strIn))
                return false;
            String strExp = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex r = new Regex(strExp);
            Match m = r.Match(strIn);
            return m.Success;
        }
        public static bool IsContainNum(string strIn)
        {
            if (string.IsNullOrEmpty(strIn))
                return false;
            String strExp = @"\d{6,}";
            Regex r = new Regex(strExp);
            Match m = r.Match(strIn);
            return m.Success;

        }
        static string getValue(Regex Reg, string Info)
        {
            if (Reg.Match(Info).Success)
            {
                return Reg.Match(Info).Value;
            }
            else
            {
                return "";
            }
        }
        //// C#正则表达式小结

        ////只能输入数字："^[0-9]*$"。
        ////只能输入n位的数字："^\d{n}$"。
        ////只能输入至少n位的数字："^\d{n,}$"。
        ////只能输入m ~n位的数字：。"^\d{m,n}$"
        ////只能输入零和非零开头的数字："^(0|[1-9][0-9]*)$"。
        ////只能输入有两位小数的正实数："^[0-9]+(.[0-9]{2})?$"。
        ////只能输入有1 ~3位小数的正实数："^[0-9]+(.[0-9]{1,3})?$"。
        ////只能输入非零的正整数："^\+?[1-9][0-9]*$"。
        ////只能输入非零的负整数："^\-[1-9][]0-9"*$。
        ////只能输入长度为3的字符："^.{3}$"。
        ////只能输入由26个英文字母组成的字符串："^[A-Za-z]+$"。
        ////只能输入由26个大写英文字母组成的字符串："^[A-Z]+$"。
        ////只能输入由26个小写英文字母组成的字符串："^[a-z]+$"。
        ////只能输入由数字和26个英文字母组成的字符串："^[A-Za-z0-9]+$"。
        ////只能输入由数字、26个英文字母或者下划线组成的字符串："^\w+$"。
        ////只能输入由数字、26个英文字母或者下划线,中文组成的字符串：^\\w+$
        ////验证用户密码："^[a-zA-Z]\w{5,17}$"正确格式为：以字母开头，长度在6 ~18之间，只能包含字符、数字和下划线。
        ////验证是否含有^%&',;=?$\"等字符："[^%&',;=?$\x22]+"。
        ////只能输入汉字："^[\u4e00-\u9fa5]{0,}$"
        ////验证Email地址："^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"。
        ////验证InternetURL："^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$"。
        ////验证电话号码："^(\(\d{3,4}-)|\d{3.4}-)?\d{7,8}$"正确格式为："XXX-XXXXXXX"、"XXXX-XXXXXXXX"、"XXX-XXXXXXX"、"XXX-XXXXXXXX"、"XXXXXXX"和"XXXXXXXX"。
        ////验证身份证号（15位或18位数字）："^\d{15}|\d{18}$"。
        ////验证一年的12个月："^(0?[1-9]|1[0-2])$"正确格式为："01"～"09"和"1"～"12"。
        ////验证一个月的31天："^((0?[1-9])|((1|2)[0-9])|30|31)$"正确格式为；"01"～"09"和"1"～"31"。 

        ////利用正则表达式限制网页表单里的文本框输入内容：

        ////用正则表达式限制只能输入中文：onkeyup="value=value.replace(/[^\u4E00-\u9FA5]/g,'')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\u4E00-\u9FA5]/g,''))"

        ////用正则表达式限制只能输入全角字符： onkeyup="value=value.replace(/[^\uFF00-\uFFFF]/g,'')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\uFF00-\uFFFF]/g,''))"

        ////用正则表达式限制只能输入数字：onkeyup="value=value.replace(/[^\d]/g,'') "onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))"

        ////用正则表达式限制只能输入数字和英文：onkeyup="value=value.replace(/[\W]/g,'') "onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))"

    }
}
