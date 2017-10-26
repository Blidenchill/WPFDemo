using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MagicCube.Model;
using System.Xml;

namespace MagicCube.HttpModel
{
    public class CurrentUserInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private long zhilianBindingInterval = 0;

        public long ZhilianBindingInterval
        {
            get { return zhilianBindingInterval; }
            set
            {
                zhilianBindingInterval = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ZhilianBindingInterval"));
                }
            }
        }

        private long zhilianDownJobInterval = 0;
        public long ZhilianDownJobInterval
        {
            get { return zhilianDownJobInterval; }
            set
            {
                zhilianDownJobInterval = value;
                if(this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ZhilianDownJobInterval"));
                }
            }
        }
        public string ZhilianTipShowTime
        {
            get;
            set;
        }

        public bool HandCloseZLTip
        {
            get;
            set;
        }
        public bool HandArriveConfirm
        {
            get;
            set;
        }
        private string defaultSendEmail;
        public string DefaultSendEmail
        {
            get { return defaultSendEmail; }
            set
            {
                defaultSendEmail = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DefaultSendEmail"));
                }
            }
        }
        public List<string> InterviewIdToUpdateTime { get; set; }
    }
}
