using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;

using MPExtensionMethods;

using VdO2013Core;
using VdO2013Core.Config;
using VdO2013SRCore;

namespace VdO2013QRCode
{
    [Serializable]
    public class QRCodeHelper : IQRCode
    {
        private QREncoding _qrEncoding = QREncoding.Byte;
        private Int16 _qrScale = 1;
        private QRVersion _qrVersion = QRVersion.v1;
        private QRErrorCorrection _qrCorrection = QRErrorCorrection.H;
        private String _qrCharacterSet = "Default UTF-8";

        [NonSerialized]
        private Color _qrCodeForeColor = Color.Black;
        [NonSerialized]
        private Color _qrCodeBackColor = Color.White;
        [NonSerialized]
        private ImageFormatClass _qrImageFormat = ImageFormatClass.Png;

        private int changed = 0;
        public Boolean Changed { get { return changed > 0; } }

        private Boolean differs(Object oldValue, Object newValue) { return !String.Format("{0}", oldValue).Equals(String.Format("{0}", newValue)); }

        public QREncoding Encoding { get { return _qrEncoding; } set { if (differs(_qrEncoding, value)) { _qrEncoding = value; changed++; } } }
        public static QREncoding ParseEncoding(String name) { return ((QREncoding)Enum.Parse(typeof(QREncoding), name)); }

        public Int16 Scale { get { return _qrScale; } set { if (differs(_qrScale, value)) { _qrScale = value; changed++; } } }
        public static Int16 ParseScale(String name) { return Int16.Parse(name); }

        public QRVersion Version { get { return _qrVersion; } set { if (differs(_qrVersion, value)) { _qrVersion = value; changed++; } } }
        public static QRVersion ParseVersion(String name) { return ((QRVersion)Enum.Parse(typeof(QRVersion), name)); }

        public QRErrorCorrection Correction { get { return _qrCorrection; } set { if (differs(_qrCorrection, value)) { _qrCorrection = value; changed++; } } }
        public static QRErrorCorrection ParseCorrection(String name) { return ((QRErrorCorrection)Enum.Parse(typeof(QRErrorCorrection), name)); }

        public String CharacterSet { get { return _qrCharacterSet; } set { if (differs(_qrCharacterSet, value)) { _qrCharacterSet = value; changed++; } } }
        public Color ForeColor { get { return _qrCodeForeColor; } set { if (differs(_qrCodeForeColor, value)) { _qrCodeForeColor = value; changed++; } } }
        public Color BackColor { get { return _qrCodeBackColor; } set { if (differs(_qrCodeBackColor, value)) { _qrCodeBackColor = value; changed++; } } }

        public ImageFormatClass ImageFormat { get { return _qrImageFormat; } set { if (differs(_qrImageFormat, value)) { _qrImageFormat = value; changed++; } } }

        public String ImageFormatFileExtension()
        {
            return ImageFormat.GetFileExtension();
        }

        public static Image Encode(QRCodeHelper coder, String link, out Exception error)
        {
            error = null;
            if (!String.IsNullOrEmpty(link))
                try
                {
                    return VdO2013QRCode.ctQRCodeEncode.Encode(link
                                    , coder.Encoding
                                    , coder.Scale
                                    , coder.Version
                                    , coder.Correction
                                    , coder.ForeColor
                                    , coder.BackColor
                                    , coder.CharacterSet);
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            return null;
        }
        public static String Decode(Image qr, out Exception error)
        {
            error = null;
            if (qr != null)
                try
                {
                    return VdO2013QRCode.ctQRCodeDecode.Decode(qr);
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            return String.Empty;
        }

        /// <summary>
        ///         <add key="QRCodeEncoding" value="0{{Int32}}" />
        ///         <add key="QRCodeScale" value="1{{Int16}}" />
        ///         <add key="QRCodeVersion" value="0{{Int32}}" />
        ///         <add key="QRCodeCorrection" value="3{{Int32}}" />
        ///         <add key="QRCodeCharacterSet" value="Default UTF-8{{String}}" />
        ///         <add key="QRCodeBackColor" value="White{{String}}" />
        ///         <add key="QRCodeForeColor" value="Black{{String}}" />
        ///         <add key="QRCodeImageFormat" value="Png{{String}}" />
        /// </summary>
        public static QRCodeHelper Default
        {
            get
            {
                return new QRCodeHelper(null)
                {
                    Encoding = (QREncoding)0,
                    Scale = 1,
                    Version = (QRVersion)0,
                    Correction = (QRErrorCorrection)3,
                    CharacterSet = "Default UTF-8",
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    ImageFormat = ImageFormatClass.Png,
                };
            }
        }

        public int BackColorValue { get => BackColor.IsEmpty ? 0 : BackColor.ToArgb(); set => BackColor = Color.FromArgb(value); }
        public int ForeColorValue { get => ForeColor.IsEmpty ? 0 : ForeColor.ToArgb(); set => ForeColor = Color.FromArgb(value); }
        public string ImageFormatText { get => ImageFormat.ToString(); set => ImageFormat = Enum.TryParse<ImageFormatClass>(value, out var v) ? v : ImageFormatClass.Png; }

        private ISiteReader2 _owner;
        public QRCodeHelper(ISiteReader2 owner)
        {
            _owner = owner;
        }

        internal int SettingsWrite(ISiteReader2 reader, out Exception error)
        {
            error = null;
            int result = 0;
            try
            {
                reader.Config.Upsert(MPCommonRes.TextRes.QRCode.Encoding, _qrEncoding);
                result++;
                reader.Config.Upsert(MPCommonRes.TextRes.QRCode.Scale, _qrScale);
                result++;
                reader.Config.Upsert(MPCommonRes.TextRes.QRCode.Version, _qrVersion);
                result++;
                reader.Config.Upsert(MPCommonRes.TextRes.QRCode.Correction, _qrCorrection);
                result++;
                reader.Config.Upsert(MPCommonRes.TextRes.QRCode.CharacterSet, _qrCharacterSet);
                result++;
                reader.Config.Upsert(MPCommonRes.TextRes.QRCode.BackColor, _qrCodeBackColor.Name);
                result++;
                reader.Config.Upsert(MPCommonRes.TextRes.QRCode.ForeColor, _qrCodeForeColor.Name);
                result++;
                reader.Config.Upsert(MPCommonRes.TextRes.QRCode.ImageFormat, _qrImageFormat.ToString());
                result++;
            }
            catch (Exception ex)
            {
                error = ex;
            }
            return result;
        }
        public int SettingsWrite(out Exception error) { return SettingsWrite(_owner, out error); }
        internal int SettingsRead(ISiteReader2 reader, out Exception error)
        {
            error = null;
            int result = 0;
            try
            {
                if (reader.Config.Exists(MPCommonRes.TextRes.QRCode.Encoding))
                {
                    _qrEncoding = ParseEncoding(reader.Config.Get<string>(MPCommonRes.TextRes.QRCode.Encoding));
                    result++;
                }
                if (reader.Config.Exists(MPCommonRes.TextRes.QRCode.Scale))
                {
                    _qrScale = ParseScale(reader.Config.Get<string>(MPCommonRes.TextRes.QRCode.Scale));
                    result++;
                }
                if (reader.Config.Exists(MPCommonRes.TextRes.QRCode.Version))
                {
                    _qrVersion = ParseVersion(reader.Config.Get<string>(MPCommonRes.TextRes.QRCode.Version));
                    result++;
                }
                if (reader.Config.Exists(MPCommonRes.TextRes.QRCode.Correction))
                {
                    _qrCorrection = ParseCorrection(reader.Config.Get<string>(MPCommonRes.TextRes.QRCode.Correction));
                    result++;
                }
                if (reader.Config.Exists(MPCommonRes.TextRes.QRCode.CharacterSet))
                {
                    _qrCharacterSet = reader.Config.Get<string>(MPCommonRes.TextRes.QRCode.CharacterSet);
                    result++;
                }
                if (reader.Config.Exists(MPCommonRes.TextRes.QRCode.BackColor))
                {
                    _qrCodeBackColor = Color.FromName(reader.Config.Get<string>(MPCommonRes.TextRes.QRCode.BackColor));
                    result++;
                }
                if (reader.Config.Exists(MPCommonRes.TextRes.QRCode.ForeColor))
                {
                    _qrCodeForeColor = Color.FromName(reader.Config.Get<string>(MPCommonRes.TextRes.QRCode.ForeColor));
                    result++;
                }
                if (reader.Config.Exists(MPCommonRes.TextRes.QRCode.ImageFormat))
                {
                    _qrImageFormat = reader.Config.Get<string>(MPCommonRes.TextRes.QRCode.ImageFormat).ParseImageClass();
                    result++;
                }

                // If no setting is found we need to write the settings
                if (result < 8) changed++;
            }
            catch (Exception ex)
            {
                error = ex;
            }
            return result;
        }
        public int SettingsRead(out Exception error) { return SettingsRead(_owner, out error); }
    }
}
