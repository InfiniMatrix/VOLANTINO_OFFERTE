using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;

using MPUtils;
using MPExtensionMethods;

using VdO2013Core;
using VdO2013Core.Config;
using System.ComponentModel;
using VdO2013SRCore.Specialized;

namespace VdO2013SRCore
{
    public interface ISiteReader2 : IDisposable
    {
        bool Initialized { get; }
        string Name { get; }
        string ReaderName { get; }
        string ReaderVersion { get; }
        string Title { get; }
        string Description { get; }
        Image Logo { get; }
        Image FullLogo { get; }
        //ReaderMode Mode { get; set; }
        string Agenzia { get; set; }
        string RagioneSociale { get; set; }
        string Tipologia { get; set; }
        int Elementi { get; set; }
        int Pagine { get; set; }
        string ReportPath { get; /*set;*/ }
        string ImagesPath { get; /*set;*/ }
        string DataPath { get; /*set;*/ }
        ImageFormatClass ImageFormat { get; }
        object this[string property, string key] { get; }
        NameObjectCollection PageItems { get; }
        FeatureElementCollection Features { get; }
        int FeatureCount { get; }
        string StandardSiteRootUrl { get; }
        string MobileSiteRootUrl { get; }
        string PageListUrlDefault { get; }
        string PageListListXPath { get; }
        int Save();
        int Reload();
        int Reset();
        IQRCode QRCode { get; }
        ISiteReaderConfiguration Config { get; }
        int InvokeCB(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack cb);
        int InvokeCB(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, SaveListCallBack cb);
        int ReadList(System.ComponentModel.BackgroundWorker worker, Specialized.ISiteReaderOnLineCheckDataList info, ReadListCallBack CB);
        int SaveList(System.ComponentModel.BackgroundWorker worker, Specialized.ISiteReaderOnLineCheckDataList info, SaveListCallBack CB);
        string MergeIVDefinitionTypeName { get; }

        bool ApplyGlobalSettings(IWebSiteRepository repository);

        //ISiteReaderOwnedCollection<ISiteReaderOwnedCollectionItem> MergeKeyFields2 { get; }
        //MergeInfos MergeRowExclusions2 { get; }
        //MergeInfos MergeColExclusions2 { get; }
    }
}
