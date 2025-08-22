using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Configuration;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Reflection;

using System.IO;

using MPExtensionMethods;
using MPUtils;
using VdO2013WS;
using VdO2013QRCode;

using VdO2013Data;
using VdO2013SRCore;
using VdO2013Core;
using VdO2013Core.Config;
using VdO2013SRCore.Specialized;
using MPCommonRes;

namespace VdO2013SR
{
    /// <summary>
    /// This is the base abstract class for all SiteReader.
    /// NOTE: Child classes MUST provide
    ///     internal static Boolean Register(ref SiteReader2Info info)
    /// in order to be fully registered by the factory.
    /// </summary>
    public abstract class SiteReader2 : IDisposable, ISiteReader2
    {
        protected const ImageFormatClass DEFAULTIMAGEFORMAT = ImageFormatClass.Png;
        private readonly Dictionary<string, object> innerStorage = new Dictionary<string, object>();
        private PathsStorage _paths;
        private ReadOnlyPathsStorage _readOnlyPaths;

        protected T Get<T>(T @default = default(T), [System.Runtime.CompilerServices.CallerMemberName] string name = "") where T : IConvertible
        {
            if (innerStorage.ContainsKey(name))
                return (T)innerStorage[name];
            else
                return @default;
        }

        protected void Set<T>(T value, [System.Runtime.CompilerServices.CallerMemberName] string name = "") where T : IConvertible
        {
            if (!CanUpdate)
                throw new InvalidOperationException("Cannot change {0}: editing is disabled.".FormatWith(name));
            if (innerStorage.ContainsKey(name))
                innerStorage[name] = value;
            else
                innerStorage.Add(name, value);
        }

        protected abstract bool CanUpdate { get; }

        protected MPLogHelper.FileLog Logger;
        private readonly QRCodeHelper _qrCode = null;
        private readonly SiteReaderConfiguration _config;
        public delegate Boolean RegisterMethodDelegate(ref ISiteReader2Info info);

        public Boolean SaveConfigSettingsOnDispose { get; set; }
        protected Boolean _StaticReadListInitialized = false;
        protected Boolean _StaticSaveListInitialized = false;

        protected delegate int ReadListCBDelegate(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack cb);
        protected ReadListCBDelegate InvokeReadListCB;
        protected delegate int SaveListCBDelegate(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, SaveListCallBack cb);
        protected SaveListCBDelegate InvokeSaveListCB;

        public int InvokeCB(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack cb)
        {
            if (InvokeReadListCB != null)
                lock (InvokeReadListCB)
                {
                    return InvokeReadListCB(worker, info, cb);
                }
            return -1;
        }
        public int InvokeCB(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, SaveListCallBack cb)
        {
            if (InvokeSaveListCB != null)
                lock (InvokeSaveListCB)
                {
                    return InvokeSaveListCB(worker, info, cb);
                }
            return -1;
        }

        public SiteReader2()
        {
            Logger = new MPLogHelper.FileLog(this);
            _paths = new PathsStorage(this);
            _readOnlyPaths = new ReadOnlyPathsStorage(this);
            InvokeSaveListCB = (worker, info, cb) =>
            {
                if (cb == null) return -1;
                cb(worker, info);
                return 0;
            };
            InvokeReadListCB = (worker, info, cb) =>
            {
                if (cb == null) return -1;
                cb(worker, info);
                return 0;
            };
            _config = new SiteReaderConfiguration(this);
            _qrCode = new QRCodeHelper(this);
            _pageItems = new NameObjectCollection();

            if (!this.GetType().Equals(typeof(EmptySiteReader)))
                System.Diagnostics.Debug.Assert(this.GetType().CanRegister(), TextRes.KClassDoesNotImplementStaticMethodFmt.FormatWith(this.GetType(), TextRes.KRegisterMethodName));

            _config.Load(out Exception error);
            if (error != null)
                Logger.WriteError(error);

            int defaultSettingCount = -1;
            int defaultFeatureCount = -1;

            if (GetDefaultSettingCount() > 0)
                defaultSettingCount = InitDefaultSettings();

            if (GetDefaultFeatureCount() > 0)
                defaultFeatureCount = InitDefaultFeatures();

            if (defaultSettingCount > 0 || defaultFeatureCount > 0)
            {
                _config.Save(out error);
                if (error != null)
                    Logger.WriteError(error);
            }

            Global.PathPrefixChangeSuccess += Global_PathPrefixChangeSuccess;
            _InitializationCount++;
        }

        protected virtual void Global_PathPrefixChangeSuccess(Global.PathPrefixChangeSuccessEventArgs args)
        {
            _config.Set(TextRes.DefaultSettingsNames.KReportPath, DefaultReportPath, out Exception error);
            Logger.WriteError(error);
            _config.Set(TextRes.DefaultSettingsNames.KImagesPath, DefaultImagesPath, out error);
            Logger.WriteError(error);
            _config.Set(TextRes.DefaultSettingsNames.KDataPath, DefaultDataPath, out error);
            Logger.WriteError(error);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (SaveConfigSettingsOnDispose)
                {
                    _config.Save();
                }
            }
        }

        #region Features
        protected static FeatureElement CreateFeature(FeatureWrapper value)
        {
            FeatureElement result = new FeatureElement()
            {
                Name = value.Name,
                Mapping = value.Mapping,
                TypeName = value.TypeName,
                XPath = value.XPath,
                MinSize = value.MinSize,
                MaxSize = value.MaxSize,
                Nullable = value.Nullable,
                IfMissingUseFeature = value.IfMissingUseFeature,
                Actions = new FeatureElementActions()
            };
            result.Actions.IsNull = value.Actions.IsNull;
            result.Actions.NullIf = value.Actions.NullIf;
            result.Actions.TrimLeft = value.Actions.TrimLeft;
            result.Actions.TrimRight = value.Actions.TrimRight;
            result.Actions.ToLowerCase = value.Actions.ToLowerCase;
            result.Actions.ToUpperCase = value.Actions.ToUpperCase;

            if (value.Actions.Replacements != null)
                foreach (var r in value.Actions.Replacements)
                    result.Actions.Replacements.Add(new ReplacementElement() { Text = r.Text, NewText = r.NewText, CaseSensitive = r.CaseSensitive, RepeatCount = r.RepeatCount });

            if (value.Actions.SubStrings != null)
                foreach (var s in value.Actions.SubStrings)
                    result.Actions.SubStrings.Add(new SubStringElement() { Start = s.Start, StartIsSearchFor = s.StartIsSearchFor, End = s.End, EndIsSearchFor = s.EndIsSearchFor, FromLeft = s.FromLeft });

            return result;
        }

        protected Boolean FeatureExists(String key)
        {
            return Features != null && Features.HasKey(key);
        }
        protected FeatureElement FeatureGet(String key)
        {
            return Features[key];
        }
        protected Boolean FeatureSet(FeatureElement value, Boolean addIfMissing)
        {
            if (Features[value.Name] == null && addIfMissing)
                Features.Add(value);
            else
                Features[value.Name] = value;
            return true;
        }
        protected Boolean FeatureSet(FeatureWrapper value, Boolean addIfMissing)
        {
            var fe = CreateFeature(value);
            if (Features[value.Name] == null && addIfMissing)
                Features.Add(fe);
            else
                Features[value.Name] = fe;
            return true;
        }

        protected Boolean FeatureAdd(FeatureElement value) { if (Features.HasKey(value.Name)) return FeatureSet(value, false); else FeatureSet(value, true); return true; }
        protected Boolean FeatureAdd(FeatureWrapper value)
        {
            if (Features == null || Features.HasKey(value.Name))
                return false;
            else
            {
                if (FeatureSet(CreateFeature(value), true))
                    _config.Save();
            }
            return true;
        }
        protected Boolean FeatureDel(String key) { if (Features.HasKey(key)) Features.Remove(Features[key]); return true; }

        public FeatureElementCollection Features
        {
            get
            {
                var section = _config.GetFeaturesSection();
                if (section != null)
                {
                    var result = section.Features;
                    return result;
                }

                return null;
            }
        }
        public int FeatureCount { get { return Features.Count; } }
        #endregion Features

        #region Common
        private static String _commonConnectionString;
        private int _InitializationCount = 0;

        protected abstract String GetName();
        protected abstract String GetReaderName();
        protected abstract String GetReaderVersion();
        protected abstract String GetTitle();
        protected abstract String GetDescription();
        protected abstract Image GetLogo();
        protected abstract Image GetFullLogo();

        protected abstract String GetPageListUrlDefault();
        protected abstract String GetPageListListXPath();

        //protected abstract ReaderMode GetReaderMode();
        //protected abstract Boolean SetReaderMode(ReaderMode value);

        protected abstract String GetNormalSiteRootUrl();
        protected abstract String GetMobileSiteRootUrl();

        protected virtual String GetConfigPath() { return Global.ConfigPath; }
        protected virtual String GetImagesPath() { return Global.ImagesPath; }
        protected virtual String GetDataPath() { return Global.DataPath; }
        protected virtual String GetReportsPath() { return Global.ReportsPath; }
        protected virtual ImageFormatClass GetImageFormat() { return DEFAULTIMAGEFORMAT; }

        public Boolean Initialized { get { return _InitializationCount > 0; } }

        public String ReaderName { get { return GetReaderName(); } }
        public String ReaderVersion { get { return GetReaderVersion(); } }
        public DateTime TimeStamp
        {
            get { return DateTime.FromBinary(_config.Get<long>(TextRes.DefaultSettingsNames.KTimeStamp)); }
            set { _config.Set(TextRes.DefaultSettingsNames.KTimeStamp, value.ToBinary()); }
        }
        public String Agenzia
        {
            get { return _config.Get<string>(TextRes.DefaultSettingsNames.KAgenzia); }
            set { _config.Set(TextRes.DefaultSettingsNames.KAgenzia, value); }
        }
        public String RagioneSociale
        {
            get { return _config.Get<string>(TextRes.DefaultSettingsNames.KRagioneSociale); }
            set { _config.Set(TextRes.DefaultSettingsNames.KRagioneSociale, value); }
        }
        public String Tipologia
        {
            get { return _config.Get<string>(TextRes.DefaultSettingsNames.KTipologia); }
            set { _config.Set(TextRes.DefaultSettingsNames.KTipologia, value); }
        }
        public int Elementi
        {
            get { return _config.Get<int>(TextRes.DefaultSettingsNames.KElementi); }
            set { _config.Set(TextRes.DefaultSettingsNames.KElementi, value); }
        }
        public int Pagine
        {
            get { return _config.Get<int>(TextRes.DefaultSettingsNames.KPagine); }
            set { _config.Set(TextRes.DefaultSettingsNames.KPagine, value); }
        }
        public ImageFormatClass ImageFormat
        {
            get
            {
                var v = _config.Get<string>(TextRes.DefaultSettingsNames.KImageFormat);
                return Enum.TryParse<ImageFormatClass>(v, out var o) ? o : DEFAULTIMAGEFORMAT;
            }
            set
            {
                _config.Set(TextRes.DefaultSettingsNames.KImageFormat, value.ToString());
            }
        }
        public String ReportPath
        {
            get { String value = Global.ReportsPath; return value.IsValidFilePath() ? value : DefaultReportPath; }
        }
        public String ImagesPath
        {
            get { String value = Global.ImagesPath; return value.IsValidFilePath() ? value : DefaultReportPath; }
        }
        public String DataPath
        {
            get { String value = Global.DataPath; return value.IsValidFilePath() ? value : DefaultReportPath; }
        }

        #region PathsStorage
        public class PathsStorage
        {
            private readonly SiteReader2 Reader;

            private String _config = Global.ConfigPath;
            private String _report = Global.ReportsPath;
            private String _image = Global.ImagesPath;
            private String _data = Global.DataPath;

            internal PathsStorage(SiteReader2 reader)
            {
                Reader = reader;
                Global.PathPrefixChangeSuccess += Global_PathPrefixChangeSuccess;
            }

            protected virtual void Global_PathPrefixChangeSuccess(Global.PathPrefixChangeSuccessEventArgs args)
            {
                
                if (ReadOnly) return;
                Reader.Logger.WriteMethod(System.Reflection.MethodBase.GetCurrentMethod(), args);
                _config = Global.ConfigPath;
                _report = Global.ReportsPath;
                _image = Global.ImagesPath;
                _data = Global.DataPath;
            }

            protected Boolean CheckReadOnly()
            {
                if (ReadOnly) throw new InvalidOperationException("Settings are readonly");
                return true;
            }
            protected Boolean CheckValidPath(String value, String propertyName)
            {
                if (!value.IsValidFilePath()) throw new InvalidOperationException("Value {0} is not valid for {1}".FormatWith(value, "Config"));
                return true;
            }

            public Boolean ReadOnly { get; protected set; }
            public String Config
            {
                get { return _config; }
                set
                {
                    if (CheckReadOnly() && CheckValidPath(value, "Config")) _config = value;
                }
            }
            public String Report
            {
                get { return _report; }
                set
                {
                    if (CheckReadOnly() && CheckValidPath(value, "Report")) _report = value;
                }
            }
            public String Image
            {
                get { return _image; }
                set
                {
                    if (CheckReadOnly() && CheckValidPath(value, "Image")) _image = value;
                }
            }
            public String Data
            {
                get { return _data; }
                set
                {
                    if (CheckReadOnly() && CheckValidPath(value, "Data")) _data = value;
                }
            }
        }
        #endregion PathsStorage

        [Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public PathsStorage Paths { get { return _paths; } }

        public class ReadOnlyPathsStorage : PathsStorage
        {
            internal ReadOnlyPathsStorage(SiteReader2 reader) : base(reader)
            {
                ReadOnly = true;
            }
        }

        [Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ReadOnlyPathsStorage PathsDefaults { get { return _readOnlyPaths; } }

        public Object this[String property, String key] { get { return GetPropertyValueByKey(property, key); } }
        protected virtual Object GetPropertyValueByKey(String property, String key)
        {
            switch (property)
            {
                case "Settings":
                    return _config[key].Value;
                case "Features":
                    return FeatureGet(key);
                default:
                    break;
            }
            return null;
        }

        protected virtual int GetDefaultSettingCount()
        {
            return 9;
        }
        protected virtual int InitDefaultSettings()
        {
            int result = 0;
            if (_config.Add(TextRes.DefaultSettingsNames.KNormalSiteUrl, GetNormalSiteRootUrl(), out Exception error))
            {
                result++;
            }
            if (_config.Add(TextRes.DefaultSettingsNames.KMobileSiteUrl, GetMobileSiteRootUrl(), out error))
            {
                result++;
            }
            if (_config.Add(TextRes.DefaultSettingsNames.KAgenzia, String.Empty, out error))
            {
                result++;
            }
            if (_config.Add(TextRes.DefaultSettingsNames.KRagioneSociale, String.Empty, out error))
            {
                result++;
            }
            if (_config.Add(TextRes.DefaultSettingsNames.KTipologia, String.Empty, out error))
            {
                result++;
            }
            if (_config.Add(TextRes.DefaultSettingsNames.KElementi, 0, out error))
            {
                result++;
            }
            if (_config.Add(TextRes.DefaultSettingsNames.KPagine, 0, out error))
            {
                result++;
            }
            if (_config.Add(TextRes.DefaultSettingsNames.KImageFormat, DEFAULTIMAGEFORMAT.ToString()))
            {
                result++;
            }

            // Merge settings
            if (_config.Add(TextRes.DefaultSettingsNames.KMergeDefinitionName, DefaultMergeIVDefinitionTypeName()))
            {
                result++;
            }
            if (_config.Add(TextRes.DefaultSettingsNames.KMergeDefinitionKeyFields, MergeSettings.ToText(DefaultMergeKeyFields())))
            {
                result++;
            }
            if (_config.Add(TextRes.DefaultSettingsNames.KMergeDefinitionColExclude, MergeSettings.ToText(DefaultMergeColExclude())))
            {
                result++;
            }
            if (_config.Add(TextRes.DefaultSettingsNames.KMergeDefinitionRowExclude, MergeSettings.ToText(DefaultMergeRowExclude())))
            {
                result++;
            }

            _config.Upsert(TextRes.DefaultSettingsNames.KReportPath, DefaultReportPath);
            result++;

            _config.Upsert(TextRes.DefaultSettingsNames.KImagesPath, DefaultImagesPath);
            result++;

            _config.Upsert(TextRes.DefaultSettingsNames.KDataPath, DefaultDataPath);
            result++;

            _config.Upsert(TextRes.DefaultSettingsNames.KVersion, this.GetType().Assembly.GetName().Version.ToString());
            result++;

            return result;
        }

        protected abstract int GetDefaultFeatureCount();
        protected abstract int InitDefaultFeatures();

        public String Name { get { return GetName(); } }
        public String Title { get { return GetTitle(); } }
        public String Description { get { return GetDescription(); } }
        public Image Logo { get { return GetLogo(); } }
        public Image FullLogo { get { return GetFullLogo(); } }
        //public ReaderMode Mode { get { return GetReaderMode(); } set { SetReaderMode(value); } }
        public String StandardSiteRootUrl { get { return GetNormalSiteRootUrl(); } }
        public String MobileSiteRootUrl { get { return GetMobileSiteRootUrl(); } }
        public String PageListUrlDefault { get { return GetPageListUrlDefault(); } }
        public String PageListListXPath { get { return GetPageListListXPath(); } }
        public String ConnectionString { get { return _commonConnectionString; } set { _commonConnectionString = value; } }
        public String DefaultConfigPath { get { return GetConfigPath(); } }
        public String DefaultReportPath { get { return GetReportsPath(); } }
        public String DefaultImagesPath { get { return GetImagesPath(); } }
        public String DefaultDataPath { get { return GetDataPath(); } }

        public virtual int Reload(out Exception error)
        {
            error = null;
            try
            {
                return _config.Load(out error, true) ? 1 : -1;
            }
            catch (Exception ex)
            {
                error = ex;
            }// try

            return error != null ? -1 : 0;
        }
        public int Reload()
        {
            return Reload(out Exception error);
        }
        public virtual int Save(out Exception error)
        {
            error = null;
            try
            {
                TimeStamp = DateTime.Now;
                return _config.Save() ? 1 : -1;
            }
            catch (Exception ex)
            {
                error = ex;
            }// try

            return error != null ? -1 : 0;
        }
        public int Save()
        {
            var result = Save(out Exception error);
            Logger.WriteError(error);
            return result;
        }
        public virtual int Reset()
        {
            Elementi = 0;
            PageItems.Clear();
            return 0;
        }

        public Boolean IsDemo { get { return CustomerMgr.VolantinoSuiteMainProductActivationPrivilege == ProductPrivilege.Demo; } }

        #region List
        public abstract int ReadList(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack CB);
        public abstract int SaveList(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, SaveListCallBack CB);

        private NameObjectCollection _pageItems;
        public NameObjectCollection PageItems
        {
            get { return _pageItems; }
        }
        #endregion List

        #endregion Common

        #region Merge
        protected abstract String DefaultMergeIVDefinitionTypeName();
        protected abstract MergeSettings DefaultMergeKeyFields();
        protected abstract MergeSettings DefaultMergeColExclude();
        protected abstract MergeSettings DefaultMergeRowExclude();

        public virtual String MergeIVDefinitionTypeName
        {
            get { return _config.Get<string>(TextRes.DefaultSettingsNames.KMergeDefinitionName); }
            set { _config.Set(TextRes.DefaultSettingsNames.KMergeDefinitionName, value); }
        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public MergeSettings MergeKeyFields
        {
            get { return MergeSettings.FromText(this, _config.Get<string>(TextRes.DefaultSettingsNames.KMergeDefinitionKeyFields)); }
            set { _config.Set(TextRes.DefaultSettingsNames.KMergeDefinitionKeyFields, MergeSettings.ToText(value)); }
        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public MergeSettings MergeColExclusions
        {
            get { return MergeSettings.FromText(this, _config.Get<string>(TextRes.DefaultSettingsNames.KMergeDefinitionColExclude)); }
            set { _config.Set(TextRes.DefaultSettingsNames.KMergeDefinitionColExclude, MergeSettings.ToText(value)); }
        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public MergeSettings MergeRowExclusions
        {
            get { return MergeSettings.FromText(this, _config.Get<string>(TextRes.DefaultSettingsNames.KMergeDefinitionRowExclude)); }
            set { _config.Set(TextRes.DefaultSettingsNames.KMergeDefinitionRowExclude, MergeSettings.ToText(value)); }
        }
        #endregion

        public IQRCode QRCode { get { return _qrCode; } }
        public ISiteReaderConfiguration Config { get { return _config; } }

        public abstract bool ApplyGlobalSettings(IWebSiteRepository repository);
    }
}
