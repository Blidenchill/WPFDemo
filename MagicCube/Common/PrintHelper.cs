using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MagicCube.Common
{
    public static class PrintHelper
    {
        public static void print(FrameworkElement ViewContainer)

        {

            FrameworkElement objectToPrint = ViewContainer as FrameworkElement;

            PrintDialog printDialog = new PrintDialog();

            //printDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;

            if ((bool)printDialog.ShowDialog().GetValueOrDefault())

            {

                //Mouse.OverrideCursor = Cursors.Wait;

                PrintCapabilities capabilities =

                            printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);

                double dpiScale = 300.0 / 96.0;

                FixedDocument document = new FixedDocument();

                try
                {

                    objectToPrint.Width = capabilities.PageImageableArea.ExtentWidth;

                    objectToPrint.UpdateLayout();

                    objectToPrint.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                    Size size = new Size(capabilities.PageImageableArea.ExtentWidth,

                                             objectToPrint.DesiredSize.Height);

                    objectToPrint.Measure(size);

                    size = new Size(capabilities.PageImageableArea.ExtentWidth,

                                    objectToPrint.DesiredSize.Height);

                    objectToPrint.Measure(size);

                    objectToPrint.Arrange(new Rect(size));

                    // Convert the UI control into a bitmap at 300 dpi

                    double dpiX = 300;

                    double dpiY = 300;

                    RenderTargetBitmap bmp = new RenderTargetBitmap(Convert.ToInt32(

                        capabilities.PageImageableArea.ExtentWidth * dpiScale),

                        Convert.ToInt32(objectToPrint.ActualHeight * dpiScale),

                        dpiX, dpiY, System.Windows.Media.PixelFormats.Pbgra32);

                    bmp.Render(objectToPrint);

                    // Convert the RenderTargetBitmap into a bitmap we can more readily use

                    PngBitmapEncoder png = new PngBitmapEncoder();

                    png.Frames.Add(BitmapFrame.Create(bmp));

                    System.Drawing.Bitmap bmp2;

                    using (MemoryStream memoryStream = new MemoryStream())

                    {

                        png.Save(memoryStream);

                        bmp2 = new System.Drawing.Bitmap(memoryStream);

                    }

                    document.DocumentPaginator.PageSize =

                      new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);

                    // break the bitmap down into pages

                    int pageBreak = 0;

                    int previousPageBreak = 0;

                    int pageHeight =

                         Convert.ToInt32(capabilities.PageImageableArea.ExtentHeight * dpiScale);

                    while (pageBreak < bmp2.Height - pageHeight)

                    {

                        pageBreak += pageHeight;  // Where we thing the end of the page should be

                        // Keep moving up a row until we find a good place to break the page

                        while (!IsRowGoodBreakingPoint(bmp2, pageBreak)) pageBreak--;

                        PageContent pageContent = generatePageContent(bmp2, previousPageBreak,

                          pageBreak, document.DocumentPaginator.PageSize.Width,

                          document.DocumentPaginator.PageSize.Height, capabilities); document.Pages.Add(pageContent); previousPageBreak = pageBreak;
                    }

                    // Last Page

                    PageContent lastPageContent = generatePageContent(bmp2, previousPageBreak,

                      bmp2.Height, document.DocumentPaginator.PageSize.Width,

                      document.DocumentPaginator.PageSize.Height, capabilities);

                    document.Pages.Add(lastPageContent);

                }

                finally

                {

                    // Scale UI control back to the original so we don't effect what is on the screen

                    objectToPrint.Width = double.NaN;

                    objectToPrint.UpdateLayout();

                    objectToPrint.LayoutTransform = new System.Windows.Media.ScaleTransform(1, 1);

                    Size size = new Size(capabilities.PageImageableArea.ExtentWidth,

                                         capabilities.PageImageableArea.ExtentHeight);

                    objectToPrint.Measure(size);

                    objectToPrint.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth,

                                          capabilities.PageImageableArea.OriginHeight), size));

                    Mouse.OverrideCursor = null;

                }

                printDialog.PrintDocument(document.DocumentPaginator, "Print Document Name");

            }
        }

        private static PageContent generatePageContent(System.Drawing.Bitmap bmp, int top, int bottom, double pageWidth, double PageHeight, System.Printing.PrintCapabilities capabilities)
        {

            FixedPage printDocumentPage = new FixedPage();

            printDocumentPage.Width = pageWidth;

            printDocumentPage.Height = PageHeight;

            int newImageHeight = bottom - top;

            System.Drawing.Bitmap bmpPage = bmp.Clone(new System.Drawing.Rectangle(0, top, bmp.Width, newImageHeight), System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // Create a new bitmap for the contents of this page  



            System.Windows.Controls.Image pageImage = new System.Windows.Controls.Image();

            BitmapSource bmpSource =

                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(

                    bmpPage.GetHbitmap(),

                    IntPtr.Zero,

                   System.Windows.Int32Rect.Empty,

                    BitmapSizeOptions.FromWidthAndHeight(bmp.Width, newImageHeight));

            pageImage.Source = bmpSource;
            pageImage.VerticalAlignment = System.Windows.VerticalAlignment.Top;

            // Place the bitmap on the page

            printDocumentPage.Children.Add(pageImage);

            PageContent pageContent = new PageContent();

            ((System.Windows.Markup.IAddChild)pageContent).AddChild(printDocumentPage);

            FixedPage.SetLeft(pageImage, capabilities.PageImageableArea.OriginWidth);

            FixedPage.SetTop(pageImage, capabilities.PageImageableArea.OriginHeight);

            pageImage.Width = capabilities.PageImageableArea.ExtentWidth;

            pageImage.Height = capabilities.PageImageableArea.ExtentHeight;

            return pageContent;
        }

        private static bool IsRowGoodBreakingPoint(System.Drawing.Bitmap bmp, int row)
        {

            double maxDeviationForEmptyLine = 1627500;

            bool goodBreakingPoint = false;

            if (rowPixelDeviation(bmp, row) < maxDeviationForEmptyLine)

                goodBreakingPoint = true;

            return goodBreakingPoint;

        }
        private static double rowPixelDeviation(System.Drawing.Bitmap bmp, int row)

        {

            int count = 0;

            double total = 0;

            double totalVariance = 0;

            double standardDeviation = 0;

            System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0,

                   bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

            int stride = bmpData.Stride;

            IntPtr firstPixelInImage = bmpData.Scan0;

            unsafe

            {

                byte* p = (byte*)(void*)firstPixelInImage;

                p += stride * row;  

                for (int column = 0; column < bmp.Width; column++)

                {

                    count++;  

                    byte blue = p[0];

                    byte green = p[1];

                    byte red = p[3];

                    int pixelValue = System.Drawing.Color.FromArgb(0, red, green, blue).ToArgb();

                    total += pixelValue;

                    double average = total / count;

                    totalVariance += Math.Pow(pixelValue - average, 2);

                    standardDeviation = Math.Sqrt(totalVariance / count);

                   

                    p += 3;

                }

            }

            bmp.UnlockBits(bmpData);

            return standardDeviation;

        }
    }
}
