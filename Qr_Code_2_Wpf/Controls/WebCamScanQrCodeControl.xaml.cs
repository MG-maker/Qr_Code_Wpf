using AForge.Video;
using AForge.Video.DirectShow;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

using ZXing;

namespace Qr_Code_2_Wpf.Controls
{
    /// <summary>
    /// Interaction logic for WebCamScanQrCode.xaml
    /// </summary>
    public partial class WebCamScanQrCodeControl : UserControl
    {
       private FilterInfoCollection filter;
       private VideoCaptureDevice captureDevice;


        protected DispatcherTimer timer = new DispatcherTimer();

        public WebCamScanQrCodeControl()
        {
            InitializeComponent();
        }
        //определение камеры
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            filter = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filter)
                cmbDevice.Items.Add(filterInfo.Name);
            cmbDevice.SelectedIndex = 0;
        }
        private void btnWebScanQrCode_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDevice.Items.Count != 0)
            {
                captureDevice = new VideoCaptureDevice(filter[cmbDevice.SelectedIndex].MonikerString);
                captureDevice.NewFrame += CaptureDevice_NewFrame;
                captureDevice.Start();

                timer.Tick += new EventHandler(timer_Tick);
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Start();
            }
            else
            {
                MessageBox.Show("Camera is not connection");
                return;
            }
        }

        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs e)
        {

            BitmapImage bi = new BitmapImage(); ;
            using (var bitmap = (Bitmap)e.Frame.Clone())
            {
                bi = bitmap.ToBitmapImage();
            }
            //System.InvalidOperationException: 'Вызывающий поток не может получить доступ к данному объекту,
            //так как владельцем этого объекта является другой поток.'
            bi.Freeze();// avoid cross thread operations and prevents leaks
           Dispatcher.BeginInvoke(
               new ThreadStart(
                   delegate 
                   {
                       imgWebScanQrCode.Source = bi; 
                   }
                   )
               );
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (imgWebScanQrCode.Source != null)
            {
                BarcodeReader reader = new BarcodeReader();
                Result result = reader.Decode((BitmapImage)imgWebScanQrCode.Source);
                if (result != null)
                {
                    txtWebScanQrCode.Text = result?.ToString();
                    timer.Stop();
                    if (captureDevice.IsRunning)
                        captureDevice.Stop();
                }
            }
        }

        private void btnCloseWebCam(object sender, RoutedEventArgs e)
        {
            if (cmbDevice.Items.Count != 0 && captureDevice!=null)
                if (captureDevice.IsRunning)
                {
                    imgWebScanQrCode.Source = null;
                    captureDevice.Stop();
                }
        }

    }
}
