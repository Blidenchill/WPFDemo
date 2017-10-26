using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MagicCube.Model
{
    public class UserSetting : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
       
        private bool _AutoLogin = false;
        /// <summary>
        ///     自动登陆标示
        /// </summary>
        public bool AutoLogin
        {
            get { return _AutoLogin; }
            set
            {
                _AutoLogin = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AutoLogin"));
            }
        }
        private bool _RememberPassword = false;
        /// <summary>
        ///     记住密码标示
        /// </summary>
        public bool RememberPassword
        {
            get { return _RememberPassword; }
            set
            {
                _RememberPassword = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("RememberPassword"));
            }
        }
        private String userAccount = string.Empty;
        /// <summary>
        ///     用户名/账号
        /// </summary>
        public String UserAccount
        {
            get { return userAccount; }
            set
            {
                userAccount = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }

        private string userMobile = string.Empty;
        public string UserMobile
        {
            get { return userMobile; }
            set
            {
                userMobile = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UserMobile"));
            }
        }

        private string _HideUserName = "";
            public string HideUserName
        {
            get { return _HideUserName; }
            set
            {
                _HideUserName = value;
                if(this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("HideUserName"));
                }
            }
        }

        private String _Password = "";
        /// <summary>
        ///     密码
        /// </summary>
        public String Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        private String _PassLink = "";
        /// <summary>
        ///     密码
        /// </summary>
         [XmlIgnoreAttribute]
        public String PassLink
        {
            get { return _PassLink; }
            set
            {
                _PassLink = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        private String _HXUsername = "";
        /// <summary>
        ///     用户名
        /// </summary>
        public String HXUsername
        {
            get { return _HXUsername; }
            set
            {
                _HXUsername = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("HXUsername"));
            }
        }
        private String _HXPassword = "";
        /// <summary>
        ///     密码
        /// </summary>
        public String HXPassword
        {
            get { return _HXPassword; }
            set
            {
                _HXPassword = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("HXPassword"));
            }
        }

        private String _avatarUrl = "";
        /// <summary>
        ///     头像路径
        /// </summary>
        public String avatarUrl
        {
            get { return _avatarUrl; }
            set
            {
                _avatarUrl = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("avatarUrl"));
            }
        }

        private String _RealName = "";
        /// <summary>
        ///     真实姓名
        /// </summary>
        public String RealName
        {
            get { return _RealName; }
            set
            {
                _RealName = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("RealName"));
            }
        }

        private string companyName = string.Empty;
        public string CompanyName
        {
            get { return companyName; }
            set 
            {
                companyName = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CompanyName"));
            }
        }

        public int CompanyId { get; set; }

        private string _briefName = string.Empty;
        public string briefName
        {
            get { return _briefName; }
            set
            {
                _briefName = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("briefName"));
            }
        }

        private String _Email = "";
        /// <summary>
        ///     真实姓名
        /// </summary>
        public String Email
        {
            get { return _Email; }
            set
            {
                _Email = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }
        private string areaCode = string.Empty;
        public string AreaCode
        {
            get { return areaCode; }    
            set
            {
                areaCode = value;
                if(this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AreaCode"));
                }
            }
        }
        private string officePhone = string.Empty;
        public string OfficePhone
        {
            get { return officePhone; }
            set
            {
                officePhone = value;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("OfficePhone"));
                }
            }
        }

        private string extensionNumber = string.Empty;
        public string ExtensionNumber
        {
            get { return extensionNumber; }
            set
            {
                extensionNumber = value;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("ExtensionNumber"));
                }
            }
                
        }

        private string userPosition = string.Empty;
        /// <summary>
        /// 用户职位信息
        /// </summary>
        public string UserPosition
        {
            get { return userPosition; }
            set
            {
                userPosition = value;
                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("UserPosition"));
                }
            }
        }



        private bool autoRunSetting = false;
        public bool AutoRunSetting
        {
            get { return autoRunSetting; }
            set
            {
                autoRunSetting = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AutoRunSetting"));
            }
        }

        private bool existAppTip = false;
        public bool ExistAppTip
        {
            get { return existAppTip; }
            set
            {
                existAppTip = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(("ExistAppTip")));
            }
        }

        private bool windowsFrontMost = false;
        public bool WindowsFrontMost
        {
            get { return windowsFrontMost; }
            set
            {
                windowsFrontMost = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("WindowsFrontMost"));
            }
        }

        private bool windowsMinWhenClose = false;
        public bool WindowsMinWhenClose
        {
            get { return windowsMinWhenClose; }
            set
            {
                windowsMinWhenClose = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(("WindowsMinWhenClose")));
            }
        }

        private bool enterSendChoose = false;
        public bool EnterSendChoose
        {
            get { return enterSendChoose; }
            set
            {
                enterSendChoose = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("EnterSendChoose"));
            }
        }

        private bool ctrlEnterSendChoose;
        public bool CtrlEnterSendChoose
        {
            get { return ctrlEnterSendChoose; }
            set
            {
                ctrlEnterSendChoose = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CtrlEnterSendChoose"));
            }
        }

        private bool isQuipReplySend;
        public bool IsQuipReplySend
        {
            get { return isQuipReplySend; }
            set
            {
                isQuipReplySend = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsQuipReplySend"));
            }
        }

        private bool noHelpTipOfResumeView;
        public bool NoHelpTipOfResumeView
        {
            get { return noHelpTipOfResumeView; }
            set
            {
                noHelpTipOfResumeView = value;
                if(this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("NoHelpTipOfResumeView"));
                }
            }
        }

        private bool noHelpTipOfConnect;
        public bool NoHelpTipOfConnect
        {
            get { return noHelpTipOfConnect; }
            set
            {
                noHelpTipOfConnect = value;
                if(this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("NoHelpTipOfConnect"));
                }
            }
        }

        public int Id { get; set; }

        private bool _validStatus;
        public bool validStatus
        {
            get { return _validStatus; }
            set
            {
                _validStatus = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("validStatus"));
                }
            }
        }

        private string defaultDownloadPath;
        public string DefaultDownloadPath
        {
            get { return defaultDownloadPath; }
            set
            {
                defaultDownloadPath = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DefaultDownloadPath"));
                }
            }
        }

        private string messageNoticeTime;
        public string MessageNoticeTime
        {
            get { return messageNoticeTime; }
            set
            {
                messageNoticeTime = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MessageNoticeTime"));
                }
            }
        }

        private bool isDefaultDownloadPath;
        public bool IsDefaultDownloadPath
        {
            get { return isDefaultDownloadPath; }
            set
            {
                isDefaultDownloadPath = value;
                if(this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsDefaultDownloadPath"));
                }
            }
        }

        private bool isPassTip = true;
        public bool IsPassTip
        {
            get { return isPassTip; }
            set
            {
                isPassTip = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsPassTip"));
                }
            }
        }

        private bool isFailTip = true;
        public bool IsFailTip
        {
            get { return isFailTip; }
            set
            {
                isFailTip = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsFailTip"));
                }
            }
        }
        private int isHRAuth;
        public int IsHRAuth
        {
            get { return isHRAuth; }
            set
            {
                isHRAuth = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsHRAuth"));
                }
            }
        }
        private int isSimilarAuth;
        public int IsSimilarAuth
        {
            get { return isSimilarAuth; }
            set
            {
                isSimilarAuth = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsSimilarAuth"));
                }
            }
        }
        public string Version { get; set; }
        public bool PasswordExist { get; set; }
        /// <summary>
        /// 推送系统设备秘钥，注册时取得
        /// </summary>
        public string PushSecureKey { get; set; }
        /// <summary>
        /// 推送系统设备唯一标示
        /// </summary>
        public string PushDeviceID { get; set; }

    }
}
