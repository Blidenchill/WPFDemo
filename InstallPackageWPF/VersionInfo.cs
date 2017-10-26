using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;

namespace InstallPackageWPF
{
    class VersionInfo : DependencyObject
    {
        public VersionInfo()
        {

        }
        public bool IsForceUpdate { get; set; }

        public bool IsForceDownLoadSetupPackage { get; set; }
        /// <summary>
        /// 版本更新信息描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 版本信息
        /// </summary>
        public string VersionName { get; set; }
        //测试
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        private ObservableCollection<VersionFileInfo> updateFileList = new ObservableCollection<VersionFileInfo>();
        /// <summary>
        /// 需要更新的文件信息
        /// </summary>
        public ObservableCollection<VersionFileInfo> UpdateFileList
        {
            get
            {
                return updateFileList;
            }
            set
            {
                updateFileList = value;
            }
        }



        public long TotalFileSize
        {
            get { return (long)GetValue(TotalFileSizeProperty); }
            set { SetValue(TotalFileSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalFileSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalFileSizeProperty =
            DependencyProperty.Register("TotalFileSize", typeof(long), typeof(VersionInfo));

        

    }

    public class VersionFileInfo
    {
        private string fileName;
        /// <summary>
        /// 文件名称，不含路径
        /// </summary>
        public string FileName { get { return fileName; } set { fileName = value; } }

        private string description;
        /// <summary>
        /// 文件描述
        /// </summary>
        public string Description { get { return description; } set { description = value; } }

        private string relativePath;
        /// <summary>
        /// 相对于要升级的主程序的路径
        /// </summary>
        public string RelativePath { get { return relativePath; } set { relativePath = value; } }

        private string realPath;
        /// <summary>
        /// 文件的实际路径
        /// </summary>
        public string LocalRealPath { get { return realPath; } set { realPath = value; } }

        private OperateType fileOperateType;
        /// <summary>
        /// 升级方式
        /// </summary>
        public OperateType FileOperateType { get { return fileOperateType; } set { fileOperateType = value; } }

        private long fileSize;
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get { return fileSize; } set { fileSize = value; } }
        /// <summary>
        /// 文件下载的url路径
        /// </summary>
        public string DownloadUrl { get; set; }
    }
    [Serializable]
    public enum OperateType
    {
        /// <summary>
        /// 新添加的文件
        /// </summary>
        Add,
        /// <summary>
        /// 需要替换的文件
        /// </summary>
        Update,
        /// <summary>
        /// 需要删除的文件
        /// </summary>
        Del,
        /// <summary>
        /// 需要运行的文件
        /// </summary>
        Run
    }
}
