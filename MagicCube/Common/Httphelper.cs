
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace MagicCube.Common
{
    public static class HttpHelper
    {
        public static string cookie;
        public static string HttpPost(string Url, string postDataStr,bool sign = false, int timeout = 30)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = postDataStr.Length;

                if(!sign)
                    request.Headers.Add(HttpRequestHeader.Cookie, cookie);
                request.Timeout = timeout * 1000;

                byte[] btBodys = Encoding.UTF8.GetBytes(postDataStr);
                request.ContentLength = btBodys.Length;  
                request.GetRequestStream().Write(btBodys, 0, btBodys.Length);
                
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                request.GetRequestStream().Flush();
                if (sign)
                {
                    cookie = response.Headers.Get("Set-Cookie");                                
                }
                string encoding = response.ContentEncoding;
                if (string.IsNullOrWhiteSpace(encoding))
                {
                    encoding = "UTF-8"; //默认编码
                }
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                string retString = reader.ReadToEnd();
                Console.WriteLine(Url +"  "+ stopwatch.Elapsed.TotalSeconds);
                stopwatch.Stop();
                return retString;
            }
            catch (WebException webEx)
            {
                Console.WriteLine(Url + "  " + stopwatch.Elapsed.TotalSeconds);
                stopwatch.Stop();
                if (webEx.Status == WebExceptionStatus.Timeout)
                {
                    
                    return "连接失败" + webEx.Message;
                }
                else
                {
                    return "连接失败" + webEx.Message;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Url + "  " + stopwatch.Elapsed.TotalSeconds);
                stopwatch.Stop();
                return "连接失败：" + ex.ToString();

            }
            
        }

        public static string HttpPostWebEx(string Url, string postDataStr, bool sign = false, int timeout = 30)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = postDataStr.Length;

                if (!sign)
                    request.Headers.Add(HttpRequestHeader.Cookie, cookie);
                request.Timeout = timeout * 1000;

                byte[] btBodys = Encoding.UTF8.GetBytes(postDataStr);
                request.ContentLength = btBodys.Length;
                request.GetRequestStream().Write(btBodys, 0, btBodys.Length);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                request.GetRequestStream().Flush();
                if (sign)
                {
                    cookie = response.Headers.Get("Set-Cookie");                 
                }
                string encoding = response.ContentEncoding;
                if (string.IsNullOrWhiteSpace(encoding))
                {
                    encoding = "UTF-8"; //默认编码
                }
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                string retString = reader.ReadToEnd();
                Console.WriteLine(Url + "  " + stopwatch.Elapsed.TotalSeconds);
                stopwatch.Stop();
                return retString;
            }
            catch (WebException webEx)
            {
                HttpWebResponse response = (HttpWebResponse)webEx.Response;
                Console.WriteLine("Error code: {0}", response.StatusCode);
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    using (Stream data = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(data))
                        {
                            Console.WriteLine(Url + "  " + stopwatch.Elapsed.TotalSeconds);
                            stopwatch.Stop();
                            string text = reader.ReadToEnd();
                            return text;
                        }
                    }
                }
                Console.WriteLine(Url + "  " + stopwatch.Elapsed.TotalSeconds);
                stopwatch.Stop();
                return "连接失败" + webEx.Message;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Url + "  " + stopwatch.Elapsed.TotalSeconds);
                stopwatch.Stop();
                return "连接失败：" + ex.ToString();

            }

        }

        public static string HttpGet(string Url)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.ContentType = "application/json";
                request.Method = "GET";
                request.Timeout = 30000;
                request.Headers.Add(HttpRequestHeader.Cookie, cookie);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string encoding = response.ContentEncoding;
                if (string.IsNullOrWhiteSpace(encoding))
                {
                    encoding = "UTF-8"; //默认编码
                }
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                string retString = reader.ReadToEnd();
                Console.WriteLine(Url + "  " + stopwatch.Elapsed.TotalSeconds);
                stopwatch.Stop();
                return retString;
            }
            catch (WebException webEx)
            {
                Console.WriteLine(Url + "  " + stopwatch.Elapsed.TotalSeconds);
                stopwatch.Stop();
                if (webEx.Status == WebExceptionStatus.Timeout)
                {
                    return "连接失败" + webEx.Message;
                }
                else
                {
                    return "连接失败" + webEx.Message;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Url + "  " + stopwatch.Elapsed.TotalSeconds);
                stopwatch.Stop();
                return "连接失败：" + ex.ToString();

            }
        }

        public static void TrackHttp(string Url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                //request.ContentType = "application/json";
                request.Method = "GET";
                request.Timeout = 2000;
                //request.Headers.Add(HttpRequestHeader.Cookie, cookie);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                
            }
            catch
            {
                //Console.WriteLine(ex.Message);
            }
          
        }

        public static string HttpPut(string Url, string postDataStr)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.ContentLength = postDataStr.Length;

                request.Headers.Add(HttpRequestHeader.Cookie, cookie);
                request.Timeout = 30000;

                byte[] btBodys = Encoding.UTF8.GetBytes(postDataStr);
                request.ContentLength = btBodys.Length;
                request.GetRequestStream().Write(btBodys, 0, btBodys.Length);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                request.GetRequestStream().Flush();
                string encoding = response.ContentEncoding;
                if (string.IsNullOrWhiteSpace(encoding))
                {
                    encoding = "UTF-8"; //默认编码
                }
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                string retString = reader.ReadToEnd();
                return retString;
            }
            catch (WebException webEx)
            {
                if (webEx.Status == WebExceptionStatus.Timeout)
                {
                    return "连接失败" + webEx.Message;
                }
                else
                {
                    return "连接失败" + webEx.Message;
                }
            }
            catch (Exception ex)
            {
                return "连接失败：" + ex.ToString();

            }

        }



        public static class JSON
        {

            public static T parse<T>(string jsonString)
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                {
                    return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
                }
            }

            public static T Deserialize<T>(string jsonString)
            {
                T obj = Activator.CreateInstance<T>();
                using (MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(jsonString)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                    return (T)serializer.ReadObject(ms);
                }  
            }

        }




        private static readonly Encoding DEFAULTENCODE = Encoding.UTF8;

        /// <summary>
        /// HttpUploadFile
        /// </summary>
        /// <param name="url"></param>
        /// <param name="file"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string HttpUploadFile(string url, string file, NameValueCollection data)
        {
            return HttpUploadFile(url, file, data, DEFAULTENCODE);
        }

        /// <summary>
        /// HttpUploadFile
        /// </summary>
        /// <param name="url"></param>
        /// <param name="file"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpUploadFile(string url, string file, NameValueCollection data, Encoding encoding)
        {
            return HttpUploadFile(url, new string[] { file }, data, encoding);
        }

        /// <summary>
        /// HttpUploadFile
        /// </summary>
        /// <param name="url"></param>
        /// <param name="files"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string HttpUploadFile(string url, string[] files, NameValueCollection data)
        {
            return HttpUploadFile(url, files, data, DEFAULTENCODE);
        }

        /// <summary>
        /// HttpUploadFile
        /// </summary>
        /// <param name="url"></param>
        /// <param name="files"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpUploadFile(string url, string[] files, NameValueCollection data, Encoding encoding)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endbytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            //1.HttpWebRequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;
                request.Headers.Add(HttpRequestHeader.Cookie, cookie);

            using (Stream stream = request.GetRequestStream())
            {
                ////1.1 key/value
                //string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                //if (data != null)
                //{
                //    foreach (string key in data.Keys)
                //    {
                //        stream.Write(boundarybytes, 0, boundarybytes.Length);
                //        string formitem = string.Format(formdataTemplate, key, data[key]);
                //        byte[] formitembytes = encoding.GetBytes(formitem);
                //        stream.Write(formitembytes, 0, formitembytes.Length);
                //    }
                //}

                //1.2 file
                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                for (int i = 0; i < files.Length; i++)
                {
                    stream.Write(boundarybytes, 0, boundarybytes.Length);
                    string header = string.Format(headerTemplate, "test1", Path.GetFileName(files[i]));
                    byte[] headerbytes = encoding.GetBytes(header);
                    stream.Write(headerbytes, 0, headerbytes.Length);
                    using (FileStream fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                    {
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            stream.Write(buffer, 0, bytesRead);
                        }
                    }
                }

                //1.3 form end
                stream.Write(endbytes, 0, endbytes.Length);
            }
            //2.WebResponse
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                return stream.ReadToEnd();
            }
        }

        public static string HttpUploadStream(string url, MemoryStream streamUpload, NameValueCollection data, Encoding encoding)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endbytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            //1.HttpWebRequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Timeout = 30000;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Headers.Add(HttpRequestHeader.Cookie, cookie);

            using (Stream stream = request.GetRequestStream())
            {
                ////1.1 key/value
                //string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                //if (data != null)
                //{
                //    foreach (string key in data.Keys)
                //    {
                //        stream.Write(boundarybytes, 0, boundarybytes.Length);
                //        string formitem = string.Format(formdataTemplate, key, data[key]);
                //        byte[] formitembytes = encoding.GetBytes(formitem);
                //        stream.Write(formitembytes, 0, formitembytes.Length);
                //    }
                //}

                //1.2 file
                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                byte[] buffer = new byte[4096];

               
                stream.Write(boundarybytes, 0, boundarybytes.Length);
                string header = string.Format(headerTemplate, "test1", "UploadFieName.png");
                byte[] headerbytes = encoding.GetBytes(header);
                stream.Write(headerbytes, 0, headerbytes.Length);
                //using (FileStream fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                //{
                //    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                //    {
                //        stream.Write(buffer, 0, bytesRead);
                //    }
                //}


                //using (BinaryReader sr = new BinaryReader(streamUpload))
                //{
                //    while ((bytesRead = sr.Read(buffer, 0, buffer.Length)) != 0)
                //    {
                //        stream.Write(buffer, 0, bytesRead);
                //    }
                //}
                byte[] tempByte = streamUpload.ToArray();
                stream.Write(tempByte, 0, tempByte.Length);

                //while ((bytesRead = streamUpload.Read(buffer, 0, buffer.Length)) != 0)
                //{
                //    stream.Write(buffer, 0, bytesRead);
                //}


                //1.3 form end
                stream.Write(endbytes, 0, endbytes.Length);
            }
            //2.WebResponse
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                return stream.ReadToEnd();
            }
        }




    }
}
