using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.ViewModel
{
    public class MCoinListViewModel:ViewModelBase
    {
        private string _urlLogo = string.Empty;
        public string UrlLogo
        {
            get { return _urlLogo; }
            set
            {
                _urlLogo = value;
                RaisePropertyChanging("UrlLogo");
            }
        }

        private string _triggerName = string.Empty;
        public string TriggerName
        {
            get { return _triggerName; }
            set
            {
                _triggerName = value;
                RaisePropertyChanged("TriggerName");
            }
        }

        private string _logoHeader = string.Empty;

        public string LogoHeader
        {
            get { return _logoHeader; }
            set
            {
                _logoHeader = value;
                RaisePropertyChanged("LogoHeader");
            }
        }

        private string _triggerParams = string.Empty;
        public string TriggerParams
        {
            get { return _triggerParams; }
            set
            {
                _triggerParams = value;
                RaisePropertyChanged("TriggerParams");
            }
        }

        private string _createTime = string.Empty;
        public string CreateTime
        {
            get { return _createTime; }
            set
            {
                _createTime = value;
                RaisePropertyChanged("CreateTime");
            }
        }

        private string _serviceQuantity = string.Empty;
        public string ServiceQuantity
        {
            get { return _serviceQuantity; }
            set
            {
                _serviceQuantity = value;
                RaisePropertyChanged("ServiceQuantity");
            }
        }
    }
}
