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
using GMap.NET.WindowsPresentation;
using System.Windows.Controls.Primitives;
using System.Diagnostics;

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// UCMarker.xaml 的交互逻辑
    /// </summary>
    public partial class UCMarker : UserControl
    {
        Popup Popup;
        Label Label;
        GMapMarker Marker;
        UCGmap MainWindow;
        public UCMarker(UCGmap window, GMapMarker marker, string title)
        {
            InitializeComponent();
            this.MainWindow = window;
            this.Marker = marker;

            Popup = new Popup();
            Label = new Label();

            this.Unloaded += new RoutedEventHandler(CustomMarkerDemo_Unloaded);
            this.Loaded += new RoutedEventHandler(CustomMarkerDemo_Loaded);
            this.SizeChanged += new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);
            this.MouseEnter += new MouseEventHandler(MarkerControl_MouseEnter);
            this.MouseLeave += new MouseEventHandler(MarkerControl_MouseLeave);
            this.MouseMove += new MouseEventHandler(CustomMarkerDemo_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonUp);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonDown);

            Popup.Placement = PlacementMode.Mouse;
            {
                Label.Background = Brushes.White;
                Label.Foreground = new SolidColorBrush(Color.FromRgb(0x66,0x66,0x66));
                Label.BorderBrush = Brushes.WhiteSmoke;
                Label.BorderThickness = new Thickness(1);
                Label.Padding = new Thickness(13);
                Label.FontSize = 12;
                Label.Content = title;
            }
            Popup.Child = Label;
        }


        void CustomMarkerDemo_Loaded(object sender, RoutedEventArgs e)
        {
            if (icon.Source.CanFreeze)
            {
                icon.Source.Freeze();
            }
        }

        void CustomMarkerDemo_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= new RoutedEventHandler(CustomMarkerDemo_Unloaded);
            this.Loaded -= new RoutedEventHandler(CustomMarkerDemo_Loaded);
            this.SizeChanged -= new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);
            this.MouseEnter -= new MouseEventHandler(MarkerControl_MouseEnter);
            this.MouseLeave -= new MouseEventHandler(MarkerControl_MouseLeave);
            this.MouseMove -= new MouseEventHandler(CustomMarkerDemo_MouseMove);
            this.MouseLeftButtonUp -= new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonUp);
            this.MouseLeftButtonDown -= new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonDown);

            Marker.Shape = null;
            icon.Source = null;
            icon = null;
            Popup = null;
            Label = null;
        }

        void CustomMarkerDemo_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Marker.Offset = new Point(-e.NewSize.Width / 2, -e.NewSize.Height);
        }

        void CustomMarkerDemo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured)
            {
                Point p = e.GetPosition(MainWindow.mapControl);
                p.X = p.X + 5;
                p.Y = p.Y + 15;
                Marker.Position = MainWindow.mapControl.FromLocalToLatLng((int)(p.X), (int)(p.Y));
                Console.Write(Marker.Position.Lat + "," + Marker.Position.Lng);
            }
            e.Handled = true;
        }

        void CustomMarkerDemo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsMouseCaptured)
            {
                Mouse.Capture(this);
            }
        }

        void CustomMarkerDemo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseCaptured)
            {
                Mouse.Capture(null);
            }
        }

        void MarkerControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Marker.ZIndex -= 10000;
            Popup.IsOpen = false;
        }

        void MarkerControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Marker.ZIndex += 10000;
            Popup.IsOpen = true;
        }


    }
}
