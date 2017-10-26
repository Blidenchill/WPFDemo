using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MagicCube.Model
{

    public class NotifyBaseModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChange<T>(Expression<Func<T>> expression)
        {

            if (PropertyChanged != null)
            {

                var propertyName = ((MemberExpression)expression.Body).Member.Name;

                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }

        }

    }
}
