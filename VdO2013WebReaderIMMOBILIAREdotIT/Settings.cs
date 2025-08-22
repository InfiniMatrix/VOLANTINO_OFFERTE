using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPExtensionMethods;
//using VdO2013Main.SiteReader;

using SR = VdO2013SRCore;
using System.Drawing;

namespace VdO2013WebReaderIMMOBILIAREdotIT
{
    [Obsolete("Class " + nameof(ReaderSettings) + " is obsolete.")]
    public class ReaderSettings : SR.SiteReaderValueListBase<SR.SettingWrapper>
    {
        internal const string KNAMEVALUE = "Immobiliare";
        internal const string KREADERNAMEVALUE = "IMMOBILIAREdotITReader";
        internal const string KTITLEVALUE = "immobiliare.it";
        internal const string KDESCRIPTIONVALUE = "Scarica le offerte da www.immobiliare.it";
        internal const string KURLNORMALVALUE = ReaderSettingsDefaults.KURLNORMALVALUE;
        internal const string KURLMOBILEVALUE = ReaderSettingsDefaults.KURLMOBILEVALUE;
        internal static readonly Image KLOGOVALUE = ReaderSettingsDefaults.KLOGOVALUE;
        internal static readonly Image KLOGOFULLVALUE = ReaderSettingsDefaults.KLOGOFULLVALUE;

        public static readonly string KREADERVERSIONNAME = ReaderSettingsNames.KREADERVERSIONNAME;
        public static readonly string KREADERVERSIONVALUE = typeof(Reader).Assembly.GetName().Version.ToString();

        public const string KPAGELIST_URLNAME = ReaderSettingsNames.KPAGELIST_URLNAME;
        public const string KPageListUrlSeparator = ReaderSettingsConsts.KPageListUrlSeparator;
        internal const string KPAGELIST_URLVALUE = ReaderSettingsDefaults.KPAGELIST_URLVALUE;

        public static string KPAGELIST_URLREDIRECTSNAME = ReaderSettingsNames.KPAGELIST_URLREDIRECTSNAME;
        private static string KPAGELIST_URLREDIRETSVALUE = @"/terreni";

        public const string KPAGELIST_MAXCOUNTNAME = ReaderSettingsNames.KPAGELIST_MAXCOUNTNAME;
        private const int KPAGELIST_MAXCOUNTVALUE = 20;

        public const string KPAGELIST_ITEMCOUNTXPATHNAME = ReaderSettingsNames.KPAGELIST_ITEMCOUNTXPATHNAME;
        private const string KPAGELIST_ITEMCOUNTXPATHVALUE = @"//*[@id='pageCount']";

        public const string KPAGELIST_TIPOLOGIAXPATHNAME = ReaderSettingsNames.KPAGELIST_TIPOLOGIAXPATHNAME;
        private const string KPAGELIST_TIPOLOGIAXPATHVALUE = @"";

        public const string KPAGELIST_RAGIONESOCIALEXPATHNAME = ReaderSettingsNames.KPAGELIST_RAGIONESOCIALEXPATHNAME;
        private const string KPAGELIST_RAGIONESOCIALEXPATHVALUE = @"//*[@id='dati']/h1";

        public const string KPAGELIST_LISTXPATHNAME = ReaderSettingsNames.KPAGELIST_LISTXPATHNAME;
        internal const string KPAGELIST_LISTXPATHVALUE = ReaderSettingsDefaults.KPAGELIST_LISTXPATHVALUE;

        internal static string KPAGELIST_PAGENUMBERXPATHNAME = ReaderSettingsNames.KPAGELIST_PAGENUMBERXPATHNAME;
        private static string KPAGELIST_PAGENUMBERXPATHVALUE = @"//*[@id=""pageCount""]/strong[1]";

        internal static string KPAGELIST_PAGECOUNTXPATHNAME = ReaderSettingsNames.KPAGELIST_PAGECOUNTXPATHNAME;
        private static string KPAGELIST_PAGECOUNTXPATHVALUE = @"//*[@id=""pageCount""]/strong[2]";

        public const string KPAGELIST_FEATURESENABLEDNAME = ReaderSettingsNames.KPAGELIST_FEATURESENABLEDNAME;
        private const bool KPAGELIST_FEATURESENABLEDVALUE = true;

        public const string KPAGELIST_FEATURESXPATHNAME = ReaderSettingsNames.KPAGELIST_FEATURESXPATHNAME;
        private const string KPAGELIST_FEATURESXPATHVALUE = @"//*[@id='dettagli']//*[@id='details']/table";

        public const string KPAGELIST_IMAGEMAXCOUNTNAME = ReaderSettingsNames.KPAGELIST_IMAGEMAXCOUNTNAME;
        private const int KPAGELIST_IMAGEMAXCOUNTVALUE = 1;

        public const string KPAGELIST_IMAGEXPATHNAME = ReaderSettingsNames.KPAGELIST_IMAGEXPATHNAME;
        private const string KPAGELIST_IMAGEXPATHVALUE = @"//*[@id='thumbs']";

        public const string KPAGELIST_IMAGETHUMBLISTXPATHNAME = ReaderSettingsNames.KPAGELIST_IMAGETHUMBLISTXPATHNAME;
        private const string KPAGELIST_IMAGETHUMBLISTXPATHVALUE = @"//*[@id='thumbs']//*[@class='box_thumb']//a//img";
        
        // Video settings
        public const string KPAGELIST_VIDEOXPATHNAME = ReaderSettingsNames.KPAGELIST_VIDEOXPATHNAME;
        private const string KPAGELIST_VIDEOXPATHVALUE = @"//*[@class='video-container']//video/source | //*[@class='video-player']//video | //*[contains(@class,'video')]//iframe";
        public const string KPAGELIST_VIDEOMAXCOUNTNAME = ReaderSettingsNames.KPAGELIST_VIDEOMAXCOUNTNAME;
        private const int KPAGELIST_VIDEOMAXCOUNTVALUE = 3;
        public const string KPAGELIST_VIDEOENABLEDNAME = ReaderSettingsNames.KPAGELIST_VIDEOENABLEDNAME;
        private const bool KPAGELIST_VIDEOENABLEDVALUE = true;

        protected override int Populate()
        {
            if (_List == null) return -1;
            _List.Add(new SR.SettingWrapper(KREADERVERSIONNAME, KREADERVERSIONVALUE, false));
            _List.Add(new SR.SettingWrapper(KPAGELIST_URLNAME, KPAGELIST_URLVALUE, false));
            _List.Add(new SR.SettingWrapper(KPAGELIST_URLREDIRECTSNAME, KPAGELIST_URLREDIRETSVALUE, false)); //
            _List.Add(new SR.SettingWrapper(KPAGELIST_MAXCOUNTNAME, KPAGELIST_MAXCOUNTVALUE, false));
            _List.Add(new SR.SettingWrapper(KPAGELIST_PAGENUMBERXPATHNAME, KPAGELIST_PAGENUMBERXPATHVALUE, false));//
            _List.Add(new SR.SettingWrapper(KPAGELIST_PAGECOUNTXPATHNAME, KPAGELIST_PAGECOUNTXPATHVALUE, false));//
            _List.Add(new SR.SettingWrapper(KPAGELIST_ITEMCOUNTXPATHNAME, KPAGELIST_ITEMCOUNTXPATHVALUE, false));
            //_List.Add(new SR.SettingWrapper(KPAGELIST_TIPOLOGIAXPATHNAME, KPAGELIST_TIPOLOGIAXPATHVALUE, false));
            _List.Add(new SR.SettingWrapper(KPAGELIST_RAGIONESOCIALEXPATHNAME, KPAGELIST_RAGIONESOCIALEXPATHVALUE, false));
            _List.Add(new SR.SettingWrapper(KPAGELIST_LISTXPATHNAME, KPAGELIST_LISTXPATHVALUE, false));
            _List.Add(new SR.SettingWrapper(KPAGELIST_FEATURESENABLEDNAME, KPAGELIST_FEATURESENABLEDVALUE, false));
            _List.Add(new SR.SettingWrapper(KPAGELIST_FEATURESXPATHNAME, KPAGELIST_FEATURESXPATHVALUE, false));
            _List.Add(new SR.SettingWrapper(KPAGELIST_IMAGEMAXCOUNTNAME, KPAGELIST_IMAGEMAXCOUNTVALUE, false));
            //_List.Add(new SR.SettingWrapper(KPageListImageFormat, KPageListImageFormatDefault, false));
            _List.Add(new SR.SettingWrapper(KPAGELIST_IMAGEXPATHNAME, KPAGELIST_IMAGEXPATHVALUE, false));
            //_List.Add(new SR.SettingWrapper(KPageListImageThumbActiveXPath, KPageListImageThumbActiveXPathDefault, false));
            _List.Add(new SR.SettingWrapper(KPAGELIST_IMAGETHUMBLISTXPATHNAME, KPAGELIST_IMAGETHUMBLISTXPATHVALUE, false));
            //_List.Add(new SR.SettingWrapper(SiteReader2.DefaultSettingsNames.ImageFormat, ImageFormat.Jpeg, false));
            
            // Add video settings
            _List.Add(new SR.SettingWrapper(KPAGELIST_VIDEOXPATHNAME, KPAGELIST_VIDEOXPATHVALUE, false));
            _List.Add(new SR.SettingWrapper(KPAGELIST_VIDEOMAXCOUNTNAME, KPAGELIST_VIDEOMAXCOUNTVALUE, false));
            _List.Add(new SR.SettingWrapper(KPAGELIST_VIDEOENABLEDNAME, KPAGELIST_VIDEOENABLEDVALUE, false));

            return _List.Count;
        }
    }
}
