using MagicCube.HttpModel;

using MagicCube.Common;
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
using MagicCube.View.Message;
using System.IO;
using MagicCube.Model;
using System.Text.RegularExpressions;
using MagicCube.TemplateUC;
using MagicCube.ViewModel;
using System.Threading.Tasks;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// ResumeView.xaml 的交互逻辑
    /// </summary>
    public partial class UCJobDetails : UserControl
    {
        public UCJobDetails()
        {
            InitializeComponent();
        }


        #region 职位详情
        public async Task iniDetial(string id)
        {
            busyCtrl.IsBusy = true;
            string propertys = MagicCube.ViewModel.ModelTools.SetHttpPropertys<ViewModel.HttpDetialJob>();
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "jobID", "properties" }, new string[] {id, propertys });
            string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobDetail, MagicGlobal.UserInfo.Version, std));
            BaseHttpModel<HttpDetialJob> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpDetialJob>>(jsonResult);
            if (model.code != 200)
            {
                busyCtrl.IsBusy = false;
                WinErroTip pWinMessage = new WinErroTip(DAL.ConfUtil.netError);
                pWinMessage.Owner = Window.GetWindow(this);
                pWinMessage.ShowDialog();
                return;
            }
            gdDetial.DataContext = model.data;
            busyCtrl.IsBusy = false;
        }
        #endregion


    }

}