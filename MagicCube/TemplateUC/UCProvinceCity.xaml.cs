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
using MagicCube.HttpModel;
using System.Collections.ObjectModel;
using System.IO;
using MagicCube.ViewModel;
using System.Threading.Tasks;

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// UCProvinceCity.xaml 的交互逻辑
    /// </summary>
    public partial class UCProvinceCity : UserControl
    {
        public delegate Task ActionDelegate(bool flag);
        public ActionDelegate OKOrCancelActionDelegate;

        public Action<bool> OKOrCancelAction;

        public int MostSelectedNum
        {
            get { return (int)GetValue(MostSelectedNumProperty); }
            set { SetValue(MostSelectedNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MostSelectedNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MostSelectedNumProperty =
            DependencyProperty.Register("MostSelectedNum", typeof(int), typeof(UCProvinceCity), new PropertyMetadata(null));



        public string StrTitle
        {
            get { return (string)GetValue(StrTitleProperty); }
            set { SetValue(StrTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrTitleProperty =
            DependencyProperty.Register("StrTitle", typeof(string), typeof(UCProvinceCity), new PropertyMetadata(null));



        public bool ForceSecondSelcted
        {
            get { return (bool)GetValue(ForceSecondSelctedProperty); }
            set { SetValue(ForceSecondSelctedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ForceSecondSelcted.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForceSecondSelctedProperty =
            DependencyProperty.Register("ForceSecondSelcted", typeof(bool), typeof(UCProvinceCity), new PropertyMetadata(null));






        public UCProvinceCity()
        {
            InitializeComponent();

            this.itemsSelectChoosed.ItemsSource = SelectCityList;
            this.StrTitle = "请选择";
        }

        public List<HttpCityCodes> OutSelectCityList = new List<HttpCityCodes>();
        private ObservableCollection<HttpCityCodes> SelectCityList = new ObservableCollection<HttpCityCodes>();
        private ObservableCollection<HttpCityCodes> cityList = new ObservableCollection<HttpCityCodes>();
        private HttpProvinceCityCodes curProvincePanel = new HttpProvinceCityCodes();
        public HttpProvinceCityCodes selectedProvince = new HttpProvinceCityCodes();

        public List<HttpProvinceCityCodes> provinceList = new List<HttpProvinceCityCodes>();
        public List<HttpProvinceCityCodes> hotCityList = new List<HttpProvinceCityCodes>();

        public JsonCityCodeMode CityCodJsonList = new JsonCityCodeMode();

        
        public void SetSelectedList(Collection<HttpCityCodes> selectCityList)
        {
            InitialProvinceCity();
            this.SelectCityList.Clear();
            foreach(var item in selectCityList)
            {
                this.SelectCityList.Add(item);
                this.OutSelectCityList.Add(item);
            }
            rbProvince.IsChecked = true;
        }

        public void SetSelectedList(HttpCityCodes selectCity)
        {
            InitialProvinceCity();
            this.SelectCityList.Clear();
            if(selectCity != null)
            {
                this.SelectCityList.Add(selectCity);
                this.OutSelectCityList.Add(selectCity);
            }
            rbProvince.IsChecked = true;
        }



        //private void ListProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    HttpProvinceCityCodes model = (sender as ListBox).SelectedItem as HttpProvinceCityCodes;
        //    this.lstCity.ItemsSource = model.city;
        //    this.rbCity.IsChecked = true;
        //}

        private void rbProvince_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void ProvinceSelect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HttpProvinceCityCodes model = (sender as TextBlock).DataContext as HttpProvinceCityCodes;
            this.curProvincePanel = model;
            this.cityList.Clear();
            foreach(var item in model.city)
            {
                item.isSelected = false;
                this.cityList.Add(item);
            }
            foreach(var item in SelectCityList)
            {
                foreach(var item2 in this.cityList)
                {
                    if(item2.code == item.code)
                    {
                        item2.isSelected = true;
                        continue;
                    }
                }
               if(item.code == model.code)
                {
                    this.cityList[0].isSelected = true;
                }
               
            }
            if(this.cityList.Count == 0)
            {
                this.selectedProvince = this.curProvincePanel;
                if (this.SelectCityList.Count >= MostSelectedNum)
                    return;
                SelectCityList.Add(new HttpCityCodes() { name = curProvincePanel.name, code = curProvincePanel.code });
                return;
            }

            this.lstCity.ItemsSource = this.cityList;
            this.rbCity.Content = model.name;
            this.rbCity.IsChecked = true;
        }

        private void CityCheck_Click(object sender, RoutedEventArgs e)
        {
        
            CheckBox chk = sender as CheckBox;
            HttpCityCodes model = chk.DataContext as HttpCityCodes;
            //判断是否为热门城市
            foreach (var item in hotCityList)
            {
                if (item.code == model.code)
                {
                    this.popDistrict.IsOpen = true;
                    this.popDistrict.StaysOpen = false;
                    this.popDistrict.PlacementTarget = chk;
                    this.popDistrict.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                    foreach(var it in item.city)
                    {
                        it.isSelected = false;
                        
                        foreach(var it2 in SelectCityList)
                        {
                            if(it.code == it2.code)
                            {
                                it.isSelected = true;
                                continue;
                            }
                        }
                    }
                    this.lstDistrict.ItemsSource = item.city;
                    chk.IsChecked = false;
                    return;
                }
            }


            if (this.SelectCityList.Count >= MostSelectedNum)
            {
                if (chk.IsChecked == true)
                {
                    chk.IsChecked = false;
                    return;
                }
            }

            if (chk.IsChecked == true)
            {
                if(model.code == "0")
                {
                    SelectCityList.Add(new HttpCityCodes() { name = curProvincePanel.name, code = curProvincePanel.code });
                    model.isSelected = true;
                    selectedProvince = curProvincePanel;
                    return;
                }
                SelectCityList.Add(model);
                selectedProvince = curProvincePanel;
            }
            else
            {
                if (model.code == "0")
                {
                    foreach (var item in SelectCityList)
                    {
                        if (item.code == selectedProvince.code)
                        {
                            SelectCityList.Remove(item);
                            model.isSelected = false;
                            break;
                        }
                    }
                    return;
                }
                foreach (var item in SelectCityList)
                {
                    if(item.code == model.code)
                    {
                        SelectCityList.Remove(item);
                        break;
                    }
                }
            }
        }

        private void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            HttpCityCodes model = btn.DataContext as HttpCityCodes;

            if (model.code == selectedProvince.code)
            {
                foreach (var item in SelectCityList)
                {
                    if (item.code == selectedProvince.code)
                    {
                        SelectCityList.Remove(item);
                        break;
                    }
                }
                this.cityList[0].isSelected = false;
                return;
            }
            foreach (var item in SelectCityList)
            {
                if (item.code == model.code)
                {
                    SelectCityList.Remove(item);
                    break;
                }
            }
            foreach(var item in this.cityList)
            {
                if(item.code == model.code)
                {
                    item.isSelected = false;
                }
            }

        }

        private bool IsHotCity(HttpCityCodes model)
        {
            string code = model.code;
            bool flag = false;
            //杭州
            if (code =="863301")
            {
                flag = true;
            }
            //武汉
            if (code==("864201"))
            {
                flag = true;
            }
            //成都
            if (code==("865101"))
            {
                flag = true;
            }
            //广州
            if (code==("864401"))
            {
                flag = true;
            }
            //深圳
            if (code==("864403"))
            {
                flag = true;
            }
            //北京
            if (code==("8611"))
            {
                flag = true;
            }
            //天津
            if (code==("8612"))
            {
                flag = true;
            }
            //上海
            if (code==("8631"))
            {
                flag = true;
            }
            //成都
            if (code==("8650"))
            {
                flag = true;
            }

            return flag;

        }

        private void InitialProvinceCity()
        {
            //加载城市二级列表
            string cityCodeStr = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "CityCode.json");
            JsonCityCodeMode jsonCityModel = DAL.JsonHelper.ToObject<JsonCityCodeMode>(cityCodeStr);
            List<CityCodeItem> ProvinceJsonList = new List<CityCodeItem>();
            List<CityCodeItem> HotCityJsonList = new List<CityCodeItem>();
            List<CityCodeItem> CityJsonList = new List<CityCodeItem>();

            foreach (var item in jsonCityModel.CityCodes.CityCode)
            {
                if (item.value == "8611")
                {
                    HotCityJsonList.Add(item);
                    continue;
                }
                if (item.value == "8612")
                {
                    HotCityJsonList.Add(item);
                    continue;
                }
                if (item.value == "8631")
                {
                    HotCityJsonList.Add(item);
                    continue;
                }
                if (item.value == "8650")
                {
                    HotCityJsonList.Add(item);
                    continue;
                }
                if (item.value.Length == 4)
                {
                    ProvinceJsonList.Add(item);
                }
                if (item.value.Length == 6)
                {
                    CityJsonList.Add(item);
                }
                //杭州
                if (item.value.StartsWith("863301"))
                {
                    if (item.value.Length == 6)
                    {
                        HotCityJsonList.Add(item);
                    }
                    else
                    {
                        CityJsonList.Add(item);
                    }
                }
                //武汉
                if (item.value.StartsWith("864201"))
                {
                    if (item.value.Length == 6)
                    {
                        HotCityJsonList.Add(item);
                    }
                    else
                    {
                        CityJsonList.Add(item);
                    }
                }
                //成都
                if (item.value.StartsWith("865101"))
                {
                    if (item.value.Length == 6)
                    {
                        HotCityJsonList.Add(item);
                    }
                    else
                    {
                        CityJsonList.Add(item);
                    }
                }
                //广州
                if (item.value.StartsWith("864401"))
                {
                    if (item.value.Length == 6)
                    {
                        HotCityJsonList.Add(item);
                    }
                    else
                    {
                        CityJsonList.Add(item);
                    }
                }
                //深圳
                if (item.value.StartsWith("864403"))
                {
                    if (item.value.Length == 6)
                    {
                        HotCityJsonList.Add(item);
                    }
                    else
                    {
                        CityJsonList.Add(item);
                    }
                }
            }
            this.provinceList.Clear();
            //this.provinceList.Add(new HttpProvinceCityCodes() { name = "不限", code = "0", city = new List<HttpCityCodes>() { new HttpCityCodes() { name = "不限", code = "0"} } });
            foreach (var item in ProvinceJsonList)
            {
                HttpProvinceCityCodes proCode = new HttpProvinceCityCodes();
                proCode.name = item.simpleName;
                proCode.code = item.value;
                proCode.city = new List<HttpCityCodes>();
                if(!ForceSecondSelcted)
                  proCode.city.Add(new HttpCityCodes() { name = "不限", code = "0" });
                foreach (var item2 in CityJsonList)
                {
                    if (item2.value.StartsWith(item.value) && item2.value != item.value)
                        proCode.city.Add(new HttpCityCodes() { name = item2.simpleName, code = item2.value });
                }
                this.provinceList.Add(proCode);
            }
            foreach (var item in HotCityJsonList)
            {
                HttpProvinceCityCodes proCode = new HttpProvinceCityCodes();
                proCode.name = item.simpleName;
                proCode.code = item.value;
                proCode.city = new List<HttpCityCodes>();
                if(!ForceSecondSelcted)
                  proCode.city.Add(new HttpCityCodes() { name = "不限", code = "0" });
                foreach (var item2 in CityJsonList)
                {
                    if (item2.value.StartsWith(item.value) && item2.value != item.value)
                        proCode.city.Add(new HttpCityCodes() { name = item2.simpleName, code = item2.value });
                }
                this.hotCityList.Add(proCode);
            }
            lstProvince.ItemsSource = this.provinceList;
            lstHotCity.ItemsSource = this.hotCityList;

        }

        private async void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.OutSelectCityList.Clear();
            foreach(var item in SelectCityList)
            {
                this.OutSelectCityList.Add(item);
            }
            if (this.OKOrCancelAction != null)
                this.OKOrCancelAction(true);
            if (this.OKOrCancelActionDelegate != null)
                await OKOrCancelActionDelegate(true);
        }

        private async void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.OKOrCancelAction != null)
                this.OKOrCancelAction(false);
            if (this.OKOrCancelActionDelegate != null)
                await OKOrCancelActionDelegate(false);
        }
    }
}
