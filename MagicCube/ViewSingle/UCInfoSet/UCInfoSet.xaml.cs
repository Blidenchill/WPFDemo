using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCInfoSet.xaml 的交互逻辑
    /// </summary>
    public partial class UCInfoSet : UserControl
    {
        public UCInfoSet()
        {
            InitializeComponent();
            ucAuthen.UploadAuthOKAction += UploadAuthOKCallback;
            ucAuthen.GotoJobAction += GotoJobCallback;
            ucAuthen.GoToJobIsVisible = Visibility.Collapsed;
        }

        public Action GotoJobAction;
        

        private async void rbCompanyInfo_Click(object sender, RoutedEventArgs e)
        {

            //埋点
            Common.TrackHelper2.TrackOperation("5.7.3.1.1", "clk");
            Common.TrackHelper2.TrackOperation("5.6.3.10.1", "pv");
            //ucCompanyInfo.InitialEditCompanyInfo();

            await ucCompanyInfo.InitailEditCompanyInfo();
            
            //ucCompanyInfo.InitialComanyInfo2();
        }

        private void rbSelfInfo_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            Common.TrackHelper2.TrackOperation("5.7.1.2.1", "clk");
            Common.TrackHelper2.TrackOperation("5.7.1.3.1", "pv");
            ucSelfInfo.InitialSelfInfo();
        }

        private void rbZhilianBinding_Click(object sender, RoutedEventArgs e)
        {
      
            //ucZhilianBinding.InitailUCZhilian();
        }

        private async void rbAuthen_Click(object sender, RoutedEventArgs e)
        {            
            await IniAuthen();
        }

        public async Task IniAuthen()
        {
            try
            {
                scrollAuthen.ScrollToTop();
                ucAuthen.ucPictureUpload.Visibility = Visibility.Collapsed;
                //判断认证状态
                string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID" }, new string[] { MagicGlobal.UserInfo.Id.ToString() });
                string result = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrGetAuthenState, MagicGlobal.UserInfo.Version, std));

                ViewModel.BaseHttpModel<ViewModel.HttpAuthStateModel> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<ViewModel.HttpAuthStateModel>>(result);
                if (model == null)
                    return;
                else
                {
                    if (model.code == 200)
                    {
                        this.ucAuthen.AuthPicture = model.data.imageUrl;
                        if (model.data == null)
                        {
                            ucAuthen.SetPanel(9);
                            return;
                        }
                        switch (model.data.auditStatus)
                        {
                            //待审核
                            case "0":
                                ucAuthen.SetPanel(0);
                                break;
                            //认证通过
                            case "1":
                                ucAuthen.SetPanel(1);
                                if (string.IsNullOrWhiteSpace(this.ucAuthen.AuthPicture))
                                    //this.ucAuthen.AuthPicture = "https://static1.mofanghr.com/portal/img/defaule-validStatusUrl.png?v=163432";
                                    this.ucAuthen.AuthPicture = string.Empty;
                                break;
                            //认证未通过
                            case "-1":
                                ucAuthen.SetPanel(-1);
                                ucAuthen.FailReason = model.data.auditReason;
                                ucAuthen.CompanyName = MagicGlobal.UserInfo.CompanyName;
                                break;
                            default:
                                ucAuthen.SetPanel(9);
                                ucAuthen.CompanyName = MagicGlobal.UserInfo.CompanyName;
                                break;

                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }

        private void UploadAuthOKCallback()
        {
            ucAuthen.SetPanel(0);
        }

        private void GotoJobCallback()
        {
            if (this.GotoJobAction != null)
                this.GotoJobAction();
        }
    }
}
