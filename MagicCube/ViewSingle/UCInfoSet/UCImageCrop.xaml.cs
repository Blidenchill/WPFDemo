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
    /// UCImageCrop.xaml 的交互逻辑
    /// </summary>
    public partial class UCImageCrop : UserControl
    {
        public UCImageCrop()
        {
            InitializeComponent();
        }
        #region "属性"



        public BitmapSource ImageSource
        {
            get { return (BitmapSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(BitmapSource), typeof(UCImageCrop), new PropertyMetadata(null));



        


        #endregion
        //鼠标相对于被拖动的Canvas控件cvSelection的坐标  
        Point childPoint = new Point();
        //鼠标相对于作为容器的Canvas控件movBg的坐标  
        Point prevPoint = new Point();

        private void mov_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            childPoint = e.GetPosition(cvSelection);
        }

        private void mov_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas c = sender as Canvas;
            Rect rc = new Rect(1, 1, movBg.ActualWidth, movBg.ActualHeight);
            Rect childRc = new Rect(Canvas.GetLeft(c), Canvas.GetTop(c), c.ActualWidth, c.ActualHeight);
            if (!rc.Contains(childRc))
            {
                childRc = AutoResize(rc, childRc);
                c.SetValue(Canvas.LeftProperty, childRc.Left);
                c.SetValue(Canvas.TopProperty, childRc.Top);
                c.Width = childRc.Width;
                c.Height = childRc.Height;
                DisplayOverlay(childRc.Left, childRc.Top, childRc.Width, childRc.Height);
            }
            c.ReleaseMouseCapture();
        }

        private void mov_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Canvas c = sender as Canvas;

                prevPoint = e.GetPosition(movBg);
                double x = prevPoint.X - childPoint.X;
                double y = prevPoint.Y - childPoint.Y;

                Rect rc = new Rect(1, 1, movBg.ActualWidth, movBg.ActualHeight);
                Rect childRc = new Rect(Canvas.GetLeft(cvSelection), Canvas.GetTop(cvSelection), cvSelection.ActualWidth, cvSelection.ActualHeight);
                if (!rc.Contains(childRc))
                {
                    //childRc = AutoResize(rc, childRc);
                    //mov.SetValue(Canvas.LeftProperty, childRc.Left);
                    //mov.SetValue(Canvas.TopProperty, childRc.Top);
                    //mov.Width = childRc.Width;
                    //mov.Height = childRc.Height;
                    //childPoint = e.GetPosition(mov);
                    //prevPoint = e.GetPosition(movBg);
                    //c.ReleaseMouseCapture();
                    Rect newchildRec = new Rect(x, y, cvSelection.ActualWidth, cvSelection.ActualHeight);
                    if (rc.Contains(newchildRec))
                    {
                        cvSelection.SetValue(Canvas.LeftProperty, x);
                        cvSelection.SetValue(Canvas.TopProperty, y);
                        DisplayOverlay(x, y, cvSelection.ActualWidth, cvSelection.ActualHeight);
                    }
                    return;
                }
                else
                {
                    //Rect newchildRec = new Rect(x, y, cvSelection.ActualWidth, cvSelection.ActualHeight);
                    //if (rc.Contains(newchildRec))
                    //{
                    //    cvSelection.SetValue(Canvas.LeftProperty, x);
                    //    cvSelection.SetValue(Canvas.TopProperty, y);
                    //    DisplayOverlay(x, y, cvSelection.ActualWidth, cvSelection.ActualHeight);
                    //}

                    cvSelection.SetValue(Canvas.LeftProperty, x);
                    cvSelection.SetValue(Canvas.TopProperty, y);
                    DisplayOverlay(x, y, cvSelection.ActualWidth, cvSelection.ActualHeight);
                    //c.CaptureMouse();
                }
            }
        }

        private void mov_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        #region "对内函数"
        private Rect AutoResize(Rect outerRc, Rect innerRc)
        {
            double with = innerRc.Width;
            double height = innerRc.Height;

            if (innerRc.Left < outerRc.Left)
            {
                innerRc.X = outerRc.Left + 1;
                innerRc.Width = with;
            }
            if (innerRc.Right > outerRc.Right)
            {
                innerRc.X = outerRc.Right - with - 1;
                innerRc.Width = with;
            }
            if (innerRc.Top < outerRc.Top)
            {
                innerRc.Y = outerRc.Top + 1;
                innerRc.Height = height;
            }
            if (innerRc.Bottom > outerRc.Bottom)
            {
                innerRc.Y = outerRc.Bottom - height - 1;
                innerRc.Height = height;
            }
            return innerRc;
        }

        private void DisplayOverlay(double x, double y, double width, double height)
        {
            var rectangle = imgOverlay.Clip as RectangleGeometry;
            var rect = rectangle.Rect;

            rect.X = x;
            rect.Y = y;
            rect.Width = width;
            rect.Height = height;

            rectangle.Rect = rect;
            imgOverlay.Visibility = Visibility.Visible;

            var render = new RenderTargetBitmap(
               rect.Width == 0 ? 1 : (int)(rect.Width),
               rect.Height == 0 ? 1 : (int)(rect.Height),
               96,
               96,
               PixelFormats.Pbgra32);

            var drawingVisual = new DrawingVisual();

            using (var context = drawingVisual.RenderOpen())
            {
                VisualBrush brush = new VisualBrush(imgOverlay);
                context.DrawRectangle(brush, null, new Rect(new Point(), rect.Size));
            }

            render.Render(drawingVisual);
        }

        #endregion


        #region "对外调用函数"
        public BitmapSource Crop()
        {
            //var path = this.imagePath;
            //if (path == null)
            //{
            //    throw new ApplicationException("The application could not find the path of the image.");
            //}

            //if (!File.Exists(path))
            //{
            //    throw new FileNotFoundException("The image could not be found.");
            //}

            double x = Canvas.GetLeft(cvSelection);
            double y = Canvas.GetTop(cvSelection);
            double width = cvSelection.Width;
            double height = cvSelection.Height;
            Rect rect = new Rect(x, y, width, height);
            BitmapSource bitmapSource = ImageProcessor.Crop(ImageSource, rect);
            return bitmapSource;

        }

        public void Rotate(bool clockwise)
        {
            BitmapSource bitmapSource;
            if (clockwise)
            {
                bitmapSource = ImageProcessor.Rotate(ImageSource, 90);
            }
            else
            {
                bitmapSource = ImageProcessor.Rotate(ImageSource, -90);

            }
            this.ImageSource = bitmapSource;
        }
        public void InitialCrop()
        {

            Canvas.SetLeft(cvSelection, 20);
            Canvas.SetTop(cvSelection, 20);
            cvSelection.Width = 100;
            cvSelection.Height = 100;
            //this.Drag(new Point(10, 10));
            DisplayOverlay(20, 20, 100, 100);
            //Update(10, 10, selection.Width, selection.Height);

            //overlay.Visibility = Visibility.Visible;
            //InitialOverLay(150, 150);
        }

        public void SetCropSelection(double db)
        {
            //double x = Canvas.GetLeft(selection);
            //double y = Canvas.GetTop(selection);
            //this.Update(x, y, db, db);

            cvSelection.Width = db;
            cvSelection.Height = db;
            DisplayOverlay(Canvas.GetLeft(cvSelection), Canvas.GetTop(cvSelection), cvSelection.Width, cvSelection.Height);

        }

        public void MouseUpSetSelection()
        {
            Canvas c = cvSelection;
            Rect rc = new Rect(1, 1, movBg.ActualWidth, movBg.ActualHeight);
            Rect childRc = new Rect(Canvas.GetLeft(c), Canvas.GetTop(c), c.ActualWidth, c.ActualHeight);
            if (!rc.Contains(childRc))
            {
                childRc = AutoResize(rc, childRc);
                c.SetValue(Canvas.LeftProperty, childRc.Left);
                c.SetValue(Canvas.TopProperty, childRc.Top);
                c.Width = childRc.Width;
                c.Height = childRc.Height;
                DisplayOverlay(childRc.Left, childRc.Top, childRc.Width, childRc.Height);
            }
            c.ReleaseMouseCapture();
        }

        #endregion

        private void cvSelection_LostMouseCapture(object sender, MouseEventArgs e)
        {
            Canvas c = cvSelection;
            Rect rc = new Rect(1, 1, movBg.ActualWidth, movBg.ActualHeight);
            Rect childRc = new Rect(Canvas.GetLeft(c), Canvas.GetTop(c), c.ActualWidth, c.ActualHeight);
            if (!rc.Contains(childRc))
            {
                childRc = AutoResize(rc, childRc);
                c.SetValue(Canvas.LeftProperty, childRc.Left);
                c.SetValue(Canvas.TopProperty, childRc.Top);
                c.Width = childRc.Width;
                c.Height = childRc.Height;
                DisplayOverlay(childRc.Left, childRc.Top, childRc.Width, childRc.Height);
            }
            c.ReleaseMouseCapture();
        }

    }
}
