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
using MagicCube.Common;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCSelfImageUpload.xaml 的交互逻辑
    /// </summary>
    public partial class UCSelfImageUpload : UserControl
    {
        #region "委托"
        public Action CancelAction;
        public Action<BitmapSource> OKAction;
        #endregion
        public UCSelfImageUpload()
        {
            InitializeComponent();
           
        }


        private string imagePath = string.Empty;
        public void GetImagePath(string imagePath)
        {
            this.imagePath = imagePath;
            this.pictrueResizer.ImageSource = ImageProcessor.GetBitmapSourceFromFile(imagePath);
            this.pictrueResizer.InitialCrop();
            sliderPic.Value = 150;
            this.pictrueResizer.SetCropSelection(sliderPic.Value);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.pictrueResizer.SetCropSelection(e.NewValue);
        }

        private void BtnClockwise_Click(object sender, RoutedEventArgs e)
        {
            this.pictrueResizer.Rotate(false);
        }

        private void BtnAnticlockwise_Click(object sender, RoutedEventArgs e)
        {
            this.pictrueResizer.Rotate(true);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource bs = this.pictrueResizer.Crop();
            ImageProcessor.SaveImageToFile(bs, "D:\\hell.png");
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource bs = this.pictrueResizer.Crop();
            if (this.OKAction != null)
                this.OKAction(bs);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.CancelAction != null)
                this.CancelAction();
        }

        private void BtnSlideMinus_Click(object sender, RoutedEventArgs e)
        {
            if (this.sliderPic.Value > 25)
                this.sliderPic.Value -= 5;
            else
                this.sliderPic.Value = 20;
        }

        private void BtnSliderPlus_Click(object sender, RoutedEventArgs e)
        {
            if (this.sliderPic.Value < 179)
                this.sliderPic.Value += 5;
            else
                this.sliderPic.Value = 184;
        }

        private void UserControl_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.pictrueResizer.MouseUpSetSelection();
        }
    }
}
