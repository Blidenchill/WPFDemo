using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace InstallPackageWPF
{
    public class SaveSetting :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _saveFileZipPath;
        public string SaveFileZipPath
        {
            get { return _saveFileZipPath; }
            set
            {
                _saveFileZipPath = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs("SaveFileZipPath"));
            }
        }

        private string _versionID;
        public string VersionID
        {
            get { return _versionID; }  
            set
            {
                _versionID = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs("VersionID"));
            }
        }
        private string _updateInfo;
        public string UpdateInfo
        {
            get { return _updateInfo; }
            set
            {
                _updateInfo = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs("UpdateInfo"));
            }
        }

        private string[] _addFilePathList;
        public string[] AddFilePathList
        {
            get { return _addFilePathList; }    
            set
            {
                _addFilePathList = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs("AddFilePathList"));
            }
        }
        
    }
}
