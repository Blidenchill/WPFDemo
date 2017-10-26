using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// LinkButton.xaml 的交互逻辑
    /// </summary>
    public partial class CollectionBottomButton : Button
    {
        public CollectionBottomButton()
        {
            InitializeComponent();
        }

        public bool isCollection
        {
            get { return (bool)GetValue(isCollectionProperty); }
            set { SetValue(isCollectionProperty, value); }
        }

        public static readonly DependencyProperty isCollectionProperty =
            DependencyProperty.Register("isCollection", typeof(bool), typeof(CollectionBottomButton), new PropertyMetadata(null));

    }
}
