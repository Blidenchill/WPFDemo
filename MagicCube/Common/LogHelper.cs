
using log4net;
using log4net.Appender;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace MagicCube.Common
{
   public class LogHelper
    {
       public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        public static void SetConfig()
        {
           
            log4net.Config.XmlConfigurator.Configure();
            var repository = LogManager.GetRepository();
            var appenders = repository.GetAppenders();
            var targetApder = appenders.First(p => p.Name == "InfoAppender") as RollingFileAppender;
            targetApder.File = DAL.ConfUtil.LocalHomePath + "InfoLog.log";
            var targetApderError = appenders.First(p => p.Name == "ErrorAppender") as RollingFileAppender;
            targetApderError.File = DAL.ConfUtil.LocalHomePath + "ErrorLog.log";
            targetApder.ActivateOptions();
        }

        public static void SetConfig(FileInfo configFile)
        {
            log4net.Config.XmlConfigurator.Configure(configFile);
        }
        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        #region static void WriteLog(Type t, Exception ex)

        public static void WriteErrorLog(Exception ex)
        {
            logerror.Error("Error", ex);
        }

        #endregion

        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        #region static void WriteLog(Type t, string msg)

        public static void WriteLog(string msg)
        {
            loginfo.Info(msg);
        }

        #endregion


    }
}