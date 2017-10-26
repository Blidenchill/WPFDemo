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
    public partial class ResumeListButton : Button
    {
        public ResumeListButton() 
        {
            InitializeComponent();
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ResumeListButton), new PropertyMetadata(null));
        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(ResumeListButton), new PropertyMetadata(null));
        public string ImageHover
        {
            get { return (string)GetValue(ImageHoverProperty); }
            set { SetValue(ImageHoverProperty, value); }
        }
         
        public static readonly DependencyProperty ImageHoverProperty =
            DependencyProperty.Register("ImageHover", typeof(string), typeof(ResumeListButton), new PropertyMetadata(null));

    }
}
