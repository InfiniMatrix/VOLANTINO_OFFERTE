using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

//using MessagingToolkit.QRCode.Codec;
//using MessagingToolkit.QRCode.Codec.Data;
//using MessagingToolkit.QRCode.Helper;

using VdO2013Core;
using ZXing;
using MPExtensionMethods;
using ZXing.QrCode;
using ZXing.Common;

namespace VdO2013QRCode
{
    using CoreRes = VdO2013Core.TextRes;

    public partial class ctQRCodeDecode : DevExpress.XtraEditors.XtraUserControl
    {
        public ctQRCodeDecode()
        {
            InitializeComponent();
        }

        public Boolean MenuStripVisible
        {
            get { return this.barMgr.MainMenu.Visible; }
            set { this.barMgr.MainMenu.Visible = value; }
        }

        public Boolean StatusStripVisible
        {
            get { return this.barMgr.StatusBar.Visible; }
            set { this.barMgr.StatusBar.Visible = value; }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            dlgOpen.InitialDirectory = Global.OutputPath;
            dlgOpen.Title = "Open";
            dlgOpen.DefaultExt = VdO2013Core.TextRes.JPG.DefaultExtension;
            dlgOpen.Filter = String.Join("|", CoreRes.JPG.DialogFilter, CoreRes.PNG.DialogFilter, CoreRes.BMP.DialogFilter);
            dlgOpen.RestoreDirectory = true;
            dlgOpen.FileName = string.Empty;

            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                String fileName = dlgOpen.FileName;
                picDecode.Image = new Bitmap(fileName);

            }
        }

        private static byte[] GetRGBValues(Bitmap bmp)
        {
            // Lock the bitmap's bits. 
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];
            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            bmp.UnlockBits(bmpData);

            return rgbValues;
        }

        public static String Decode(Image Image)
        {
            String fResult = String.Empty;

            //var decoder = new QRCodeDecoder();
            //fResult = decoder.Decode(new QRCodeBitmapImage(new Bitmap(Image)));

            var reader = new QRCodeReader();
            if (Image.ToArray(out var bytes))
            {
                var luminance = new RGBLuminanceSource(bytes, Image.Width, Image.Height);
                //var luminance = new BitmapLuminanceSource((Bitmap)Bitmap.FromStream(Image.ToStream()));
                var binarizer = new HybridBinarizer(luminance);
                var image = new BinaryBitmap(binarizer);
                var r = reader.decode(image);
                fResult = r.Text;
            }
            return fResult;
        }

        private void btnDecode_Click(object sender, EventArgs e)
        {
            try
            {
                txtDecodedData.Text = Decode(picDecode.Image);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
