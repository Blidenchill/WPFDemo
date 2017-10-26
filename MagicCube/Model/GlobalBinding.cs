using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace MagicCube.Model
{
    public class MainHintBinding : NotifyBaseModel
    {
        private bool _OpenPhblish = false;
        public bool OpenPhblish
        {
            get { return _OpenPhblish; }
            set
            {
                _OpenPhblish = value;
                NotifyPropertyChange(() => OpenPhblish);
            }
        }
    }
}
