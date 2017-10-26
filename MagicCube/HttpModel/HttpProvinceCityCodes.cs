using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MagicCube.HttpModel
{
    public class HttpProvinceCityCodes : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public List<HttpCityCodes> city { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        private bool _isSelectedCity;
     
        public bool isSelectedCity
        {
            get { return _isSelectedCity; }
            set
            {
                _isSelectedCity = value;
                if(PropertyChanged !=null)
                    PropertyChanged(this, new PropertyChangedEventArgs("isSelectedCity"));
            }
        }

        public int selectedCityCount { get; set; }
    }

    public class HttpCityCodes : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string code { get; set; }
        public string name { get; set; }
        private bool _isSelected;
        public bool isSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("isSelected"));
            }
        }
    }

    public class NameCodes
    {
        public string code { get; set; }
        public string name { get; set; }
    }
}
