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

using MagicCube.BindingConvert;
using MagicCube.View.Message;
using MagicCube.TemplateUC;
using System.Threading.Tasks;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCTagsSetting.xaml 的交互逻辑
    /// </summary>
    public partial class UCTagsSetting : UserControl
    {

        private bool IsModifyEdit = false;
        private TagsManagerModel selectedTagModel = new TagsManagerModel();
        private string[] tagsColorAll = new string[10] { "#70E0A5", "#FFC540", "#94A1FA", "#7FD3D8", "#FDA76D", "#FF8176", "#7CC4FF", "#DC98EE", "#797979", "#9C7C40" };
        private ObservableCollection<string> TagsColorArray = new ObservableCollection<string>() { "#70E0A5", "#FFC540", "#94A1FA", "#7FD3D8", "#FDA76D", "#FF8176", "#7CC4FF", "#DC98EE", "#797979", "#9C7C40" };

        public UCTagsSetting()
        {
            InitializeComponent();
            this.lstTagsManage.ItemsSource = MagicGlobal.TagsManagerList;
            this.UpdateTagsColorArray();
            this.cmbColorSelect.ItemsSource = TagsColorArray;
            //if(IsManagePanel)
            //{
            //    this.grdManage.Visibility = Visibility.Visible;
            //    this.grdEdit.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    this.grdManage.Visibility = Visibility.Collapsed;
            //    this.grdEdit.Visibility = Visibility.Visible;
            //}
            this.btnAdd.SetBinding(Button.VisibilityProperty, new Binding() { Source = MagicGlobal.TagsManagerList, Path = new PropertyPath("Count"), Converter = new ListCount5ToCollapseConverter() });
            this.tbTips.SetBinding(TextBlock.VisibilityProperty, new Binding() { Source = MagicGlobal.TagsManagerList, Path = new PropertyPath("Count"), Converter = new ListCount5ToVisiblityConverter() });
        }

        public void ChoosePanel(bool isManagePanel)
        {
            //非法输入判断显示清空
            this.tbValidate.Visibility = Visibility.Collapsed;
            //this.rectValidate.Visibility = Visibility.Collapsed;
            this.rectValidate.Stroke = new SolidColorBrush(Color.FromRgb(0x23, 0xc4, 0xea));

            if (isManagePanel)
            {
                this.grdManage.Visibility = Visibility.Visible;
                this.grdEdit.Visibility = Visibility.Collapsed;

            }
            else
            {
                this.UpdateTagsColorArray();
                if (MagicGlobal.TagsManagerList.Count < 5)
                {
                    this.grdManage.Visibility = Visibility.Collapsed;
                    this.grdEdit.Visibility = Visibility.Visible;
                    this.tbEditTitle.Text = "添加标记";
                    this.cmbColorSelect.SelectedIndex = -1;
                    this.txtTagName.Text = string.Empty;
                    this.txtTagName.Focus();
                    bool isSame = false;
                    foreach (var item in this.cmbColorSelect.Items)
                    {
                        isSame = false;
                        foreach (var tag in MagicGlobal.TagsManagerList)
                        {
                            if (item.ToString() == tag.TagColor)
                            {
                                isSame = true;
                                break;
                            }
                        }
                        if (!isSame)
                        {
                            this.cmbColorSelect.SelectedItem = item;
                            break;
                        }
                    }


                }
                else
                {
                    this.grdManage.Visibility = Visibility.Visible;
                    this.grdEdit.Visibility = Visibility.Collapsed;
                }

            }
        }

        #region "委托"
        public Action CloseAction;
        #endregion

        #region "事件"
        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            TagsManagerModel tags = btn.DataContext as TagsManagerModel;
            if (tags == null)
                return;
            this.selectedTagModel = tags;
            this.grdManage.Visibility = Visibility.Collapsed;
            this.grdEdit.Visibility = Visibility.Visible;
            this.IsModifyEdit = true;
            this.txtTagName.Text = tags.TagContent;
            this.txtTagName.Focus();
            this.tbEditTitle.Text = "修改标记";
            this.UpdateTagsColorArray();
            this.TagsColorArray.Add(tags.TagColor);
            this.cmbColorSelect.SelectedIndex = this.TagsColorArray.Count - 1;
            //foreach (var item in this.cmbColorSelect.Items)
            //{
            //    if(item.ToString() == tags.TagColor)
            //    {
            //        this.cmbColorSelect.SelectedItem = item;
            //        break;
            //    }
            //}


        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            WinConfirmTip message = new WinConfirmTip("删除标记后，相关简历也将移除此标记（简历不会被删除）,是否确定？");
            message.Owner = Window.GetWindow(this);
            if (!(bool)message.ShowDialog())
            {
                return;
            }
            Button btn = sender as Button;
            TagsManagerModel tags = btn.DataContext as TagsManagerModel;

            string std = Uri.EscapeDataString(DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "userSignID" },
                        new string[] { MagicGlobal.UserInfo.Id.ToString(),tags.Id.ToString() }));
            string url = string.Format(DAL.ConfUtil.AddrResumTagsDelete, MagicGlobal.UserInfo.Version, std);
            this.Cursor = Cursors.Wait;
            string result = await DAL.HttpHelper.Instance.HttpGetAsync(url);
            this.Cursor = Cursors.Arrow;
            ViewModel.BaseHttpModel model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel>(result);
            if (model == null)
            {
                WinErroTip message2 = new WinErroTip("网络异常,请重试。");
                message2.Owner = Window.GetWindow(this);
                message2.ShowDialog();
            }
            else
            {
                if (model.code == 200)
                {
                    foreach (var item in MagicGlobal.TagsManagerList)
                    {
                        if (item.Id == tags.Id)
                        {
                            MagicGlobal.TagsManagerList.Remove(item);
                            break;
                        }
                    }
                }
                else
                {
                    WinErroTip win = new WinErroTip(model.msg);
                    win.Owner = Window.GetWindow(this);
                    win.ShowDialog();
                    return;
                }
            }

            //测试删除账号
            //string delResult = DAL.HttpHelper.Instance.HttpPost(string.Format(ConfUtil.ServerTagsManageDelete, tags.Id), string.Empty);
            //Console.WriteLine("删除结果：" + delResult);

            //if (delResult.Contains("200"))
            //{
            //    foreach (var item in MagicGlobal.TagsManagerList)
            //    {
            //        if (item.Id == tags.Id)
            //        {
            //            MagicGlobal.TagsManagerList.Remove(item);
            //            break;
            //        }
            //    }
            //}
            //else
            //{
            //    WinErroTip winMessage = new WinErroTip("网络异常，请重试");
            //    winMessage.ShowDialog();
            //}
            //this.Cursor = Cursors.Arrow;

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.tbValidate.Visibility = Visibility.Collapsed;
            //this.rectValidate.Visibility = Visibility.Collapsed;
            this.rectValidate.Stroke = new SolidColorBrush(Color.FromRgb(0x23, 0xc4, 0xea));
            if (CloseAction != null)
            {
                this.CloseAction();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            this.grdManage.Visibility = Visibility.Collapsed;
            this.grdEdit.Visibility = Visibility.Visible;
            this.IsModifyEdit = false;
            this.cmbColorSelect.SelectedIndex = -1;
            this.txtTagName.Text = string.Empty;
            this.txtTagName.Focus();
            this.tbEditTitle.Text = "添加标记";
            bool isSame = false;
            this.UpdateTagsColorArray();
            foreach (var item in this.cmbColorSelect.Items)
            {
                isSame = false;
                foreach (var tag in MagicGlobal.TagsManagerList)
                {
                    if (item.ToString() == tag.TagColor)
                    {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame)
                {
                    this.cmbColorSelect.SelectedItem = item;
                    break;
                }
            }

        }

        private async void EditOK_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtTagName.Text.Trim()))
            {
                this.tbValidate.Visibility = Visibility.Visible;
                this.tbValidate.Text = "请输入标记名";
                //this.rectValidate.Visibility = Visibility.Visible;
                this.rectValidate.Stroke = new SolidColorBrush(Colors.Red);
                return;
            }
            string tempStr = this.txtTagName.Text.TrimEnd();
            tempStr = tempStr.TrimStart();
            if (tempStr.Length > 8)
            {
                this.tbValidate.Visibility = Visibility.Visible;
                this.tbValidate.Text = "最多可输入8个字";
                //this.rectValidate.Visibility = Visibility.Visible;
                this.rectValidate.Stroke = new SolidColorBrush(Colors.Red);
                return;
            }
            //判断标签名是否重复
            foreach (var item in MagicGlobal.TagsManagerList)
            {
                if (item.TagContent == tempStr)
                {
                    this.tbValidate.Visibility = Visibility.Visible;
                    this.tbValidate.Text = "该标记名已存在";
                    //this.rectValidate.Visibility = Visibility.Visible;
                    this.rectValidate.Stroke = new SolidColorBrush(Colors.Red);
                    return;
                }
            }
            bool isSame = false;
            if (this.cmbColorSelect.SelectedIndex == -1)
            {
                foreach (var item in this.cmbColorSelect.Items)
                {
                    isSame = false;
                    foreach (var tag in MagicGlobal.TagsManagerList)
                    {
                        if (item.ToString() == tag.TagColor)
                        {
                            isSame = true;
                            break;
                        }
                    }
                    if (!isSame)
                    {
                        this.cmbColorSelect.SelectedItem = item;
                        break;
                    }
                }
            }
            try
            {
                string color = this.cmbColorSelect.SelectedItem.ToString();
                if (this.Cursor != Cursors.Wait)
                {
                    this.Cursor = Cursors.Wait;

                    if (this.IsModifyEdit)
                    {

                        string std = Uri.EscapeDataString(DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "userSignName", "userSignColor", "userSignID" },
                            new string[] { MagicGlobal.UserInfo.Id.ToString(), tempStr, color, this.selectedTagModel.Id.ToString() }));
                        string url = string.Format(DAL.ConfUtil.AddrResumTagsModify, MagicGlobal.UserInfo.Version, std);
                        string result = await DAL.HttpHelper.Instance.HttpGetAsync(url);
                        this.Cursor = Cursors.Arrow;
                        ViewModel.BaseHttpModel model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel>(result);
                        if (model == null)
                        {
                            WinErroTip message = new WinErroTip("网络异常,请重试。");
                            message.Owner = Window.GetWindow(this);
                            message.ShowDialog();
                        }
                        else
                        {
                            if (model.code == 200)
                            {
                                this.selectedTagModel.TagColor = color;
                                this.selectedTagModel.TagContent = tempStr;
                            }
                            else
                            {
                                WinErroTip win = new WinErroTip(model.msg);
                                win.Owner = Window.GetWindow(this);
                                win.ShowDialog();
                                return;
                            }
                        }
                    }
                    else
                    {
                        string std = Uri.EscapeDataString(DAL.JsonHelper.JsonParamsToString(new string[] { "userID", "userSignName", "userSignColor" }, new string[] { MagicGlobal.UserInfo.Id.ToString(), tempStr, color }));

                        string url = string.Format(DAL.ConfUtil.AddrResumTagsAdd, MagicGlobal.UserInfo.Version, std);
                        string result = await DAL.HttpHelper.Instance.HttpGetAsync(url);

                        ViewModel.BaseHttpModel model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel>(result);
                        if (model == null)
                        {
                            this.Cursor = Cursors.Arrow;
                            WinErroTip message = new WinErroTip("网络异常,请重试。");
                            message.Owner = Window.GetWindow(this);
                            message.ShowDialog();
                        }
                        else
                        {
                            if (model.code == 200)
                            {
                                await InitalTagsManage();
                                this.Cursor = Cursors.Arrow;
                            }
                            else
                            {
                                this.Cursor = Cursors.Arrow;
                                WinErroTip win = new WinErroTip(model.msg);
                                win.Owner = Window.GetWindow(this);
                                win.ShowDialog();
                                return;
                            }
                        }
                    }

                    this.grdManage.Visibility = Visibility.Visible;
                    this.grdEdit.Visibility = Visibility.Collapsed;
                    //this.Cursor = Cursors.Arrow;
                }
            }
            catch
            {
                this.grdManage.Visibility = Visibility.Visible;
                this.grdEdit.Visibility = Visibility.Collapsed;
                this.Cursor = Cursors.Arrow;
            }

        }

        private void EditCancel_Click(object sender, RoutedEventArgs e)
        {
            this.grdManage.Visibility = Visibility.Visible;
            this.grdEdit.Visibility = Visibility.Collapsed;

            //this.tbValidate.Visibility = Visibility.Collapsed;
            this.rectValidate.Stroke = new SolidColorBrush(Color.FromRgb(0x23, 0xc4, 0xea));
            //this.rectValidate.Visibility = Visibility.Collapsed;
        }
        #endregion

        private void txtTagName_GotFocus(object sender, RoutedEventArgs e)
        {
            this.tbValidate.Visibility = Visibility.Collapsed;
            //this.rectValidate.Visibility = Visibility.Collapsed;
            this.rectValidate.Stroke = new SolidColorBrush(Color.FromRgb(0x23, 0xc4, 0xea));
        }

        private void txtTagName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtTagName.Text.Length > 8)
            {
                this.tbValidate.Visibility = Visibility.Visible;
                this.tbValidate.Text = "输入已超过8字符";
                //this.rectValidate.Visibility = Visibility.Visible;
                this.rectValidate.Stroke = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.tbValidate.Visibility = Visibility.Collapsed;
                //this.rectValidate.Visibility = Visibility.Collapsed;
                this.rectValidate.Stroke = new SolidColorBrush(Color.FromRgb(0x23, 0xc4, 0xea));
            }
        }

        private void UpdateTagsColorArray()
        {
            this.TagsColorArray.Clear();
            foreach (var item in this.tagsColorAll)
            {
                bool flag = true;
                foreach (var tag in MagicGlobal.TagsManagerList)
                {
                    if (item == tag.TagColor)
                    {
                        flag = false;
                        break;
                    }

                }
                if (flag)
                    this.TagsColorArray.Add(item);
            }
        }
        private async Task InitalTagsManage()
        {
            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID" }, new string[] { MagicGlobal.UserInfo.Id.ToString() });
            string url = string.Format(DAL.ConfUtil.AddrResumTagsList, MagicGlobal.UserInfo.Version, std);
            string result = await DAL.HttpHelper.Instance.HttpGetAsync(url);
            ViewModel.BaseHttpModel<List<ViewModel.HttpResumTags>> model = DAL.JsonHelper.ToObject<ViewModel.BaseHttpModel<List<ViewModel.HttpResumTags>>>(result);
            if (model == null)
                return;
            else
            {
                if (model.code == 200)
                {
                    MagicGlobal.TagsManagerList.Clear();
                    foreach (var item in model.data)
                    {
                        MagicGlobal.TagsManagerList.Add(new TagsManagerModel() { Id = item.userSignID, TagColor = item.userSignColor, TagContent = item.userSignName });
                    }
                }
            }
           


        }
    }
}
