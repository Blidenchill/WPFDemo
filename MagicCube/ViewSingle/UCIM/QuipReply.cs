using MagicCube.Common;
using MagicCube.HttpModel;
using MagicCube.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;

namespace MagicCube.ViewSingle
{
    public partial class UCIM : UserControl
    {

        HttpCommonReply httpCommonReply = new HttpCommonReply();

        private void QuipReplyContent_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            if ((bool)this.RBRecommend.IsChecked)
            {
            }
            else if ((bool)this.RBInterest.IsChecked)
            {
            }
            else if ((bool)this.RBRecnt.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.1.10.1", "clk");
            }

            this.PopupQuipReplySetting.IsOpen = true;
            lstQuipReply.SelectedItem = null;
        }
        private void btnQuipSetting_Click(object sender, RoutedEventArgs e)
        {
            //埋点
            if ((bool)this.RBRecommend.IsChecked)
            {
            }
            else if ((bool)this.RBInterest.IsChecked)
            {
            }
            else if ((bool)this.RBRecnt.IsChecked)
            {
                Common.TrackHelper2.TrackOperation("5.2.1.12.1", "clk");
            }

            GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.IsOpenQuipReplySetting, true));
            this.PopupQuipReplySetting.IsOpen = false;
        }
        private async Task InitialQuipReply()
        {
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID" }, new string[] { MagicGlobal.UserInfo.Id.ToString() });
            string result = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrGetQuipReplyList, MagicGlobal.UserInfo.Version, std));



            MagicGlobal.ContentCommonReplyList.Clear();
            MagicGlobal.ContentCommonReplyList.Add(new ContentCommonReply() { context = "您好，我对您的情况还不太了解，请先完善简历，谢谢。", title = "完善简历" });
            ViewModel.BaseHttpModel model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel>(result);
            if (model != null)
            {
                if (model.code == 200)
                {
                    if (model.data.ToString() == "{}")
                        return;

                    List<ContentCommonReply> pContentCommonReply = DAL.JsonHelper.ToObject<List<ContentCommonReply>>(model.data.ToString());
                    if (pContentCommonReply == null)
                        return;
                    foreach (ContentCommonReply iContentCommonReply in pContentCommonReply)
                    {
                        MagicGlobal.ContentCommonReplyList.Add(iContentCommonReply);
                    }
                }
            }
            //Action method = delegate
            //{
            //    string result = DAL.HttpHelper.Instance.HttpGet(ConfUtil.ServerCommonReplySearch);
            //    if (result.StartsWith("连接失败"))
            //    {
            //        return;
            //    }
            //    string sendStr = string.Empty;
            //    this.Dispatcher.BeginInvoke(new Action(() =>
            //    {
            //        MagicGlobal.ContentCommonReplyList.Clear();
            //        httpCommonReply = DAL.JsonHelper.ToObject<HttpCommonReply>(result);
            //        if (httpCommonReply.result.Count == 0)
            //        {
            //            MagicGlobal.ContentCommonReplyList.Add(new ContentCommonReply() { content = "您好，我现在忙，一会儿再回复您", title = "现在正忙" });
            //            MagicGlobal.ContentCommonReplyList.Add(new ContentCommonReply() { content = "您好，我们公司对您很感兴趣，希望能与我联系", title = "感兴趣" });
            //            MagicGlobal.ContentCommonReplyList.Add(new ContentCommonReply() { content = "抱歉，该职位已招满，感谢您的关注", title = "职位已招满" });
            //            List<ContentCommonReply> saveReplyList = new List<ContentCommonReply>();
            //            foreach (var item in MagicGlobal.ContentCommonReplyList)
            //            {
            //                saveReplyList.Add(item);
            //            }
            //            sendStr = MagicCube.DAL.JsonHelper.ToJsonString(saveReplyList);
            //            Action method1 = delegate
            //            {
            //                DAL.HttpHelper.Instance.HttpPost(ConfUtil.ServerCommonReplyEdit, sendStr);
            //            };
            //            method1.BeginInvoke(null, null);
            //        }
            //        else
            //        {
            //            foreach (var item in httpCommonReply.result)
            //            {
            //                MagicGlobal.ContentCommonReplyList.Add(item);
            //            }
            //        }
            //        MagicGlobal.ContentCommonReplyList.Insert(0, new ContentCommonReply() { content = "您好，我对您的情况还不太了解，请先完善简历，谢谢。", title = "完善简历" });

            //    }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);

            //};
            //method.BeginInvoke(null, null);
        }
        private void EnterSendChoose_Click(object sender, RoutedEventArgs e)
        {
            PopMenu.PlacementTarget = (sender as Button);
            PopMenu.IsOpen = true;
        }

        private void TbConvrInput_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        private void TbConvrInput_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TbConvrInput_KeyDown(object sender, KeyEventArgs e)
        {

            //if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            //{
            //    if (MagicGlobal.UserInfo.CtrlEnterSendChoose)
            //    {
            //        BtnSend_Click(BtnSend, null);
            //        return;
            //    }
            //}
            //else
            //{
            //    if(e.Key == Key.Enter)
            //    {
            //        if (!MagicGlobal.UserInfo.CtrlEnterSendChoose)
            //        {
            //            BtnSend_Click(BtnSend, null);
            //        }
            //    }
            //}
        }

        private void GrdSendMessage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (!MagicGlobal.UserInfo.EnterSendChoose)
                {
                    BtnSend_Click(BtnSend, null);
                    e.Handled = true;
                    return;
                }
                else
                {
                    var caretIndex = this.TbConvrInput.CaretIndex;
                    this.TbConvrInput.Text = this.TbConvrInput.Text.Insert(caretIndex, "\r\n");
                    this.TbConvrInput.SelectionStart = this.TbConvrInput.Text.Length;
                }
            }
            else
            {
                if (e.Key == Key.Enter)
                {
                    if (MagicGlobal.UserInfo.EnterSendChoose)
                    {
                        BtnSend_Click(BtnSend, null);
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
