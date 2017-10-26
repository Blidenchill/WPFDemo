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

using MagicCube.Common;

using MagicCube.View.Message;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCQuipReplySetting.xaml 的交互逻辑
    /// </summary>
    public partial class UCQuipReplySetting : UserControl
    {
        public UCQuipReplySetting()
        {
            InitializeComponent();
        }

        private List<ContentCommonReply> saveReplyList;

        private async void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            
            if(!SaveQuipReply())
            {
                return;
            }

            string contentStr = DAL.JsonHelper.ToJsonString(saveReplyList);
            string sendStr = DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "content" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), contentStr });
            string url = string.Format(DAL.ConfUtil.AddrSaveQuipReplyList, MagicGlobal.UserInfo.Version);
            string result = await DAL.HttpHelper.Instance.HttpPostAsync(url, sendStr);
            GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.IsOpenQuipReplySetting, false));
        }

        public void InitialQuipReplyList()
        {
            this.txtContent1.Text = string.Empty;
            this.txtTitle1.Text = string.Empty;
            this.txtContent2.Text = string.Empty;
            this.txtTitle2.Text = string.Empty;
            this.txtContent3.Text = string.Empty;
            this.txtTitle3.Text = string.Empty;

            for (int i = 0; i < MagicGlobal.ContentCommonReplyList.Count; i++)
            {
                if(i==1)
                {
                    this.txtTitle1.Text = MagicGlobal.ContentCommonReplyList[1].title;
                    this.txtContent1.Text = MagicGlobal.ContentCommonReplyList[1].context;
                }
                else if(i == 2)
                {
                    this.txtTitle2.Text = MagicGlobal.ContentCommonReplyList[2].title;
                    this.txtContent2.Text = MagicGlobal.ContentCommonReplyList[2].context;
                }
                else if(i == 3)
                {
                    this.txtTitle3.Text = MagicGlobal.ContentCommonReplyList[3].title;
                    this.txtContent3.Text = MagicGlobal.ContentCommonReplyList[3].context;
                }
            }
        }

        private bool SaveQuipReply()
        {
            if(string.IsNullOrEmpty( this.txtTitle1.Text))
            {
                if (this.txtContent1.Text.Length > 0)
                {
                    DisappearShow ds = new DisappearShow("请填写快捷回复标题", 1);
                    ds.Owner = Window.GetWindow(this);
                    ds.ShowDialog();
                    return false;
                }
            }
            else
            {
                if(this.txtContent1.Text.Length < 1)
                {
                    DisappearShow ds = new DisappearShow("请填写快捷回复内容", 1);
                    ds.Owner = Window.GetWindow(this);
                    ds.ShowDialog();
                    return false;
                }
            }
            if (string.IsNullOrEmpty(this.txtTitle2.Text))
            {
                if (this.txtContent2.Text.Length > 0)
                {
                    DisappearShow ds = new DisappearShow("请填写快捷回复标题", 1);
                    ds.Owner = Window.GetWindow(this);
                    ds.ShowDialog();
                    return false;
                }
            }
            else
            {
                if (this.txtContent2.Text.Length < 1)
                {
                    DisappearShow ds = new DisappearShow("请填写快捷回复内容", 1);
                    ds.Owner = Window.GetWindow(this);
                    ds.ShowDialog();
                    return false;
                }
            }
            if (string.IsNullOrEmpty(this.txtTitle3.Text))
            {
                if (this.txtContent3.Text.Length > 0)
                {
                    DisappearShow ds = new DisappearShow("请填写快捷回复标题", 1);
                    ds.Owner = Window.GetWindow(this);
                    ds.ShowDialog();
                    return false;
                }
                    
            }
            else
            {
                if (this.txtContent3.Text.Length < 1)
                {
                    DisappearShow ds = new DisappearShow("请填写快捷回复内容", 1);
                    ds.Owner = Window.GetWindow(this);
                    ds.ShowDialog();
                    return false;
                }
            }

            MagicGlobal.ContentCommonReplyList.Clear();
            if (!string.IsNullOrEmpty(this.txtContent1.Text) || !string.IsNullOrEmpty(this.txtTitle1.Text))
                MagicGlobal.ContentCommonReplyList.Add(new HttpModel.ContentCommonReply() { context = this.txtContent1.Text, title = this.txtTitle1.Text });
            if (!string.IsNullOrEmpty(this.txtContent2.Text) || !string.IsNullOrEmpty(this.txtTitle2.Text))
                MagicGlobal.ContentCommonReplyList.Add(new HttpModel.ContentCommonReply() { context = this.txtContent2.Text, title = this.txtTitle2.Text });
            if (!string.IsNullOrEmpty(this.txtContent3.Text) || !string.IsNullOrEmpty(this.txtTitle3.Text))
                MagicGlobal.ContentCommonReplyList.Add(new HttpModel.ContentCommonReply() { context = this.txtContent3.Text, title = this.txtTitle3.Text });

            saveReplyList = new List<ContentCommonReply>();
            foreach(var item in MagicGlobal.ContentCommonReplyList)
            {
                saveReplyList.Add(item);
            }

            MagicGlobal.ContentCommonReplyList.Insert(0, new HttpModel.ContentCommonReply() { context = "您好，我对您的情况还不太了解，请先完善简历，谢谢。", title = "完善简历" });
            return true;

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.IsOpenQuipReplySetting, false));
        }
    }
}
