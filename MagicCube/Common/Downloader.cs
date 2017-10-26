
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Threading;

namespace MagicCube.Common
{
    public static class Downloader
    {
        /// <summary>        
        /// 下载图片        
        /// </summary>        
        /// <param name="picUrl">图片Http地址</param>        
        /// <param name="savePath">保存路径</param>        
        /// <param name="timeOut">Request最大请求时间，如果为-1则无限制</param>        
        /// <returns></returns>        
        public static void DownloadPicture(string picUrl, string savePath)
        {
            bool value = false;
            WebResponse response = null;
            Stream stream = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(picUrl);
                request.Timeout = 2000;
                response = request.GetResponse();
                stream = response.GetResponseStream();
                if (!response.ContentType.ToLower().StartsWith("text/"))
                    value = SaveBinaryFile(response, savePath);

            }
            catch (Exception ex)
            {
                Console.WriteLine("DownloadPicture" + ex.Message);
            }
            finally
            {
                if (stream != null) stream.Close();
                if (response != null) response.Close();
            }

        }

        public static bool SaveBinaryFile(WebResponse response, string savePath)
        {
            bool value = false;
            byte[] buffer = new byte[1024];
            Stream outStream = null;
            Stream inStream = null;
            try
            {
                if (File.Exists(savePath)) File.Delete(savePath);
                outStream = System.IO.File.Create(savePath);
                inStream = response.GetResponseStream();
                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0) outStream.Write(buffer, 0, l);
                }
                while (l > 0);
                value = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (outStream != null) outStream.Close();
                if (inStream != null) inStream.Close();
            }
            return value;
        }


        private static string[] colorArray = new string[] { "#c6aafd", "#7dd2a5", "#ffb480", "#89b7f6" };
        private static Random rd = new Random();
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string FilterSpecial(string str)
        {
            if (str == "")
            {
                return str;
            }
            else
            {
                str = str.Replace("'", "");
                str = str.Replace("<", "");
                str = str.Replace(">", "");
                str = str.Replace("%", "");
                str = str.Replace("'delete", "");
                str = str.Replace("''", "");
                str = str.Replace("\"\"", "");
                str = str.Replace(",", "");
                str = str.Replace(".", "");
                str = str.Replace(">=", "");
                str = str.Replace("=<", "");
                str = str.Replace("-", "");
                str = str.Replace("_", "");
                str = str.Replace(";", "");
                str = str.Replace("||", "");
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                str = str.Replace("&", "");
                str = str.Replace("#", "");
                str = str.Replace("/", "");
                str = str.Replace("-", "");
                str = str.Replace("|", "");
                str = str.Replace("?", "");
                str = str.Replace(">?", "");
                str = str.Replace("?<", "");
                str = str.Replace(" ", "");
                return str;
            }
        }
        private static bool isChinese(string str)
        {
            //判断是否存在数字
            for (int i = 0; i < str.Length; i++)
            {
                if ((int)str[i] > 0x4E00 && (int)str[i] < 0x9FA5)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
                
        }


        public static string LocalPath(string avatarUrl,string name, bool isFemale=false)
        {
            string avatarUrlTemp = string.Format(avatarUrl + "?w={0}&h={1}", 120, 120);
            if ((!string.IsNullOrEmpty(avatarUrl)) && avatarUrl.StartsWith("http"))
            {
                if (avatarUrl.StartsWith("http://www.mofanghr.com/static"))
                {
                    if(isFemale)
                    {
                        return AppDomain.CurrentDomain.BaseDirectory + "UserDefaultFemale.png";
                    }
                    return AppDomain.CurrentDomain.BaseDirectory + "UserDefault.png";
                }
                else
                {
                    string picname = avatarUrl.Split('/')[(avatarUrl.Split('/').Count() - 1)];
                    string impath = DAL.ConfUtil.LocalHomePath + MagicGlobal.UserInfo.UserAccount + "/" + picname;
                    if (!File.Exists(impath))
                    {
                        ThreadPool.QueueUserWorkItem((obj) => { Downloader.DownloadPicture(avatarUrlTemp, impath); });
                        return avatarUrlTemp;
                    }
                    else
                    {
                        return impath;
                    }
                    //return impath;
                  
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(name))
                {
                    string nameEnd = string.Empty;
                    if (name.Length >= 2)
                        nameEnd = name.Substring(name.Length - 2,2);
                    else
                        nameEnd = name;
                    if(!isChinese(nameEnd))
                    {
                        if (isFemale)
                        {
                            return AppDomain.CurrentDomain.BaseDirectory + "UserDefaultFemale.png";
                        }
                        return AppDomain.CurrentDomain.BaseDirectory + "UserDefault.png";
                    }

                    string impath = string.Empty;

                    impath = DAL.ConfUtil.LocalHomePath + MagicGlobal.UserInfo.UserAccount + "\\" + nameEnd + ".png";
                    if (File.Exists(impath))
                        return impath;
                    try
                    {
                        Bitmap bmp = new Bitmap(100, 100);
                        Graphics g = Graphics.FromImage(bmp);
                        Font font = new Font("黑体", 24, FontStyle.Bold);
                        //g.FillRectangle(Brushes.White, new Rectangle() { X = 0, Y = 0, Height = 100, Width = 100 });

                        Color _color = System.Drawing.ColorTranslator.FromHtml(colorArray[rd.Next(0, 4)]);
                        Brush br = new SolidBrush(_color);
                        //g.FillEllipse(br, new Rectangle() { X = 0, Y = 0, Height = 100, Width = 100 });
                        g.FillRectangle(br, new Rectangle() { X = 0, Y = 0, Height = 100, Width = 100 });
                        string insetName = string.Empty;
                        if (name.Length < 2)
                        {
                            insetName = name;
                            g.DrawString(insetName, font, Brushes.White, new PointF() { X = 28, Y = 35 });
                        }
                        else
                        {
                            insetName = name.Substring(name.Length - 2, 2);
                            g.DrawString(insetName, font, Brushes.White, new PointF() { X = 12, Y = 35 });
                        }

                        bmp.Save(impath);
                        return impath;
                    }
                    catch { }
                    
                 

                }
                return AppDomain.CurrentDomain.BaseDirectory + "UserDefault.png";
            }
            
        }

        public static string LocalPathCompany(string avatarUrl)
        {
            if (string.IsNullOrWhiteSpace(avatarUrl))
                return AppDomain.CurrentDomain.BaseDirectory + "HRDefault.png";
            if (avatarUrl.StartsWith("http"))
            {
               return  avatarUrl;
            }
            else
            {
                return AppDomain.CurrentDomain.BaseDirectory + "HRDefault.png"; ;
            }

        }

    }
}
