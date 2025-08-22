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
using ZXing.QrCode.Internal;

namespace VdO2013QRCode
{
    using CoreRes = VdO2013Core.TextRes;
#pragma warning disable IDE1006 // Naming Styles
    public partial class ctQRCodeEncode : DevExpress.XtraEditors.XtraUserControl
#pragma warning restore IDE1006 // Naming Styles
    {
        private int fInizialized = 0;

        public ctQRCodeEncode()
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

        public Boolean EncodeTextVisible
        {
            get { return this.txtEncodeData.Visible; }
            set { this.txtEncodeData.Visible = value; }
        }

        public Boolean OptionsVisible
        {
            get { return this.pnlOptions.Visible; }
            set { this.pnlOptions.Visible = value; }
        }

        public Boolean Initialized
        {
            get { return fInizialized > 0; }
        }
        
        public String EncodeText
        {
            get { return this.txtEncodeData.Text; }
            set { this.txtEncodeData.Text = value; }
        }

        public Image Image
        {
            get { return this.picEncode.Image; }
            set { this.picEncode.Image = value; }
        }

        public short ImageSize
        {
            get { return (short)this.txtSize.Value; }
            set { this.txtSize.Value = value; }
        }

        public System.Drawing.Color ForeGroundColor
        {
            get { return this.btnQRCodeForeColor.BackColor; }
            set { this.btnQRCodeForeColor.BackColor = value; }
        }

        public System.Drawing.Color BackGroundColor
        {
            get { return this.btnQRCodeBackColor.BackColor; }
            set { this.btnQRCodeBackColor.BackColor = value; }
        }

        public QREncoding Encoding
        {
            get
            {
                foreach (String s in Enum.GetNames(typeof(QREncoding)))
                {
                    if (s == cboEncoding.Text)
                        return (QREncoding)cboEncoding.SelectedIndex;
                }
                return QREncoding.Byte;
            }
            set
            {
                cboEncoding.SelectedIndex = cboEncoding.Properties.Items.IndexOf(value.ToString());
            }
        }

        public QRVersion Version
        {
            get
            {
                foreach (String s in Enum.GetNames(typeof(QRVersion)))
                {
                    if (s == cboVersion.Text)
                        return (QRVersion)cboVersion.SelectedIndex;
                }
                return QRVersion.v1;
            }
            set
            {
                cboVersion.SelectedIndex = cboVersion.Properties.Items.IndexOf(value.ToString());
            }
        }

        public QRErrorCorrection ErrorCorrection
        {
            get
            {
                foreach (String s in Enum.GetNames(typeof(QRErrorCorrection)))
                {
                    if (s == cboCorrectionLevel.Text)
                        return (QRErrorCorrection)cboCorrectionLevel.SelectedIndex;
                }
                return QRErrorCorrection.L;
            }
            set
            {
                cboCorrectionLevel.SelectedIndex = cboCorrectionLevel.Properties.Items.IndexOf(value.ToString());
            }
        }

        public String CharacterSet
        {
            get
            {
                return cboCharacterSet.Text;
            }
            set
            {
                cboCharacterSet.SelectedIndex = cboCharacterSet.Properties.Items.IndexOf(value);
            }
        }

        public Boolean SaveToFile(String aFileName, System.Drawing.Imaging.ImageFormat Format)
        {
            Boolean fResult = false;
            System.IO.FileStream fs = null;
            try
            {
                if (System.IO.File.Exists(aFileName))
                    System.IO.File.Delete(aFileName);

                // Saves the Image via a FileStream created by the OpenFile method.
                fs = new System.IO.FileStream(aFileName, System.IO.FileMode.OpenOrCreate);
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                this.picEncode.Image.Save(fs, Format);
                fResult = true;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
            return fResult;
        }

#pragma warning disable IDE1006 // Naming Styles
        private void btnSave_Click(object sender, EventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            dlgSave.InitialDirectory = Global.OutputPath;
            dlgSave.CheckFileExists = false;
            dlgSave.CheckPathExists = true;
            dlgSave.DefaultExt = CoreRes.JPG.DefaultExtension;
            dlgSave.Filter = String.Join("|", CoreRes.JPG.DialogFilter, CoreRes.PNG.DialogFilter, CoreRes.BMP.DialogFilter);
            dlgSave.Title = "Save";
            dlgSave.FileName = string.Empty;
            dlgSave.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (dlgSave.FileName != "")
            {
                switch (dlgSave.FilterIndex)
                {
                    case 1:
                        SaveToFile(dlgSave.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case 2:
                        SaveToFile(dlgSave.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 3:
                        SaveToFile(dlgSave.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case 4:
                        SaveToFile(dlgSave.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                }
            }
        }

        public static Image Encode(String Text, QREncoding Encoding, Int16 Scale, QRVersion Version, QRErrorCorrection ErrorCorrection, System.Drawing.Color ForegroundColor, System.Drawing.Color BackgroundColor, String CharacterSet)
        {
            Image fResult = null;

            if (string.IsNullOrEmpty(Text.Trim()))
            {
                throw new Exception("Data must not be empty.");
            }

            //QRCodeEncoder qrCodeEncoder = new QRCodeEncoder()
            //{
            //    QRCodeBackgroundColor = BackgroundColor,
            //    QRCodeForegroundColor = ForegroundColor,
            //    CharacterSet = QRCodeCharacterSet.CharacterSets[CharacterSet]
            //};
            //switch (Encoding)
            //{
            //    case QREncoding.Byte:
            //        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            //        break;
            //    case QREncoding.AlphaNumeric:
            //        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
            //        break;
            //    case QREncoding.Numeric:
            //        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
            //        break;
            //    default:
            //        throw new Exception("QREncode not selected!");
            //}
            //qrCodeEncoder.QRCodeScale = Scale;
            //qrCodeEncoder.QRCodeVersion = Int16.Parse(Version.ToString().Replace("v", "")) - 1;

            //switch (ErrorCorrection)
            //{
            //    case QRErrorCorrection.L:
            //        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            //        break;
            //    case QRErrorCorrection.M:
            //        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            //        break;
            //    case QRErrorCorrection.Q:
            //        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
            //        break;
            //    case QRErrorCorrection.H:
            //        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            //        break;
            //    default:
            //        break;
            //}
            //fResult = qrCodeEncoder.Encode(Text);

            var ec = ErrorCorrectionLevel.L;
            switch (ErrorCorrection)
            {
                case QRErrorCorrection.L:
                    ec = ErrorCorrectionLevel.L;
                    break;
                case QRErrorCorrection.M:
                    ec = ErrorCorrectionLevel.M;
                    break;
                case QRErrorCorrection.Q:
                    ec = ErrorCorrectionLevel.Q;
                    break;
                case QRErrorCorrection.H:
                    ec = ErrorCorrectionLevel.H;
                    break;
                default:
                    break;
            }

            var writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };
            writer.Options = new QrCodeEncodingOptions() { ErrorCorrection = ec };
            var matrix = writer.Write(Text);
            var barcodeBitmap = new Bitmap(matrix);

            fResult = barcodeBitmap;

            return fResult;
        }

        public Image Encode()
        {
            Image fResult = null;

            try
            {
                fResult = Encode(txtEncodeData.Text
                    , Encoding
                    , (short)txtSize.Value
                    , Version
                    , ErrorCorrection
                    , btnQRCodeForeColor.BackColor
                    , btnQRCodeBackColor.BackColor
                    , cboCharacterSet.Text);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }

            return fResult;
        }

#pragma warning disable IDE1006 // Naming Styles
        private void btnEncode_Click(object sender, EventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            picEncode.Image = Encode();
        }

#pragma warning disable IDE1006 // Naming Styles
        private void btnPrint_Click(object sender, EventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            dlgPrint.Document = docPrint;
            DialogResult r = dlgPrint.ShowDialog();
            if (r == DialogResult.OK)
            {
                docPrint.Print();
            }
        }

#pragma warning disable IDE1006 // Naming Styles
        private void btnQRCodeForeColor_Click(object sender, EventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            dlgColor.AllowFullOpen = true;
            dlgColor.ShowHelp = true;
            dlgColor.Color = this.btnQRCodeForeColor.BackColor;
            if (dlgColor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.btnQRCodeForeColor.BackColor = dlgColor.Color;
            }
        }

#pragma warning disable IDE1006 // Naming Styles
        private void btnQRCodeBackColor_Click(object sender, EventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            dlgColor.AllowFullOpen = true;
            dlgColor.ShowHelp = true;
            dlgColor.Color = this.btnQRCodeBackColor.BackColor;
            if (dlgColor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.btnQRCodeBackColor.BackColor = dlgColor.Color;
            }
        }

#pragma warning disable IDE1006 // Naming Styles
        public void doInit()
#pragma warning restore IDE1006 // Naming Styles
        {
            cboCorrectionLevel.Properties.Items.Clear();
            cboCorrectionLevel.Properties.Items.AddRange(Enum.GetNames(typeof(QRErrorCorrection)));
            ErrorCorrection = QRErrorCorrection.L;

            cboEncoding.Properties.Items.Clear();
            cboEncoding.Properties.Items.AddRange(Enum.GetNames(typeof(QREncoding)));
            Encoding = QREncoding.Byte;

            cboVersion.Properties.Items.Clear();
            cboVersion.Properties.Items.AddRange(Enum.GetNames(typeof(QRVersion)));
            Version = QRVersion.v1;

            // Populate the character set
            cboCharacterSet.Properties.Items.Clear();
            cboCharacterSet.Properties.Items.AddRange(QRCodeCharacterSet.CharacterSets.Keys.ToArray());
            cboCharacterSet.SelectedIndex = 0;

            fInizialized++;
        }

#pragma warning disable IDE1006 // Naming Styles
        private void ctQRCodeEncode_Load(object sender, EventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            if (!Initialized) doInit();
        }

#pragma warning disable IDE1006 // Naming Styles
        private void bmbEncode_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            btnEncode_Click(sender, EventArgs.Empty);
        }

#pragma warning disable IDE1006 // Naming Styles
        private void bmbSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            btnSave_Click(sender, EventArgs.Empty);
        }

#pragma warning disable IDE1006 // Naming Styles
        private void bmbPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
#pragma warning restore IDE1006 // Naming Styles
        {
            btnPrint_Click(sender, EventArgs.Empty);
        }
    }
}
