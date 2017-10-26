using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MagicCube.HttpModel
{
    public class HttpCommonReply:INotifyPropertyChanged
    {
           public event PropertyChangedEventHandler PropertyChanged;
        public string status { get; set; }
        private List<ContentCommonReply> _result = new List<ContentCommonReply>();
        public List<ContentCommonReply> result
        {
            get { return _result; }
            set
            {
                _result = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("result"));
            }
        }
    }
    public class ContentCommonReply : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _context = string.Empty;
        public string context
        {
            get { return _context; }
            set
            {
                _context = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("context"));
            }
        }

        private string _title = string.Empty;
        public string title
        {
            get { return _title; }
            set
            {
                _title = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("title"));
            }
        }
    }
}
