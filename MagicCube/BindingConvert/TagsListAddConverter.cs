using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using System.Windows;
using System.Collections.ObjectModel;
using MagicCube.HttpModel;

namespace MagicCube.BindingConvert
{
    public class TagsListAddConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            ObservableCollection<TagV> ptagVs = new ObservableCollection<TagV>();

            if(value as ObservableCollection<TagV> !=null)
            {
                foreach(TagV iTagV in value as ObservableCollection<TagV>)
                {
                    ptagVs.Add(new TagV() { id = iTagV.id,color = iTagV.color, flag=iTagV.flag, name = iTagV.name });
                }
            }
            ptagVs.Add(new TagV() { id = -2 });
            return ptagVs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
