using MagicCube.HttpModel;
using MagicCube.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MagicCube.Model;
using System.Collections.ObjectModel;
using MagicCube.TemplateUC;
using System.Threading.Tasks;
using MagicCube.ViewModel;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// ResumeView.xaml 的交互逻辑
    /// </summary>
    public partial class ResumeDetial : UserControl
    {
        public Action SeeCall; 
        public Action PhoneAction;

        public delegate Task delegateUpdataFlag(int id,bool? add);
        public delegateUpdataFlag UpdataFlag;
        private Brush normalColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e5e5e5"));
        private Brush errColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f25751"));
        private Brush tbColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00beff"));
        private Brush tbStaticColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b4b4b4"));
        ObservableCollection<ResumeTag> tagVsView = new ObservableCollection<ResumeTag>();
        public ResumeDetial()
        {
            InitializeComponent();
            this.DataContextChanged += ResumeView_DataContextChanged;
        }

        async void ResumeView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           
            if (this.DataContext as ResumeDetailModel != null)
            {
                ResumeDetailModel pResumeDetailModel = this.DataContext as ResumeDetailModel;
                tagVsView = new ObservableCollection<ResumeTag>();

                if (pResumeDetailModel.tags != null)
                {
                    foreach (ResumeTag iTagV in pResumeDetailModel.tags)
                    {
                        if (iTagV != null)
                            tagVsView.Add(new ResumeTag() { id = iTagV.id, color = iTagV.color, name = iTagV.name });
                    }
                }
                tagVsView.Add(new ResumeTag() { id = -2 });
                icTag.ItemsSource = tagVsView;
                await iniViewContact();
            }
        }

        public void iniResumeComment()
        {
            tbCount.Foreground = tbColor;
            tbCountStatic.Foreground = tbStaticColor;
            tbBody.BorderBrush = normalColor;
            tbError.Visibility = Visibility.Collapsed;
            tbBody.Text = string.Empty;
        }

        #region 标签管理
        private async void ClearFlag_Click(object sender, RoutedEventArgs e)
        {

            ResumeDetailModel pHttpResume = (this.DataContext as ResumeDetailModel);
            int tagid = ((sender as Button).DataContext as ResumeTag).id;
            for (int i = 0; i < pHttpResume.tags.Count; i++)
            {
                if (pHttpResume.tags[i].id == tagid)
                {
                    pHttpResume.tags.Remove(pHttpResume.tags[i]);
                    tagVsView.Remove(tagVsView[i]);
                }
            }
            if (UpdataFlag != null)
                await  UpdataFlag(tagid, false);
            else
                await JudgeHelper.ResumeUnSign(pHttpResume.deliveryID, tagid);
        }
        private void addTag_Click(object sender, RoutedEventArgs e)
        {
            PopMenuTag.PlacementTarget = sender as Button;
            PopMenuTag.IsOpen = true;
            List<TagsMenuModel> menuList = new List<TagsMenuModel>();
            foreach (var item in MagicGlobal.TagsManagerList)
            {
                bool isChecked = false;
                if ((this.DataContext as ResumeDetailModel).tags != null)
                {
                    foreach (var temp in (this.DataContext as ResumeDetailModel).tags)
                    {
                        if (temp != null)
                        {
                            if (temp.id == item.Id)
                            {
                                isChecked = true;
                                break;
                            }
                        }
                    }
                }
                menuList.Add(new TagsMenuModel() { Id = item.Id, IsChecked = isChecked, TagColor = item.TagColor, TagContent = item.TagContent });
            }
            this.popTagsList2.ItemsSource = menuList;
        }
        private async void pCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ResumeDetailModel pHttpResume = this.DataContext as ResumeDetailModel;
            TagsMenuModel pTagsManagerModel = (sender as CheckBox).DataContext as TagsMenuModel;
            if ((sender as CheckBox).IsChecked == true)
            {

                pHttpResume.tags.Insert(tagVsView.Count() - 1, new ResumeTag() { id = pTagsManagerModel.Id, name = pTagsManagerModel.TagContent, color = pTagsManagerModel.TagColor });
                tagVsView.Insert(tagVsView.Count() - 1, new ResumeTag() { id = pTagsManagerModel.Id, name = pTagsManagerModel.TagContent, color = pTagsManagerModel.TagColor });
                if (UpdataFlag != null)
                    await UpdataFlag(pTagsManagerModel.Id, true);
                else
                    await JudgeHelper.ResumeSign(pHttpResume.deliveryID, pTagsManagerModel.Id);

            }

            else
            {
                for (int i = 0; i < pHttpResume.tags.Count; i++)
                {
                    if (pHttpResume.tags[i].id == pTagsManagerModel.Id)
                    {
                        pHttpResume.tags.Remove(pHttpResume.tags[i]);
                        tagVsView.Remove(tagVsView[i]);
                    }
                }
                if (UpdataFlag != null)
                    await UpdataFlag(pTagsManagerModel.Id, false);
                else
                    await JudgeHelper.ResumeUnSign(pHttpResume.deliveryID, pTagsManagerModel.Id);
            }
            PopMenuTag.IsOpen = false;
            
        }
        private void btnTagAdd_Click(object sender, RoutedEventArgs e)
        {
            PopMenuTag.IsOpen = false;
            MagicCube.Common.GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.openTagsAdd, null));
        }
        private void btnTagManager_Click(object sender, RoutedEventArgs e)
        {
            PopMenuTag.IsOpen = false;
            MagicCube.Common.GlobalEvent.GlobalEventHandler(null, new GlobalEventArgs(GlobalFlag.openTagsManager, null));
        }
        #endregion

        #region 事件
        private async void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            ResumeDetailModel pHttpResume = this.DataContext as ResumeDetailModel;
            if (pHttpResume.accountStatus == -3 || pHttpResume.accountStatus == -1 || pHttpResume.accountStatus == -4)
            {
                WinErroTip pWinErroTip = new WinErroTip("该账户已被冻结");
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
                return;
            }

            ViewModel.BaseHttpModel<ViewModel.HttpMobileRoot> pModel = await IMHelper.ViewContact((this.DataContext as ResumeDetailModel).userID);
            if (pModel!=null)
            {
                if (pModel.code==200)
                {
                    (this.DataContext as ResumeDetailModel).viewContact = true;
                    (this.DataContext as ResumeDetailModel).mobile = pModel.data.mobile.mobile;
                }
                else if (pModel.code == -132)
                {
                    WinPayMB pWinPayMB = new WinPayMB();
                    pWinPayMB.Owner = Window.GetWindow(this);
                    pWinPayMB.ShowDialog();
                }
                else
                {
                    WinErroTip pWinErroTip = new WinErroTip(pModel.msg);
                    pWinErroTip.Owner = Window.GetWindow(this);
                    pWinErroTip.ShowDialog();
                }
            }
            else
            {
                WinErroTip pWinErroTip = new WinErroTip(DAL.ConfUtil.netError);
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
            }

           
        }
        private async void BtnPhone_Click(object sender, RoutedEventArgs e)
        {
            ResumeDetailModel pHttpResume = this.DataContext as ResumeDetailModel;
            if (pHttpResume.accountStatus == -3 || pHttpResume.accountStatus == -1 || pHttpResume.accountStatus == -4)
            {
                WinErroTip pWinErroTip = new WinErroTip("该账户已被冻结");
                pWinErroTip.Owner = Window.GetWindow(this);
                pWinErroTip.ShowDialog();
                return;
            }
            WinPhoneCall pWinPhoneHint = new WinPhoneCall(Window.GetWindow(this));
            if (pWinPhoneHint.ShowDialog() == true)
            {
                ViewModel.BaseHttpModel<ViewModel.HttpPhoneRoot> pBaseHttpModel = await IMHelper.FreePhone((this.DataContext as ResumeDetailModel).userID);
                if (pBaseHttpModel!=null)
                {
                    if(pBaseHttpModel.code!=200)
                    {
                        if(pBaseHttpModel.code == -129)
                        {
                            WinMessageLink pWinErroTip = new WinMessageLink("很抱歉，对方可能不方便接听电话\r\n请您在每日（9:00-21:00）联系TA", "知道了");
                            pWinErroTip.Owner = Window.GetWindow(this);
                            pWinErroTip.ShowDialog();
                        }
                        else if (pBaseHttpModel.code == -503)
                        {
                            WinMessageLink pWinErroTip = new WinMessageLink("今天已经给TA打过5次电话了，已达上限", "知道了");
                            pWinErroTip.Owner = Window.GetWindow(this);
                            pWinErroTip.ShowDialog();
                        }
                        else if (pBaseHttpModel.code == -132)
                        {
                            WinPayMB pWinPayMB = new WinPayMB();
                            pWinPayMB.Owner = Window.GetWindow(this);
                            pWinPayMB.ShowDialog();
                        }
                    }
                    else
                    {
                        (this.DataContext as ResumeDetailModel).mobile = pBaseHttpModel.data.contact.mobile;
                        (this.DataContext as ResumeDetailModel).viewContact = true;
                    }
                    
                }
                
            }
  
        }

        private  void BtnVideo_Click(object sender, RoutedEventArgs e)
        {
            WinMessageLink pWinConfirmTip = new WinMessageLink("此功能即将上线，敬请期待！", "我知道了");
            pWinConfirmTip.Owner = Window.GetWindow(this);
            pWinConfirmTip.ShowDialog();
            //ResumeDetailModel pResumeDetailModel = this.DataContext as ResumeDetailModel;
            //if (await VideoHelper.GetVideoSign())
            //{
            //   if(await VideoHelper.VideoInvite(pResumeDetailModel.userID))
            //    {
            //        VideoHelper.writeIni();
            //        System.Diagnostics.Process.Start(@"D:\magiccube_desktop\sample\bin\Release\MagicCubeVideo.exe");
            //    }
            //}
        }
        #endregion

        private async Task iniViewContact()
        {
            ResumeDetailModel pResumeDetailModel = this.DataContext as ResumeDetailModel;
            if (pResumeDetailModel != null)
            {
                Console.WriteLine(pResumeDetailModel.costSalary);
                Console.WriteLine(pResumeDetailModel.priceSalary);

                string presult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrGetMB, MagicGlobal.UserInfo.Version,
                     DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "serviceSign" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), "S_ViewContact" })));

                Console.WriteLine(presult);
                HttpMBTotalRoot pObject = DAL.JsonHelper.ToObject<HttpMBTotalRoot>(presult);

                if (pObject != null)
                {
                    if (pObject.code == 200)
                    {

                        if (pObject.data.totalAmount > 0)
                        {
                            pResumeDetailModel.priceVis = Visibility.Collapsed;
                            pResumeDetailModel.loadSalary = Math.Round(pObject.data.totalAmount).ToString();
                        }
                    }
                }

            }
        }

        private async void CommentDelete_Click(object sender, RoutedEventArgs e)
        {
            ResumeDetailModel pHttpResume = this.DataContext as ResumeDetailModel;
            ResumeComments pResumeComments = (sender as Button).DataContext as ResumeComments;
            if (pResumeComments != null)
            {
                string presult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrCommentDelete, MagicGlobal.UserInfo.Version,
                       DAL.JsonHelper.JsonParamsToString(new string[] { "hrUserID","commentID" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), pResumeComments.commentID.ToString() })));

                BaseHttpModel model = DAL.JsonHelper.ToObject<BaseHttpModel>(presult);
                if (model != null)
                {
                    if (model.code == 200)
                    {
                        pHttpResume.resumeComments.Remove(pResumeComments);
                    }
                }
            }

        }

        private void tbBody_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void tbBody_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (checkBody())
            {
                tbBody.BorderBrush = normalColor;
                tbError.Visibility = Visibility.Collapsed;
            }
        }

        public bool checkBody()
        {
            bool bSucess = true;

            if (string.IsNullOrWhiteSpace(tbBody.Text))
            {
                tbError.Text = "请填写备注内容";
                bSucess = false;
            }
            if (tbBody.Text.Length > 100)
            {
                tbError.Text = "备注不能超过100个字";
                tbCount.Foreground = errColor;
                tbCountStatic.Foreground = errColor;
                bSucess = false;
            }
            else
            {
                tbCount.Foreground = tbColor;
                tbCountStatic.Foreground = tbStaticColor;
            }
            return bSucess;
        }

        private async void CommentAdd_Click(object sender, RoutedEventArgs e)
        {
            ResumeDetailModel pHttpResume = this.DataContext as ResumeDetailModel;
            if (!checkBody())
            {
                tbBody.BorderBrush = errColor;
                tbError.Visibility = Visibility.Visible;
                return;
            }
            btnAddComment.IsEnabled = false;
            string presult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrCommentInsert, MagicGlobal.UserInfo.Version,
                    DAL.JsonHelper.JsonParamsToString(new string[] { "hrUserID", "jhUserID", "comment" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), pHttpResume.userID.ToString(), tbBody.Text })));
            btnAddComment.IsEnabled = true;

            BaseHttpModel<HttpresumeComments> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpresumeComments>> (presult);
            if(model != null)
            {
                if(model.code==200)
                {
                    
                    pHttpResume.resumeComments.Insert(0, new ResumeComments() { canDelete = Visibility,commentID = model.data.commentID, comment = tbBody.Text, createTime = DateTime.Now.ToString("yyyy-MM-dd"), hrName = MagicGlobal.UserInfo.RealName });
                    tbBody.Text = string.Empty;
                }
            }

        }
    }
}
