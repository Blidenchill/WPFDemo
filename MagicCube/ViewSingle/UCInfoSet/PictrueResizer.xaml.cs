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
using System.IO;
using MagicCube.Common;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// PictrueResizer.xaml 的交互逻辑
    /// </summary>
    public partial class PictrueResizer : UserControl
    {
        #region "构造函数"
        public PictrueResizer()
        {
            InitializeComponent();
           
        }

        
        #endregion

        #region "变量"
        private bool isDragging;


        private double oldWidth;
        private double oldHeight;

        private ResizeDirection direction;

        private Point dragPoint;
        private Point startPoint;

        public double Top { get; set; }

        public double Left { get; set; }

        public double Right { get; set; }

        public double Bottom { get; set; }

        //public Image Image { get; set; }

        /// <summary>Identifies the SelectedArea dependency property.</summary>
        public static DependencyProperty SelectedAreaProperty;

        /// <summary>The area selected to be cropped. This is a dependency property.</summary>
        public BitmapSource SelectedArea
        {
            get { return GetValue(SelectedAreaProperty) as BitmapSource; }
            set { SetValue(SelectedAreaProperty, value); }
        }

        #endregion

        #region "属性"


        public BitmapSource ImageSoource
        {
            get { return (BitmapSource)GetValue(ImageSoourceProperty); }
            set { SetValue(ImageSoourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSoource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSoourceProperty =
            DependencyProperty.Register("ImageSoource", typeof(BitmapSource), typeof(PictrueResizer), new PropertyMetadata(null));


        #endregion

        #region "事件"


        private void croppedArea_MouseEnter(object sender, MouseEventArgs e)
        {
            //var canvas = sender as Canvas;
            canvas.Cursor = Cursors.SizeAll;

            //isMouseOverSelectedArea = true;
        }

        private void croppedArea_MouseLeave(object sender, MouseEventArgs e)
        {
            //If the selected area is being dragged, this event will be fired as a false positive, so we return.
            if (isDragging)
            {
                return;
            }

            var canvas = sender as Canvas;
            canvas.Background = Brushes.Transparent;
        }
        Point prevPoint = new Point();
        Point childPoint = new Point();
        private void croppedArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //Console.WriteLine(Canvas.GetLeft(selection).ToString() + "," + Canvas.GetTop(selection).ToString());
                //double left = Canvas.GetLeft(selection);
                //double top = Canvas.GetTop(selection);
                //if(top < 2)
                //{
                //    e.Handled = true;
                //    return;
                //}
                //if(left < 2)
                //{
                //    e.Handled = true;
                //    return;
                //}
                //if(top + selection.Height > localUC.Height - 2)
                //{
                //    e.Handled = true;
                //    return;
                //}
                //if(left + selection.Width > localUC.Width - 2)
                //{
                //    e.Handled = true;
                //    return;
                //}
                Canvas c = sender as Canvas;

                prevPoint = e.GetPosition(canvas);
                
                double x = prevPoint.X - childPoint.X;
                double y = prevPoint.Y - childPoint.Y;
                //Console.WriteLine(x + "," + y);
                Rect rc = new Rect(10, 10, canvas.ActualWidth - 20, canvas.ActualHeight - 20);
                Rect childRc = new Rect(Canvas.GetLeft(selection), Canvas.GetTop(selection), selection.ActualWidth, selection.ActualHeight);

                if(!rc.Contains(childRc))
                {
                    childRc = AutoResize(rc, childRc);

                    Rect newChildRc = new Rect(x, y, selection.ActualWidth, selection.ActualHeight);
                    if (rc.Contains(newChildRc))
                    {
                        Drag(e.GetPosition(canvas));
                    }
                    //bool leftOut = (childRc.Left < rc.Left) ? true : false;
                    //bool topOut = (childRc.Top < rc.Top) ? true : false;
                    //bool rightOut = (childRc.Left + childRc.Width > rc.Width) ? true : false;
                    //bool bottomOut = (childRc.Top + childRc.Height > childRc.Height) ? true : false;
                    //if(x >= 0 && y >= 0)
                    //{
                    //    if(rightOut || bottomOut)
                    //    {
                    //        return;
                    //    }
                    //}
                    //if(x <=0 && y <= 0)
                    //{
                    //    if(leftOut || topOut)
                    //    {
                    //        return;
                    //    }
                    //}
                    //if(x >= 0 && y <= 0)
                    //{
                    //    if(rightOut || topOut)
                    //    {
                    //        return;
                    //    }
                    //}
                    //if(x <= 0 && y >= 0)
                    //{
                    //    if(leftOut || bottomOut)
                    //    {
                    //        return;
                    //    }
                    //}
                    //Drag(e.GetPosition(canvas));
                    return;
                }
                else
                {
                    ////canvas.SetValue(Canvas.LeftProperty, x);
                    ////canvas.SetValue(Canvas.TopProperty, y);
                    //////canvas.CaptureMouse();
                    ////Drag(e.GetPosition(canvas));
                    //Canvas.SetLeft(selection, x);
                    //Canvas.SetTop(selection, y);
                    //Update(x, y, selection.Width, selection.Height);
                    //Console.WriteLine("正常");
                    //c.CaptureMouse();
                    Drag(e.GetPosition(canvas));
                }

                //Drag(e.GetPosition(canvas));
            }
            e.Handled = true;
        }

        private Rect AutoResize(Rect outerRc, Rect innerRc)
          {  
              double with = innerRc.Width;  
              double height = innerRc.Height;  
   
             if (innerRc.Left<outerRc.Left)  
              {  
                  innerRc.X = outerRc.Left + 1;  
                  innerRc.Width = with;  
              }  
              if (innerRc.Right > outerRc.Right)  
              {  
                  innerRc.X = outerRc.Right - with - 1;  
                  innerRc.Width = with;  
              }  
              if (innerRc.Top<outerRc.Top)  
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

        private void croppedArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            childPoint = e.GetPosition(canvas);
            dragPoint = e.GetPosition(canvas);
            e.Handled = true;
        }

        private void croppedArea_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }





        #endregion

        #region "对内方法"

        private void InitialOverLay(double width, double height)
        {

            image.Opacity = 0.2;

            var rectangle = overlay.Clip as RectangleGeometry;
            var rect = rectangle.Rect;
            rect.Width = width;
            rect.Height = height;

            rectangle.Rect = rect;
            overlay.Visibility = Visibility.Visible;

            var render = new RenderTargetBitmap(
                rect.Width == 0 ? 1 : (int)(rect.Width),
                rect.Height == 0 ? 1 : (int)(rect.Height),
                96,
                96,
                PixelFormats.Pbgra32);

            var drawingVisual = new DrawingVisual();

            using (var context = drawingVisual.RenderOpen())
            {
                VisualBrush brush = new VisualBrush(overlay);
                context.DrawRectangle(brush, null, new Rect(new Point(), rect.Size));
            }

            render.Render(drawingVisual);
        }

        protected void Update(double x, double y, double width, double height)
        {
            Top = y;
            Left = x;
            Right = x + width;
            Bottom = y + height;

            DisplayOverlay(x, y, width, height);
        }

        protected void DisplayOverlay(double x, double y, double width, double height)
        {
            image.Opacity = 0.2;

            var rectangle = overlay.Clip as RectangleGeometry;
            var rect = rectangle.Rect;

            rect.X = x;
            rect.Y = y;
            rect.Width = width;
            rect.Height = height;

            rectangle.Rect = rect;
            overlay.Visibility = Visibility.Visible;

            var render = new RenderTargetBitmap(
                rect.Width == 0 ? 1 : (int)(rect.Width),
                rect.Height == 0 ? 1 : (int)(rect.Height),
                96,
                96,
                PixelFormats.Pbgra32);

            var drawingVisual = new DrawingVisual();

            using (var context = drawingVisual.RenderOpen())
            {
                VisualBrush brush = new VisualBrush(overlay);
                context.DrawRectangle(brush, null, new Rect(new Point(), rect.Size));
            }

            render.Render(drawingVisual);

            //SelectedArea = render;
        }

        protected void SelectArea(Point position)
        {
            var x = Math.Min(position.X, startPoint.X);
            var y = Math.Min(position.Y, startPoint.Y);

            var w = Math.Max(position.X, startPoint.X) - x;
            var h = Math.Max(position.Y, startPoint.Y) - y;

            selection.Width = w;
            selection.Height = h;

            Canvas.SetLeft(selection, x);
            Canvas.SetTop(selection, y);

            Update(x, y, w, h);
        }

        protected void Drag(Point position)
        {
            isDragging = true;

            var x = startPoint.X - (dragPoint.X - position.X);
            var y = startPoint.Y - (dragPoint.Y - position.Y);

            Canvas.SetLeft(selection, x);
            Canvas.SetTop(selection, y);

            Update(x, y, selection.Width, selection.Height);
        }

        protected void Resize(Point position)
        {
            //isResizing = true;

            var x = Math.Min(position.X, startPoint.X);
            var y = Math.Min(position.Y, startPoint.Y);

            var w = Math.Max(position.X, startPoint.X) - x;
            var h = Math.Max(position.Y, startPoint.Y) - y;

            if (oldWidth == 0 && oldHeight == 0)
            {
                oldWidth = selection.Width;
                oldHeight = selection.Height;
            }

            switch (direction)
            {
                case ResizeDirection.Top:
                    Canvas.SetTop(selection, y);
                    selection.Height = h + oldHeight;
                    break;

                case ResizeDirection.Left:
                    //Debug.WriteLine("{0} {1} {2} {3}", selection.Width, position.X, startPoint.X, selection.Width + (startPoint.X - position.X));
                    Canvas.SetLeft(selection, x);
                    selection.Width = w + oldWidth;
                    break;

                case ResizeDirection.Right:
                    selection.Width = selection.Width + (position.X - selection.Width - startPoint.X);
                    break;

                case ResizeDirection.Bottom:
                    selection.Height = selection.Height + (position.Y - selection.Height - startPoint.Y);
                    break;

                case ResizeDirection.TopLeft:
                    Canvas.SetTop(selection, y);
                    Canvas.SetLeft(selection, x);
                    selection.Height = h + oldHeight;
                    selection.Width = w + oldWidth;
                    break;

                case ResizeDirection.TopRight:
                    Canvas.SetTop(selection, y);
                    selection.Height = h + oldHeight;
                    selection.Width = selection.Width + (position.X - selection.Width - startPoint.X);
                    break;

                case ResizeDirection.BottomLeft:
                    Canvas.SetLeft(selection, x);
                    selection.Width = w + oldWidth;
                    selection.Height = selection.Height + (position.Y - selection.Height - startPoint.Y);
                    break;

                case ResizeDirection.BottomRight:
                    selection.Width = selection.Width + (position.X - selection.Width - startPoint.X);
                    selection.Height = selection.Height + (position.Y - selection.Height - startPoint.Y);
                    break;
            }

            Update(x, y, selection.Width, selection.Height);
        }

        protected void PrepareForResizing(Point position)
        {
            var width = selection.ActualWidth;
            var height = selection.ActualHeight;
            var thickness = selection.BorderThickness.Left + 1;

            //top left corner
            if (position.X >= 0 && position.X <= thickness && position.Y >= 0 && position.Y <= thickness)
            {
                selection.Cursor = Cursors.SizeNWSE;
                direction = ResizeDirection.TopLeft;
            }
            //top right corner
            else if (position.X >= width - thickness && position.Y >= 0 && position.Y <= thickness)
            {
                selection.Cursor = Cursors.SizeNESW;
                direction = ResizeDirection.TopRight;
            }
            //bottom left corner
            else if (position.X >= 0 && position.X <= thickness && position.Y >= height - thickness)
            {
                selection.Cursor = Cursors.SizeNESW;
                direction = ResizeDirection.BottomLeft;
            }
            //bottom right corner
            else if (position.X >= width - thickness && position.Y >= height - thickness)
            {
                selection.Cursor = Cursors.SizeNWSE;
                direction = ResizeDirection.BottomRight;
            }
            //top 
            else if (position.Y <= thickness)
            {
                selection.Cursor = Cursors.SizeNS;
                direction = ResizeDirection.Top;
            }
            //bottom 
            else if (position.Y >= height - thickness)
            {
                selection.Cursor = Cursors.SizeNS;
                direction = ResizeDirection.Bottom;
            }
            //left 
            else if (position.X <= thickness)
            {
                selection.Cursor = Cursors.SizeWE;
                direction = ResizeDirection.Left;
            }
            //right 
            else if (position.X >= width - thickness)
            {
                selection.Cursor = Cursors.SizeWE;
                direction = ResizeDirection.Right;
            }
        }
        #endregion

        #region "公共方法"
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
           
            double x = Canvas.GetLeft(selection);
            double y = Canvas.GetTop(selection);
            double width = selection.Width;
            double height = selection.Height;
            Rect rect = new Rect(x, y, width, height);
            BitmapSource bitmapSource = ImageProcessor.Crop(ImageSoource, rect);
            return bitmapSource;

        }

        public void Rotate(bool clockwise)
        {
            BitmapSource bitmapSource;
            if (clockwise)
            {
                bitmapSource = ImageProcessor.Rotate(ImageSoource, 90);
            }
            else
            {
                 bitmapSource = ImageProcessor.Rotate(ImageSoource, -90);

            }
            this.ImageSoource = bitmapSource;
        }
        public void InitialCrop()
        {
        
            isDragging = true;
            Canvas.SetLeft(selection, 10);
            Canvas.SetTop(selection, 10);
            selection.Width = 150;
            selection.Height = 150;
            //this.Drag(new Point(10, 10));

            Update(10, 10, selection.Width, selection.Height);

            //overlay.Visibility = Visibility.Visible;
            //InitialOverLay(150, 150);
        }

        public void SetCropSelection(double db)
        {
            //double x = Canvas.GetLeft(selection);
            //double y = Canvas.GetTop(selection);
            //this.Update(x, y, db, db);

            selection.Width = db;
            selection.Height = db;
            InitialOverLay(db, db);

        }

        #endregion

     
    }

    enum ResizeDirection
    {
        Top,
        Left,
        Right,
        Bottom,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}
