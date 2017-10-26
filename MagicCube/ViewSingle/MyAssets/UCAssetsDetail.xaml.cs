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
using MagicCube.ViewModel;
using System.Collections.ObjectModel;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// UCAssetsDetail.xaml 的交互逻辑
    /// </summary>
    public partial class UCAssetsDetail : UserControl
    {
        private ViewModel.AssetsDetailViewModel model = new ViewModel.AssetsDetailViewModel();
        bool handSelectDate = false;
        public UCAssetsDetail()
        {
            InitializeComponent();
            this.DataContext = model;
        }

        public void InitialUCAssetDetail()
        {
            model.InitalCommand.Execute(null);
            this.rdoIntergral.IsChecked = true;
            this.cmbType.ItemsSource = model.ServiveActionModeList ;
            this.cmbType.SelectedIndex = 0;

        }

        private void lstMCoinDetail_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void rdoIntergral_Click(object sender, RoutedEventArgs e)
        {
            

            this.cmbType.ItemsSource = model.ServiveActionModeList;
            this.cmbType.SelectedIndex = 0;
            dpEnd.Text = string.Empty;
            model.EndDt = string.Empty;
            dpStart.Text = string.Empty;
            model.StartDt = string.Empty;

            model.RdoClickCommand.Execute(sender);
        }

        private void rdoMCoin_Click(object sender, RoutedEventArgs e)
        {
            this.cmbType.ItemsSource = model.TriggerCodeModeList;
            this.cmbType.SelectedIndex = 0;
            dpEnd.Text = string.Empty;
            model.EndDt = string.Empty;
            dpStart.Text = string.Empty;
            model.StartDt = string.Empty;
            model.RdoClickCommand.Execute(sender);
        }

        private void rdoDownload_Click(object sender, RoutedEventArgs e)
        {
            this.cmbType.ItemsSource = model.ServiveActionModeList;
            this.cmbType.SelectedIndex = 0;
            dpEnd.Text = string.Empty;
            model.EndDt = string.Empty;
            dpStart.Text = string.Empty;
            model.StartDt = string.Empty;
            model.RdoClickCommand.Execute(sender);
        }

        private void calStart_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (handSelectDate)
            {
                if (calStart.SelectedDate == null)
                    return;

                calStart.DisplayDate = Convert.ToDateTime(calStart.SelectedDate);
                dpStart.Text = calStart.DisplayDate.ToString("yyyy/M/d");
                model.StartDt = calStart.DisplayDate.ToString("yyyy-MM-dd hh:mm:ss");

                if (calEnd.DisplayDate < calStart.DisplayDate)
                {
                    calEnd.SelectedDate = calEnd.DisplayDate = calStart.DisplayDate;
                    dpEnd.Text = calEnd.DisplayDate.ToString("yyyy/M/d");
                    model.EndDt = calEnd.DisplayDate.ToString("yyyy-MM-dd hh:mm:ss");
                }
                handSelectDate = false;
            }
        }

        private void calEnd_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (handSelectDate)
            {
                if (calEnd.SelectedDate == null)
                    return;
                calEnd.DisplayDate = Convert.ToDateTime(calEnd.SelectedDate);
                dpEnd.Text = calEnd.DisplayDate.ToString("yyyy/M/d");
                model.EndDt = calEnd.DisplayDate.ToString("yyyy-MM-dd hh:mm:ss");

                if (calStart.DisplayDate > calEnd.DisplayDate)
                {
                    calStart.SelectedDate = calStart.DisplayDate = calEnd.DisplayDate;
                    dpStart.Text = calStart.DisplayDate.ToString("yyyy/M/d");
                    model.StartDt = calStart.DisplayDate.ToString("yyyy-MM-dd hh:mm:ss");
                }
                handSelectDate = false;
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            popStart.IsOpen = false;
            popEnd.IsOpen = false;
            handSelectDate = true;
        }

        private void dpStart_Click(object sender, RoutedEventArgs e)
        {
            if (popStart.IsOpen)
            {
                popStart.IsOpen = false;
            }
            else
            {
                popStart.IsOpen = true;
            }

        }

        private void dpEnd_Click(object sender, RoutedEventArgs e)
        {
            if (popEnd.IsOpen)
            {
                popEnd.IsOpen = false;
            }
            else
            {
                popEnd.IsOpen = true;
            }
        }

        private void btnStartToday_Click(object sender, RoutedEventArgs e)
        {
            calStart.DisplayDate = DateTime.Now;
            dpStart.Text = calStart.DisplayDate.ToString("yyyy/M/d");
            model.StartDt = calStart.DisplayDate.ToString("yyyy-MM-dd hh:mm:ss");
            this.popStart.IsOpen = false;
          
        }

        private void btnStartClear_Click(object sender, RoutedEventArgs e)
        {
            
            dpStart.Text = string.Empty;
            model.StartDt = string.Empty;
            this.calStart.SelectedDate = null;
            this.popStart.IsOpen = false;
        }

        private void btnStartClose_Click(object sender, RoutedEventArgs e)
        {
            this.popStart.IsOpen = false;
        }

        private void btnEndTody_Click(object sender, RoutedEventArgs e)
        {
            calEnd.DisplayDate = DateTime.Now;
            dpEnd.Text = calEnd.DisplayDate.ToString("yyyy/M/d");
            model.EndDt = calEnd.DisplayDate.ToString("yyyy-MM-dd hh:mm:ss");
            this.popEnd.IsOpen = false;
         
        }

        private void btnEndClear_Click(object sender, RoutedEventArgs e)
        {
            dpEnd.Text = string.Empty;
            model.EndDt = string.Empty;
            this.calEnd.SelectedDate = null;
            this.popEnd.IsOpen = false;
        }

        private void btnEndClose_Click(object sender, RoutedEventArgs e)
        {
            this.popEnd.IsOpen = false;
        }
    }
}
