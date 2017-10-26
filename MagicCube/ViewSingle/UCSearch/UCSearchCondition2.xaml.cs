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
using MagicCube.Model;
using System.IO;
using MagicCube.HttpModel;
using MagicCube.ViewModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCSearchCondition2.xaml 的交互逻辑
    /// </summary>
    public partial class UCSearchCondition2 : UserControl
    {
        public Action OpenPublishJobAction;
        public UCSearchCondition2()
        {
            InitializeComponent();
            this.stkCondition.DataContext = conditionModel;
            this.ucJobAssociation.OpenAciton += OpenJobTreeCallback;
            this.Loaded += UCSearchCondition_Loaded;
            this.ucJobTree.ResultAction += UCJobTreeCallback;
            this.ucIndustry.OpenAction += UCIndustryOpenCallback;
            this.ucPageTurn.PageChange += PageChangeCallback;
            Messaging.Messenger.Default.Register<Messaging.MSBodyBase>(this, GotoJobSearchCallback);
        }
        #region "变量"
        private SearchConditioanModel conditionModel = new SearchConditioanModel();
        private ObservableCollection<CodeModel> workingExpLowList = new ObservableCollection<CodeModel>();
        private ObservableCollection<CodeModel> workingExpHighList = new ObservableCollection<CodeModel>();
        private ObservableCollection<CodeModel> educationLowList = new ObservableCollection<CodeModel>();
        private ObservableCollection<CodeModel> educationHighList = new ObservableCollection<CodeModel>();
        private ObservableCollection<CodeModel> salaryLowList = new ObservableCollection<CodeModel>();
        private ObservableCollection<CodeModel> salaryHighList = new ObservableCollection<CodeModel>();
        private ObservableCollection<CodeModel> updatetimeList = new ObservableCollection<CodeModel>();
        private ObservableCollection<CodeModel> lauguageList = new ObservableCollection<CodeModel>();
        private ObservableCollection<CodeModel> statusList = new ObservableCollection<CodeModel>();
        private ObservableCollection<ViewModel.HttpConditionSaveModel> ConditionSaveList = new ObservableCollection<ViewModel.HttpConditionSaveModel>();

        private ObservableCollection<JobManageModel> jobPulishList = new ObservableCollection<JobManageModel>();

        private ObservableCollection<HttpIndustresItem> industriesList = new ObservableCollection<HttpIndustresItem>();
        private List<HttpIndustresItem> selectIndustryTemp = new List<HttpIndustresItem>();
        private ObservableCollection<string> autoAssociateList = new ObservableCollection<string>();
        private JsonCityCodeMode jsonCityModel;


        List<HttpProvinceCityCodes> provinceList = new List<HttpProvinceCityCodes>();
        List<HttpProvinceCityCodes> hotCityList = new List<HttpProvinceCityCodes>();

        public delegate Task ConditionAction(SearchConditionSaveModel model);
        public ConditionAction SearchConditionAction;
        int totalData = 0;
        int curPage = 1;
        int totalPage = 0;


        //public Action<SearchConditionSaveModel> SearchConditionAction;

        #endregion 

        #region "回调函数"

        private void UCSearchCondition_Loaded(object sender, RoutedEventArgs e)
        {
           


        }
        private void OpenJobTreeCallback(bool isOpen)
        {

            this.gdJobTree.Visibility = Visibility.Visible;

            //this.ucJobTree.SelectItems = this.ucJobAssociation.SelectedVals;
            this.ucJobTree.SelectItems.Clear();
            foreach (var item in this.ucJobAssociation.SelectedVals)
            {
                this.ucJobTree.SelectItems.Add(item);
            }
            this.ucJobTree.iniJobTree();

        }

        private void UCIndustryOpenCallback()
        {

            foreach (var item in this.industriesList)
            {
                item.isChoose = false;
                foreach (var item2 in this.ucIndustry.selectIndustry)
                {
                    if (item.code == item2.code)
                    {
                        item.isChoose = true;
                        break;
                    }
                }
            }

            //this.ucIndustry.selectIndustry.Clear();
            //foreach(var item in selectIndustryTemp)
            //{
            //    this.ucIndustry.selectIndustry.Add(item);
            //}
            this.selectIndustryTemp.Clear();
            foreach(var item in this.ucIndustry.selectIndustry)
            {
                this.selectIndustryTemp.Add(item);
            }
            this.gdIndustry.Visibility = Visibility.Visible;

        }

        private void UCJobTreeCallback(bool flag)
        {
            this.gdJobTree.Visibility = Visibility.Collapsed;
            if(flag)
            {
                this.ucJobAssociation.SelectedVals.Clear();
                foreach (var item in this.ucJobTree.SelectItems)
                {
                    
                    this.ucJobAssociation.SelectedVals.Add(item);
                }
            }
            else
            {
                this.ucJobTree.SelectItems.Clear();
                foreach (var item in this.ucJobAssociation.SelectedVals)
                {
                    this.ucJobTree.SelectItems.Add(item);
                };
            }
            if(this.ucJobAssociation.SelectedVals.Count > 0)
            {
                this.ucJobAssociation.txtWaterMark.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.ucJobAssociation.txtWaterMark.Visibility = Visibility.Visible;
            }
        }
        private async Task PageChangeCallback(int pcurPage)
        {
            curPage = pcurPage;
            await UpdateJobPublishList();
        }

        private void ProvinceCityCallback(bool flag)
        {
            this.chkExpectCityOpen.IsChecked = false;
            if(flag)
            {
                this.conditionModel.ExpectCity.Clear();
                foreach (var item in ucProvinceCity.OutSelectCityList)
                {
                    this.conditionModel.ExpectCity.Add(item);
                }

            }
            if (this.conditionModel.ExpectCity.Count == 0)
            {
                this.txtExpectCity.Hint = "请输入或选择";
            }
            else
            {
                this.txtExpectCity.Hint = "";
            }

        }

        private void LocationCityCallback(bool flag)
        {
            this.chkLocationOpen.IsChecked = false;
            if (flag)
            {
                this.conditionModel.LocationCity.Clear();
                foreach (var item in ucLocaitonCityPanel.OutSelectCityList)
                {
                    this.conditionModel.LocationCity.Add(item);
                }
            }
            if (this.conditionModel.LocationCity.Count == 0)
            {
                this.txtLocationCity.Hint = "请输入或选择";
            }
            else
            {
                this.txtLocationCity.Hint = "";
            }
        }


        private async void GotoJobSearchCallback(Messaging.MSBodyBase sender)
        {
            if(sender.type == "jobPublish")
            {
                this.rbResumCondition.IsChecked = true;
                await this.UpdateJobPublishList();
            }
        }

        #endregion
        #region "对内函数"
        private void MatchAgeSetting()
        {
            if (string.IsNullOrEmpty(this.txtAgeMin.Text) && string.IsNullOrEmpty(this.txtAgeMax.Text))
            {
                return;
            }
            if (string.IsNullOrEmpty(this.txtAgeMin.Text))
            {
                //this.txtAgeMin.Text = "1";
                return;
            }
            if (string.IsNullOrEmpty(this.txtAgeMax.Text))
            {
                //this.txtAgeMax.Text = "99";
                return;
            }
            if (Convert.ToInt32(this.txtAgeMin.Text) > Convert.ToInt32(this.txtAgeMax.Text))
            {
                string temp = this.txtAgeMin.Text;
                this.txtAgeMin.Text = this.txtAgeMax.Text;
                this.txtAgeMax.Text = temp;
            }
        }

     
        private void InitialWorkingExpList()
        {
            workingExpLowList.Clear();
            workingExpLowList.Add(new CodeModel() { name = "不限", code = "0" });
            workingExpLowList.Add(new CodeModel() { name = "在读学生", code ="1" });
            workingExpLowList.Add(new CodeModel() { name = "应届毕业生", code = "2" });
            workingExpLowList.Add(new CodeModel() { name = "1年", code = "3" });
            workingExpLowList.Add(new CodeModel() { name = "2年", code = "4" });
            workingExpLowList.Add(new CodeModel() { name = "3年", code = "5" });
            workingExpLowList.Add(new CodeModel() { name = "4年", code = "6" });
            workingExpLowList.Add(new CodeModel() { name = "5年", code = "7" });
            workingExpLowList.Add(new CodeModel() { name = "6年", code = "8" });
            workingExpLowList.Add(new CodeModel() { name = "7年", code = "9" });
            workingExpLowList.Add(new CodeModel() { name = "8年", code = "10" });
            workingExpLowList.Add(new CodeModel() { name = "9年", code = "11" });
            workingExpLowList.Add(new CodeModel() { name = "10年", code = "12" });


            this.cmbWorkExpLow.ItemsSource = this.workingExpLowList;
            this.cmbWorkExpLow.SelectedIndex = 0;
        }

        private void InitialEducationList()
        {
            this.educationLowList.Clear();
            this.educationLowList.Add(new CodeModel() { name = "不限", code = "0" });
            this.educationLowList.Add(new CodeModel() { name = "初中", code = "1" });
            this.educationLowList.Add(new CodeModel() { name = "高中/中技/中专", code = "2" });
            this.educationLowList.Add(new CodeModel() { name = "大专", code = "3" });
            this.educationLowList.Add(new CodeModel() { name = "本科", code = "4" });
            this.educationLowList.Add(new CodeModel() { name = "硕士", code = "5" });
            this.educationLowList.Add(new CodeModel() { name = "博士", code = "6" });
            this.cmbEducaitonLow.ItemsSource = this.educationLowList;
            this.cmbEducaitonLow.SelectedIndex = 0;
        }

        private void InitialSalaryList()
        {
            this.salaryLowList.Clear();
            this.salaryLowList.Add(new CodeModel() { name = "不限", code = "0" });
            this.salaryLowList.Add(new CodeModel() { name = "1K", code = "1" });
            this.salaryLowList.Add(new CodeModel() { name = "2K", code = "2" });
            this.salaryLowList.Add(new CodeModel() { name = "3K", code = "3" });
            this.salaryLowList.Add(new CodeModel() { name = "4K", code = "4" });
            this.salaryLowList.Add(new CodeModel() { name = "5K", code = "5" });
            this.salaryLowList.Add(new CodeModel() { name = "6K", code = "6" });
            this.salaryLowList.Add(new CodeModel() { name = "8K", code = "7" });
            this.salaryLowList.Add(new CodeModel() { name = "10K", code = "8" });
            this.salaryLowList.Add(new CodeModel() { name = "15K", code = "9" });
            this.salaryLowList.Add(new CodeModel() { name = "20K", code = "10" });
            this.salaryLowList.Add(new CodeModel() { name = "25K", code = "11" });
            this.salaryLowList.Add(new CodeModel() { name = "30K", code = "12" });
            this.salaryLowList.Add(new CodeModel() { name = "40K", code = "13" });
            this.salaryLowList.Add(new CodeModel() { name = "50K", code = "14" });
            this.salaryLowList.Add(new CodeModel() { name = "70K", code = "15" });
            this.salaryLowList.Add(new CodeModel() { name = "100K", code = "16" });
            this.salaryLowList.Add(new CodeModel() { name = "及以上", code = "17" });

            this.cmbSalaryLow.ItemsSource = this.salaryLowList;
            this.cmbSalaryLow.SelectedIndex = 0;

        }

        private void InitialProvinceCity()
        {
            //加载城市二级列表
            string cityCodeStr = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "CityCode.json");
            this.jsonCityModel = DAL.JsonHelper.ToObject<JsonCityCodeMode>(cityCodeStr);
            this.ucProvinceCity.OKOrCancelAction += ProvinceCityCallback;
            this.ucLocaitonCityPanel.OKOrCancelAction += LocationCityCallback;
            this.conditionModel.ExpectCity.Clear();
            this.conditionModel.LocationCity.Clear();
            this.txtExpectCity.Hint = "请输入或选择";
            this.txtLocationCity.Hint = "请输入或选择";
            //this.ucLocationCity
            //List<CityCodeItem> ProvinceJsonList = new List<CityCodeItem>();
            //List<CityCodeItem> HotCityJsonList = new List<CityCodeItem>();
            //List<CityCodeItem> CityJsonList = new List<CityCodeItem>();

            //foreach (var item in jsonCityModel.CityCodes.CityCode)
            //{
            //    if(item.value == "8611")
            //    {
            //        HotCityJsonList.Add(item);
            //        continue;
            //    }
            //    if(item.value == "8612")
            //    {
            //        HotCityJsonList.Add(item);
            //        continue;
            //    }
            //    if(item.value == "8631")
            //    {
            //        HotCityJsonList.Add(item);
            //        continue;
            //    }
            //    if(item.value == "8650")
            //    {
            //        HotCityJsonList.Add(item);
            //        continue;
            //    }
            //    if (item.value.Length == 4)
            //    {
            //        ProvinceJsonList.Add(item);
            //    }
            //    if (item.value.Length == 6)
            //    {
            //        CityJsonList.Add(item);
            //    }
            //    //杭州
            //    if (item.value.StartsWith("863301"))
            //    {
            //        if (item.value.Length == 6)
            //        {
            //            HotCityJsonList.Add(item);
            //        }
            //        else
            //        {
            //            CityJsonList.Add(item);
            //        }
            //    }
            //    //武汉
            //    if (item.value.StartsWith("864201"))
            //    {
            //        if (item.value.Length == 6)
            //        {
            //            HotCityJsonList.Add(item);
            //        }
            //        else
            //        {
            //            CityJsonList.Add(item);
            //        }
            //    }
            //    //成都
            //    if (item.value.StartsWith("865101"))
            //    {
            //        if (item.value.Length == 6)
            //        {
            //            HotCityJsonList.Add(item);
            //        }
            //        else
            //        {
            //            CityJsonList.Add(item);
            //        }
            //    }
            //    //广州
            //    if (item.value.StartsWith("864401"))
            //    {
            //        if (item.value.Length == 6)
            //        {
            //            HotCityJsonList.Add(item);
            //        }
            //        else
            //        {
            //            CityJsonList.Add(item);
            //        }
            //    }
            //    //深圳
            //    if (item.value.StartsWith("864403"))
            //    {
            //        if (item.value.Length == 6)
            //        {
            //            HotCityJsonList.Add(item);
            //        }
            //        else
            //        {
            //            CityJsonList.Add(item);
            //        }
            //    }
            //}
            //this.provinceList.Clear();
            ////this.provinceList.Add(new HttpProvinceCityCodes() { name = "不限", code = "0", city = new List<HttpCityCodes>() { new HttpCityCodes() { name = "不限", code = "0"} } });
            //foreach (var item in ProvinceJsonList)
            //{
            //    HttpProvinceCityCodes proCode = new HttpProvinceCityCodes();
            //    proCode.name = item.simpleName;
            //    proCode.code = item.value;
            //    proCode.city = new List<HttpCityCodes>();
            //    //proCode.city.Add(new HttpCityCodes() { name = "不限", code = "0" });
            //    foreach (var item2 in CityJsonList)
            //    {
            //        if (item2.value.StartsWith(item.value) && item2.value != item.value)
            //            proCode.city.Add(new HttpCityCodes() { name = item2.simpleName, code = item2.value });
            //    }
            //    this.provinceList.Add(proCode);
            //}
            //foreach(var item in HotCityJsonList)
            //{
            //    HttpProvinceCityCodes proCode = new HttpProvinceCityCodes();
            //    proCode.name = item.simpleName;
            //    proCode.code = item.value;
            //    proCode.city = new List<HttpCityCodes>();
            //    //proCode.city.Add(new HttpCityCodes() { name = "不限", code = "0" });
            //    foreach (var item2 in CityJsonList)
            //    {
            //        if (item2.value.StartsWith(item.value) && item2.value != item.value)
            //            proCode.city.Add(new HttpCityCodes() { name = item2.simpleName, code = item2.value });
            //    }
            //    this.hotCityList.Add(proCode);
            //}

            ////this.cmbProvince.ItemsSource = this.provinceList;
            //this.cmblocationProvince.ItemsSource = this.provinceList;
            ////this.cmbCity.SelectedIndex = -1;
            ////this.cmbProvince.SelectedIndex = -1;
            ////this.cmbLocationCity.SelectedIndex = -1;
            ////this.cmblocationProvince.SelectedIndex = -1;
            ////this.cmbProvince.SelectedIndex = 0;
            //this.cmblocationProvince.SelectedIndex = 0;
            ////this.ucProvinceCity.lstProvince.ItemsSource = this.provinceList;
            ////this.ucProvinceCity.lstHotCity.ItemsSource = this.hotCityList;

        }
        private string GetHotCityCode(string districtCode)
        {
            string tempCode = districtCode;
            //杭州
            if (districtCode.StartsWith("863301"))
            {
                tempCode = "863301";
            }
            //武汉
            if (districtCode.StartsWith("864201"))
            {
                tempCode = "864201";
            }
            //成都
            if (districtCode.StartsWith("865101"))
            {
                tempCode = "865101";
            }
            //广州
            if (districtCode.StartsWith("864401"))
            {
                tempCode = "864401";
            }
            //深圳
            if (districtCode.StartsWith("864403"))
            {
                tempCode = "864401";
            }
            //北京
            if(districtCode.StartsWith("8611"))
            {
                tempCode = "8611";
            }
            //天津
            if(districtCode.StartsWith("8612"))
            {
                tempCode = "8612";
            }
            //上海
            if(districtCode.StartsWith("8631"))
            {
                tempCode = "8631";
            }
            //成都
            if(districtCode.StartsWith("8650"))
            {
                tempCode = "8650";
            }

            return tempCode;
        }

        private void InitialUpdateTime()
        {
            this.updatetimeList.Clear();
            this.updatetimeList.Add(new CodeModel() { name = "不限", code = "0" });
            this.updatetimeList.Add(new CodeModel() { name = "三天内", code = "1" });
            this.updatetimeList.Add(new CodeModel() { name = "一周内", code = "2" });
            this.updatetimeList.Add(new CodeModel() { name = "两周内", code = "3" });
            this.updatetimeList.Add(new CodeModel() { name = "一月内", code = "4" });
            this.updatetimeList.Add(new CodeModel() { name = "两月内", code = "5" });
            this.updatetimeList.Add(new CodeModel() { name = "三月内", code = "6" });
            this.updatetimeList.Add(new CodeModel() { name = "半年内", code = "7"  });

            this.cmbUpdateTime.ItemsSource = this.updatetimeList;
            this.cmbUpdateTime.SelectedIndex = 0;

        }

        private void InitialLauguageList()
        {
            this.lauguageList.Clear();
            this.lauguageList.Add(new CodeModel() { name = "不限", code = "0" });
            this.lauguageList.Add(new CodeModel() { name = "英语", code = "1" });
            this.lauguageList.Add(new CodeModel() { name = "法语", code = "2" });
            this.lauguageList.Add(new CodeModel() { name = "日语", code = "3" });
            this.lauguageList.Add(new CodeModel() { name = "韩语", code = "4" });
            this.lauguageList.Add(new CodeModel() { name = "德语", code = "5" });
            this.lauguageList.Add(new CodeModel() { name = "俄语", code = "6" });
            this.lauguageList.Add(new CodeModel() { name = "阿拉伯语", code = "7" });
            this.lauguageList.Add(new CodeModel() { name = "普通话", code = "8" });
            this.lauguageList.Add(new CodeModel() { name = "粤语", code = "9" });


            this.cmbLauguage.ItemsSource = this.lauguageList;
            this.cmbLauguage.SelectedIndex = 0;

        }

        private void InitialStatusList()
        {
            this.statusList.Clear();
            this.statusList.Add(new CodeModel() { name = "不限", code = "0" });
            this.statusList.Add(new CodeModel() { name = "目前正在找工作", code = "1" });
            this.statusList.Add(new CodeModel() { name = "在职，正考虑换工作", code = "2" });
            this.statusList.Add(new CodeModel() { name = "在职，只考虑更好的机会", code = "3" });
            this.statusList.Add(new CodeModel() { name = "在职，暂无跳槽打算", code = "4" });

            this.cmbStatus.ItemsSource = this.statusList;
            this.cmbStatus.SelectedIndex = 0;

        }

        private void InitialIndustryList()
        {
            string industriesResult = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "companyInfoDict.json");
            jsonCompanyInfoDictModel jsonCompanyModel = DAL.JsonHelper.ToObject<jsonCompanyInfoDictModel>(industriesResult);
            if (jsonCompanyModel != null)
            {
                industriesList.Clear();
                foreach (var item in jsonCompanyModel.DataDic.element)
                {
                    if (item.name == "industry")
                    {
                        foreach (var industryItem in item.mappingItem)
                        {
                            industriesList.Add(new HttpIndustresItem() { name = industryItem.fullName, code = industryItem.value, isChoose = false });
                        }
                    }

                }
            }
            itemsIndustries.ItemsSource = industriesList;
        }

        private SearchConditionSaveModel GetParamCondition()
        {
            SearchConditionSaveModel model = new SearchConditionSaveModel();
            model.words = this.conditionModel.KeyWords.Trim();
            model.companyName = this.conditionModel.CompanyName.Trim();
            model.onlyLastCompany = this.conditionModel.OnlyLastCompany;

            model.jobType = new List<CodeModel>();
            foreach(var item in ucJobAssociation.SelectedVals)
            {
                model.jobType.Add(new CodeModel() { name = item.name, code = item.code });
            }

            model.industry = new List<CodeModel>();
            foreach(var item in ucIndustry.selectIndustry)
            {
                model.industry.Add(new CodeModel() { name = item.name, code =item.code });
            }

            model.age = new Age();
            model.age.minAge = this.conditionModel.MinAge;
            model.age.maxAge = this.conditionModel.MaxAge;

            model.workExp = new WorkExp();
            model.workExp.minWorkExp = this.conditionModel.WorkingExpLow;
            model.workExp.maxWorkExp = this.conditionModel.WorkingExpHigh;
            //一年及以下
            if(this.conditionModel.WorkingExpHigh.code == "-4")
            {
                model.workExp.minWorkExp = new CodeModel() { name = "不限", code = "0" };
                model.workExp.maxWorkExp = new CodeModel() { name = "1", code = "3" };
            }
            //及以上
            if(this.conditionModel.WorkingExpHigh.code == "-5")
            {
                model.workExp.maxWorkExp = new CodeModel() { name = "不限", code = "0" };
            }

            model.degree = new Degree();
            model.degree.minDegreeCode = this.conditionModel.EducationLow.code.ToString();
            model.degree.maxDegreeCode = this.conditionModel.EducationHigh.code.ToString();
            model.degree.expand = new List<CodeModel>();
            if ((bool)this.chk985.IsChecked)
                model.degree.expand.Add(new CodeModel() { name = "985", code = "0" });
            if ((bool)this.chk211.IsChecked)
                model.degree.expand.Add(new CodeModel() { name = "211", code = "1" });
            if ((bool)this.chkAllStu.IsChecked)
                model.degree.expand.Add(new CodeModel() { name = "全日制", code = "2" });
            if ((bool)this.chkOverSea.IsChecked)
                model.degree.expand.Add(new CodeModel() { name = "海外留学", code = "3" });

            model.salary = new Salary();
            model.salary.minSalaryCode = this.conditionModel.SalaryLow.code;
            model.salary.maxSalaryCode = this.conditionModel.SalaryHigh.code;

            model.expCity = new List<CodeModel>();
            foreach(var item in conditionModel.ExpectCity)
            {
                model.expCity.Add(new CodeModel() { name = item.name, code = item.code });
            }
            //if(this.conditionModel.LocationCity != null)
            //{
            //    if (this.conditionModel.ExpectCity.code == "0")
            //    {
            //        model.expCity.Add(new CodeModel() { name = this.conditionModel.ExpectProvince.name, code = this.conditionModel.ExpectProvince.code });
            //    }
            //    else
            //    {
            //        model.expCity.Add(new CodeModel() { name = this.conditionModel.ExpectCity.name, code = this.conditionModel.ExpectCity.code });
            //    }
            //}
           
            model.address = new List<CodeModel>();
            foreach (var item in conditionModel.LocationCity)
            {
                model.address.Add(new CodeModel() { name = item.name, code = item.code });
            }
            //if(this.conditionModel.LocationCity != null)
            //{
            //    if(this.conditionModel.LocationCity.code == "0")
            //    {
            //        model.address.Add(new CodeModel() { name = this.conditionModel.LocationProvince.name, code = this.conditionModel.LocationProvince.code });
            //    }
            //    else
            //    {
            //        model.address.Add(new CodeModel() { name = this.conditionModel.LocationCity.name, code = this.conditionModel.LocationCity.code });

            //    }
            //}

            model.gender = this.conditionModel.Gender;
            if(conditionModel.UpdateTime != null)
                model.updateTime = this.conditionModel.UpdateTime.code.ToString();
            model.major = this.conditionModel.Major;
            model.school = this.conditionModel.School;
            if(conditionModel.Lauguage !=null)
            model.language = this.conditionModel.Lauguage.name.ToString();
            if(conditionModel.Status!=null)
            model.status = this.conditionModel.Status.code.ToString();


            if((bool)this.rbGenderMan.IsChecked)
            {
                model.gender = "1";
            }
            else if((bool)this.rbGenderWoman.IsChecked)
            {
                model.gender = "0";
            }
            else
            {
                model.gender = "-1";
            }
            model.school211 =(bool)this.chk211.IsChecked ;
            model.school985 = (bool)this.chk985.IsChecked;
            model.schoolAll = (bool)this.chkAllStu.IsChecked;
            model.schoolOverSea = (bool)this.chkOverSea.IsChecked;

            return model;

        }

        private async Task InitialConditionSaveList()
        {
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID" }, new string[] { MagicGlobal.UserInfo.Id.ToString() });
            string urlTemp = string.Format(DAL.ConfUtil.AddrConditionSaveList, MagicGlobal.UserInfo.Version, std);
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(urlTemp);
            ViewModel.BaseHttpModel<HttpConditionSaveListModel> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<HttpConditionSaveListModel>>(resultStr);
            if (model == null)
                return;
            else
            {
                if(model.code == 200)
                {
                    this.ConditionSaveList.Clear();
                    foreach(var item in model.data.conditionList)
                    {
                        this.ConditionSaveList.Add(item);
                    }
                }
            }
            this.lstSaveSearchCondition.ItemsSource = this.ConditionSaveList;
        }

        private string GetSaveConditionString()
        {
            string temp = string.Empty;
            if(!string.IsNullOrEmpty( this.conditionModel.KeyWords.Trim()))
                temp += this.conditionModel.KeyWords + "/";
            if(!string.IsNullOrEmpty(this.conditionModel.CompanyName.Trim()))
            {
                temp += this.conditionModel.CompanyName + "/";
            }
            if((bool)this.chkOnlyLastCompany.IsChecked)
            {
                temp += "只搜最近工作/";
            }
            if(this.ucIndustry.selectIndustry.Count > 0)
            {
                string temp2 = string.Empty;
                foreach(var item in this.ucIndustry.selectIndustry)
                {
                    temp2 += item.name + "、";
                }
                temp2 = temp2.TrimEnd(new char[] { '、' });
                temp += temp2 + "/";
            }


            if(this.ucJobAssociation.SelectedVals.Count > 0)
            {
                string temp2 = string.Empty;
                foreach(var item in this.ucJobAssociation.SelectedVals)
                {
                    temp2 += item.name + "、";
                }
                temp2 = temp2.TrimEnd(new char[] { '、' });
                temp += temp2 + "/";
            }
            if(!string.IsNullOrEmpty(this.txtAgeMin.Text))
            {
                temp += this.txtAgeMin.Text + "岁-";
            }
            if(!string.IsNullOrEmpty(this.txtAgeMax.Text))
            {
                temp += this.txtAgeMax.Text + "岁/";
            }
            if(this.cmbWorkExpLow.SelectedIndex >0)
            {
                temp += (this.cmbWorkExpLow.SelectedItem as CodeModel).name + "-";
                temp += (this.cmbWorkExpHigh.SelectedItem as CodeModel).name + "/";
            }
            if(this.cmbEducaitonLow.SelectedIndex >0)
            {
                temp += (this.cmbEducaitonLow.SelectedItem as CodeModel).name + "-";
                temp += (this.cmbEducationHigh.SelectedItem as CodeModel).name + "/";
            }
            if((bool)this.chk211.IsChecked)
            {
                temp += "211/";
            }
            if((bool)this.chk985.IsChecked)
            {
                temp += "985/";
            }
            if((bool)this.chkAllStu.IsChecked)
            {
                temp += "全日制/";
            }
            if((bool)this.chkOverSea.IsChecked)
            {
                temp += "海外留学/";
            }
            if(this.cmbSalaryLow.SelectedIndex >0)
            {
                temp += (this.cmbSalaryLow.SelectedItem as CodeModel).name + "-";
                temp += (this.cmbSalaryHigh.SelectedItem as CodeModel).name + "/";
            }
            //if(this.cmbCity.SelectedIndex == 0)
            //{
            //    if(this.cmbProvince.SelectedIndex > 0)
            //    {
            //        temp += (this.cmbProvince.SelectedItem as HttpProvinceCityCodes).name + "/";
            //    }
            //}
            //if(this.cmbCity.SelectedIndex >0)
            //{
            //    temp += (this.cmbCity.SelectedItem as HttpCityCodes).name + "/";
            //}
            bool isHaveExpectCity = false;
            foreach(var item in conditionModel.ExpectCity)
            {
                temp += item.name + "、";
                isHaveExpectCity = true;
            }
            temp = temp.TrimEnd(new char[] { '、' });
            if(isHaveExpectCity)
                temp += "/";

            //if(this.cmbLocationCity.SelectedIndex == 0)
            //{
            //    if(this.cmblocationProvince.SelectedIndex > 0)
            //    {
            //        temp += (this.cmblocationProvince.SelectedItem as HttpProvinceCityCodes).name + "/";
            //    }
            //}
            //if(this.cmbLocationCity.SelectedIndex >0)
            //{
            //    temp += (this.cmbLocationCity.SelectedItem as HttpCityCodes).name + "/";
            //}
            bool isHaveLocationCity = false;
            foreach (var item in conditionModel.LocationCity)
            {
                temp += item.name + "、";
                isHaveLocationCity = true;
            }
            temp = temp.TrimEnd(new char[] { '、' });
            if(isHaveLocationCity)
                temp += "/";

            if ((bool)rbGenderMan.IsChecked)
            {
                temp += "男/";
            }
            if((bool)rbGenderWoman.IsChecked)
            {
                temp += "女/";
            }
            if(this.cmbUpdateTime.SelectedIndex >0)
            {
                temp += (this.cmbUpdateTime.SelectedItem as CodeModel).name + "/";
            }
            if(!string.IsNullOrEmpty(this.txtMajor.Text))
            {
                temp += this.txtMajor.Text + "/";
            }
            if(!string.IsNullOrEmpty(this.txtSchool.Text))
            {
                temp += this.txtSchool.Text + "/";
            }
            if(cmbLauguage.SelectedIndex > 0)
            {
                temp += (this.cmbLauguage.SelectedItem as CodeModel).name + "/";
            }
            if(cmbStatus.SelectedIndex > 0)
            {
                temp += (this.cmbStatus.SelectedItem as CodeModel).name + "/";
            }


            temp = temp.TrimEnd(new char[] { '/' });
            if(temp.Length >20)
            {
                temp = temp.Substring(0, 17) + "...";
            }
            

            return temp;
        }

        private async Task UpdateJobPublishList()
        {
            busyCtrl.IsBusy = true;
            GdNoJob.Visibility = Visibility.Collapsed;
            itemResumCondition.ItemsSource = null;
            string propertys = MagicCube.ViewModel.ModelTools.SetHttpPropertys<HttpJobList>();
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "startRow", "pageSize", "properties", "status", "desc", "jobType" }, 
                new string[] { MagicGlobal.UserInfo.Id.ToString(), ((curPage - 1) * DAL.ConfUtil.PageContent).ToString(), DAL.ConfUtil.PageContent.ToString(), propertys, "1", "fullName","2" });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobList, MagicGlobal.UserInfo.Version, std));
            BaseHttpModel<HttpJobListData> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpJobListData>>(jsonResult);
            busyCtrl.IsBusy = false;
            if (model == null)
            {

                TemplateUC.WinErroTip pWinMessage = new TemplateUC.WinErroTip(DAL.ConfUtil.netError);
                pWinMessage.Owner = Window.GetWindow(this);
                pWinMessage.ShowDialog();
                itemResumCondition.ItemsSource = null;
                return;
            }
            if (model.code != 200)
            {

                TemplateUC.WinErroTip pWinMessage = new TemplateUC.WinErroTip(DAL.ConfUtil.netError);
                pWinMessage.Owner = Window.GetWindow(this);
                pWinMessage.ShowDialog();
                itemResumCondition.ItemsSource = null;
                return;
            }
            totalData = model.data.total.onlineJobNum;

            jobPulishList.Clear();
            foreach (HttpJobList iHttpJobList in model.data.data)
            {
                jobPulishList.Add(SetModel.SetJobManageModel(iHttpJobList));
            }

            totalPage = totalData % DAL.ConfUtil.PageContent > 0 ? (totalData / DAL.ConfUtil.PageContent) + 1 : totalData / DAL.ConfUtil.PageContent;
            if (totalData == 0)
                curPage = 0;
            if (totalPage == 0)
                totalPage = 0;
            ucPageTurn.setPageState(curPage, totalPage, totalData);
            itemResumCondition.ItemsSource = jobPulishList;

            if (totalData == 0)
            {
                GdNoJob.Visibility = Visibility.Visible;
            }
            else
            {
                scollMain.ScrollToTop();
            }
        }

  


        #endregion

        #region "对外调用函数"
        public async Task InitialUCSearchCondition()
        {


            InitialUpdateTime();
            InitialLauguageList();
            InitialStatusList();

            InitialWorkingExpList();
            InitialEducationList();
            InitialSalaryList();
            InitialProvinceCity();
            InitialIndustryList();

            //this.txtAgeMax.Text = string.Empty; 
            //this.txtAgeMin.Text = string.Empty;
            //this.txtMajor.Text = string.Empty;
            this.conditionModel.MaxAge = string.Empty;
            this.conditionModel.MinAge = string.Empty;
            this.conditionModel.Major = string.Empty;
            this.conditionModel.School = string.Empty;
            this.txtSchool.Clear();
            this.chk211.IsChecked = false;
            this.chk985.IsChecked = false;
            this.chkAllStu.IsChecked = false;
            this.chkOverSea.IsChecked = false;
            this.chkOnlyLastCompany.IsChecked = false;

            //this.txtKeyWords.Text = string.Empty;
            //this.txtCompanyName.Text = string.Empty;
            this.conditionModel.KeyWords = string.Empty;
            this.conditionModel.CompanyName = string.Empty;
            this.ucIndustry.selectIndustry.Clear();
            this.ucIndustry.txtWaterMark.Visibility = Visibility.Visible;
            this.ucJobAssociation.SelectedVals.Clear();
            this.ucJobAssociation.txtWaterMark.Visibility = Visibility.Visible;
            this.rbNullGender.IsChecked = true;
            
            this.scollMain.ScrollToTop();
            //InitialConditionModel();

            this.chkMore.IsChecked = false;
            await InitialConditionSaveList();
            curPage = 1;
            //await UpdateJobPublishList();

        }

        public void SetRetunCondition(SearchConditionSaveModel model)
        {
            if(model == null)
            {
                this.rbSeachGondition.IsChecked = true;
                return;
            }
            this.txtKeyWords.Focusable = false;
            this.conditionModel.KeyWords = model.words.Trim();
            
            this.conditionModel.CompanyName = model.companyName.Trim();
            this.conditionModel.OnlyLastCompany = model.onlyLastCompany;
            this.chk211.IsChecked = model.school211;
            this.chk985.IsChecked = model.school985;
            this.chkAllStu.IsChecked = model.schoolAll;
            this.chkOverSea.IsChecked = model.schoolOverSea;
            this.ucJobAssociation.SelectedVals.Clear();
            foreach (var item in model.jobType)
            {
               
                this.ucJobAssociation.SelectedVals.Add(new ValCommon1() { name = item.name, code = item.code });
            }
            if(this.ucJobAssociation.SelectedVals.Count > 0)
            {
                this.ucJobAssociation.txtWaterMark.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.ucJobAssociation.txtWaterMark.Visibility = Visibility.Visible;
            }
            this.ucIndustry.selectIndustry.Clear();
            foreach(var item in model.industry)
            {
                this.ucIndustry.selectIndustry.Add(new HttpIndustresItem() { name = item.name, code = item.code, isChoose = true });
            }
            if(this.ucIndustry.selectIndustry.Count > 0)
            {
                this.ucIndustry.txtWaterMark.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.ucIndustry.txtWaterMark.Visibility = Visibility.Visible;
            }
            if(model.age != null)
            {
                this.conditionModel.MinAge = model.age.minAge;
                this.conditionModel.MaxAge = model.age.maxAge;
            }

            if(model.workExp.minWorkExp.code == "0" && model.workExp.maxWorkExp.code == "3")
            {
                this.conditionModel.WorkingExpLow = workingExpLowList[3];
                foreach (var item in this.workingExpHighList)
                {
                    if (item.code == "-4")
                    {
                        this.conditionModel.WorkingExpHigh = item;

                    }
                }
            } 
            else if(model.workExp.maxWorkExp.code == "0")
            {
                foreach (var item in this.workingExpLowList)
                {
                    if (item.code == model.workExp.minWorkExp.code)
                    {
                        this.conditionModel.WorkingExpLow = item;
                    }
                }
                foreach (var item in this.workingExpHighList)
                {
                    if (item.code == "-5")
                    {
                        this.conditionModel.WorkingExpHigh = item;

                    }
                }

            }
            else
            {
                foreach (var item in this.workingExpLowList)
                {
                    if (item.code == model.workExp.minWorkExp.code)
                    {
                        this.conditionModel.WorkingExpLow = item;
                    }
                }
                foreach (var item in this.workingExpHighList)
                {
                    if (item.code == model.workExp.maxWorkExp.code)
                    {
                        this.conditionModel.WorkingExpHigh = item;

                    }
                }
            }
          

           
           
            foreach(var item in this.educationLowList)
            {
                if(item.code == model.degree.minDegreeCode)
                {
                    this.conditionModel.EducationLow = item;
                    break;
                }
            }

            if(model.degree.maxDegreeCode == "7")
            {
                this.conditionModel.EducationHigh = new CodeModel() { name = "及以上", code = "7" };
            }
            else
            {
                foreach (var item in this.educationLowList)
                {
                    if (item.code == model.degree.maxDegreeCode)
                    {
                        this.conditionModel.EducationHigh = item;
                        break;
                    }
                }
            }
            this.conditionModel.Group211 = false;
            this.conditionModel.Gourp985 = false;
            this.conditionModel.FullTime = false;
            this.conditionModel.Oversea = false;
            if(model.degree.expand != null)
            {
                foreach (var item in model.degree.expand)
                {
                    if(item.code == "1")
                    {
                        this.conditionModel.Group211 = true;
                        chk211.IsChecked = true;
                    }
                    else if(item.code == "0")
                    {
                        this.conditionModel.Gourp985 = true;
                        chk985.IsChecked = true;
                    }
                    else if(item.code == "2")
                    {
                        this.conditionModel.FullTime = true;
                        chkAllStu.IsChecked = true;
                    }
                    else if(item.code == "3")
                    {
                        this.conditionModel.Oversea = true;
                        chkOverSea.IsChecked = true;
                    }

                }
            }
            foreach(var item in salaryLowList)
            {
                if(item.code == model.salary.minSalaryCode)
                {
                    this.conditionModel.SalaryLow = item;
                    break;
                }
            }
            foreach(var item in salaryLowList)
            {
                if(item.code == model.salary.maxSalaryCode)
                {
                    this.conditionModel.SalaryHigh = item;
                    break;
                }
            }
            if (model.expCity != null && model.expCity.Count > 0)
            {
                this.conditionModel.ExpectCity.Clear();
                foreach(var item in model.expCity)
                {
                    this.conditionModel.ExpectCity.Add(new HttpCityCodes() { code = item.code, name = item.name });
                }
                if(this.conditionModel.ExpectCity.Count > 0)
                {
                    this.txtExpectCity.Hint = "";
                }
                else
                {
                    this.txtExpectCity.Hint = "请输入或选择";
                }

            }

            if(model.address != null && model.address.Count > 0)
            {
                this.conditionModel.LocationCity.Clear();
                foreach (var item in model.address)
                {
                    this.conditionModel.LocationCity.Add(new HttpCityCodes() { code = item.code, name = item.name });
                }

                if(this.conditionModel.LocationCity.Count > 0)
                {
                    this.txtLocationCity.Hint = "";
                }
                else
                {
                    this.txtLocationCity.Hint = "请输入或选择";
                }
              
            }

            this.conditionModel.Gender = model.gender;
            switch(model.gender)
            {
                case "-1":
                    this.rbNullGender.IsChecked = true;
                    break;
                case "0":
                    this.rbGenderWoman.IsChecked = true;
                    break;
                case "1":
                    this.rbGenderMan.IsChecked = true;
                    break;

            }

            foreach(var item in this.updatetimeList)
            {
                if(item.code == model.updateTime)
                {
                    this.conditionModel.UpdateTime = item;
                    break;
                }
            }

            this.conditionModel.Major = model.major;
            this.conditionModel.School = model.school;
            foreach(var item in lauguageList)
            {
                if(item.name == model.language)
                {
                    this.conditionModel.Lauguage = item;
                    break;
                }
            }
            foreach(var item in statusList)
            {
                if(item.code == model.status)
                {
                    this.conditionModel.Status = item;
                    break;
                }
            }




        }
        #endregion

        private async void ClearHistorySearchCondition_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.6.1.37.1", "clk");
            HttpConditionSaveModel itemCondition = (sender as Button).DataContext as HttpConditionSaveModel;
            if (itemCondition == null)
                return;
            this.ConditionSaveList.Remove(itemCondition);
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "conditionID" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), itemCondition.conditionID });
            string resultStd = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrConditionDelete, MagicGlobal.UserInfo.Version, std));
        }
        /// <summary>
        /// 点击保存的搜索条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.6.1.36.1", "clk");
            HttpConditionSaveModel itemCondition = (sender as StackPanel).DataContext as HttpConditionSaveModel;
            if (itemCondition == null)
                return;
            SearchConditionSaveModel model = DAL.JsonHelper.ToObject<SearchConditionSaveModel>(itemCondition.conditionContent);
            if (model == null)
                return;
            model.conditionHeadShow = itemCondition.conditionName;
            if (this.SearchConditionAction != null)
            {
                await this.SearchConditionAction(model);
            }
        }
        /// <summary>
        /// 搜索按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
           
            //MatchAgeSetting();

            SearchConditionSaveModel model = this.GetParamCondition();
            model.conditionHeadShow = this.GetSaveConditionString();
            if(this.SearchConditionAction != null)
            {
                await this.SearchConditionAction(model);
            }
        }

        private void txtAge_LostFocus(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            if (fe.Name == "txtAgeMin")
                this.rectAgeMin.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
            else if (fe.Name == "txtAgeMax")
                this.rectAgeMax.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
            MatchAgeSetting();
        }

        private void txt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if(textBox.Text.Length == 0)
            {
                if (e.Key == Key.NumPad0 || e.Key == Key.D0)
                {
                    e.Handled = true;
                    return;
                }
            }
            
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Back)
            {
                
            }
            else
                e.Handled = true;
           
        }

        private void cmbWorkExpLow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            if (cmb == null)
                return;
            int index = cmb.SelectedIndex;
            this.cmbWorkExpHigh.SelectedIndex = -1;
            this.workingExpHighList.Clear();
            CodeModel model = cmb.SelectedItem as CodeModel;
            if (model != null)
            {
                if (model.name == "不限")
                {
                    this.workingExpHighList.Add(model);
                }
                else if (model.name == "在读学生")
                {
                    this.workingExpHighList.Add(model);
                }
                else if (model.name == "应届毕业生")
                {
                    this.workingExpHighList.Add(model);
                }
                else if(model.code == "3")
                {
                    this.workingExpHighList.Add(new CodeModel() { name = "以内", code = "-4" });
                    this.workingExpHighList.Add(new CodeModel() { name = "1年", code = "1" });
                    for (int i = index + 1; i < workingExpLowList.Count; i++)
                    {
                        this.workingExpHighList.Add(this.workingExpLowList[i]);
                    }
                    workingExpHighList.Add(new CodeModel() { name = "及以上", code = "-5" });
                }
                else
                {
                    for (int i = index; i < workingExpLowList.Count; i++)
                    {
                        this.workingExpHighList.Add(this.workingExpLowList[i]);
                    }
                    workingExpHighList.Add(new CodeModel() { name = "及以上", code = "-5" });
                }
            }            
            this.cmbWorkExpHigh.ItemsSource = this.workingExpHighList;
            this.cmbWorkExpHigh.SelectedIndex = this.workingExpHighList.Count - 1;
        }


        private void cmbEducaitonLow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            if (cmb == null)
                return;
            int index = cmb.SelectedIndex;

            this.cmbEducationHigh.SelectedIndex = -1;
            this.educationHighList.Clear();

            CodeModel model = cmb.SelectedItem as CodeModel;
            if(model != null)
            {
                if(model.name == "不限")
                {
                    this.educationHighList.Add(model);
                }
                else
                {
                    for (int i = index; i < educationLowList.Count; i++)
                    {
                        if (index == -1)
                            continue;
                        this.educationHighList.Add(this.educationLowList[i]);
                    }
                    this.educationHighList.Add(new CodeModel() { name = "及以上", code = "7" });
                }
            }

            

            this.cmbEducationHigh.ItemsSource = this.educationHighList;
            this.cmbEducationHigh.SelectedIndex = this.educationHighList.Count - 1;

        }

        private void cmbSalaryLow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            if (cmb == null)
                return;
            int index = cmb.SelectedIndex;
            this.cmbSalaryHigh.SelectedIndex = -1;
            this.salaryHighList.Clear();

            if(index == 0)
            {
                this.salaryHighList.Add(this.salaryLowList[0]);
            }
            else
            {
                //this.salaryHighList.Add(this.salaryLowList[salaryLowList.Count - 1]);
                for (int i = index; i < salaryLowList.Count; i++)
                {
                    if (index == -1)
                        return;
                    this.salaryHighList.Add(this.salaryLowList[i]);
                }
            }
            
            this.cmbSalaryHigh.ItemsSource = this.salaryHighList;
            this.cmbSalaryHigh.SelectedIndex = 0;
        }

        //private void cmbProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    this.cmbCity.SelectedIndex = 0;
        //}

        //private void cmbLocationProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    this.cmbLocationCity.SelectedIndex = 0;
        //}

       
        private async void btnSaveConditionOK_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.6.2.3.1", "clk");
            if(string.IsNullOrEmpty(this.txtSearchHeadName.Text))
            {
                this.rctSearchHeadName.Stroke = new SolidColorBrush(Color.FromRgb(0xf2, 0x57, 0x51));
                this.txtErroConditionName.Visibility = Visibility.Visible;
                return;
            }
            
            SearchConditionSaveModel Savemodel = this.GetParamCondition();
        
            ViewModel.HttpConditionSaveModel model = new HttpConditionSaveModel();
            model.conditionName = this.txtSearchHeadName.Text;
            model.userID = MagicGlobal.UserInfo.Id.ToString();
            model.conditionContent = DAL.JsonHelper.ToJsonString(Savemodel);
            string std = Uri.EscapeDataString(DAL.JsonHelper.ToJsonString(model));
            this.busyCtrl.IsBusy = true;
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrConditionUpload, MagicGlobal.UserInfo.Version, std));
            this.busyCtrl.IsBusy = false;
            ViewModel.BaseHttpModel<ViewModel.HttpConditionSaveModel> ResultModel = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<ViewModel.HttpConditionSaveModel>>(resultStr);
            if(ResultModel == null)
            {
                return;
            }
            else
            {
                if(ResultModel.code == 200)
                {
                    this.gdSaveCondition.Visibility = Visibility.Collapsed;
                    this.rctSearchHeadName.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
                    this.txtErroConditionName.Visibility = Visibility.Collapsed;
                    this.ConditionSaveList.Add(ResultModel.data);
                    //scollMain.ScrollToTop();
                    //if (this.SearchConditionAction != null)
                    //{
                    //    Savemodel.conditionHeadShow = ResultModel.data.conditionName;
                    //    await this.SearchConditionAction(Savemodel);
                    //}
                }
            }


          
        }

        private void btnSaveConditionCancel_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.6.2.4.1", "clk");
            this.gdSaveCondition.Visibility = Visibility.Collapsed;
            this.rctSearchHeadName.Stroke = new SolidColorBrush(Color.FromRgb(0xe5,0xe5,0xe5));
            this.txtErroConditionName.Visibility = Visibility.Collapsed;
        }

        private void IndustrySelectOK_Click(object sender, RoutedEventArgs e)
        {
            this.ucIndustry.selectIndustry.Clear();
            foreach(var item in this.selectIndustryTemp)
            {
                this.ucIndustry.selectIndustry.Add(item);
            }
            //foreach (var item in this.industriesList)
            //{
            //    if (item.isChoose)
            //    {
            //        this.ucIndustry.selectIndustry.Add(item);
            //    }
            //}
            this.gdIndustry.Visibility = Visibility.Collapsed;
            if (this.ucIndustry.selectIndustry.Count > 0)
            {
                this.ucIndustry.txtWaterMark.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.ucIndustry.txtWaterMark.Visibility = Visibility.Visible;
            }
        }

        private void IndustrySelectCancel_Click(object sender, RoutedEventArgs e)
        {
            this.gdIndustry.Visibility = Visibility.Collapsed;
            if (this.ucIndustry.selectIndustry.Count > 0)
            {
                this.ucIndustry.txtWaterMark.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.ucIndustry.txtWaterMark.Visibility = Visibility.Visible;
            }
        }

        private void chkIndustry_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if ((bool)chk.IsChecked)
            {
                if (this.selectIndustryTemp.Count == 3)
                {
                    foreach (var item in industriesList)
                    {
                        if (item.name == this.selectIndustryTemp[0].name)
                        {
                            item.isChoose = false;
                            this.itemsIndustries.ItemsSource = null;
                            this.itemsIndustries.ItemsSource = this.industriesList;
                            break;
                        }
                    }
                    foreach (var item in industriesList)
                    {
                        if (item.name == (chk.Content as TextBlock).Text)
                        {
                            this.selectIndustryTemp.RemoveAt(0);
                            this.selectIndustryTemp.Add(item);
                        }
                    }

                }
                else
                {
                    this.selectIndustryTemp.Add(chk.DataContext as HttpIndustresItem);
                }
            }
            else
            {
                foreach (var item in this.selectIndustryTemp)
                {
                    if (item.name == (chk.Content as TextBlock).Text)
                    {
                        this.selectIndustryTemp.Remove(item);
                        break;
                    }
                }
            }
        }

        private async void txtKeyWordsTextChanged_Click(object sender, TextChangedEventArgs e)
        {
            if (this.rbResumCondition.IsChecked == true)
                return;
           TextBox txt = sender as TextBox;
            await TaskEx.Delay(200);
            if (!txt.Focusable)
                return;
           if (string.IsNullOrWhiteSpace(txt.Text))
            {
                this.popupAuto.IsOpen = false;
                return;
            }
               
            else
            {
                string std = DAL.JsonHelper.JsonParamsToString(new string[] { "word" }, new string[] { txt.Text });
                string resultStd = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrGetKeyWordsAutoAssociate, MagicGlobal.UserInfo.Version, std));
                ViewModel.BaseHttpModel<ViewModel.HttpKeyWordSuggestModel> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<ViewModel.HttpKeyWordSuggestModel>>(resultStd);
                if (model == null)
                {
                    this.popupAuto.IsOpen = false;
                    return;

                }
                else
                {
                    if(model.code == 200)
                    {
                        this.autoAssociateList.Clear();
                       foreach(var item in model.data.suggestList)
                        {
                            this.autoAssociateList.Add(item);
                        }
                       if(this.autoAssociateList.Count > 0)
                        {
                            this.popupAutoList.ItemsSource = this.autoAssociateList;
                            this.popupAuto.Tag = "words";
                            this.popupAuto.PlacementTarget = this.gdKeyWords;
                            //this.popupAuto.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                            this.popupAuto.IsOpen = true;
                        }
                        else { this.popupAuto.IsOpen = false; }
                            
                        return;
                    }
                }
            }
            this.popupAuto.IsOpen = false;
        }

        private void tb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
          
            this.popupAuto.IsOpen = false;
            TextBlock tb = sender as TextBlock;
            if (tb == null)
                return;
            switch (this.popupAuto.Tag.ToString())
            {
                case "words":
                    if (this.txtKeyWords == null)
                        return;
                    this.txtKeyWords.Text = tb.DataContext as string;
                    this.txtKeyWords.Focusable = false;
                    break;
                case "companyName":
                    if (this.txtCompanyName == null)
                        return;
                    this.txtCompanyName.Text = tb.DataContext as string;
                    this.txtCompanyName.Focusable = false;
                    break;
                case "school":
                    if (this.txtSchool == null)
                        return;
                    this.txtSchool.Text = tb.DataContext as string;
                    this.txtSchool.Focusable = false;
                    break;
                case "expectCity":
                    string cityName = tb.DataContext as string;
                    string code = Common.CityCodeHelper.GetCodeFromName(cityName);
                    if (this.txtExpectCity == null)
                        return;
                    this.txtExpectCity.Clear();
                    conditionModel.ExpectCity.Add(new HttpCityCodes() { code = code, name = cityName });
                    this.txtExpectCity.Hint = string.Empty;
                    break;
                case "locationCity":
                    string cityName2 = tb.DataContext as string;
                    string code2 = Common.CityCodeHelper.GetCodeFromName(cityName2);
                    if (this.txtLocationCity == null)
                        return;
                    this.txtLocationCity.Clear();
                    conditionModel.LocationCity.Add(new HttpCityCodes() { code = code2, name = cityName2 });
                    this.txtLocationCity.Hint = string.Empty;
                    break;
            }
           
        }

        private void rbSeachGondition_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.6.1.3.1", "clk");
            Common.TrackHelper2.TrackOperation("5.6.1.4.1", "pv");
            this.GdNoJob.Visibility = Visibility.Collapsed;
            //InitialUCSearchCondition();
        }

        private async void rbResumCondition_Click(object sender, RoutedEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.6.3.1.1", "clk");
            Common.TrackHelper2.TrackOperation("5.6.3.2.1", "pv");
            curPage = 1;
            await this.UpdateJobPublishList();
        }
        /// <summary>
        /// 职位搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async  void btnJobSearch_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.6.3.3.1", "clk");
            JobManageModel jobModel = (sender as Button).DataContext as JobManageModel;
            //SearchConditionSaveModel model = this.GetJobPublishCondition(jobModel);
            //model.conditionHeadShow = jobModel.jobName;
            //if (this.SearchConditionAction != null)
            //{
            //    await this.SearchConditionAction(model);
            //}
            await InitialUCSearchCondition();
            SearchConditioanModel ConditionViewModel = new SearchConditioanModel();
            ViewModel.HttpSearchResumModel model = new HttpSearchResumModel();
            if (string.IsNullOrEmpty(jobModel.thirdJobFunc))
            {
                model.thirdJobCode = jobModel.secondJobFunc;
                model.words = Common.JobCharactHelper.GetName(jobModel.secondJobFunc);
               
            }
            else
            {
                model.thirdJobCode = jobModel.thirdJobFunc;
                model.words = Common.JobCharactHelper.GetName(jobModel.thirdJobFunc);
            }
            conditionModel.KeyWords = model.words;
            this.ucJobAssociation.SelectedVals.Add(new ValCommon1() { name = Common.JobCharactHelper.GetName(model.thirdJobCode), code = model.thirdJobCode });
            this.ucJobAssociation.txtWaterMark.Visibility = Visibility.Collapsed;

            model.cityCode = GetHotCityCode(jobModel.jobCity);
            this.conditionModel.ExpectCity.Add(new HttpCityCodes() { code = model.cityCode, name = Common.CityCodeHelper.GetCityName(model.cityCode) });
            this.txtExpectCity.Hint = "";


            model.salary = model.RangeConvert(jobModel.jobMinSalary.ToString(), jobModel.jobMaxSalary.ToString());

            SetSalaryReturnUI(jobModel.jobMinSalary, jobModel.jobMaxSalary);

            switch (jobModel.minExp)
            {
                case "在读学生":
                    model.workExp = null;
                    model.workExpType = 1;
                    this.cmbWorkExpLow.SelectedIndex = 1;
                    break;
                case "应届毕业生":
                    model.workExp = null;
                    model.workExpType = 2;
                    this.cmbWorkExpLow.SelectedIndex = 2;
                    break;
                case "1年以下":
                    model.workExp = "$lte:1";
                    this.cmbWorkExpLow.SelectedIndex = 3;
                    this.cmbWorkExpHigh.SelectedIndex = 0;
                    break;
                case "1-3年":
                    model.workExp = "$gte:1,$lte:3";
                    this.cmbWorkExpLow.SelectedIndex = 3;
                    this.cmbWorkExpHigh.SelectedIndex = 3;
                    break;
                case "3-5年":
                    model.workExp = "$gte:3,$lte:5";
                    this.cmbWorkExpLow.SelectedIndex = 5;
                    this.cmbWorkExpHigh.SelectedIndex = 2;
                    break;
                case "5-10年":
                    model.workExp = "$gte:5,$lte:10";
                    this.cmbWorkExpLow.SelectedIndex = 7;
                    this.cmbWorkExpHigh.SelectedIndex = 5;
                    break;
                case "10年以上":
                    model.workExp = "$gte:10";
                    this.cmbWorkExpLow.SelectedIndex = 12;
                    this.cmbWorkExpHigh.SelectedIndex = 1;
                    break;
                case "不限":
                    model.workExp = null;
                    model.workExpType = 0;
                    break;
            }




            model.education = model.RangeConvert(MagicCube.Common.MinDegreeHelper.GetCode(jobModel.minDegree), null);
            this.cmbEducaitonLow.SelectedIndex = Convert.ToInt32(MagicCube.Common.MinDegreeHelper.GetCode(jobModel.minDegree));

            model.page = 1;
            model.size = DAL.ConfUtil.PageContent;
            model.sort = 0;
            model.userID = MagicGlobal.UserInfo.Id;
            model.appKey = "xiaopin";
            model.properties = ViewModel.ModelTools.SetHttpPropertys<ViewModel.HttpResumeItemModel>();
            model.desc = "fullName";
            Messaging.MSJobSearchConditionModel msModel = new Messaging.MSJobSearchConditionModel() { ConditionHeadShow = jobModel.jobName, model = model };
            Messaging.Messenger.Default.Send<Messaging.MSJobSearchConditionModel>(msModel);

        }

        private void SetSalaryReturnUI(int min, int max)
        {
            int[] dic = new int[] { 1, 2, 3, 4, 5, 6, 8, 10, 15, 20, 25, 30, 40, 50, 70, 100 };
            int minIndex = 0;
            int maxIndex = 0;
            for(int i = 0; i < dic.Length; i ++)
            {
                if(min <dic[i])
                {
                    minIndex = i;
                    break;
                }
            }
            for (int i = 0; i < dic.Length; i++)
            {
                if (max <= dic[i])
                {
                    maxIndex = i;
                    break;
                }
            }

            this.cmbSalaryLow.SelectedIndex = minIndex;
            this.cmbSalaryHigh.SelectedIndex = maxIndex - minIndex + 1;

           

        }

        private SearchConditionSaveModel GetJobPublishCondition(JobManageModel jobModel)
        {
            SearchConditionSaveModel model = new SearchConditionSaveModel();
            
            model.companyName = null;
            model.onlyLastCompany = false;
            model.jobType = new List<CodeModel>();
            if(string.IsNullOrEmpty(jobModel.thirdJobFunc))
            {
                if(!string.IsNullOrEmpty(jobModel.secondJobFunc))
                {
                    string temp = Common.JobCharactHelper.GetName(jobModel.secondJobFunc);
                    model.jobType.Add(new CodeModel() { name = temp, code = jobModel.secondJobFunc });
                    model.words = model.jobType[0].name;
                }
            }
            else
            {
                string temp = Common.JobCharactHelper.GetName(jobModel.thirdJobFunc);
                model.jobType.Add(new CodeModel() { name = temp, code = jobModel.thirdJobFunc });
                model.words = model.jobType[0].name;
            }
           
            model.workExp = new WorkExp();
            switch (jobModel.minExp)
            {
                case "在读学生":
                    model.workExp.minWorkExp = new CodeModel() { name = "在读学生", code = "-1" };
                    model.workExp.maxWorkExp = null;
                    break;
                case "应届毕业生":
                    model.workExp.minWorkExp = new CodeModel() { name = "应届毕业身", code = "-2" };
                    model.workExp.maxWorkExp = null;
                    break;
                case "1年以下":
                    model.workExp.minWorkExp = new CodeModel() { name = "1年", code = "1" };
                    model.workExp.maxWorkExp = new CodeModel() { name = "及以下", code = "-4" };
                    break;
                case "1-3年":
                    model.workExp.minWorkExp = new CodeModel() { name = "1年", code = "1" };
                    model.workExp.maxWorkExp = new CodeModel() { name = "3年", code = "3" };
                    break;
                case "3-5年":
                    model.workExp.minWorkExp = new CodeModel() { name = "3年", code = "3" };
                    model.workExp.maxWorkExp = new CodeModel() { name = "5年", code = "5" };
                    break;
                case "5-10年":
                    model.workExp.minWorkExp = new CodeModel() { name = "5年", code = "5" };
                    model.workExp.maxWorkExp = new CodeModel() { name = "10年", code = "10" };
                    break;
                case "10年以上":
                    model.workExp.minWorkExp = new CodeModel() { name = "10年", code = "10" };
                    model.workExp.maxWorkExp = new CodeModel() { name = "及以上", code = "11" };
                    break;
                case "不限":
                    model.workExp.minWorkExp = new CodeModel() { name = "不限", code = "0" };
                    model.workExp.maxWorkExp = new CodeModel() { name = "不限", code = "0" };
                    break;
            }


            model.salary = new Salary();
            foreach (var item in salaryLowList)
            {
                
                if(jobModel.jobMaxSalary < Convert.ToInt32(item.code))
                {
                    model.salary.maxSalaryCode = item.code;
                    break;
                }
            }


            for(int i = 0; i < salaryLowList.Count; i++)
            {
                if(jobModel.jobMinSalary < Convert.ToInt32(salaryLowList[i].code))
                {
                    if(i == 0)
                    {
                        model.salary.minSalaryCode =salaryLowList[0].code;
                    }
                    else
                    model.salary.minSalaryCode = salaryLowList[i - 1].code;
                    break;
                }
            }

            model.degree = new Degree();
            model.degree.minDegreeCode = MagicCube.Common.MinDegreeHelper.GetCode(jobModel.minDegree);
            model.degree.maxDegreeCode = "0";
            model.expCity = new List<CodeModel>();
            string tempcCity = GetHotCityCode(jobModel.jobCity);
            model.expCity.Add(new CodeModel() { code = tempcCity, name = Common.CityCodeHelper.GetSingleName(tempcCity) });


            model.industry = new List<CodeModel>();
            model.address = new List<CodeModel>();






            return model;

        }

        private void txtSearchHeadName_GotFocus(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.6.2.2.1", "clk");
            this.rctSearchHeadName.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
            this.txtErroConditionName.Visibility = Visibility.Collapsed;
        }

        private  void txtCompanyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //TextBox txt = sender as TextBox;
            //await TaskEx.Delay(200);
            //if (!txt.Focusable)
            //    return;
            //if (string.IsNullOrWhiteSpace(txt.Text))
            //{
            //    this.popupAuto.IsOpen = false;
            //    return;
            //}
               
            //else
            //{
            //    string std1 = ViewModel.ModelTools.SetHttpPropertys<ViewModel.HttpCompanyNameModel>();
            //    string std = DAL.JsonHelper.JsonParamsToString(new string[] { "companyName","properties" }, new string[] { txt.Text, std1 });
            //   string resultStd = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrGetCompanyAutoAssociate, MagicGlobal.UserInfo.Version, std));
            //    ViewModel.BaseHttpModel<List<ViewModel.HttpCompanyNameModel>> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<List<ViewModel.HttpCompanyNameModel>>>(resultStd);
            //    if (model == null)
            //    {
            //        this.popupAuto.IsOpen = false;
            //        return;

            //    }
            //    else
            //    {
            //        if (model.code == 200)
            //        {
            //            this.autoAssociateList.Clear();
            //            foreach (var item in model.data)
            //            {
            //                this.autoAssociateList.Add(item.companyName);
            //            }
            //            if (this.autoAssociateList.Count > 0)
            //            {
            //                this.popupAutoList.ItemsSource = this.autoAssociateList;
            //                this.popupAuto.Tag = "companyName";
            //                this.popupAuto.PlacementTarget = this.gdCompnayName;
            //                //this.popupAuto.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            //                this.popupAuto.IsOpen = true;
            //            }
            //            return;
            //        }
            //    }
            //}
            //this.popupAuto.IsOpen = false;
        }

        private async void txtSchool_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            await TaskEx.Delay(200);
            if (!txt.Focusable)
                return;
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                this.popupAuto.IsOpen = false;
                return;
            }

            else
            {
                string std = DAL.JsonHelper.JsonParamsToString(new string[] { "word","type" }, new string[] { txt.Text,"1" });
                string resultStd = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrGetKeyWordsAutoAssociate, MagicGlobal.UserInfo.Version, std));
                ViewModel.BaseHttpModel<ViewModel.HttpKeyWordSuggestModel> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<ViewModel.HttpKeyWordSuggestModel>>(resultStd);
                if (model == null)
                {
                    this.popupAuto.IsOpen = false;
                    return;

                }
                else
                {
                    if (model.code == 200)
                    {
                        this.autoAssociateList.Clear();
                        foreach (var item in model.data.suggestList)
                        {
                            this.autoAssociateList.Add(item);
                        }
                        if(this.autoAssociateList.Count > 0)
                        {
                            this.popupAutoList.ItemsSource = this.autoAssociateList;
                            this.popupAuto.Tag = "school";
                            this.popupAuto.PlacementTarget = this.gdSchool;
                            //this.popupAuto.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                            this.popupAuto.IsOpen = true;
                        }
                        else
                        {
                            this.popupAuto.IsOpen = false;
                        }
                        

                        return;
                    }
                }
            }
            this.popupAuto.IsOpen = false;
        }

        private void btnJobPublish_Click(object sender, RoutedEventArgs e)
        {
            if (OpenPublishJobAction != null)
                OpenPublishJobAction();
        }

        private void btnGoToConditionSearch_Click(object sender, RoutedEventArgs e)
        {
            this.rbSeachGondition.IsChecked = true;
            this.GdNoJob.Visibility = Visibility.Collapsed;
            this.scollMain.ScrollToTop();
        }

        private void txtKeyWords_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void txtCompanyName_MouseEnter(object sender, MouseEventArgs e)
        {
            this.txtCompanyName.Focusable = true;
        }

        private void txtSchool_MouseEnter(object sender, MouseEventArgs e)
        {
            this.txtSchool.Focusable = true;
        }

        private void btnSaveCondition_Click(object sender, RoutedEventArgs e)
        {
            //新埋点
            Common.TrackHelper2.TrackOperation("5.6.1.34.1", "clk");
            Common.TrackHelper2.TrackOperation("5.6.2.1.1", "pv");
                this.gdSaveCondition.Visibility = Visibility.Visible;
                this.txtSearchHeadName.Text = this.GetSaveConditionString();
        }

        private void Redo_CanExcute(object sender, CanExecuteRoutedEventArgs e)
        {
            string temp = GetSaveConditionString();
            if (string.IsNullOrEmpty(temp))
                e.CanExecute = false;
            else
                e.CanExecute = true;
            e.Handled = true;
        }

        private async void Redo_Excute(object sender, ExecutedRoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.6.1.35.1", "clk");
            await InitialUCSearchCondition();
        }

     

        private void TrackOperation_GotFocus(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            switch(fe.Name)
            {
                case "txtKeyWords":
                    Common.TrackHelper2.TrackOperation("5.6.1.5.1", "clk");
                    this.rectKeyWords.Stroke = new SolidColorBrush(Color.FromRgb(0x00,0xbe,0xff));
                    break;
                case "txtCompanyName":
                    Common.TrackHelper2.TrackOperation("5.6.1.7.1", "clk");
                    this.rectCompanyName.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
                    break;
                case "chkOnlyLastCompany":
                    Common.TrackHelper2.TrackOperation("5.6.1.8.1", "clk");
                    break;
                case "ucJobAssociation":
                    Common.TrackHelper2.TrackOperation("5.6.1.9.1", "clk");
                    break;
                case "ucIndustry":
                    Common.TrackHelper2.TrackOperation("5.6.1.10.1", "clk");
                    break;
                case "txtAgeMin":
                    Common.TrackHelper2.TrackOperation("5.6.1.11.1", "clk");
                    this.rectAgeMin.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
                    break;
                case "txtAgeMax":
                    Common.TrackHelper2.TrackOperation("5.6.1.12.1", "clk");
                    this.rectAgeMax.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff)); 
                    break;
                case "cmbWorkExpLow":
                    
                    break;
                case "cmbWorkExpHigh":
               
                    break;
                case "cmbEducaitonLow":
                  
                    break;
                case "cmbEducationHigh":
                    
                    break;
                case "chk985":
                    Common.TrackHelper2.TrackOperation("5.6.1.17.1", "clk");
                    break;
                case "chk211":
                    Common.TrackHelper2.TrackOperation("5.6.1.18.1", "clk");
                    break;
                case "chkAllStu":
                    Common.TrackHelper2.TrackOperation("5.6.1.19.1", "clk");
                    break;
                case "chkOverSea":
                    Common.TrackHelper2.TrackOperation("5.6.1.20.1", "clk");
                    break;
                case "cmbSalaryLow":
                  
                    break;
                case "cmbSalaryHigh":
                  
                    break;
                case "cmbProvince":
                    Common.TrackHelper2.TrackOperation("5.6.1.23.1", "clk");
                    break;
                case "cmblocationProvince":
                    Common.TrackHelper2.TrackOperation("5.6.1.24.1", "clk");
                    break;
                case "rbNullGender":
                    Common.TrackHelper2.TrackOperation("5.6.1.25.1", "clk");
                    break;
                case "rbGenderMan":
                    Common.TrackHelper2.TrackOperation("5.6.1.26.1", "clk");
                    break;
                case "rbGenderWoman":
                    Common.TrackHelper2.TrackOperation("5.6.1.27.1", "clk");
                    break;
                case "cmbUpdateTime":
                    
                    break;
                case "txtMajor":
                    Common.TrackHelper2.TrackOperation("5.6.1.29.1", "clk");
                    this.rectMajor.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
                    break;
                case "txtSchool":
                    Common.TrackHelper2.TrackOperation("5.6.1.30.1", "clk");
                    this.rectSchool.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
                    break;
                case "cmbLauguage":
               
                    break;
                case "cmbStatus":
                    
                    break;
                case "chkMore":
                    Common.TrackHelper2.TrackOperation("5.6.1.33.1", "clk");
                    break;

            }
        }

        private void txtKeyWords_KeyDown(object sender, KeyEventArgs e)
        {
           if(e.Key == Key.Down)
            {
                this.popupAuto.Focus();
                this.popupAutoList.Focus();
            }
        }
        private void txtKeyWords_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.txtKeyWords.Focusable = true;

        }

        private void txtKeyWords_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.txtKeyWords.Focus();
            txtKeyWords.SelectionStart = txtKeyWords.Text.Length;

        }

        private void popupAutoList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                switch (this.popupAuto.Tag.ToString())
                {
                    case "words":
                        this.txtKeyWords.Focusable = false;
                        this.txtKeyWords.Text = this.popupAutoList.SelectedValue.ToString();
                        this.popupAuto.IsOpen = false;
                        break;
                    case "school":
                        
                        break;
                        
                }
                
            }
        }

        private void txtExpectCity_LostFocus(object sender, RoutedEventArgs e)
        {
            this.txtExpectCity.Clear();
        }

        private async void txtExpectCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            await TaskEx.Delay(200);
            if (!txt.Focusable)
                return;
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                this.popupAuto.IsOpen = false;
                return;
            }

            else
            {
                List<string> temp = new List<string>();
                foreach (var item in this.jsonCityModel.CityCodes.CityCode)
                {
                    if (txt.Text.Contains(item.simpleName))
                        temp.Add(item.simpleName);
                }
                if (temp.Count == 0)
                    return;
                this.popupAutoList.ItemsSource = temp;
                this.popupAuto.Tag = "expectCity";
                this.popupAuto.PlacementTarget = this.bdExpectCity;
                //this.popupAuto.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                this.popupAuto.IsOpen = true;
            }
              
        }

        private void btnCityDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            HttpCityCodes code = btn.DataContext as HttpCityCodes;
            foreach(var item in  this.conditionModel.ExpectCity)
            {
                if(item.code == code.code)
                {
                    this.conditionModel.ExpectCity.Remove(item);
                    break;
                }
            }
            if(this.conditionModel.ExpectCity.Count == 0)
            {
                this.txtExpectCity.Hint = "请输入或选择";
            }
            this.txtExpectCity.Focus();

        }

        private void chkExpectCityOpen_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if(chk.IsChecked == true)
            {
                ucProvinceCity.SetSelectedList(this.conditionModel.ExpectCity);
            }
        }

        private void chkLocationOpen_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked == true)
            {
                ucLocaitonCityPanel.SetSelectedList(this.conditionModel.LocationCity);
            }
        }

        private void txtLocationCity_LostFocus(object sender, RoutedEventArgs e)
        {
            this.txtLocationCity.Clear();
        }

        private async void txtLocationCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            await TaskEx.Delay(200);
            if (!txt.Focusable)
                return;
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                this.popupAuto.IsOpen = false;
                return;
            }

            else
            {
                List<string> temp = new List<string>();
                foreach (var item in this.jsonCityModel.CityCodes.CityCode)
                {
                    if (txt.Text.Contains(item.simpleName))
                        temp.Add(item.simpleName);
                }
                if (temp.Count == 0)
                    return;
                this.popupAutoList.ItemsSource = temp;
                this.popupAuto.Tag = "locationCity";
                this.popupAuto.PlacementTarget = this.bdLocationCity;
                //this.popupAuto.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                this.popupAuto.IsOpen = true;
            }
        }

        private void btnlocationDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            HttpCityCodes code = btn.DataContext as HttpCityCodes;
            foreach (var item in this.conditionModel.LocationCity)
            {
                if (item.code == code.code)
                {
                    this.conditionModel.LocationCity.Remove(item);
                    break;
                }
            }
            if (this.conditionModel.LocationCity.Count == 0)
            {
                this.txtLocationCity.Hint = "请输入或选择";
            }
            this.txtLocationCity.Focus();
        }

        private void txtKeyWords_LostFocus(object sender, RoutedEventArgs e)
        {
            this.rectKeyWords.Stroke = new SolidColorBrush(Color.FromRgb(0xe5,0xe5,0xe5));
        }

        private void txtCompanyName_LostFocus(object sender, RoutedEventArgs e)
        {
            this.rectCompanyName.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
        }

        private void txtMajor_LostFocus(object sender, RoutedEventArgs e)
        {
            this.rectMajor.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
        }

        private void txtSchool_LostFocus(object sender, RoutedEventArgs e)
        {
            this.rectSchool.Stroke = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
        }

        private void bdExpectCity_GotFocus(object sender, RoutedEventArgs e)
        {
            this.bdExpectCity.BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
        }

        private void bdExpectCity_LostFocus(object sender, RoutedEventArgs e)
        {
            this.bdExpectCity.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
        }

        private void bdLocationCity_GotFocus(object sender, RoutedEventArgs e)
        {
            this.bdLocationCity.BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xbe, 0xff));
        }

        private void bdLocationCity_LostFocus(object sender, RoutedEventArgs e)
        {
            this.bdLocationCity.BorderBrush = new SolidColorBrush(Color.FromRgb(0xe5, 0xe5, 0xe5));
        }

        private void cmbWorkExpLow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.6.1.13.1", "clk");
        }

        private void cmbWorkExpHigh_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.6.1.14.1", "clk");
        }

        private void cmbEducaitonLow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.6.1.15.1", "clk");
        }

        private void cmbEducationHigh_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.6.1.16.1", "clk");
        }

        private void cmbSalaryLow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.6.1.21.1", "clk");
        }

        private void cmbSalaryHigh_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.6.1.22.1", "clk");
        }

        private void cmbUpdateTime_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.6.1.28.1", "clk");
        }

        private void cmbLauguage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.6.1.31.1", "clk");
        }

        private void cmbStatus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Common.TrackHelper2.TrackOperation("5.6.1.32.1", "clk");
        }
    }
}
