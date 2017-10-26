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
using System.Windows.Shapes;
using MagicCube.Common;

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// WinNameEdit.xaml 的交互逻辑
    /// </summary>
    public partial class WinNameEdit : Window
    {
        public WinNameEdit(string textShow, bool isActivate, bool isNameEditVisible)
        {
            InitializeComponent();
            if (isNameEditVisible)
            {
                this.NameShow = textShow;
                this.txtTitle.Text = "修改名称";
            }
              
            else
            {
                this.PositionShow = textShow;
                this.txtTitle.Text = "修改职位";
            }
              
            this.IsActivate = isActivate;
            this.IsNameEditVisible = isNameEditVisible;
        }



        public string NameShow
        {
            get { return (string)GetValue(NameShowProperty); }
            set { SetValue(NameShowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NameShow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameShowProperty =
            DependencyProperty.Register("NameShow", typeof(string), typeof(WinNameEdit), new PropertyMetadata(null));



        public string PositionShow
        {
            get { return (string)GetValue(PositionShowProperty); }
            set { SetValue(PositionShowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PositionShow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositionShowProperty =
            DependencyProperty.Register("PositionShow", typeof(string), typeof(WinNameEdit), new PropertyMetadata(null));



        public bool IsNameEditVisible
        {
            get { return (bool)GetValue(IsNameEditVisibleProperty); }
            set { SetValue(IsNameEditVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsNameEditVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNameEditVisibleProperty =
            DependencyProperty.Register("IsNameEditVisible", typeof(bool), typeof(WinNameEdit), new PropertyMetadata(null));









        public bool IsActivate
        {
            get { return (bool)GetValue(IsActivateProperty); }
            set { SetValue(IsActivateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsActivate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsActivateProperty =
            DependencyProperty.Register("IsActivate", typeof(bool), typeof(WinNameEdit), new PropertyMetadata(null));




        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.DialogResult = false;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if(this.IsNameEditVisible)
            {
                string tempStr = this.txtName.Text.Replace(" ", "");
                if (tempStr.Length == 0)
                {
                    this.rctValidate.Stroke = new SolidColorBrush(Colors.Red);
                    this.txtValidate.Visibility = Visibility.Visible;
                    return;
                }
            }
            else
            {
                string tempStr = this.txtPosition.Text.Replace(" ", "");
                if(tempStr.Length == 0)
                {
                    this.rctPositionValidate.Stroke = new SolidColorBrush(Colors.Red);
                    this.txtValidate.Visibility = Visibility.Visible;
                    return;
                }
            }
          
            this.DialogResult = true;
            this.Close();

        }

        private void txtName_GotFocus(object sender, RoutedEventArgs e)
        {
            this.rctValidate.Stroke = new SolidColorBrush(Color.FromRgb(229, 229, 229));
            this.txtValidate.Visibility = Visibility.Collapsed;

        }

        private void txtPosition_GotFocus(object sender, RoutedEventArgs e)
        {
            this.rctPositionValidate.Stroke = new SolidColorBrush(Color.FromRgb(229, 229, 229));
            this.txtValidate.Visibility = Visibility.Collapsed;
        }

        
        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string temp = this.txtName.Text;
            if(string.IsNullOrEmpty(temp))
            {
                return;
            }
            temp = temp.Replace(" ", "");
            if (temp == string.Empty)
                return;
            if(CommonValidationMethod.UserNameValidate(temp))
            {
                this.txtUserNameValidate.Visibility = Visibility.Collapsed;
                this.rctValidate.Stroke = new SolidColorBrush(Color.FromRgb(229, 229, 229));
                this.btnOK.IsEnabled = true;
            }
            else
            {
                this.txtUserNameValidate.Visibility = Visibility.Visible;
                this.rctValidate.Stroke = new SolidColorBrush(Colors.Red);
                this.btnOK.IsEnabled = false;
            }

           
        }
    }

  

}
