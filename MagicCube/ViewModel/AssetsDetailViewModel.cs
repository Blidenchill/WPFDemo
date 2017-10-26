using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicCube.Command;
using MagicCube.Messaging;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace MagicCube.ViewModel
{
    public class AssetsDetailViewModel :ViewModelBase
    {
        private string _mCoinTotalCount = "0";
        private string _mCoinLockCount = "0";
        private string _integralTotalCount = "0";
        private string _downloadCount = "0";
        private bool _isBusy = false;
        private bool _isLoadingAll = false;

        public bool IntegralIsChecked = true;
        public bool MCoinIsChecked = false;
        public bool DownloadIsChecked = false;
        public string StartDt = string.Empty;
        public string EndDt = string.Empty;

        public AssetsDetailViewModel()
        {
            InitalCommand = new RelayCommand(initialCallback);
            RdoClickCommand = new RelayCommand<RadioButton>(RdoClickCallback);
            SelectClickCommand = new RelayCommand(SelectClickCallback);

            Messaging.Messenger.Default.Register<object>(this, LoadListCallback);
            Messaging.Messenger.Default.Register<HttpModel.HttpMBTotalRoot>(this,MCoinCountCallback);
            Messaging.Messenger.Default.Register<Messaging.MSBodyBase<string>>(this, AssetCallback);

            this.InitialServiceActionList();
            this.InitialTriggerCodeList();
        }


        #region "属性"

        public string MCoinTotalCount
        {
            get { return _mCoinTotalCount; }
            set
            {
                _mCoinTotalCount = value;
                RaisePropertyChanged("MCoinTotalCount");
            }
        }
         
        public string MCoinLockCount
        {
            get { return _mCoinLockCount; }
            set
            {
                _mCoinLockCount = value;
                RaisePropertyChanged("MCoinLockCount");
            }
        }   

        public string IntegralTotalCount
        {
            get { return _integralTotalCount; }
            set
            {
                _integralTotalCount = value;
                RaisePropertyChanged("IntegralTotalCount");
            }
        }

        public string DownloadCount
        {
            get { return _downloadCount; }
            set
            {
                _downloadCount = value;
                RaisePropertyChanged("DownloadCount");
            }
        }
        private ObservableCollection<MCoinListViewModel> _MCoinList = new ObservableCollection<MCoinListViewModel>();
        public ObservableCollection<MCoinListViewModel> MCoinList
        {
            get { return _MCoinList; }
            set
            {
                _MCoinList = value;
                RaisePropertyChanged("MCoinList");
            }
        }

        private ObservableCollection<MCoinListViewModel> _intergralList = new ObservableCollection<MCoinListViewModel>();
        public ObservableCollection<MCoinListViewModel> IntergralList
        {
            get { return _intergralList; }
            set
            {
                _intergralList = value;
                RaisePropertyChanged("IntergralList");
            }
        }
        private ObservableCollection<MCoinListViewModel> _downloadList = new ObservableCollection<MCoinListViewModel>();

        public ObservableCollection<MCoinListViewModel> DownloadList
        {
            get { return _downloadList; }
            set
            {
                _downloadList = value;
                RaisePropertyChanged("DownloadList");
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public bool IsLoadingAll
        {
            get { return _isLoadingAll; }
            set
            {
                _isLoadingAll = value;
                RaisePropertyChanged("IsLoadingAll");
            }
        }
        private bool _isListNull = false;
        public bool IsListNull
        {
            get { return _isListNull; }
            set
            {
                _isListNull = value;
                RaisePropertyChanged("IsListNull");
            }
        }


        private ObservableCollection<TypeMode> _triggerCodeModeList = new ObservableCollection<TypeMode>();

        public ObservableCollection<TypeMode> TriggerCodeModeList
        {
            get { return _triggerCodeModeList; }
            set
            {
                _triggerCodeModeList = value;
                RaisePropertyChanged("TriggerCodeModeList");
            }
        }

        private ObservableCollection<TypeMode> _serviceActionModeList = new ObservableCollection<TypeMode>();
        public ObservableCollection<TypeMode> ServiveActionModeList
        {
            get { return _serviceActionModeList; }
            set
            {
                _serviceActionModeList = value;
                RaisePropertyChanged("ServiveActionModeList");
            }
        }

        private TypeMode _typeModeSel = new TypeMode();
        public TypeMode TypeModeSel
        {
            get { return _typeModeSel; }
            set
            {
                _typeModeSel = value;
                RaisePropertyChanged("TypeModeSel");
            }
        }


        #endregion

        #region "命令"
        public RelayCommand InitalCommand { get; private set; }

        public RelayCommand<RadioButton> RdoClickCommand { get; private set; } 

        public RelayCommand SelectClickCommand { get; private set; }
        #endregion

        #region "回调函数"
        private async void initialCallback()
        {
            IntegralIsChecked = true;
            await GetInitalIntegralList();
            if(this.IntergralList.Count == 0)
            {
                this.IsListNull = true;
            }

            await GetInitalMCoinList();
            await GetInitalDownloadList();
        }

        private async void LoadListCallback(object sender)
        {
            if (this.IsLoadingAll)
                return;
            IsBusy = true;
            int count = 0;
            if (MCoinIsChecked)
                count = await this.GetMCoinList(15, this.MCoinList.Count);
            else if (IntegralIsChecked)
                count = await this.GetIntegraList(15, this.IntergralList.Count);
            else if (DownloadIsChecked)
                count = await this.GetDownloadList(15, this.DownloadList.Count);
            IsBusy = false;
            if (count == 0)
            {
                this.IsLoadingAll = true;
            }
            else
            {
                this.IsLoadingAll = false;
            }
        }

        private void MCoinCountCallback(HttpModel.HttpMBTotalRoot sender)
        {
            MCoinTotalCount = sender.data.totalAmount.ToString();
            MCoinLockCount = sender.data.lockedAmount.ToString();
        }

        public void AssetCallback(Messaging.MSBodyBase<string> sender)
        {
            if(sender.type == "Intergral")
            {
                IntegralTotalCount = sender.body;
            }
            if(sender.type == "download")
            {
                DownloadCount = sender.body;
            }
        }

        public async void RdoClickCallback(RadioButton sender)
        {
           
            if (sender == null)
                return;
            this.IsListNull = false;
            this.IsLoadingAll = false;

            if (sender.Name == "rdoIntergral")
            {
                this.IntegralIsChecked = true;
                this.MCoinIsChecked = false;
                this.DownloadIsChecked = false;
                await this.GetInitalIntegralList();
                if (this.IntergralList.Count == 0)
                {
                    this.IsListNull = true;
                }
            }
            else if(sender.Name == "rdoMCoin")
            {
                this.IntegralIsChecked = false;
                this.MCoinIsChecked = true;
                this.DownloadIsChecked = false;
                await this.GetInitalMCoinList();
                if (this.MCoinList.Count == 0)
                {
                    this.IsListNull = true;
                }
            }
            else if(sender.Name == "rdoDownload")
            {
                this.IntegralIsChecked = false;
                this.MCoinIsChecked = false;
                this.DownloadIsChecked = true;
                await this.GetInitalDownloadList();
                if (this.DownloadList.Count == 0)
                {
                    this.IsListNull = true;
                }
            }
            
        }

        public async void SelectClickCallback()
        {
            this.IsListNull = false;
            this.IsBusy = false;
            this.IsLoadingAll = false;
            if (this.IntegralIsChecked)
            {
                await GetInitalIntegralList();
                if (this.IntergralList.Count == 0)
                {
                    this.IsListNull = true;
                }
            }
                
            else if (this.MCoinIsChecked)
            {
                await GetInitalMCoinList();
                if (this.MCoinList.Count == 0)
                {
                    this.IsListNull = true;
                }
            }
                
            else if (this.DownloadIsChecked)
            {
                await GetInitalDownloadList();
                if (this.DownloadList.Count == 0)
                {
                    this.IsListNull = true;
                }
            }
                
        }
        #endregion

        #region "对内函数"

        private async Task GetInitalMCoinList()
        {
            MCoinList.Clear();
            string std = DAL.JsonHelper.JsonParamsToString(
                new string[] { "startTime", "endTime", "triggerCode", "pageSize", "startRow", "userID" },
                new string[] { StartDt, EndDt, this.TypeModeSel.TypeStr, "15", "0", MagicGlobal.UserInfo.Id.ToString() });

            string url = string.Format(DAL.ConfUtil.AddrGetMCoinList, MagicGlobal.UserInfo.Version, std);
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(url);
            BaseHttpModel<HttpMCoinListModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpMCoinListModel>>(resultStr);
            if (model == null)
            {
                return;
            }
            else
            {
                if (model.code == 200)
                {
                    foreach (var item in model.data.result)
                    {
                        MCoinListViewModel mCoinModel = new MCoinListViewModel();
                        mCoinModel.TriggerName = item.triggerName;
                        mCoinModel.TriggerParams = item.triggerParams;
                        mCoinModel.CreateTime = item.createTime;
                        mCoinModel.UrlLogo = GetLogoUrlByOperationName(item.triggerName);
                        mCoinModel.LogoHeader = item.triggerName;
                        mCoinModel.ServiceQuantity = item.serviceQuantity.ToString();
                        MCoinList.Add(mCoinModel);
                    }
                }
            }
        }

        private async Task GetInitalIntegralList()
        {
            IntergralList.Clear();
            string std = DAL.JsonHelper.JsonParamsToString(
                new string[] { "beginTime", "endTime", "serviceAction", "pageSize", "startRow", "userID" },
                new string[] { StartDt, EndDt, this.TypeModeSel.TypeStr, "15", "0", MagicGlobal.UserInfo.Id.ToString() });

            string url = string.Format(DAL.ConfUtil.AddrGetIntegralList, MagicGlobal.UserInfo.Version, std);
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(url);
            BaseHttpModel<HttpMCoinListModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpMCoinListModel>>(resultStr);
            if (model == null)
            {
                return;
            }
            else
            {
                if (model.code == 200)
                {
                    foreach (var item in model.data.result)
                    {
                        MCoinListViewModel mCoinModel = new MCoinListViewModel();
                        mCoinModel.TriggerName = item.triggerName;
                        mCoinModel.TriggerParams = item.triggerParams;
                        mCoinModel.CreateTime = item.createTime;
                        mCoinModel.UrlLogo = GetLogoUrlByOperationName(item.triggerName);
                        mCoinModel.LogoHeader = item.triggerName;
                        if (item.serviceAction == -1)
                        {
                            mCoinModel.LogoHeader = "支出";
                            mCoinModel.UrlLogo = "/MagicCube;component/Resources/ImageSingle/Micon3.png";
                        }
                        else if (item.serviceAction == 1)
                        {
                            mCoinModel.LogoHeader = "获得";
                            mCoinModel.UrlLogo = "/MagicCube;component/Resources/ImageSingle/Micon4.png";
                        }


                        mCoinModel.ServiceQuantity = item.serviceQuantity.ToString();
                        IntergralList.Add(mCoinModel);
                    }
                }
            }
        }

        private async Task GetInitalDownloadList()
        {
            DownloadList.Clear();
            
            string std = DAL.JsonHelper.JsonParamsToString(
                new string[] { "beginTime", "endTime", "serviceAction", "pageSize", "startRow", "serviceSign", "userID" },
                new string[] { StartDt, EndDt, this.TypeModeSel.TypeStr, "15", "0", "S_ViewContact", MagicGlobal.UserInfo.Id.ToString() });

            string url = string.Format(DAL.ConfUtil.AddrGetDownloadList, MagicGlobal.UserInfo.Version, std);
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(url);
            BaseHttpModel<HttpMCoinListModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpMCoinListModel>>(resultStr);
            if (model == null)
            {
                return;
            }
            else
            {
                if (model.code == 200)
                {
                    foreach (var item in model.data.result)
                    {
                        MCoinListViewModel mCoinModel = new MCoinListViewModel();
                        mCoinModel.TriggerName = item.triggerName;
                        mCoinModel.TriggerParams = item.triggerParams;
                        mCoinModel.CreateTime = item.createTime;
                        mCoinModel.UrlLogo = GetLogoUrlByOperationName(item.triggerName);
                        mCoinModel.LogoHeader = item.triggerName;
                        if (item.serviceAction == -1)
                        {
                            mCoinModel.LogoHeader = "支出";
                            mCoinModel.UrlLogo = "/MagicCube;component/Resources/ImageSingle/Micon3.png";
                        }
                        else if (item.serviceAction == 1)
                        {
                            mCoinModel.LogoHeader = "购买";
                            mCoinModel.UrlLogo = "/MagicCube;component/Resources/ImageSingle/Micon4.png";
                        }


                        mCoinModel.ServiceQuantity = item.serviceQuantity.ToString();
                        DownloadList.Add(mCoinModel);
                    }
                }
            }
        }



        private async Task<int> GetMCoinList(int count, int startRow)
        {

            string std = DAL.JsonHelper.JsonParamsToString(
         new string[] { "startTime", "endTime", "triggerCode", "pageSize", "startRow", "userID" },
         new string[] { StartDt, EndDt, this.TypeModeSel.TypeStr, count.ToString(), startRow.ToString(), MagicGlobal.UserInfo.Id.ToString() });

            string url = string.Format(DAL.ConfUtil.AddrGetMCoinList, MagicGlobal.UserInfo.Version, std);
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(url);
            BaseHttpModel<HttpMCoinListModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpMCoinListModel>>(resultStr);
            if (model == null)
            {
                return 0;
            }
            else
            {
                if (model.code == 200)
                {
                    if (model.data.result == null)
                        return 0;
                    foreach (var item in model.data.result)
                    {
                        MCoinListViewModel mCoinModel = new MCoinListViewModel();
                        mCoinModel.TriggerName = item.triggerName;
                        mCoinModel.TriggerParams = item.triggerParams;
                        mCoinModel.CreateTime = item.createTime;
                        mCoinModel.UrlLogo = GetLogoUrlByOperationName(item.triggerName);
                        mCoinModel.LogoHeader = item.triggerName;
                        mCoinModel.ServiceQuantity = item.serviceQuantity.ToString();
                        MCoinList.Add(mCoinModel);
                    }
                    return model.data.result.Count;
                }
            }
            return 0;
        }

        private async Task<int> GetIntegraList(int count, int startRow)
        {
            string std = DAL.JsonHelper.JsonParamsToString(
               new string[] { "beginTime", "endTime", "serviceAction", "pageSize", "startRow", "userID" },
               new string[] { StartDt, EndDt, this.TypeModeSel.TypeStr, count.ToString(), startRow.ToString(), MagicGlobal.UserInfo.Id.ToString() });

            string url = string.Format(DAL.ConfUtil.AddrGetIntegralList, MagicGlobal.UserInfo.Version, std);
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(url);
            BaseHttpModel<HttpMCoinListModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpMCoinListModel>>(resultStr);
            if (model == null)
            {
                return 0;
            }
            else
            {
                if (model.code == 200)
                {
                    if (model.data.result == null)
                        return 0;
                    foreach (var item in model.data.result)
                    {
                        MCoinListViewModel mCoinModel = new MCoinListViewModel();
                        mCoinModel.TriggerName = item.triggerName;
                        mCoinModel.TriggerParams = item.triggerParams;
                        mCoinModel.CreateTime = item.createTime;
                        mCoinModel.UrlLogo = GetLogoUrlByOperationName(item.triggerName);
                        mCoinModel.LogoHeader = item.triggerName;
                        if (item.serviceAction == -1)
                        {
                            mCoinModel.LogoHeader = "支出";
                            mCoinModel.UrlLogo = "/MagicCube;component/Resources/ImageSingle/Micon3.png";
                        }
                        else if (item.serviceAction == 1)
                        {
                            mCoinModel.LogoHeader = "获得";
                            mCoinModel.UrlLogo = "/MagicCube;component/Resources/ImageSingle/Micon4.png";
                        }


                        mCoinModel.ServiceQuantity = item.serviceQuantity.ToString();
                        IntergralList.Add(mCoinModel);
                    }
                    return model.data.result.Count;
                }
            }
            return 0;
        }

        private async Task<int> GetDownloadList(int count, int startRow)
        {
            string std = DAL.JsonHelper.JsonParamsToString(
                new string[] { "beginTime", "endTime", "serviceAction", "pageSize", "startRow", "serviceSign", "userID" },
                new string[] { StartDt, EndDt, this.TypeModeSel.TypeStr, count.ToString(), startRow.ToString(), "S_ViewContact", MagicGlobal.UserInfo.Id.ToString() });

            string url = string.Format(DAL.ConfUtil.AddrGetDownloadList, MagicGlobal.UserInfo.Version, std);
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(url);
            BaseHttpModel<HttpMCoinListModel> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpMCoinListModel>>(resultStr);
            if (model == null)
            {
                return 0;
            }
            else
            {
                if (model.code == 200)
                {
                    if (model.data == null)
                        return 0;
                    foreach (var item in model.data.result)
                    {
                        MCoinListViewModel mCoinModel = new MCoinListViewModel();
                        mCoinModel.TriggerName = item.triggerName;
                        mCoinModel.TriggerParams = item.triggerParams;
                        mCoinModel.CreateTime = item.createTime;
                        mCoinModel.UrlLogo = GetLogoUrlByOperationName(item.triggerName);
                        mCoinModel.LogoHeader = item.triggerName;
                        if (item.serviceAction == -1)
                        {
                            mCoinModel.LogoHeader = "支出";
                            mCoinModel.UrlLogo = "/MagicCube;component/Resources/ImageSingle/Micon3.png";
                        }
                        else if (item.serviceAction == 1)
                        {
                            mCoinModel.LogoHeader = "购买";
                            mCoinModel.UrlLogo = "/MagicCube;component/Resources/ImageSingle/Micon4.png";
                        }


                        mCoinModel.ServiceQuantity = item.serviceQuantity.ToString();
                        DownloadList.Add(mCoinModel);
                    }
                    return model.data.result.Count;
                }
            }

            return 0;
        }


        private string GetLogoUrlByOperationName(string triggerCode)
        {
            string temp = string.Empty;
            switch (triggerCode)
            {
                //冻结
                case "冻结":
                    temp = "/MagicCube;component/Resources/ImageSingle/Micon1.png";
                    break;
                //解冻
                case "解冻":
                    temp = "/MagicCube;component/Resources/ImageSingle/Micon2.png";
                    break;
                //支出
                case "支出":
                    temp = "/MagicCube;component/Resources/ImageSingle/Micon3.png";
                    break;
                //购买
                case "充值":
                    temp = "/MagicCube;component/Resources/ImageSingle/Micon4.png";
                    break;


            }
            return temp;
        }



        private void InitialTriggerCodeList()
        {
            this._triggerCodeModeList.Clear();
            this._triggerCodeModeList.Add(new TypeMode() { TypeStr = string.Empty, BodyStr = "不限" });
            this._triggerCodeModeList.Add(new TypeMode() { TypeStr = "charge", BodyStr = "充值" });
            this._triggerCodeModeList.Add(new TypeMode() { TypeStr = "release_interview_onsite", BodyStr = "解冻" });
            this._triggerCodeModeList.Add(new TypeMode() { TypeStr = "lock_interview_onsite", BodyStr = "冻结" });
            this._triggerCodeModeList.Add(new TypeMode() { TypeStr = "consume_interview_onsite", BodyStr = "现场面试消费" });
            this._triggerCodeModeList.Add(new TypeMode() { TypeStr = "poundage_interview_onsite", BodyStr = "现场面试手续费" });
        }

        private void InitialServiceActionList()
        {
            this._serviceActionModeList.Clear();
            this._serviceActionModeList.Add(new TypeMode() { TypeStr = string.Empty, BodyStr = "不限" });
            this._serviceActionModeList.Add(new TypeMode() { TypeStr = "1", BodyStr = "获得" });
            this._serviceActionModeList.Add(new TypeMode() { TypeStr = "-1", BodyStr = "支出" });
        }

        

        #endregion

    }

    public class TypeMode
    {
        public string TypeStr { get; set; }
        public string BodyStr { get; set; }
    }

}
