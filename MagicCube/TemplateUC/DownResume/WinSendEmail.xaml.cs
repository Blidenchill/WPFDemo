using MagicCube.Common;
using MagicCube.View.Message;
using MagicCube.ViewModel;
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
using System.Windows.Shapes;

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// WinPhoneHint.xaml 的交互逻辑
    /// </summary>
    public partial class WinSendEmail : Window
    {
        private long jobId = 0;
        private string userName;
        private long userId = 0;
        private string sendTitle = string.Empty;
        private const string EmaliEmpty = "请填写接收简历的邮箱";
        private const string EmaliError = "格式有误，请重新填写";
        public WinSendEmail(Window ow,long UserId,string name,long JobID = 0,string jobName = "")
        {
            TrackHelper2.TrackOperation("5.5.9.1.1", "pv");
            userId = UserId;
            jobId = JobID;
            userName = name;        
            InitializeComponent();
            if (ow.ActualWidth == SystemParameters.WorkArea.Width && ow.ActualHeight == SystemParameters.WorkArea.Height)
            {
                this.Width = ow.ActualWidth;
                this.Height = ow.ActualHeight;
                this.Left = 0;
                this.Top = 0;
            }
            else
            {
                this.Width = ow.ActualWidth - 20;
                this.Height = ow.ActualHeight - 20;
                this.Left = ow.Left+10;
                this.Top = ow.Top+10;
            }
            this.Owner = ow;
            if (string.IsNullOrWhiteSpace(MagicGlobal.currentUserInfo.DefaultSendEmail))
            {
                tbEmailBody.Text = "您好，该简历我已查阅，请您评估一下。若觉得合适，我们将安排面试，谢谢！";
            }
            else
            {
                tbEmailBody.Text = MagicGlobal.currentUserInfo.DefaultSendEmail;
            }
            if (string.IsNullOrWhiteSpace(jobName))
            {
                tbTitle.Text = "转发 " + userName + " 的简历给同事";
                sendTitle = MagicGlobal.UserInfo.RealName + "转发了" + userName + "的简历给同事";
            }
            else
            {
                tbTitle.Text = "转发 " + userName + "【" + jobName + "】" + " 的简历给同事";
                sendTitle = MagicGlobal.UserInfo.RealName + "转发了" + userName + "【" + jobName + "】" + "的简历给同事";
            }
            this.ContentRendered += WinSendEmail_ContentRendered;
           
        }



        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private async void WinSendEmail_ContentRendered(object sender, EventArgs e)
        {

            string std = DAL.JsonHelper.JsonParamsToString(new string[] { "userID","jobType" }, new string[] { MagicGlobal.UserInfo.Id.ToString(),"2" });
            string resultStr = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrJobList, MagicGlobal.UserInfo.Version, std));
            ViewModel.BaseHttpModel<HttpJobListData> listModel = DAL.JsonHelper.ToObject<BaseHttpModel<HttpJobListData>>(resultStr);
            List<HttpJobList> plstJob = new List<HttpJobList>();
            plstJob.Add(new HttpJobList() { jobID = 0, jobName = "请选择" });
            if (listModel != null)
            {
                if (listModel.code == 200)
                {                 
                    foreach (HttpJobList iHttpJobList in listModel.data.data)
                    {
                        plstJob.Add(iHttpJobList);
                    }
                }
            }

            if(plstJob.Count>1)
            {
                cbJob.ItemsSource = plstJob;
                cbJob.SelectedItem = (cbJob.ItemsSource as List<HttpJobList>).FirstOrDefault(x => x.jobID == jobId);
                cbJob.Visibility = Visibility.Visible;
                btNoJob.Visibility = Visibility.Collapsed;
            }
            else
            {
                cbJob.Visibility = Visibility.Collapsed;
                btNoJob.Visibility = Visibility.Visible;
            }
            
        }


        private Brush normalColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e5e5e5"));
        private Brush errColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f25751"));
        private Brush tbColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00beff"));
        private Brush tbStaticColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b4b4b4"));
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.5.9.7.1", "clk");
            this.DialogResult = false;
            this.Close();
        }

        private async void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.5.9.7.1", "clk");
            if (CheckInput())
            {
                string emailAddr = string.Empty;
                MagicGlobal.currentUserInfo.DefaultSendEmail = tbEmailBody.Text;
                if (!string.IsNullOrWhiteSpace(tbEmail1.Text))
                {
                    emailAddr += tbEmail1.Text.Trim();
                }
                if (!string.IsNullOrWhiteSpace(tbEmail2.Text) && tbEmail2.Visibility == Visibility.Visible)
                {
                    emailAddr +=","+ tbEmail2.Text.Trim();
                }
                if (!string.IsNullOrWhiteSpace(tbEmail3.Text) && tbEmail3.Visibility == Visibility.Visible)
                {
                    emailAddr += "," + tbEmail3.Text.Trim();
                }
                string title =MagicGlobal.UserInfo.RealName+ tbTitle.Text.Substring(0, tbTitle.Text.Length-2);
                title += "您-魔方面面";
                btnSend.IsEnabled = false;
                string jsonStr = DAL.JsonHelper.JsonParamsToString(new string[] { "jh_userID", "email", "subject", "appKey", "BUserID", "paragraph" },
                    new string[] { userId.ToString(), emailAddr, title, "EMAIL_HR", MagicGlobal.UserInfo.Id.ToString(), tbEmailBody.Text });
                string jsonResult = await DAL.HttpHelper.Instance.HttpGetAsync(string.Format(DAL.ConfUtil.AddrEmailSend, MagicGlobal.UserInfo.Version, jsonStr));
                btnSend.IsEnabled = true;
                BaseHttpModel<HttpMessageRoot> model = DAL.JsonHelper.ToObject<BaseHttpModel<HttpMessageRoot>>(jsonResult);
                if(model!=null)
                {
                    if(model.code == 200)
                    {
                        DisappearShow disappear = new DisappearShow("转发成功", 1);
                        disappear.Owner = Window.GetWindow(this);
                        disappear.ShowDialog();
                    }
                }

                this.DialogResult = true;
                this.Close();
            }
           
        }

        private bool CheckInput()
        {
            bool bSucess = true ;

            if(!checkBody())
            {
                tbEmailBody.BorderBrush = errColor;
                tbError.Visibility = Visibility.Visible;
                bSucess = false;
            }
            if(tbErrorEmail1.Visibility == Visibility.Visible|| tbErrorEmail2.Visibility == Visibility.Visible|| tbErrorEmail3.Visibility == Visibility.Visible)
            {
                return false;
            }
            if(!checkEmailEmpty())
            {
                tbErrorEmail.Visibility = Visibility.Visible;
                bSucess = false;
            }
            return bSucess;
                
        }
        public bool checkBody()
        {
            bool bSucess = true;

            if (string.IsNullOrWhiteSpace(tbEmailBody.Text))
            {
                tbError.Text = "请填写邮件正文";
                bSucess = false;
            }
            if (tbEmailBody.Text.Length>100)
            {
                tbError.Text = "请输入100字以内";
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
        public bool checkEmailEmpty()
        {
            bool bSucess = true;
            if (string.IsNullOrWhiteSpace(tbEmail1.Text))
            {
                if (tbEmail2.Visibility == Visibility.Collapsed)
                {
                    tbEmail1.BorderBrush = errColor;
                    return false;
                }
                else
                {
                    if (tbEmail2.Visibility == Visibility.Visible)
                    {
                        if (string.IsNullOrWhiteSpace(tbEmail2.Text))
                        {
                            if (tbEmail3.Visibility == Visibility.Visible)
                            {
                                if (string.IsNullOrWhiteSpace(tbEmail3.Text))
                                {
                                    tbEmail1.BorderBrush = errColor;
                                    return false;
                                }
                            }
                            else
                            {
                                tbEmail1.BorderBrush = errColor;
                                return false;
                            }
                        }
                    }
                }
            }          
            return bSucess;
        }
        private void AddEmail_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.5.9.5.1", "clk");
            if (tbEmail2.Visibility == Visibility.Collapsed)
            {
                tbEmail2.Visibility = Visibility.Visible;
                return;
            }
            if (tbEmail3.Visibility == Visibility.Collapsed)
            {
                btnAddEmail.Visibility = Visibility.Collapsed;
                tbEmail3.Visibility = Visibility.Visible;
                return;
            }
        }


        private void tbEmail1_LostFocus(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.5.9.4.1", "clk");
            if (!string.IsNullOrWhiteSpace(tbEmail1.Text))
            {
                if (CommonValidationMethod.IsEmail(tbEmail1.Text))          
                {
                    tbEmail1.BorderBrush = normalColor;
                    tbErrorEmail1.Visibility = Visibility.Collapsed;
                }
                else
                {
                    tbEmail1.BorderBrush = errColor;
                    tbErrorEmail1.Visibility = Visibility.Visible;
                }
            }
            else
            {
                tbEmail1.BorderBrush = normalColor;
                tbErrorEmail1.Visibility = Visibility.Collapsed;
            }
        }

        private void tbEmail2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbEmail2.Text))
            {
                if (CommonValidationMethod.IsEmail(tbEmail2.Text))
                {
                    tbEmail2.BorderBrush = normalColor;
                    tbErrorEmail2.Visibility = Visibility.Collapsed;
                }
                else
                {
                    tbEmail2.BorderBrush = errColor;
                    tbErrorEmail2.Visibility = Visibility.Visible;
                }
            }
            else
            {
                tbEmail2.BorderBrush = normalColor;
                tbErrorEmail2.Visibility = Visibility.Collapsed;
            }
        }

        private void tbEmail3_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbEmail3.Text))
            {
                if (CommonValidationMethod.IsEmail(tbEmail3.Text))
                {
                    tbEmail3.BorderBrush = normalColor;
                    tbErrorEmail3.Visibility = Visibility.Collapsed;
                }
                else
                {
                    tbEmail3.BorderBrush = errColor;
                    tbErrorEmail3.Visibility = Visibility.Visible;
                }
            }
            else
            {
                tbEmail3.BorderBrush = normalColor;
                tbErrorEmail3.Visibility = Visibility.Collapsed;
            }
        }

        private void tbEmailBody_LostFocus(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.5.9.3.1", "clk");
            if (string.IsNullOrWhiteSpace(tbEmailBody.Text))
            {
                tbEmailBody.BorderBrush = errColor;
                tbError.Visibility = Visibility.Visible;
            }
            else
            {
                tbEmailBody.BorderBrush = normalColor;
                tbError.Visibility = Visibility.Collapsed;
            }
        }

        private void cbJob_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbJob.SelectedItem == null)
            {
                tbTitle.Text = "转发 " + userName + " 的简历给同事";
                sendTitle = MagicGlobal.UserInfo.RealName + "转发了" + userName + "的简历给同事";
            }
            else
            {
                if ((cbJob.SelectedItem as HttpJobList).jobID != 0)
                {
                    tbTitle.Text = "转发 " + userName + "【" + (cbJob.SelectedItem as HttpJobList).jobName + "】" + " 的简历给同事";
                    sendTitle = MagicGlobal.UserInfo.RealName + "转发了" + userName + "【" + (cbJob.SelectedItem as HttpJobList).jobName + "】" + "的简历给同事";
                }
                else
                {
                    tbTitle.Text = "转发 " + userName + " 的简历给同事";
                    sendTitle = MagicGlobal.UserInfo.RealName + "转发了" + userName + "的简历给同事";
                }
            }               
        }

        private void tbEmailBody_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!checkBody())
            {
                tbEmailBody.BorderBrush = errColor;
                tbError.Visibility = Visibility.Visible;
            }
            else
            {
                tbEmailBody.BorderBrush = normalColor;
                tbError.Visibility = Visibility.Collapsed;
            }
        }

        private void btNoJob_Click(object sender, RoutedEventArgs e)
        {
            TrackHelper2.TrackOperation("5.5.9.2.1", "clk");
            PopMenu.IsOpen = true;
        }


        private void tbEmail1_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbErrorEmail.Visibility == Visibility.Visible && tbErrorEmail.Text == EmaliEmpty)
            {
                tbErrorEmail.Visibility = Visibility.Collapsed;
                tbEmail1.BorderBrush = normalColor;
                tbEmail2.BorderBrush = normalColor;
                tbEmail3.BorderBrush = normalColor;
            }
        }

        private void tbEmail2_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbErrorEmail.Visibility == Visibility.Visible && tbErrorEmail.Text == EmaliEmpty)
            {
                tbErrorEmail.Visibility = Visibility.Collapsed;
                tbEmail1.BorderBrush = normalColor;
                tbEmail2.BorderBrush = normalColor;
                tbEmail3.BorderBrush = normalColor;
            }
        }

        private void tbEmail3_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbErrorEmail.Visibility == Visibility.Visible && tbErrorEmail.Text == EmaliEmpty)
            {
                tbErrorEmail.Visibility = Visibility.Collapsed;
                tbEmail1.BorderBrush = normalColor;
                tbEmail2.BorderBrush = normalColor;
                tbEmail3.BorderBrush = normalColor;
            }
        }

        private void cbJob_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            TrackHelper2.TrackOperation("5.5.9.2.1", "clk");
        }
    }
}
