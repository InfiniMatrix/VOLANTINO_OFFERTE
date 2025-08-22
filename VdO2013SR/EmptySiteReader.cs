using MPExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VdO2013Core;
using VdO2013Data;
using VdO2013SRCore;
using VdO2013SRCore.Specialized;

namespace VdO2013SR
{
    public class EmptySiteReader : SiteReader2, ISiteReader2
    {
        protected override String GetName() { return TextRes.EmptySiteReader.KEmpty; }

        protected override string GetReaderVersion() { return TextRes.EmptySiteReader.KEmpty; }
        protected override String GetReaderName() { return TextRes.EmptySiteReader.KEmpty; }

        protected override String GetTitle() { return TextRes.EmptySiteReader.KEmpty; }

        protected override String GetDescription() { return TextRes.EmptySiteReader.KEmpty; }

        protected override Image GetLogo() { return null; }

        protected override Image GetFullLogo() { return null; }

        //protected override ReaderMode GetReaderMode() { return TextRes.EmptySiteReader.KMode; }
        //protected override Boolean SetReaderMode(ReaderMode value) { return false; }

        protected override String GetNormalSiteRootUrl() { return TextRes.EmptySiteReader.KEmpty; }

        protected override String GetMobileSiteRootUrl() { return TextRes.EmptySiteReader.KEmpty; }

        protected override String GetPageListUrlDefault() { return TextRes.EmptySiteReader.KEmpty; }
        protected override String GetPageListListXPath() { return TextRes.EmptySiteReader.KEmpty; }

        protected override ImageFormatClass GetImageFormat() { return DEFAULTIMAGEFORMAT; }

        protected override int GetDefaultFeatureCount() { return -1; }

        protected override int InitDefaultFeatures() { return -1; }

        //public IQRCode QRCode { get { return null; } }
        //public ISiteReaderConfiguration Config { get { return null; } }

        public override int ReadList(BackgroundWorker worker, VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList info, ReadListCallBack CB)
        {
            throw new NotImplementedException();
        }

        public override int SaveList(BackgroundWorker worker, VdO2013SRCore.Specialized.ISiteReaderOnLineCheckDataList info, SaveListCallBack CB)
        {
            throw new NotImplementedException();
        }

        #region Merge
        protected override String DefaultMergeIVDefinitionTypeName() { return String.Empty; }

        protected override MergeSettings DefaultMergeColExclude() { return new MergeSettings(this, null, MergeKind.Exclude, MergeContext.Column); }
        protected override MergeSettings DefaultMergeRowExclude() { return new MergeSettings(this, null, MergeKind.Exclude, MergeContext.Row); }
        protected override MergeSettings DefaultMergeKeyFields() { return new MergeSettings(this, null, MergeKind.IsKey, MergeContext.Column); }
        #endregion

        public override bool ApplyGlobalSettings(IWebSiteRepository repository)
        {
            throw new NotImplementedException();
        }

        protected override bool CanUpdate => false;
    }
}
