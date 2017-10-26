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

namespace MagicCube.TemplateUC
{
    /// <summary>
    /// UCPageTurn.xaml 的交互逻辑
    /// </summary>
    public partial class UCPageTurn : UserControl
    {
        public int totalData = 0;
        public int curPage = 0;
        public int totalPage = 0;

        public delegate Task methodPageChange(int page);
        public methodPageChange PageChange;

        public UCPageTurn()
        {
            InitializeComponent();
        }

        public void setPageState(int pcurPage, int ptotalPage, int ptotalData)
        {
            curPage = pcurPage;
            totalPage = ptotalPage;
            totalData = ptotalData;
            if (totalData == 0)
            {
                pagePanel.IsEnabled = false;
            }
            else
            {
                pagePanel.IsEnabled = true;
            }
            tbTotal.Text = totalData.ToString();
            tbTotlePage.Text = totalPage.ToString();
            tbCurPage.Text = curPage.ToString();
            setPageButton();
        }

        private async void Page_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case "PageFirst":
                    curPage = 1;
                    break;
                case "PageUp":
                    curPage = curPage - 1;
                    break;
                case "PageDown":
                    curPage = curPage + 1;
                    break;
                case "PageLast":
                    curPage = totalPage;
                    break;
            }
            await  PageChange(curPage);
        }

        private void setPageButton()
        {
            PageFirst.IsEnabled = true;

            PageLast.IsEnabled = true;

            PageUp.IsEnabled = true;

            PageDown.IsEnabled = true;

            PageGo.IsEnabled = true;

            if (curPage == 0)
            {
                PageFirst.IsEnabled = false;
                PageUp.IsEnabled = false;
                PageLast.IsEnabled = false;
                PageDown.IsEnabled = false;
                PageGo.IsEnabled = false;
            }

            if (curPage == 1)
            {
                PageFirst.IsEnabled = false;
                PageUp.IsEnabled = false;
            }
            if (curPage == totalPage)
            {
                PageLast.IsEnabled = false;
                PageDown.IsEnabled = false;
            }
        }

        private async void PageGo_Click(object sender, RoutedEventArgs e)
        {
            int t = 0;
            // 判断是不是数字
            if (int.TryParse(tbChangePage.Text, out t))
            {
                if (Convert.ToInt32(tbChangePage.Text) >= 1 && Convert.ToInt32(tbChangePage.Text) <= Convert.ToInt32(totalPage))
                {
                    curPage = Convert.ToInt32(tbChangePage.Text);
                    await PageChange(curPage);
                }
                else
                {
                    WinErroTip pWinMessage = new WinErroTip("请输入1到" + totalPage.ToString() + "的数字");
                    pWinMessage.Owner = Window.GetWindow(this);
                    pWinMessage.ShowDialog();
                }
            }
            else
            {
                WinErroTip pWinMessage = new WinErroTip("请输入1到" + totalPage.ToString() + "的数字");
                pWinMessage.Owner = Window.GetWindow(this);
                pWinMessage.ShowDialog();
            }
        }


    }
}
