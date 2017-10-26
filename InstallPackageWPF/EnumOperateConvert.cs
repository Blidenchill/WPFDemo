using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace InstallPackageWPF
{
    public class EnumOperateConvert 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            OperateType type = (OperateType)value;
            string returnType = string.Empty;
            switch(type)
            {
                case OperateType.Add:
                    returnType = "增加文件";
                    break;
                case OperateType.Del:
                    returnType = "删除文件";
                    break;
                case OperateType.Run:
                    returnType = "运行文件";
                    break;
                case OperateType.Update:
                    returnType = "替换文件";
                    break;
            }
            return returnType;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            OperateType type = OperateType.Add;
            switch(value.ToString())
            {
                case "增加文件":
                    type = OperateType.Add;
                    break;
                case "删除文件":
                    type = OperateType.Del;
                    break;
                case "运行文件":
                    type = OperateType.Run;
                    break;
                case "替换文件":
                    type = OperateType.Update;
                    break;
            }
            return type;
        }

        public string[] EnumConvert(Type enumType)
        {
            string[] types = { "增加文件", "增加文件", "运行文件", "替换文件" };
            return types;
            
        }
    }
}
