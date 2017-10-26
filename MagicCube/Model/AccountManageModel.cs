using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;

namespace MagicCube.Model
{
    public class AccountManageModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string name { get; set; }
        public string url { get; set; }
        //public Visibility IsBinding { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string sourceType { get; set; }

        private Visibility isBinding;
        public Visibility IsBinding
        {
            get
            {
                return isBinding;
            }
            set
            {
                isBinding = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsBinding"));
            }

        }

      


    }
        

}
