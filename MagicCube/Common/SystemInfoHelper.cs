using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.Common
{
    public static class SystemInfoHelper
    {
        private static string SystemInfo = string.Empty;
        public static string GetSystemInfo()
        {
            if(string.IsNullOrEmpty(SystemInfo))
            {
                //获取系统信息
                System.OperatingSystem osInfo = System.Environment.OSVersion;
                SystemInfo = osInfo.Platform.ToString() + "_" + osInfo.Version.ToString();
            }
            return SystemInfo;
        }

        /// <summary>
        /// 判断操作系统是否为XP操作系统
        /// </summary>
        /// <returns></returns>
        public static bool IsWindowsXP
        {
            get
            {
                return (Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor == 1);
            }
        }

        public static bool IsWindows7
        {
            get
            {
                return (Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major == 6) && (Environment.OSVersion.Version.Minor == 1);
            }
        }

        public static bool IsWindowsVista
        {
            get
            {
                return (Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major == 6) && (Environment.OSVersion.Version.Minor == 0);
            }
        }
    }
}
