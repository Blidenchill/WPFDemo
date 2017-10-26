using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace MagicCube.Common
{
    public class AutoRunSetting
    {
        public static void SetAutoRun(bool isAutoRun)
        {
            RegistryKey reg = null;
            string fileName = GetApplicationPath();
            try
            {
                if (!System.IO.File.Exists(fileName))
                    throw new Exception("该文件不存在!");
                RegistryKey HKLM = Registry.CurrentUser;
                reg = HKLM.CreateSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");


                String name = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                //reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                //if (reg == null)
                //    reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (isAutoRun)
                    reg.SetValue(name, fileName);
                else
                    reg.SetValue(name, false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (reg != null)
                    reg.Close();
            }

        }

        public static void JudgeAutoRun()
        {
            RegistryKey reg = null;
            string fileName = GetApplicationPath();
            RegistryKey HKLM = Registry.CurrentUser;
            reg = HKLM.OpenSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            if (reg == null)
                return;
            String name = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
            object result = reg.GetValue(name);
            if(result == null)
            {
                MagicGlobal.UserInfo.AutoRunSetting = false;
                return;
            }

            else if(result.ToString().ToLower() == "false")
            {
                MagicGlobal.UserInfo.AutoRunSetting = false;
            }
            else if(result.ToString() == fileName)
            {
                MagicGlobal.UserInfo.AutoRunSetting = true;
            }
        }
        private static string GetApplicationPath()
        {
            RegistryKey key = Registry.LocalMachine;
            RegistryKey temp;
            if (Environment.Is64BitOperatingSystem)
                temp = key.OpenSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Mofanghr");
            else
                temp = key.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Mofanghr");
            if (temp == null)
                return string.Empty;
            string path = temp.GetValue("InstallString").ToString();
            return path;
        }
    }
}
