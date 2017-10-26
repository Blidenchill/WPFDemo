using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace MagicCube.DAL
{
    public static class JsonHelper
    { 
        //对象转json字符
        public static string ToJsonString(object jsonObject)
        {
            string str = null;
            try
            {
                str = JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
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

        /// <summary>
        /// Json反序列号
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
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

        public static string JsonParamsToString(string[] paramNames, string[] paramValues)
        {
            JObject jsonObj = new JObject();
            for (int i = 0; i < paramNames.Length; i++)
            {
                jsonObj.Add(paramNames[i], paramValues[i]);
            }
            return ToJsonString(jsonObj);
        }
        public static string JsonParamsToString(List<string> paramNames, List<string> paramValues)
        {
            JObject jsonObj = new JObject();
            for (int i = 0; i < paramNames.Count; i++)
            {
                jsonObj.Add(paramNames[i], paramValues[i]);
            }
            return ToJsonString(jsonObj);
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
