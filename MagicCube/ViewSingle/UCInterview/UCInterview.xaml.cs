using MagicCube.Model;
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
using System.Threading.Tasks;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCInterview.xaml 的交互逻辑
    /// </summary>
    public partial class UCInterview : UserControl
    {
        public UCInterview()
        {
            InitializeComponent();
            ucHomePage.ArriveConfirmPage = ArriveConfirmPageCallBack;
            ucHomePage.actionJobDetials = JobDetialsCallBack;
            ucHomePage.actionMBPage = MBPageCallBack;
            ucUnconfirm.actionReturn += ReturnCallBack;
            ucDetail.ReturnHomeAction += ReturnCallBack;
            ucMCoinManage.ReturnHomeAction += ReturnCallBack;
            //ShowUC(UCShowEnum.HomePage);
            
            
        }

       

        private async Task MBPageCallBack()
        {
            ShowUC(UCShowEnum.MCoinManage);
            await ucMCoinManage.InitialMCoinManage();
        }

        private async Task JobDetialsCallBack(string name, string time, string jobId,string sessionId,double cost,InterviewProcess pInterviewProcess)
        {
            Common.TrackHelper2.TrackOperation("5.4.5.2.1", "pv");
            ShowUC(UCShowEnum.Detail);
           await ucDetail.IniUCInterviewDetail(name, time, jobId,sessionId,cost, pInterviewProcess);
        }

        #region "委托回调函数"

        private void JobDetialsCallBack(int id)
        {
            //ShowUC(UCShowEnum.Detail);
            //ucDetail.IniUCInterviewDetail(id);
        }

        private async Task ArriveConfirmPageCallBack()
        {
            Common.TrackHelper2.TrackOperation("5.4.2.1.1", "pv");
            ShowUC(UCShowEnum.Unconfirm);
            await ucUnconfirm.iniUCInterviewUnconfirm();
        }

        private async Task ReturnCallBack()
        {
            ShowUC(UCShowEnum.HomePage);
           await iniHomepage();
        }

        
        #endregion

        #region "对外方法"

        public async Task IniInterview(bool isPower)
        {
            if (isPower)
            {

                Common.TrackHelper2.TrackOperation("5.4.1.2.1", "pv");
                if(ucDetail.Visibility == Visibility.Visible)
                {
                    ucDetail.LeiveNewTags();
                }
                gdNoOpen.Visibility = Visibility.Collapsed;
                await iniHomepage();
                
            }
            else
            {
                gdNoOpen.Visibility = Visibility.Visible;
            }
        }


        private async Task iniHomepage()
        {
            ShowUC(UCShowEnum.HomePage);
            ucHomePage.iniMB();
            ucHomePage.iniUnconfimCount();
           await ucHomePage.iniHomePage();
        }


        #endregion

        #region "对内功能函数"
        private void ShowUC(UCShowEnum uc)
        {
            ucHomePage.Visibility = Visibility.Collapsed;
            ucUnconfirm.Visibility = Visibility.Collapsed;
            ucDetail.Visibility = Visibility.Collapsed;
            ucMCoinManage.Visibility = Visibility.Collapsed;
            switch(uc)
            {
                case UCShowEnum.Detail:
                    ucDetail.Visibility = Visibility.Visible;
                    break;
                case UCShowEnum.HomePage:
                    ucHomePage.Visibility = Visibility.Visible;
                    break;
                case UCShowEnum.MCoinManage:
                    ucMCoinManage.Visibility = Visibility.Visible;
                    break;
                case UCShowEnum.Unconfirm:
                    ucUnconfirm.Visibility = Visibility.Visible;
                    break;
            }
        }
        public enum UCShowEnum
        {
            HomePage,
            Unconfirm,
            Detail,
            MCoinManage,
        }
        #endregion
    }
}
