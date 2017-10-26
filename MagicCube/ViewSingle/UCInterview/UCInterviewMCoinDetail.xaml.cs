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
using System.Collections.ObjectModel;
using MagicCube.Model;
using MagicCube.HttpModel;
using MagicCube.Common;
using System.Threading.Tasks;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCInterviewMCoinDetail.xaml 的交互逻辑
    /// </summary>
    public partial class UCInterviewMCoinDetail : UserControl
    {
        public ObservableCollection<MCoinDetailListModel> mCoinDetailList = new ObservableCollection<MCoinDetailListModel>();


        public string TotalAmount
        {
            get { return (string)GetValue(TotalAmountProperty); }
            set { SetValue(TotalAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalAmountProperty =
            DependencyProperty.Register("TotalAmount", typeof(string), typeof(UCInterviewMCoinDetail), new PropertyMetadata(null));



        public string LockAmount
        {
            get { return (string)GetValue(LockAmountProperty); }
            set { SetValue(LockAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LockAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LockAmountProperty =
            DependencyProperty.Register("LockAmount", typeof(string), typeof(UCInterviewMCoinDetail), new PropertyMetadata(null));



        public string EnableAmount
        {
            get { return (string)GetValue(EnableAmountProperty); }
            set { SetValue(EnableAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnableAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableAmountProperty =
            DependencyProperty.Register("EnableAmount", typeof(string), typeof(UCInterviewMCoinDetail), new PropertyMetadata(null));






        public UCInterviewMCoinDetail()
        {
            InitializeComponent();
            
            this.lstMCoinDetail.ItemsSource = mCoinDetailList;
        }

        public async Task InitailCoinDetail()
        {
            
            this.busyCtrl.IsBusy = true;
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "pageSize" }, new string[] { MagicGlobal.UserInfo.Id.ToString(),"100000" });
            string strResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrCoinList,MagicGlobal.UserInfo.Version, std));
            //strResult = strResult.Replace("operator", "xxx2");
            ViewModel.BaseHttpModel<DataLst> model= DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<DataLst>>(strResult);

            this.busyCtrl.IsBusy = false;
            if (model != null)
            {
                if (model.code == 200)
                {
                    mCoinDetailList.Clear();
                    foreach (var item in model.data.result)
                    {
                        mCoinDetailList.Add(this.SetModelFromHttp(item));
                    }
                    if (mCoinDetailList.Count > 0)
                    {
                        lstMCoinDetail.ScrollIntoView(lstMCoinDetail.Items[0]);
                    }
                }
                if (this.mCoinDetailList.Count == 0)
                {
                    this.gdVoidDetail.Visibility = Visibility.Visible;
                }
                else
                {
                    this.gdVoidDetail.Visibility = Visibility.Collapsed;
                }
            }
        }

        public void InitialAmount()
        {
            Action action = new Action(() =>
            {
                string presult = DAL.HttpHelper.Instance.HttpGet(string.Format(DAL.ConfUtil.AddrO2OMB, MagicGlobal.UserInfo.Version,
                  DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "serviceSign" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "S_Coin" })));

                ViewModel.BaseHttpModel<HttpMBTotalData> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<HttpMBTotalData>>(presult);
                if (model != null)
                {
                    
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (model.code == 200)
                            {
                                TotalAmount = Math.Round(model.data.totalAmount, 2).ToString() + "M";
                                LockAmount = Math.Round(model.data.lockedAmount, 2).ToString() + "M";
                                EnableAmount = Math.Round(model.data.availableAmount, 2).ToString() + "M";
                            }
                            else if(model.code == -2)
                            {
                                TotalAmount =  "0M";
                                LockAmount = "0M";
                                EnableAmount = "0M";
                            }
                        }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
                    }
                
            });
            action.BeginInvoke(null, null);
        }

        #region "对内函数"

        private string GetLogoUrlByOperationName(string triggerCode)
        {
            string temp = string.Empty;
            switch(triggerCode)
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
                case "购买":
                    temp = "/MagicCube;component/Resources/ImageSingle/Micon4.png";
                    break;

                       
            }
            return temp;
        }

        private MCoinDetailListModel SetModelFromHttp(HttpMcoinDetailItem item)
        {
            MCoinDetailListModel temp = new MCoinDetailListModel();
            temp.MCoin = item.serviceQuantity.ToString();
            temp.OperationContent = item.triggerParams;
            temp.OperationLogoUrl = GetLogoUrlByOperationName(item.triggerName);
            temp.OperationName = item.triggerName;
            if (temp.OperationName == "消费")
                temp.OperationName = "支出";
            if (temp.OperationName == "充值")
                temp.OperationName = "购买";
            if (temp.OperationName == "购买")
                temp.OperationContent = "购买";
            temp.OperationLogoUrl = GetLogoUrlByOperationName(temp.OperationName);
            DateTime dt = Convert.ToDateTime(item.createTime);
            temp.OperationTime = dt.ToString("yyyy/MM/dd HH:mm:ss");


            return temp;
        }

        #endregion
    }
}
