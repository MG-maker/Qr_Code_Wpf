using MessagingToolkit.QRCode.Codec;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
    /// Interaction logic for CreateQrCodeControl.xaml
    /// </summary>
    public partial class CreateQrCodeControl : UserControl
    {
        QRCodeEncoder encoder=new QRCodeEncoder();
        Bitmap bitmap;
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        public CreateQrCodeControl()
        {
            InitializeComponent();
        }

        private void btn_CreateQrCode_Click(object sender, RoutedEventArgs e)
        {
            if (txtQrCode.Text != "")
            {
                encoder.QRCodeScale = 8;
                bitmap = encoder.Encode(txtQrCode.Text);
                imgQrCode.Source = ConvertImage.ToBitmapImage(bitmap);
            }
            else
                MessageBox.Show("Please enter some text for create QRCode");
        }

        private void btn_SaveQrCode_Click(object sender, RoutedEventArgs e)
        {
            if (imgQrCode.Source != null)
            {
                saveFileDialog.Filter = "PNG|*.png";
                saveFileDialog.FileName = txtQrCode.Text;
                if (saveFileDialog.ShowDialog() == true)
                {
                    if (bitmap != null)
                    {
                        bitmap.Save(string.Concat(saveFileDialog.FileName, ".png"), ImageFormat.Png);
                    }
                }
            }
            else
                MessageBox.Show("QRCode is not created. Please enter some text and create QRCode");
        }

    }
}
