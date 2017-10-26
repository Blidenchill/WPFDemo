using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;


namespace MagicCube.Model
{
    public class TagsMenuModel : NotifyBaseModel
    {
        private string tagContent = string.Empty;
        public string TagContent
        {
            get { return tagContent; }
            set
            {
                tagContent = value;
                NotifyPropertyChange(() => TagContent);
            }
        }

        private string tagColor = string.Empty;
        public string TagColor
        {
            get { return tagColor; }
            set
            {
                tagColor = value;
                NotifyPropertyChange(() => TagColor);
            }
        }
        private int id = 0;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                NotifyPropertyChange(() => Id);
            }
        }

        private bool isChecked = false;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                NotifyPropertyChange(() => IsChecked);
            }
        }
    }
}
