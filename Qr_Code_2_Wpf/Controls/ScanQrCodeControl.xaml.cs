using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace Qr_Code_2_Wpf.Controls
{
    /// <summary>
    /// Interaction logic for ScanQrCodeControl.xaml
    /// </summary>
    public partial class ScanQrCodeControl : UserControl
    {
        QRCodeDecoder decoder = new QRCodeDecoder();
        OpenFileDialog openFileDialog = new OpenFileDialog();

        public ScanQrCodeControl()
        {
            InitializeComponent();
        }

        private void btnScanQrCode_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == true)
            {
                imgScanQrCode.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                txtScanQrCode.Text = decoder.Decode(new QRCodeBitmapImage(new Bitmap(openFileDialog.FileName)));
            }
        }
    }
}
