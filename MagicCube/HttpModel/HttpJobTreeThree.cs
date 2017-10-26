using MagicCube.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicCube.HttpModel
{
    public class Val2 : NotifyBaseModel
    {
        public string code { get; set; }
        public string name { get; set; }
        private bool _isChoose;
        public bool isChoose
        {
            get { return _isChoose; }
            set
            {
                _isChoose = value;
                NotifyPropertyChange(() => isChoose);
            }
        }
        public string parentCode { get; set; }
        public Val parent
        {
            get;
            set;
        }
    }

    public class Val : NotifyBaseModel
    {
        public HttpJobTreeThree parent
        {
            get;
            set;
        }
        public string parentCode { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        private bool _isChoose;
        public bool isChoose
        {
            get { return _isChoose; }
            set
            {
                _isChoose = value;
                NotifyPropertyChange(() => isChoose);
            }
        }
        public List<Val2> val { get; set; }
    }

    public class HttpJobTreeThree : NotifyBaseModel
    {
        public string code { get; set; }
        public string name { get; set; }
        public List<Val> val { get; set; }

        private bool _IsCheck;
        public bool IsCheck
        {
            get { return _IsCheck; }
            set
            {
                _IsCheck = value;
                NotifyPropertyChange(() => IsCheck);
            }
        }
    }

    public class ValCommon1
    {
        public string code { get; set; }
        public string name { get; set; }

        public string parentCode { get; set; }

        public List<ValCommon2> ChildList { get; set; }
        
    }
    public class ValCommon2
    {
        public string code1 { get; set; }
        public string name2 { get; set; }

        public string parentCode1 { get; set; }

       

    }
}
