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
using MagicCube.Model;
using System.Collections.ObjectModel;
using MagicCube.TemplateUC;
using System.Threading.Tasks;

namespace MagicCube.ViewSingle
{
    /// <summary>
    /// ResumeView.xaml 的交互逻辑
    /// </summary>
    public partial class ResumeDetialPrint : UserControl
    {

        ObservableCollection<ResumeTag> tagVsView = new ObservableCollection<ResumeTag>();
        public ResumeDetialPrint()
        {
            InitializeComponent();
            this.DataContextChanged += ResumeView_DataContextChanged;
        }

        public void ResumeView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
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
                icTag.ItemsSource = tagVsView;
                if(icTag.Items.Count>0)
                {
                    gridTag.Visibility = Visibility.Visible;
                }
                else
                {
                    gridTag.Visibility = Visibility.Collapsed;
                }
                if (pResumeDetailModel.resumeComments.Count > 0)
                {
                    gridComment.Visibility = Visibility.Visible;
                }
                else
                {
                    gridComment.Visibility = Visibility.Collapsed;
                }
                if (!string.IsNullOrWhiteSpace(pResumeDetailModel.jobName))
                {
                    pResumeDetailModel.jobType = "应聘职位：";
                }
                else
                {
                    pResumeDetailModel.jobType = "求职意向：";
                    pResumeDetailModel.jobName = pResumeDetailModel.exptPositionDesc;
                }
            }
        }
    }
}
