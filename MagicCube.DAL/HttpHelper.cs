using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Net.Http.Headers;

namespace MagicCube.DAL
{
    public class HttpHelper
    {
        #region "单例"
        private static HttpHelper instance;
        private static object objLock = new object();
        public static HttpHelper Instance
        {
            get
            {
                if(instance == null)
                {
                    lock(objLock)
                    {
                        if(instance == null)
                        {
                            instance = new HttpHelper();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region "变量"
        private HttpClient client;
        private HttpClientHandler handle;
        private string curUrl = string.Empty;
        private List<Cookie> cookieList = new List<Cookie>();
        private string cookieSavePath = string.Empty;

        public delegate Task AutoLoginDelegate();
        public AutoLoginDelegate autoLoginActionDelegate;
        public Action AutoLoginAction;
        #endregion

        #region "构造函数"
        private HttpHelper()
        {
            handle = new HttpClientHandler();
            handle.UseCookies = true;
            client = new HttpClient(handle);
            //三十秒延时
            client.Timeout = new TimeSpan(0, 0, 30);
            cookieSavePath = ConfUtil.LocalHomePath + "cookie";
        }

        #endregion

        #region "HttpGet"
        public async Task<string> HttpGetAsync(string url)
        {
            try
            {
                this.curUrl = url;
                HttpResponseMessage msg = await client.GetAsync(url);
              
                
                string temp = await msg.Content.ReadAsStringAsync();
                //string temp = await client.GetStringAsync(url);
                if(!url.Contains("loginByUserId.json"))
                {
                    if (msg.Headers.Contains("Set-Cookie"))
                    {
                        var temp3 = msg.Headers.GetValues("Set-Cookie");
                        handle = new HttpClientHandler();
                        handle.UseCookies = true;
                        client = new HttpClient(handle);
                        foreach (var item in temp3)
                        {
                            string[] split = item.Split(new char[] { '=', ';' });
                            if (split.Length >= 2)
                                handle.CookieContainer.Add(new Cookie() { Domain = "desktop-application.mofanghr.com", Name = split[0], Value = split[1], Path = "/" });

                        }
                        if (this.autoLoginActionDelegate != null)
                            await this.autoLoginActionDelegate();
                    }
                
                }

                return temp;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            
        }

        public async Task<string> HttpLogoutGetAsync(string url)
        {
            try
            {
                this.curUrl = url;
                HttpResponseMessage msg = await client.GetAsync(url);


                string temp = await msg.Content.ReadAsStringAsync();
                //string temp = await client.GetStringAsync(url);
                

                return temp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        public async Task<string> HttpFirstAutoLoginGetAsync(string url)
        {
            try
            {
                this.curUrl = url;
                HttpResponseMessage msg = await client.GetAsync(url);
                string temp = await msg.Content.ReadAsStringAsync();
                //string temp = await client.GetStringAsync(url);
                if (url.Contains("loginByUserId.json"))
                {
                    if (msg.Headers.Contains("Set-Cookie"))
                    {
                        var temp3 = msg.Headers.GetValues("Set-Cookie");

                        handle = new HttpClientHandler();
                        handle.UseCookies = true;
                        client = new HttpClient(handle);
                        foreach (var item in temp3)
                        {
                            string[] split = item.Split(new char[] { '=', ';' });
                            if (split.Length >= 2)
                                handle.CookieContainer.Add(new Cookie() { Domain = "desktop-application.mofanghr.com", Name = split[0], Value = split[1], Path = "/" });

                        }
                    }

                }

                return temp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }
        public async Task<byte[]> HttpGetByteAsync(string url)
        {
            try
            {
                this.curUrl = url;
                HttpResponseMessage msg = await client.GetAsync(url);
                byte[] temp = await msg.Content.ReadAsByteArrayAsync();
                //string temp = await client.GetStringAsync(url);
                if (!url.Contains("loginByUserId.json"))
                {
                    if (msg.Headers.Contains("Set-Cookie"))
                    {
                        var temp3 = msg.Headers.GetValues("Set-Cookie");

                        handle = new HttpClientHandler();
                        handle.UseCookies = true;
                        client = new HttpClient(handle);
                        foreach (var item in temp3)
                        {
                            string[] split = item.Split(new char[] { '=', ';' });
                            if (split.Length >= 2)
                                handle.CookieContainer.Add(new Cookie() { Domain = "desktop-application.mofanghr.com", Name = split[0], Value = split[1], Path = "/" });

                        }
                        if (this.autoLoginActionDelegate != null)
                            await this.autoLoginActionDelegate();
                    }

                }

                return temp;
            }
            catch
            {
                return null;
            }

        }
        public async Task<Stream> HttpGetStreamAsync(string url)
        {
            try
            {
                this.curUrl = url;
                HttpResponseMessage msg = await client.GetAsync(url);

                Stream temp = await msg.Content.ReadAsStreamAsync();

                //string temp = await client.GetStringAsync(url);
                if (!url.Contains("loginByUserId.json"))
                {
                    if (msg.Headers.Contains("Set-Cookie"))
                    {
                        var temp3 = msg.Headers.GetValues("Set-Cookie");

                        handle = new HttpClientHandler();
                        handle.UseCookies = true;
                        client = new HttpClient(handle);
                        foreach (var item in temp3)
                        {
                            string[] split = item.Split(new char[] { '=', ';' });
                            if (split.Length >= 2)
                                handle.CookieContainer.Add(new Cookie() { Domain = "desktop-application.mofanghr.com", Name = split[0], Value = split[1], Path = "/" });

                        }
                        if (this.autoLoginActionDelegate != null)
                            await this.autoLoginActionDelegate();
                    }

                }

                return temp;
            }
            catch
            {
                return null;
            }

        }
        public string HttpGet(string url)
        {
            try
            {
                this.curUrl = url;
                HttpResponseMessage msg = client.GetAsync(url).Result;
                string strTemp = msg.Content.ReadAsStringAsync().Result;
                if (!url.Contains("loginByUserId.json"))
                {
                    if (msg.Headers.Contains("Set-Cookie"))
                    {
                        var temp3 = msg.Headers.GetValues("Set-Cookie");

                        handle = new HttpClientHandler();
                        handle.UseCookies = true;
                        client = new HttpClient(handle);
                        foreach (var item in temp3)
                        {
                            string[] split = item.Split(new char[] { '=', ';' });
                            if (split.Length >= 2)
                                handle.CookieContainer.Add(new Cookie() { Domain = "desktop-application.mofanghr.com", Name = split[0], Value = split[1], Path = "/" });

                        }
                        if (this.AutoLoginAction != null)
                            this.AutoLoginAction();
                    }
                }
                   

                return strTemp;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
          
        }

        #endregion

        #region "HttpPost"
        public async Task<string> HttpPostAsync(string url, string strJson)
        {
            try
            {
                this.curUrl = url;
                var content = new StringContent(strJson, Encoding.UTF8, "application/json");
                HttpResponseMessage msg = await client.PostAsync(url, content);
                string strTemp = await msg.Content.ReadAsStringAsync();

                if (msg.Headers.Contains("Set-Cookie"))
                {

                    var temp3 = msg.Headers.GetValues("Set-Cookie");

                    handle = new HttpClientHandler();
                    handle.UseCookies = true;
                    client = new HttpClient(handle);
                    foreach (var item in temp3)
                    {
                        string[] split = item.Split(new char[] { '=', ';' });
                        if (split.Length >= 2)
                            handle.CookieContainer.Add(new Cookie() { Domain = "desktop-application.mofanghr.com", Name = split[0], Value = split[1], Path = "/" });

                    }
                    if (this.autoLoginActionDelegate != null)
                        await this.autoLoginActionDelegate();
                }
              

                return strTemp;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
         
        }

        public string HttpPost(string url, string strJson)
        {
            try
            {
                this.curUrl = url;
                var content = new StringContent(strJson, Encoding.UTF8, "application/json");
                HttpResponseMessage msg = client.PostAsync(url, content).Result;
                string strTemp = msg.Content.ReadAsStringAsync().Result;
                if (!url.Contains("loginByUserId.json"))
                {
                    if (msg.Headers.Contains("Set-Cookie"))
                    {

                        var temp3 = msg.Headers.GetValues("Set-Cookie");

                        handle = new HttpClientHandler();
                        handle.UseCookies = true;
                        client = new HttpClient(handle);
                        foreach (var item in temp3)
                        {
                            string[] split = item.Split(new char[] { '=', ';' });
                            if (split.Length >= 2)
                                handle.CookieContainer.Add(new Cookie() { Domain = "desktop-application.mofanghr.com", Name = split[0], Value = split[1], Path = "/" });

                        }

                        if (this.AutoLoginAction != null)
                            this.AutoLoginAction();
                    }
                }
                return strTemp;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
       
        }

        #endregion

        #region "上传文件"

        private StreamContent CreateFileContent(Stream stream, string fileName, string contentType)
        {
            try
            {
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = fileName,
                    FileName = fileName,
                }; // the extra quotes are key here
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                return fileContent;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<string> HttpUploadFile(string url, Stream stream, string fileName)
        {
            try
            {
                var content = new MultipartFormDataContent();
                StreamContent sContent = CreateFileContent(stream, fileName, "image/jpeg");
                content.Add(sContent);
                HttpResponseMessage temp = await client.PostAsync(new Uri(url), content);
                string result = await temp.Content.ReadAsStringAsync();
                
                return result;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
           
        }
       
        #endregion

        #region "Cookie操作"


        public void GetCookie()
        {
            try
            {
                this.cookieList.Clear();
                foreach (Cookie item in this.handle.CookieContainer.GetCookies(new Uri(this.curUrl)))
                {
                    this.cookieList.Add(item);
                }



            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
           
        }

        public void SaveCookie()
        {
            GetCookie();

            byte[] data = SerializeHelper.SerializeObject(this.cookieList);
            using (FileStream fs = new FileStream(this.cookieSavePath, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(data);
                }
            }
        }
        public void ClearCookieFile()
        {
            File.Delete(this.cookieSavePath);
        }
        public void SetCookie()
        {
            try
            {
                foreach(var item in this.cookieList)
                {
                    string url = item.Domain;
                    
                    this.handle.CookieContainer.Add(item);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public bool ReadCookie()
        {
            try
            {
                if (File.Exists(this.cookieSavePath))
                {
                    List<byte> tempList = new List<byte>();
                    using (FileStream fs = new FileStream(this.cookieSavePath, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            int lengthTag = 0;
                            do
                            {
                                byte[] temp = br.ReadBytes(1024);
                                lengthTag = temp.Length;
                                tempList.AddRange(temp);
                            }
                            while (lengthTag == 1024);
                        }
                    }
                    this.cookieList = SerializeHelper.DeserializeObject(tempList.ToArray()) as List<Cookie>;
                    SetCookie();
                    if(this.cookieList != null)
                    {
                        if(this.handle.CookieContainer.Count >= 2)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
           
        }

        public string GetCookieToken()
        {
            GetCookie();
            return this.cookieList[0].Value;
        }
        public string GetCookieTime()
        {
            GetCookie();
            return this.cookieList[1].Value;
        }

        #endregion
    }
}
