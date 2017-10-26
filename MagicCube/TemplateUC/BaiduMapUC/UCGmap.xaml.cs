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
using GMap.NET;
using GMap.NET.CacheProviders;
using GMap.NET.Internals;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using GMap.NET.Projections;
using System.Threading.Tasks;
using System.Net.Http;

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// UCGmap.xaml 的交互逻辑
    /// </summary>
    public partial class UCGmap : UserControl
    {
        #region "变量"
        private string key = "GhwcEQNhtGOmDqdjxYuSBreX3xjeA5te";
        private string httpAddr = @"http://api.map.baidu.com/geocoder/v2/?ak={0}&output=json&address={1}&city={2}";
        private string city;
        private double lat;
        private GMapMarker marker;

        public double Lat
        {
            get { return lat; }
        }
        private double lng;
        public double Lng
        {
            get { return lng; }
        }
        public string DetailAddress
        {
            get { return (string)GetValue(DetailAddressProperty); }
            set { SetValue(DetailAddressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DetailAddress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DetailAddressProperty =
            DependencyProperty.Register("DetailAddress", typeof(string), typeof(UCGmap), new PropertyMetadata(null));
        #endregion

        public UCGmap()
        {
            InitializeComponent();
           
        }

        #region "对外方法"
        /// <summary>
        /// 根据location定位位置
        /// </summary>
        /// <param name="location"></param>
        /// <param name="city"></param>
        public async void InitialUCGamp(string location, string city)
        {
            mapControl.MapProvider = AMapProvider.Instance;
            mapControl.MinZoom = 2;  //最小缩放
            mapControl.MaxZoom = 17; //最大缩放
            mapControl.Zoom = 14;     //当前缩放
            mapControl.ShowCenter = false; //不显示中心十字点
            mapControl.DragButton = MouseButton.Left; //左键拖拽地图
            mapControl.IgnoreMarkerOnMouseWheel = true;
            //mapControl.Position = new PointLatLng(39.98243548609636, 116.32748126005255); //地图中心位置：南京
            marker = new GMapMarker(new PointLatLng(this.lat, this.lng));
            await this.SetAddress(location, city);
            mapControl.MouseLeftButtonUp += new MouseButtonEventHandler(mapControl_MouseLeftButtonUp);
            
            UCMarker s = new UCMarker(this, marker, DetailAddress);
            marker.Shape = s;
            mapControl.Markers.Clear();
            mapControl.Markers.Add(marker);
        }

        public void InitialUCGamp()
        {
            mapControl.MapProvider = AMapProvider.Instance;
            mapControl.MinZoom = 2;  //最小缩放
            mapControl.MaxZoom = 17; //最大缩放
            mapControl.Zoom = 2;     //当前缩放
            mapControl.ShowCenter = false; //不显示中心十字点
            mapControl.DragButton = MouseButton.Left; //左键拖拽地图
            mapControl.IgnoreMarkerOnMouseWheel = true;
            mapControl.Position = new PointLatLng(39.931432, 116.398462); //地图中心位置：南京
            marker = new GMapMarker(new PointLatLng(this.lat, this.lng));
            //await this.SetAddress("", "中国");
            mapControl.MouseLeftButtonUp += new MouseButtonEventHandler(mapControl_MouseLeftButtonUp);

            UCMarker s = new UCMarker(this, marker, DetailAddress);
            marker.Shape = s;
            mapControl.Markers.Clear();
            mapControl.Markers.Add(marker);
        }
        /// <summary>
        /// 根据经纬坐标，定位位置
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        public void InitialUCGamp(string location, string city,double lat, double lng)
        {
            this.city = city;
            this.DetailAddress = location;
            this.lat = lat;
            this.lng = lng;

            marker = new GMapMarker(new PointLatLng(lat, lng));
            UCMarker s = new UCMarker(this, marker, DetailAddress);
            marker.Shape = s;

            mapControl.MapProvider = AMapProvider.Instance;
            mapControl.MinZoom = 2;  //最小缩放
            mapControl.MaxZoom = 17; //最大缩放
            mapControl.Zoom = 10;     //当前缩放
            mapControl.ShowCenter = false; //不显示中心十字点
            mapControl.DragButton = MouseButton.Left; //左键拖拽地图
            mapControl.Position = new PointLatLng(lat, lng); 
            mapControl.MouseLeftButtonUp += new MouseButtonEventHandler(mapControl_MouseLeftButtonUp);
            mapControl.Markers.Clear();
            mapControl.Markers.Add(marker);
        }

      

        public async Task SetAddress(string location, string city)
        {
            this.city = city;
            this.DetailAddress = location;
            await GetAddressLatlng();
            
            this.mapControl.Position = new PointLatLng(this.lat - 0.0058156686716, this.lng-0.0066823953065);
            this.marker.Position = new PointLatLng(this.lat - 0.0058156686716, this.lng - 0.0066823953065);
            mapControl.Zoom = 14;
        }

        public void GmapZoom(double zoomstep)
        {
            this.mapControl.Zoom = this.mapControl.Zoom + zoomstep;
        }

        public Point GetMarkerLatLng()
        {
            Point pt = new Point();
            pt.X = this.marker.Position.Lat + 0.0058156686716;
            pt.Y = this.marker.Position.Lng + 0.0066823953065;
            return pt;
        }

        #endregion 


        #region "对内方法"

        private async Task GetAddressLatlng()
        {
            HttpClient client = new HttpClient();
            string temp = await client.GetStringAsync(string.Format(httpAddr, key, this.DetailAddress, this.city));
            BaiduLatLngModel model = DAL.JsonHelper.ToObject<BaiduLatLngModel>(temp);
            if (model != null)
            {
                if (model.result != null)
                {
                    if (model.result.location != null)
                    {
                        this.lat = model.result.location.lat;
                        this.lng = model.result.location.lng;
                    }
                }

            }
        }


        #endregion

        private void mapControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.lat = mapControl.Position.Lat;
            this.lng = mapControl.Position.Lng;

        }

    }



    public abstract class AMapProviderBase : GMapProvider
    {
        public AMapProviderBase()
        {
            MaxZoom = null;
            RefererUrl = "http://www.amap.com/";
            //Copyright = string.Format("©{0} 高德 Corporation, ©{0} NAVTEQ, ©{0} Image courtesy of NASA", DateTime.Today.Year);    
        }

        public override PureProjection Projection
        {
            get { return MercatorProjection.Instance; }
        }

        GMapProvider[] overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] { this };
                }
                return overlays;
            }
        }
    }

    public class AMapProvider : AMapProviderBase
    {
        public static readonly AMapProvider Instance;

        readonly Guid id = new Guid("EF3DD303-3F74-4938-BF40-232D0595EE88");
        public override Guid Id
        {
            get { return id; }
        }

        readonly string name = "AMap";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        static AMapProvider()
        {
            Instance = new AMapProvider();
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = MakeTileImageUrl(pos, zoom, LanguageStr);
                return GetTileImageUsingHttp(url);
            }
            catch
            {
                return null;
            }
        }

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            var num = (pos.X + pos.Y) % 4 + 1;
            //string url = string.Format(UrlFormat, num, pos.X, pos.Y, zoom);
            string url = string.Format(UrlFormat, pos.X, pos.Y, zoom);
            return url;
        }

        //static readonly string UrlFormat = "http://webrd04.is.autonavi.com/appmaptile?x={0}&y={1}&z={2}&lang=zh_cn&size=1&scale=1&style=7";
        static readonly string UrlFormat = "http://webrd01.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=7&x={0}&y={1}&z={2}";
    }


    class BaiduLatLngModel
    {
        public int status { get; set; }
        public Result result { get; set; }
    }

    public class Location
    {
        public double lng { get; set; }
        public double lat { get; set; }
    }

    public class Result
    {
        public Location location { get; set; }
        public int precise { get; set; }
        public int confidence { get; set; }
        public string level { get; set; }
    }



}
