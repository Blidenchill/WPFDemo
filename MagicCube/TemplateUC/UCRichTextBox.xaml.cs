using MagicCube.HTML;
using MarkupConverter;
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
using System.ComponentModel;

namespace MagicCube.View
{
    /// <summary>
    /// UCRichTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class UCRichTextBox : UserControl
    {
        public Action<bool> actionFocus;
        public Action<bool> actionText;



        public Visibility TitleVisible
        {
            get { return (Visibility)GetValue(TitleVisibleProperty); }
            set { SetValue(TitleVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TitleVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleVisibleProperty =
            DependencyProperty.Register("TitleVisible", typeof(Visibility), typeof(UCRichTextBox), new PropertyMetadata(null));



        public bool JustReadOnly
        {
            get { return (bool)GetValue(JustReadOnlyProperty); }
            set { SetValue(JustReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for JustReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty JustReadOnlyProperty =
            DependencyProperty.Register("JustReadOnly", typeof(bool), typeof(UCRichTextBox), new PropertyMetadata(null));



        public UCRichTextBox()
        {
            InitializeComponent();
            

        }
        public void setText(string Html)
        {
            _richTextBox.Text = HtmlToRtfConverter.ConvertHtmlToRtf(Html);
            tbCount.Text = getLength().ToString();
        }
        public string getText()
        {
            return RtfToHtmlConverter.ConvertRtfToHtml(_richTextBox.Text);
        }
        public string getStringText()
        {
            TextRange textRange = new TextRange(_richTextBox.Document.ContentStart, _richTextBox.Document.ContentEnd);
            return textRange.Text.Replace("\r\n", "");
        }
        public long getLength()
        {
            TextRange textRange = new TextRange(_richTextBox.Document.ContentStart, _richTextBox.Document.ContentEnd);
            return textRange.Text.Replace("\r\n", "").Length;
        }
        private void SetToolbar()
        {
            // Set font family combo
            var textRange = new TextRange(_richTextBox.Selection.Start, _richTextBox.Selection.End);
            
            // Set Font buttons
            if (!String.IsNullOrEmpty(textRange.Text))
            {
                BoldButton.IsChecked = textRange.GetPropertyValue(TextElement.FontWeightProperty).Equals(FontWeights.Bold);
                UnderlineButton.IsChecked = textRange.GetPropertyValue(Inline.TextDecorationsProperty).Equals(TextDecorations.Underline);
                BulletsButton.IsChecked = textRange.GetPropertyValue(Inline.TextDecorationsProperty).Equals(TextMarkerStyle.Disc);
                NumberingButton.IsChecked = textRange.GetPropertyValue(Inline.TextDecorationsProperty).Equals(TextMarkerStyle.Decimal);
                
            }
        }

        private void richTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            SetToolbar();
            tbCount.Text = getLength().ToString();
        }

        private void _richTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //spTextCount.Visibility = System.Windows.Visibility.Visible;
            if (actionFocus!=null)
                actionFocus(true);
        }

        private void _richTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //spTextCount.Visibility = System.Windows.Visibility.Collapsed;
            if (actionFocus != null)
                actionFocus(false);
        }

        private void _richTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(actionText!=null)
            {
                if (getStringText() != string.Empty)
                {
                    actionText(true);
                }
                else
                {
                    actionText(false);
                }
            }

        }

    }


}
