using MagicCube.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MagicCube.Common
{
    public static class CityCodeHelper
    {

        public static JsonCityCodeMode jsonCityModel;
        public static JsonCityCodeMode GetCityCode()
        {
            if (jsonCityModel == null)
            {
                string cityCodeStr = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "CityCode.json");
                jsonCityModel = DAL.JsonHelper.ToObject<JsonCityCodeMode>(cityCodeStr);
            }
            return jsonCityModel;
        }

        public static string GetCodeFromName(string name)
        {
            string code = string.Empty;
            code = GetCityCode().CityCodes.CityCode.FirstOrDefault(x => x.simpleName == name).value;
            return code;
        }

        public static string GetCityName(string strCode)
        {
            string name = string.Empty;
            if (strCode == null)
                return name;
            if (GetCityCode().CityCodes.CityCode.FirstOrDefault(x => x.value == strCode) == null)
                return name;
            name = GetCityCode().CityCodes.CityCode.FirstOrDefault(x => x.value == strCode).fullName;
            if (name != null)
                name = name.Replace("省", "-").Replace("市", "-").Replace("区", "-").Substring(0, name.Length - 1);
            else
                name = string.Empty;
            return name;
        }

        public static string GetSingleName(string strCode)
        {
            string name = string.Empty;
            if (strCode == null)
                return name;
            if (GetCityCode().CityCodes.CityCode.FirstOrDefault(x => x.value == strCode) == null)
                return name;
            name = GetCityCode().CityCodes.CityCode.FirstOrDefault(x => x.value == strCode).defaultName;
            return name;
        }
    }

    public static class MinDegreeHelper
    {

        public static MappingRoot jsonModel;
        public static MappingRoot GetCode()
        {
            if (jsonModel == null)
            {
                string strEducation = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "educationCode.json");
                jsonModel = DAL.JsonHelper.ToObject<MappingRoot>(strEducation);
            }
            return jsonModel;
        }

        public static string GetName(string strCode)
        {

            string name = string.Empty;
            if (string.IsNullOrWhiteSpace(strCode))
                return name;
            if (GetCode().root.FirstOrDefault(x => x.value == strCode) == null)
                return name;
            name = GetCode().root.FirstOrDefault(x => x.value == strCode).fullName;
            if (name == null)
                name = string.Empty;
            return name;
        }
        public static string GetCode(string name)
        {
            string code = string.Empty;
            code = GetCode().root.FirstOrDefault(x => x.fullName == name).value;
            return code;
        }
    }

    public static class MinExpHelper
    {

        public static MappingRoot jsonModel;
        public static MappingRoot GetCode()
        {
            if (jsonModel == null)
            {
                string strExp = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "jobExp.json");
                jsonModel = DAL.JsonHelper.ToObject<MappingRoot>(strExp);
            }
            return jsonModel;
        }

        public static string GetName(string strCode)
        {
            string name = string.Empty;
            if (string.IsNullOrWhiteSpace(strCode))
                return "不限";
            if (GetCode().root.FirstOrDefault(x => x.value == strCode) == null)
                return "不限";
            name = GetCode().root.FirstOrDefault(x => x.value == strCode).fullName;
            if (name == null)
                name = string.Empty;
            return name;
        }
    }

    public static class workCharactHelper
    {

        public static MappingRoot jsonModel;
        public static MappingRoot GetCode()
        {
            if (jsonModel == null)
            {
                string strCharact = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "jobCharact.json");
                jsonModel = DAL.JsonHelper.ToObject<MappingRoot>(strCharact);

            }
            return jsonModel;
        }

        public static string GetName(string strCode)
        {
            string name = string.Empty;
            if (strCode == "0")
                return name;
            if (GetCode().root.FirstOrDefault(x => x.value == strCode) == null)
                return name;
            name = GetCode().root.FirstOrDefault(x => x.value == strCode).fullName;
            if (name == null)
                name = string.Empty;
            return name;
        }
    }

    public static class JobCharactHelper
    {
        public static string GetName(string strCode)
        {
            string pResult = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "jobCatagary.json");
            HttpJobCatagaryCodeRoot pJobCatagaryRoot = DAL.JsonHelper.ToObject<HttpJobCatagaryCodeRoot>(pResult);
            foreach (HttpJobCatagaryCodeRECORD iRECORD in pJobCatagaryRoot.RECORDS.RECORD)
            {
                if (iRECORD.type_code == strCode)
                    return iRECORD.type_name;
            }
            return string.Empty;
        }
    }

    public static class ResumeIndustryHelper
    {

        public static MappingRoot jsonModel;
        public static MappingRoot GetCode()
        {
            if (jsonModel == null)
            {
                string strCharact = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "resumeIndustry.json");
                jsonModel = DAL.JsonHelper.ToObject<MappingRoot>(strCharact);

            }
            return jsonModel;
        }

        public static string GetName(string strCode)
        {
            string name = string.Empty;
            if (GetCode().root.FirstOrDefault(x => x.value == strCode) == null)
                return name;
            name = GetCode().root.FirstOrDefault(x => x.value == strCode).fullName;
            if (name == null)
                name = string.Empty;
            return name;
        }
    }
    public static class ResumeStatusHelper
    {

        public static MappingRoot jsonModel;
        public static MappingRoot GetCode()
        {
            if (jsonModel == null)
            {
                string strCharact = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "resumeStatus.json");
                jsonModel = DAL.JsonHelper.ToObject<MappingRoot>(strCharact);

            }
            return jsonModel;
        }

        public static string GetName(string strCode)
        {
            string name = string.Empty;
            if (GetCode().root.FirstOrDefault(x => x.value == strCode) == null)
                return name;
            name = GetCode().root.FirstOrDefault(x => x.value == strCode).fullName;
            if (name == null)
                name = string.Empty;
            return name;
        }
    }
    public static class ResumeName
    {


        public static string GetName(string name,string userID)
        {
            string temp = name;
            if (string.IsNullOrWhiteSpace(name))
            {
                temp = "求职者" + userID.PadLeft(4, '0').Substring(userID.PadLeft(4, '0').Length - 4);
            }
            if (IsNumeric(name) && name.Length > 4)
            {
                temp = "求职者" + userID.PadLeft(4, '0').Substring(userID.PadLeft(4, '0').Length - 4);
            }
            return temp;
        }
        public static string GetName(string name)
        {
            string temp = name;
            if (string.IsNullOrWhiteSpace(name))
            {
                temp = "求职者";
            }

            if (IsNumeric(name) && name.Length>4)
            {
                temp = "求职者";
            }
            return temp;
        }
        public static bool IsNumeric(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");
            return reg1.IsMatch(str);
        }
    }
}
   
