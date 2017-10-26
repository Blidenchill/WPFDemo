using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace MagicCube.Util
{
    public static class HttpUtil
    {
        public const string HTTP_METHOD_GET = "GET";
        public const string HTTP_METHOD_POST = "POST";
        public const string HTTP_CONTENT_TYPE_JSON = "application/json";
        public const string HTTP_CONTENT_TYPE_FORM = "application/x-www-form-urlencoded";


        //private Encoding httpEncoding = Encoding.UTF8;
        //private string httpReqMethod = "POST";
        //private string httpContentType = "application/json";
        //private string httpToken;

        //public void SetHttpEncoding(Encoding httpEncoding)
        //{
        //    this.httpEncoding = httpEncoding;
        //}

        //public void SetHttpReqMethod(string httpReqMethod)
        //{
        //    this.httpReqMethod = httpReqMethod;
        //}

        //public void SetHttpContentType(string httpContentType)
        //{
        //    this.httpContentType = httpContentType;
        //}

        //public void SetHttpToken(string httpToken)
        //{
        //    this.httpToken = httpToken;
        //}

        //发送文本并接收应答
        public static string SendAndReceiveMessage(string url, string sendPackage)
        {
            return SendAndReceiveMessage(url, sendPackage, HTTP_METHOD_POST, HTTP_CONTENT_TYPE_JSON, Encoding.UTF8, string.Empty);
        }

        //发送文本并接收应答
        public static string SendAndReceiveMessage(string url, string sendPackage, string token)
        {
            return SendAndReceiveMessage(url, sendPackage, HTTP_METHOD_POST, HTTP_CONTENT_TYPE_JSON, Encoding.UTF8, token);
        }

        //发送文本并接收应答基础方法
        private static string SendAndReceiveMessage(string url, string sendPackage, string method, string token)
        {
            return SendAndReceiveMessage(url, sendPackage, method, HTTP_CONTENT_TYPE_JSON, Encoding.UTF8, token);
        }

        //发送文本并接收应答基础方法
        private static string SendAndReceiveMessage(string url, string sendPackage, string method, string contentType, Encoding httpEncoding, string token)
        {
            byte[] source = httpEncoding.GetBytes(sendPackage);
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.KeepAlive = false;
            httpRequest.ProtocolVersion = HttpVersion.Version11;

            if ("POST".Equals(method.ToUpper()))
            {
                httpRequest.Method = "POST";
            }
            else
            {
                httpRequest.Method = "GET";
            }
            
            if ("application/json".Equals(contentType.ToLower()))
            {
                httpRequest.ContentType = "application/json";
            }
            else
            {
                httpRequest.ContentType = "application/x-www-form-urlencoded";
            }

            if (!string.IsNullOrEmpty(token) && token.Length > 1)
            {
                httpRequest.Headers.Add("Authorization", "Bearer " + token);
            }


            //if (((HttpWebResponse)httpRequest.GetResponse()).StatusCode == HttpStatusCode.OK)
            //{
                BufferedStream requestStream = null;

                HttpWebResponse webResponse = null;
                StringBuilder responseData = new StringBuilder();
                try
                {
                    if ("POST".Equals(httpRequest.Method))
                    {
                        Stream pStream = httpRequest.GetRequestStream();
                        if (pStream == null)
                        {
                         
                            return null;
                        }
                        requestStream = new BufferedStream(httpRequest.GetRequestStream());
                        requestStream.Write(source, 0, source.Length);
                        requestStream.Flush();
                        requestStream.Close();
                    }

                    webResponse = (HttpWebResponse)httpRequest.GetResponse();
                    Stream receiveStream = webResponse.GetResponseStream();
                    StreamReader streamReader = new StreamReader(receiveStream, httpEncoding);

                    string line = null;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        responseData.Append(line);
                    }

                    webResponse.Close();
                    return responseData.ToString();
                }
                catch (Exception e)
                {
                  
                    throw e;
                }
                finally
                {
                    if (requestStream != null)
                    {
                        try
                        {
                            requestStream.Close();
                        }
                        catch 
                        {
                         
                        }
                    }
                    if (webResponse != null)
                    {
                        try
                        {
                            webResponse.Close();
                        }
                        catch 
                        {
                         
                        }
                    }
                }
            //}
            //else
            //{
            //    MessageBox.Show("连接服务器失败");
            //    return null;
            //}
        }




        #region "异步方法"


        #endregion
    }
}
