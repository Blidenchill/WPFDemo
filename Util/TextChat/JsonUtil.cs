using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Xml;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;


namespace MagicCube.Util
{
    public static class JsonUtil
    {
        //对象转json字符
        public static string ToJsonString(object jsonObject)
        {
            string str = null;
            try
            {
                str = JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonObjct">对象</param>
        /// <param name="props">传入的属性数组</param>
        /// <param name="retain">true:表示props是需要保留的字段  false：表示props是要排除的字段</param>
        /// <returns></returns>
        public static string ToJsonString(object jsonObjct, string[] props, bool retain = true)
        {
            string temp;
            try
            {
                JsonSerializerSettings jsSetting = new JsonSerializerSettings();
                jsSetting.ContractResolver = new LimitPropsContractResolver(props, retain);
                jsSetting.NullValueHandling = NullValueHandling.Ignore;
                temp = JsonConvert.SerializeObject(jsonObjct, Newtonsoft.Json.Formatting.Indented, jsSetting);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return temp;
        }

        //json转泛型对象
        public static T ToObject<T>(string str)
        {
            object obj2 = null;
            try
            {
                obj2 = JsonConvert.DeserializeObject<T>(str, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (T)obj2;
        }

        //从xml应答中获取body的json串
        public static string GetJsonTextFromXml(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            XmlNodeList nodeList = document.GetElementsByTagName("body");
            if (nodeList != null)
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlNode node = nodeList[i];
                    XmlNodeList childNodeList = node.ChildNodes;
                    if (childNodeList != null && childNodeList.Count > 0)
                    {
                        XmlNode childNode = node.ChildNodes[0];
                        if (childNode.NodeType == XmlNodeType.Text)
                        {
                            string jsonData = childNode.InnerText;
                            return jsonData;
                        }
                    }
                }
            }
            return string.Empty;
        }


        public static string Sign(string us, string ps)
        {
            JObject jsonObj1 = new JObject();
            jsonObj1.Add("username", us);
            jsonObj1.Add("password", ps);
            return JsonUtil.ToJsonString(jsonObj1);
        }

        public static string TalentResearch(string str, int page)
        {
            JObject jsonObj1 = new JObject();
            jsonObj1.Add("fuzzyCondition", str);
            //jsonObj1.Add("start", (page - 1) * ConfUtil.PageContent);
            //jsonObj1.Add("count", percount);
            return JsonUtil.ToJsonString(jsonObj1);
        }

        public static string JsonParamsToString(string[] paramNames, string[] paramValues)
        {
            JObject jsonObj = new JObject();
            for(int i = 0; i < paramNames.Length; i++)
            {
                jsonObj.Add(paramNames[i], paramValues[i]);
            }
            return ToJsonString(jsonObj);
        }

        public static string ResumePay(int contactRecordId, string payType)
        {
            JObject jsonObj1 = new JObject();
            //jsonObj1.Add("uniqueKey", resumeId);
            jsonObj1.Add("contactRecordId", contactRecordId);
            jsonObj1.Add("payType", payType);
            return JsonUtil.ToJsonString(jsonObj1);
        }

        public static string CallFor(int resumeId)
        {
            JObject jsonObj1 = new JObject();
            //jsonObj1.Add("uniqueKey", resumeId);
            jsonObj1.Add("contactRecordId", resumeId);
            return JsonUtil.ToJsonString(jsonObj1);
        }
        public static string CallForByUniqueKey(string uniqueKey)
        {
            JObject jsonObj1 = new JObject();
            jsonObj1.Add("uniqueKey", uniqueKey);
            //jsonObj1.Add("contactRecordId", resumeId);
            return JsonUtil.ToJsonString(jsonObj1);
        }

        public static string pagingList(int start, int count)
        {
            JObject jsonObj1 = new JObject();

            jsonObj1.Add("start", start);
            jsonObj1.Add("count", count);
            return JsonUtil.ToJsonString(jsonObj1);
        }

        public static string ContactRecord(int job_id, int record_id, string resume_id, string unique_key)
        {
            JObject jsonObj1 = new JObject();

            jsonObj1.Add("jobId", job_id);
            if (record_id == 0)
                jsonObj1.Add("contactRecordId", "");
            else
                jsonObj1.Add("contactRecordId", record_id);
            jsonObj1.Add("resumeId", resume_id);
            jsonObj1.Add("uniqueKey", unique_key);
            return JsonUtil.ToJsonString(jsonObj1);
        }
        public static string onsitePersionList(int onsiteTimeSlotId, int onsiteJobId, int start, int count)
        {
            JObject jsonObj1 = new JObject();
            jsonObj1.Add("onsiteTimeSlotId", onsiteTimeSlotId);
            //jsonObj1.Add("onsiteJobTimeSlotId", onsiteJobId);
            jsonObj1.Add("start", start);
            jsonObj1.Add("count", count);
            return JsonUtil.ToJsonString(jsonObj1);
        }
        public static string onsiteJobList(int onsiteJobId, int start, int count, string startDay, string endDay, string status)
        {
            JObject jsonObj1 = new JObject();
            jsonObj1.Add("startDay", startDay);
            jsonObj1.Add("endDay", endDay);
            jsonObj1.Add("status", status);
            jsonObj1.Add("onsiteJobId", onsiteJobId);
            jsonObj1.Add("start", start);
            jsonObj1.Add("count", count);
            return JsonUtil.ToJsonString(jsonObj1);
        }

        public static string resultReview(int jobId,int interviewId, string status, string result,string name)
        {
            JObject jsonObj1 = new JObject();
            jsonObj1.Add("jobId", jobId);
            jsonObj1.Add("interviewId", interviewId);
            jsonObj1.Add("status", status);
            jsonObj1.Add("result", result);
            jsonObj1.Add("name", name);
            return JsonUtil.ToJsonString(jsonObj1);
        }
        public static string InterviewResult(int jobId, int interviewId, string name)
        {
            JObject jsonObj1 = new JObject();
            jsonObj1.Add("jobId", jobId);
            jsonObj1.Add("interviewId", interviewId);
            jsonObj1.Add("name", name);
            return JsonUtil.ToJsonString(jsonObj1);
        }
    }

    public class LimitPropsContractResolver : DefaultContractResolver
    {
        string[] props = null;

        bool retain;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="props">传入的属性数组</param>
        /// <param name="retain">true:表示props是需要保留的字段  false：表示props是要排除的字段</param>
        public LimitPropsContractResolver(string[] props, bool retain = true)
        {
            //指定要序列化属性的清单
            this.props = props;
            this.retain = retain;
        }

        protected override IList<JsonProperty> CreateProperties(Type type,

        MemberSerialization memberSerialization)
        {
            IList<JsonProperty> list =
            base.CreateProperties(type, memberSerialization);
            //只保留清单有列出的属性
            return list.Where(p =>
            {
                if (retain)
                {
                    return props.Contains(p.PropertyName);
                }
                else
                {
                    return !props.Contains(p.PropertyName);
                }
            }).ToList();
        }
    }
}
