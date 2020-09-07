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
using BarcodeLib;
using QRCode;
using ZXing;
using ThoughtWorks.QRCode;
using System.Drawing;
using System.IO;
using Microsoft.Win32;

namespace 生成二维码
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        int i = 1;
        Bitmap map;
        OpenFileDialog file = new OpenFileDialog();
        System.Drawing.Printing.PrintDocument p = new System.Drawing.Printing.PrintDocument();
        newland.CodeLib.CodeMakePrint code = new newland.CodeLib.CodeMakePrint();
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            code.Height = 200;
            code.Width = 200;
            code.CodeStr = txt1.Text;
            if (cb.SelectedIndex == 0)
            {                
                map = code.GetABarcode();
                code.BarCodeType = BarcodeLib.TYPE.CODE128;
            }
            else if (cb.SelectedIndex == 1)
            {
                map = code.GetAQRimg();
                code.BarCodeType = BarcodeLib.TYPE.CODE39;
            }
            else
            {
                MessageBox.Show("请先选择生成条码类型", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            //Bitmap map = code.GetAQRimg();
            map.Save(".\\" + i + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
            img.Source = new BitmapImage(new Uri(System.Windows.Forms.Application.StartupPath+"\\" + i + ".jpeg",UriKind.Absolute));
            i++;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {            
            file.Filter = "图片|*.jpg;*.png;*.gif;*.bmp;*.jpeg";
            if ((bool)file.ShowDialog())
            {
                txtb2.Text= file.FileName;
                img.Source = new BitmapImage(new Uri(file.FileName));
            }
            else
            {
                MessageBox.Show("请先选择图片","提示");
                return;
            }            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            LibCodeMake.DeCode coder = new LibCodeMake.DeCode();
            map=new Bitmap(file.FileName);
            txtb2.Text= coder.AnalysisOneCode(map);
            
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                if (map == null)
                {
                    MessageBox.Show("请先解析条码");
                    return;
                }
                else
                {
                    p.PrintPage += p_PrintPage;
                }

                p.Print();
            }
            catch { MessageBox.Show("未安装打印机"); return; }
            p.Dispose();
        }

        void p_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {                      
            e.Graphics.DrawImage(map, 0, 0);            
        }
    }
}
