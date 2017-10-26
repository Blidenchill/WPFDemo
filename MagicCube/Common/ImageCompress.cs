using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.IO;

namespace MagicCube.Common
{
    public class ImageCompress
    {
        private static bool GetPicThumbnail(string sFile, string outPath, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;

            //以下代码为保存图片时，设置压缩质量  
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100  
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    iSource.Save(outPath, jpegICIinfo, ep);//dFile是压缩后的新路径  
                }
                else
                {
                    iSource.Save(outPath, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                iSource.Dispose();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFile"></param>
        /// <param name="outPutPath"></param>
        /// <returns> 当返回为false，说明没有压缩</returns>
        public static bool CompressTo1M(string sFile, string outPutPath)
        {
            FileInfo fileInfo = new FileInfo(sFile);
            long len = fileInfo.Length;
            long oneMLen = 1024 * 1024;
            if (len < oneMLen)
            {
                return false;
            }
            int quality = (int)(oneMLen * 100 / len);
           return  GetPicThumbnail(sFile, outPutPath, quality);
            
        }
    }
}
