using MagicCube.Model;
using MagicCube.TemplateUC;
using MagicCube.View.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MagicCube.Common
{
    public static class DownloadResume
    {
        static string type = "word";
        public static async Task<byte[]> getFile(ResumeDetailModel pResumeDetailModel,string jobName)
        {
            List<string> pkey = new List<string> { "userID", "sourceTime", "type", "B_userID" };
            List<string> pvalue = new List<string> { pResumeDetailModel.userID.ToString(), pResumeDetailModel.resumeTime, type, MagicGlobal.UserInfo.Id.ToString()  };

            if(pResumeDetailModel.deliveryID>0)
            {
                pkey.Add("deliveryID");
                pvalue.Add(pResumeDetailModel.deliveryID.ToString());
            }
            if(!string.IsNullOrWhiteSpace(pResumeDetailModel.downloadType))
            {
                pkey.Add("source");
                pvalue.Add(pResumeDetailModel.downloadType);
            }
            if (!string.IsNullOrWhiteSpace(jobName))
            {
                pkey.Add("jobName");
                pvalue.Add(jobName);
            }
            string std = DAL.JsonHelper.JsonParamsToString(pkey, pvalue);

            return await DAL.HttpHelper.Instance.HttpGetByteAsync(string.Format(DAL.ConfUtil.AddrResumeDownload, MagicGlobal.UserInfo.Version, std));
        }

        public static async Task DownLoadResume(ResumeDetailModel pResumeDetailModel,Window curWindow,string jobName = "")
        {
            if (pResumeDetailModel != null)
            {
                WinDownLoadType pWinDownLoadType = new WinDownLoadType(curWindow);
                if (pWinDownLoadType.ShowDialog() == true)
                {
                    if (pWinDownLoadType.typeWord.IsChecked == true)
                    {
                        type = "word";

                    }
                    if (pWinDownLoadType.typePDF.IsChecked == true)
                    {
                        type = "pdf";

                    }
                    if (pWinDownLoadType.typeHtml.IsChecked == true)
                    {
                        type = "html";

                    }
                    string fileName = pResumeDetailModel.name;
                    if (!string.IsNullOrWhiteSpace(pResumeDetailModel.educationDesc))
                    {
                        fileName += "-" + pResumeDetailModel.educationDesc.Replace("/","|");
                    }
                    fileName += "-魔方面面简历";
                    if (type == "word")
                    {
                        fileName += ".docx";
                    }
                    if (type == "pdf")
                    {
                        fileName += ".pdf";
                    }
                    if (type == "html")
                    {
                        fileName += ".html";
                    }
                    if (string.IsNullOrWhiteSpace(MagicGlobal.UserInfo.DefaultDownloadPath) || MagicGlobal.UserInfo.IsDefaultDownloadPath == false || DownloadResume.IsEnough(MagicGlobal.UserInfo.DefaultDownloadPath) == false)
                    {
                        WinDownLoadPath pWinDownLoadPath = new WinDownLoadPath(curWindow,type, fileName);
                        if (pWinDownLoadPath.ShowDialog() == true)
                        {
                            fileName = pWinDownLoadPath.tbDownloadPath.Text + "/" + fileName;
                           
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        fileName = MagicGlobal.UserInfo.DefaultDownloadPath + "/" + fileName;
                       
                    }
                    fileName = GetNewPathForDupes(fileName);
                    FileStream fs;                  
                    byte[] data = await getFile(pResumeDetailModel,jobName);
                    fs = new FileStream(fileName, FileMode.CreateNew);
                    BinaryWriter br = new BinaryWriter(fs);
                    br.Write(data, 0, data.Length);
                    br.Close();
                    fs.Close();
                    DisappearShow disappear = new DisappearShow("保存成功", 1);
                    disappear.Owner = curWindow;
                    disappear.ShowDialog();
                }
            }
        }
        private static string GetNewPathForDupes(string path)
        {
            string newFullPath = path.Trim();
            //if (System.IO.File.Exists(path))
            //    MessageBox.Show("存在");
            //else
            //    MessageBox.Show("不存在");
            if (System.IO.File.Exists(path))
            {
                string directory = Path.GetDirectoryName(path);
                string filename = Path.GetFileNameWithoutExtension(path);
                string extension = Path.GetExtension(path);
                int counter = 1;
                do
                {
                    //string newFilename = "{0}({1}).{2}".FormatWith(filename, counter, extension);
                    string newFilename = string.Format("{0}({1}){2}", filename, counter, extension);
                    newFullPath = Path.Combine(directory, newFilename);
                    counter++;
                } while (System.IO.File.Exists(newFullPath));
            }
            return newFullPath;
        }
        public static bool IsEnough(string path)
        {
            DriveInfo drive = new DriveInfo(path);
            if (((double)drive.AvailableFreeSpace) <= 1024)
            {
                return false;
            }
            return true;
        }
    }

}

