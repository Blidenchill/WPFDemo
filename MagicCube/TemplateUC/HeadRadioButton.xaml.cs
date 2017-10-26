﻿using System;
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
    public partial class HeadRadioButton : RadioButton
    {
        public HeadRadioButton()
        {
            
            InitializeComponent();
        }


        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Hint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(HeadRadioButton), new PropertyMetadata(null));


    }
}
