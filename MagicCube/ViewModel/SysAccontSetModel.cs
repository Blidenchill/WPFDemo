using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using MagicCube.Command;

namespace MagicCube.ViewModel
{
    public class SysAccontSetModel : Model.NotifyBaseModel
    {
        public SysAccontSetModel()
        {
            CloseCommand = new RelayCommand(CloseCallback);
            ChangeAccoutCommand = new RelayCommand(ChangeAccountCallback);
            Messaging.Messenger.Default.Register<string> (this, UpdateDataCallback);
        }

        private string avatarUrl = string.Empty;
        public string AvatarUrl
        {
            get { return avatarUrl; }
            set
            {
                avatarUrl = value;
                NotifyPropertyChange(() => AvatarUrl);
            }
        }

        private string hrName = string.Empty;
        public string HrName
        {
            get { return hrName; }
            set
            {
                hrName = value;
                NotifyPropertyChange(() => HrName);
            }
        }

        private string hrPosition = string.Empty;
        public string HrPosition
        {
            get { return hrPosition; }
            set
            {
                hrPosition = value;
                NotifyPropertyChange(() => hrPosition);
            }
        }

        public RelayCommand CloseCommand { get; private set; }

        public RelayCommand ChangeAccoutCommand { get; private set; }


        private void CloseCallback()
        {
            Console.WriteLine("关闭程序");
            Messaging.Messenger.Default.Send<Messaging.MSCloseWindow, ViewSingle.MainWindow>(null);
        }
        private void ChangeAccountCallback()
        {
            Messaging.Messenger.Default.Send<Messaging.MSChangeAccount, ViewSingle.MainWindow>(null);
        }

        public void UpdateDataCallback(string sender)
        {

            AvatarUrl = MagicGlobal.UserInfo.avatarUrl;
            HrPosition = MagicGlobal.UserInfo.UserPosition;
            HrName = MagicGlobal.UserInfo.RealName;
        }

        
    }
}
