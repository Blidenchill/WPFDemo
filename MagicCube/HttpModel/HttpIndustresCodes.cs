using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MagicCube.HttpModel
{
    public class HttpIndustresCodes
    {
        public int status { get; set; }
        public List<HttpIndustresResult> result { get; set; }
    }
    public class HttpIndustresResult
    {
        public string updatedTime { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int level { get; set; }
        public bool deleted { get; set; }
        public int parentLevel { get; set; }
        public string ownerType { get; set; }
        public string parentCode { get; set; }
        public string createdTime { get; set; }
        public int id { get; set; }
    }

    public class HttpIndustresItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string code { get; set; }
        public string name { get; set; }

        private bool _isChoose;
        public bool isChoose
        {
            get { return _isChoose; }
            set
            {
                this._isChoose = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("isChoose"));
            }
        }
    }

}
