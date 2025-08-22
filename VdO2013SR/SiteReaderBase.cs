using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MPUtils;
using MPExtensionMethods;

using VdO2013Core;
using VdO2013Core.Config;
using VdO2013QRCode;
using VdO2013SRCore;
using VdO2013SRCore.Specialized;

namespace VdO2013SR
{
    [Serializable]
    public abstract class SiteReaderBase : MPUtils.PluginFactory.PluginBase, MPUtils.PluginFactory.IPlugin, ISiteReader2, ISiteReader3
    {
        protected const ImageFormatClass DEFAULTIMAGEFORMAT = ImageFormatClass.Png;
        protected readonly MPLogHelper.FileLog Logger;

        private QRCodeHelper _qrCode;
        private SiteReaderConfiguration _config;

        [NonSerialized]
        private NameObjectCollection _pageItems;

        protected string NULLSTRING = null;
        protected int NULLINT = -1;
        protected bool NULLBOOL = false;
        protected DateTime NULLDATE = DateTime.MinValue;

        protected MPUtils.PluginFactory.IElementPropertiesStorage Properties { get { return GetPropertiesStorage(); } }
        protected delegate int ReadListCBDelegate(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack cb);
        protected ReadListCBDelegate InvokeReadListCB;
        protected delegate int SaveListCBDelegate(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, SaveListCallBack cb);
        protected SaveListCBDelegate InvokeSaveListCB;
        protected virtual int GetMAXPAGEDEFAULT() => 25;

        public DateTime LoadTimeStamp { get => Properties.GetPropertyValue(DateTime.MinValue); private set => Properties.SetPropertyValue(value); }
        public DateTime SaveTimeStamp { get => Properties.GetPropertyValue(DateTime.MinValue); private set => Properties.SetPropertyValue(value); }

        public virtual string PropertiesFile { get => System.IO.Path.Combine(ConfigPath, this.ReaderName + TextRes.SettingFileExtension.Extension); }

        public SiteReaderBase()
        {
            Logger = new MPLogHelper.FileLog(this);
            _config = new SiteReaderConfiguration(this);
            _qrCode = new QRCodeHelper(this);
            _pageItems = new NameObjectCollection();

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

            Global.PathPrefixChangeSuccess += Global_PathPrefixChangeSuccess;
        }

        private void Global_PathPrefixChangeSuccess(Global.PathPrefixChangeSuccessEventArgs args)
        {
            _config.Set(TextRes.DefaultSettingsNames.KReportPath, DefaultReportPath, out Exception error);
            Logger.WriteError(error);
            _config.Set(TextRes.DefaultSettingsNames.KImagesPath, DefaultImagesPath, out error);
            Logger.WriteError(error);
            _config.Set(TextRes.DefaultSettingsNames.KDataPath, DefaultDataPath, out error);
            Logger.WriteError(error);

            Initialize(null);
        }

        public override void Initialize(string xmlFileName)
        {
            Logger.WriteMethod(System.Reflection.MethodBase.GetCurrentMethod(), xmlFileName);
            LoadProperties();
            if (_config == null) _config = new SiteReaderConfiguration(this);
            if (_qrCode == null) _qrCode = new QRCodeHelper(this);
            if (_pageItems == null) _pageItems = new NameObjectCollection();
            if (FeatureCount <= 0)
                LoadFeatures();
        }

        [Obsolete("Vecchia proprietà ereditata da ISiteReader2. Utilizzare Features o Options per recuperare i valori.")]
        public abstract object this[string property, string key] { get; }

        public IQRCode QRCode { get { return _qrCode; } }
        public ISiteReaderConfiguration Config { get { return _config; } }

        //protected bool ConfigLoad(out Exception error, bool reload = false) { return _config.Load(out error, reload); }
        //protected bool ConfigSave(out Exception error) { return _config.Save(out error); }

        /// <summary>
        /// Ritorna <see cref="Global.ConfigPath"/>
        /// </summary>
        /// <returns></returns>
        protected virtual string GetConfigPath() { return Global.ConfigPath; }
        /// <summary>
        /// Ritorna <see cref="Global.ImagesPath"/>
        /// </summary>
        /// <returns></returns>
        protected virtual string GetImagesPath() { return Global.ImagesPath; }
        /// <summary>
        /// Ritorna <see cref="Global.DataPath"/>
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDataPath() { return Global.DataPath; }
        /// <summary>
        /// Ritorna <see cref="Global.ReportsPath"/>
        /// </summary>
        /// <returns></returns>
        protected virtual string GetReportsPath() { return Global.ReportsPath; }

        /// <summary>
        /// Se non viene eseguito l'override di <see cref="GetConfigPath"/> ritorna <see cref="Global.ConfigPath"/>
        /// </summary>
        public string DefaultConfigPath { get { return GetConfigPath(); } }
        /// <summary>
        /// Se non viene eseguito l'override di <see cref="GetImagesPath"/> ritorna <see cref="Global.ImagesPath"/>
        /// </summary>
        public string DefaultImagesPath { get { return GetImagesPath(); } }
        /// <summary>
        /// Se non viene eseguito l'override di <see cref="GetDataPath"/> ritorna <see cref="Global.DataPath"/>
        /// </summary>
        public string DefaultDataPath { get { return GetDataPath(); } }
        /// <summary>
        /// Se non viene eseguito l'override di <see cref="GetReportsPath"/> ritorna <see cref="Global.ReportsPath"/>
        /// </summary>
        public string DefaultReportPath { get { return GetReportsPath(); } }

        protected internal abstract string GetReaderName();

        protected internal abstract string GetUrlNormal();
        protected internal abstract void SetUrlNormal(string value);

        protected internal abstract string GetUrlMobile();
        protected internal abstract void SetUrlMobile(string value);

        public abstract Type GetSiteReader2Type();
        public int MAXPAGEDEFAULT { get => GetMAXPAGEDEFAULT(); }

        protected internal abstract Image GetLogo();
        protected internal abstract Image GetFullLogo();

        protected internal abstract string GetPageListUrlDefault();
        protected internal abstract string GetPageListListXPath();

        protected internal abstract string GetPageListFeaturesXPath();
        protected internal abstract void SetPageListFeaturesXPath(string value);

        protected internal abstract bool GetPageListFeaturesEnabled();
        protected internal abstract void SetPageListFeaturesEnabled(bool value);

        protected internal abstract string GetMergeIVDefinitionTypeName();

        #region Merge
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public VdO2013Data.MergeSettings MergeDefinitionKeyFields
        {
            get { return VdO2013Data.MergeSettings.FromText(this, Properties.GetPropertyValue(NULLSTRING)); }
            set { Properties.SetPropertyValue(VdO2013Data.MergeSettings.ToText(value)); }
        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public VdO2013Data.MergeSettings MergeDefinitionColExclude
        {
            get { return VdO2013Data.MergeSettings.FromText(this, Properties.GetPropertyValue(NULLSTRING)); }
            set { Properties.SetPropertyValue(VdO2013Data.MergeSettings.ToText(value)); }
        }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public VdO2013Data.MergeSettings MergeDefinitionRowExclude
        {
            get { return VdO2013Data.MergeSettings.FromText(this, Properties.GetPropertyValue(NULLSTRING)); }
            set { Properties.SetPropertyValue(VdO2013Data.MergeSettings.ToText(value)); }
        }
        #endregion

        public abstract bool Initialized { get; }
        public bool Running { get; private set; }

        public string Name { get => GetName(); protected set => SetName(value); }
        public Guid Code => GetGuid();

        public string ReaderName { get => GetReaderName(); }
        public string ReaderVersion { get => Properties.GetPropertyValue(NULLSTRING); protected set => Properties.SetPropertyValue(value); }

        public string SettingsVersion { get => Properties.GetPropertyValue(NULLSTRING); protected set => Properties.SetPropertyValue(value); }
        public DateTime SettingsReleaseDate { get => Properties.GetPropertyValue(NULLDATE); protected set => Properties.SetPropertyValue(value); }
        public int MaxPageCount { get => Properties.GetPropertyValue(NULLINT); set => Properties.SetPropertyValue(value); }

        public string Title { get => Properties.GetPropertyValue(NULLSTRING); protected set => Properties.SetPropertyValue(value); }
        public Image Logo { get => GetLogo(); }
        public Image FullLogo { get => GetFullLogo(); }
        public string Agenzia { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }
        public string Numero { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }
        public string RagioneSociale { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }
        public string Tipologia { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }
        public int Elementi { get => Properties.GetPropertyValue(NULLINT); set => Properties.SetPropertyValue(value); }
        public int Pagine { get => Properties.GetPropertyValue(NULLINT); set => Properties.SetPropertyValue(value); }

        public string ConfigPath { get { var value = Global.ConfigPath; return value.IsValidFilePath() ? value : DefaultConfigPath; } }
        public string ReportPath { get { var value = Global.ReportsPath; return value.IsValidFilePath() ? value : DefaultReportPath; } }
        public string ImagesPath { get { var value = Global.ImagesPath; return value.IsValidFilePath() ? value : DefaultImagesPath; } }
        public string DataPath { get { var value = Global.DataPath; return value.IsValidFilePath() ? value : DefaultDataPath; } }

        public virtual ImageFormatClass ImageFormat { get { return DEFAULTIMAGEFORMAT; } }
        public NameObjectCollection PageItems { get { return _pageItems; } }

        #region Features Support
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
        protected bool FeatureExists(string key)
        {
            return Features != null && Features.HasKey(key);
        }
        protected FeatureElement FeatureGet(string key)
        {
            return Features[key];
        }
        protected bool FeatureSet(FeatureElement value, bool addIfMissing)
        {
            if (Features[value.Name] == null && addIfMissing)
                Features.Add(value);
            else
                Features[value.Name] = value;
            return true;
        }
        protected bool FeatureSet(FeatureWrapper value, bool addIfMissing)
        {
            var fe = CreateFeature(value);
            if (Features[value.Name] == null && addIfMissing)
                Features.Add(fe);
            else
                Features[value.Name] = fe;
            return true;
        }
        protected bool FeatureAdd(FeatureElement value)
        {
            if (Features.HasKey(value.Name))
                return FeatureSet(value, false);
            else
                FeatureSet(value, true);
            return true;
        }
        protected bool FeatureAdd(FeatureWrapper value, bool configSave = true)
        {
            if (Features == null || Features.HasKey(value.Name))
                return false;
            else
            {
                if (FeatureSet(CreateFeature(value), true))
                    if (configSave)
                        Save();
            }
            return true;
        }
        protected bool FeatureDel(string key)
        {
            if (Features.HasKey(key))
                Features.Remove(Features[key]); return true;
        }
        public FeatureElementCollection Features
        {
            get
            {
                var section = _config.GetFeaturesSection();
                var result = section?.Features;
                return result;
            }
        }
        public int FeatureCount => Features != null ? Features.Count : -1;
        //protected abstract IEnumerable<FeatureWrapper> GetDefaultFeatures();

        /// <summary>
        /// Richiede al reader le feature di default nel caso in cui non ve ne siano nel file
        /// di configurazione <see cref="LoadFeatures"/>.
        /// </summary>
        /// <param name="features"></param>
        /// <returns></returns>
        protected abstract int InternalGetDefaultReaderFeatures(out IEnumerable<FeatureWrapper> features);

        /// <summary>
        /// Questo metodo carica le feature dal file di configurazione. Se non vengono trovate feature al suo
        /// interno richiama <see cref="InternalGetDefaultReaderFeatures(out IEnumerable{FeatureWrapper})"/> per
        /// recuperare la feature di default dal reader.
        /// </summary>
        /// <returns></returns>
        public int LoadFeatures()
        {
            var mb = System.Reflection.MethodBase.GetCurrentMethod();
            if (Global.DebugLevel > 1) Logger.WriteMethod(mb);
            var result = 0;
            try
            {
                ConfigSettingsHelper.Load(this.ReaderName, out var configuration, out var error);
                if (error != null) throw error;
                var section = ConfigSettingsHelper.GetFeaturesSection(configuration);
                var fes = new List<FeatureElement>();
                result = ConfigSettingsHelper.GetFeatures(section, fes);

                if (fes.Count > 0)
                {
                    foreach (var ff in fes)
                        if (!this.FeatureExists(ff.Name))
                            this.FeatureAdd(ff);
                }
                else
                {
                    result = InternalGetDefaultReaderFeatures(out var features);
                    if (result < 0) return result;

                    if (features == null)
                    {
                        if (Global.DebugLevel > 1) Logger.WriteDebug("{0}.{1} is returning null.".FormatWith(mb.ReflectedType?.Name, mb.Name));
                    }
                    else if (!features.Any())
                    {
                        if (Global.DebugLevel > 1) Logger.WriteDebug("{0}.{1} is returning {2} elements.".FormatWith(mb.ReflectedType?.Name, mb.Name, features.Count()));
                    }

                    if (features != null && Features.Count == 0)
                    {
                        Logger.WriteWarn("{0}.{1} initializing default features (count={0}).".FormatWith(mb.ReflectedType?.Name, mb.Name, features.Count()));
                        foreach (var ff in features)
                            if (!this.FeatureExists(ff.Name))
                                this.FeatureAdd(ff);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                result = -1;
            }

            if (Global.DebugLevel > 1) Logger.WriteMethodResult(mb, result);
            return result;
        }

        /// <summary>
        /// Salva le feature nel file di configurazione.
        /// </summary>
        /// <returns></returns>
        public int SaveFeatures()
        {
            var mb = System.Reflection.MethodBase.GetCurrentMethod();
            if (Global.DebugLevel > 1) Logger.WriteMethod(mb);
            var result = -1;

            try
            {
                //InternalSaveFeaturesToFile(Features);
                Features.CurrentConfiguration.Save(System.Configuration.ConfigurationSaveMode.Modified);
                result = Features.Count;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
            if (Global.DebugLevel > 1) Logger.WriteMethodResult(mb, result);
            return result;
        }

        // protected abstract int AppendDefaultFeatures();
        //protected int AppendDefaultFeatures()
        //{
        //    int result = 0;
        //    var defaultFeatures = GetDefaultFeatures();

        //    if (defaultFeatures == null || defaultFeatures.Count() == 0)
        //    {
        //        Logger.WriteWarn("GetDefaultFeatures returned null or zero elements.");
        //        return - 1;
        //    }
        //    foreach (var ff in defaultFeatures)
        //        if (!this.FeatureExists(ff.Name))
        //            this.FeatureAdd(ff);

        //    result += this.Features.Count;
        //    return result;
        //}

        protected void DoReadListItemWriteFeatures(ISiteReaderOnLineCheckDataList info, BackgroundWorker worker, ReadListCallBack CB, Dictionary<string, string> storage, out Exception error)
        {
            error = null;
            Logger.WriteMethod(System.Reflection.MethodBase.GetCurrentMethod(), info, worker, CB, storage, error);

            foreach (FeatureElement fe in Features)
            {
                if (!string.IsNullOrEmpty(fe.XPath))
                {
                    string webItem = fe.Name;
                    string webPart = string.Empty;
                    string[] xPaths = fe.XPath.Split('|', StringSplitOptions.RemoveEmptyEntries);

                    foreach (string x in xPaths)
                    {
                        webPart = HtmlHelper.FindNode_InnerText(HtmlHelper.DocumentNode, fe.XPath, true, 0, "null", out error);
                        if (!string.IsNullOrEmpty(webPart))
                            break;
                    }// foreach (String x in xPaths)

                    if (error != null)
                    {
                        info.AddError(error);
                        InvokeCB(worker, info, CB);
                    }

                    if (!storage.ContainsKey(webItem))
                    {
                        storage.Add(webItem, webPart);
                        info.AddInfo(string.Format("--> Feature '{0}' added: (Value:{1})", webItem, webPart));
                    }
                    else
                    {
                        string oldValue = storage[webItem];
                        storage[webItem] = webPart;
                        info.AddInfo(string.Format("--> Feature '{0}' updated: (Value:{1}; NewValue:{2})", webItem, oldValue, webPart));
                    }
                    InvokeCB(worker, info, CB);
                }
            }
        }
        #endregion Features Support

        public string StandardSiteRootUrl { get => GetUrlNormal(); protected set => SetUrlNormal(value); }
        public string MobileSiteRootUrl { get => GetUrlMobile(); protected set => SetUrlMobile(value); }
        public string PageListUrlDefault { get => GetPageListUrlDefault(); }
        public string PageListListXPath { get => GetPageListListXPath(); }
        public string PageListFeaturesXPath { get => GetPageListFeaturesXPath(); set => SetPageListFeaturesXPath(value); }
        public bool PageListFeaturesEnabled { get => GetPageListFeaturesEnabled(); set => SetPageListFeaturesEnabled(value); }
        public string MergeIVDefinitionTypeName { get => GetMergeIVDefinitionTypeName(); }

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

        //protected string GetPropertiesFile()
        //{
        //    var config = Global.ConfigPath;
        //    //var name = this.GetType().FullName + ".vdosettings";
        //    var name = this.ReaderName + TextRes.SettingFileExtension.Extension;
        //    return System.IO.Path.Combine(config, name);
        //}

        protected virtual System.Xml.XmlWriterSettings GetPropertiesWriterSettings()
        {
            return new System.Xml.XmlWriterSettings()
            {
                Encoding = System.Text.Encoding.UTF8,
                Indent = true,
                IndentChars = "  ",
                NewLineChars = Environment.NewLine,
                NewLineOnAttributes = true,
                NewLineHandling = System.Xml.NewLineHandling.Entitize,
                WriteEndDocumentOnClose = true
            };
        }

        protected System.Xml.XmlWriter GetPropertiesWriter()
        {
            return System.Xml.XmlWriter.Create(PropertiesFile, GetPropertiesWriterSettings());
        }

        protected virtual System.Xml.XmlReaderSettings GetPropertiesReaderSettings()
        {
            return new System.Xml.XmlReaderSettings()
            {
                IgnoreWhitespace = true,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.AllowXmlAttributes,
            };
        }

        protected System.Xml.XmlReader GetPropertiesReader()
        {
            return System.Xml.XmlReader.Create(PropertiesFile, GetPropertiesReaderSettings());
        }

        protected int LoadProperties()
        {
            if (System.IO.File.Exists(PropertiesFile))
            {
                using (var reader = GetPropertiesReader())
                {
                    Properties.ReadXml(reader);
                }
            }
            else
                return 0;
            return Properties.Count;
        }

        protected int SaveProperties()
        {
            using (var writer = GetPropertiesWriter())
            {
                Properties.WriteXml(writer);
            }
            return Properties.Count;
        }

        #region IOResult
        [Flags]
        protected enum IOResult : int
        {
            None = 0,
            Config = 1,
            Properties = 2,
            Features = 4,
            All = Config + Properties + Features,
        }

        protected class IOResultInfo
        {
            public const int All_KO = -1;
            public const int All_OK = 0;
            public const int Partial = 1;

            private Dictionary<IOResult, int> _Infos;
            private DateTime _TimeStamp;

            public IDictionary<IOResult, int> Infos { get { return _Infos; } }
            public DateTime TimeStamp { get { return _TimeStamp; } }

            public IOResultInfo()
            {
                _TimeStamp = DateTime.Now;
                _Infos = new Dictionary<SiteReaderBase.IOResult, int>() { { IOResult.Config, -1 }, { IOResult.Properties, -1 }, { IOResult.Features, -1 } };
            }

            public int Config { get => _Infos[IOResult.Config]; set => _Infos[IOResult.Config] = value; }
            public int Properties { get => _Infos[IOResult.Properties]; set => _Infos[IOResult.Properties] = value; }
            public int Features { get => _Infos[IOResult.Features]; set => _Infos[IOResult.Features] = value; }

            public int Sum() => _Infos.Values.Where(v => v > 0).Sum();
            public int Min() => _Infos.Values.Min();
            public int Max() => _Infos.Values.Max();

            public IOResult Success() => (from kv in _Infos where kv.Value >= 0 select kv.Key).Aggregate((v1, v2) => { return v1 |= v2; });
            public IOResult Failed() => (from kv in _Infos where kv.Value < 0 select kv.Key).Aggregate((v1, v2) => { return v1 |= v2; });

            public int ToResult() => (Failed() == IOResult.All ? All_KO : (Success() == IOResult.All ? All_OK : Partial));
        }
        #endregion IOResult

        protected virtual IOResult InternalReload(out Exception error)
        {
            var mb = System.Reflection.MethodBase.GetCurrentMethod();

            if (Global.DebugLevel > 1) Logger.WriteMethod(mb);
            var result = new IOResultInfo();
            error = null;

            try
            {
                result.Config = _config.Load(out error, true) ? 1 : -1;
                if (error != null) throw error;
                result.Properties = LoadProperties();
                result.Features = LoadFeatures();

                LoadTimeStamp = result.TimeStamp;
            }
            catch (Exception ex)
            {
                error = ex;
            }// try

            if (Global.DebugLevel > 1) Logger.WriteMethodResult(mb, result.Success());
            return result.Success();
        }
        public int Reload(out Exception error)
        {
            return (int)InternalReload(out error);
        }
        public int Reload()
        {
            return Reload(out Exception error);
        }

        protected virtual IOResult InternalSave(out Exception error)
        {
            var mb = System.Reflection.MethodBase.GetCurrentMethod();

            if (Global.DebugLevel > 1) Logger.WriteMethod(mb);
            var result = new IOResultInfo();
            error = null;

            try
            {
                result.Config = _config.Save(out error) ? 1 : -1;
                if (error != null) throw error;
                result.Properties = SaveProperties();
                result.Features = SaveFeatures();

                SaveTimeStamp = result.TimeStamp;
            }
            catch (Exception ex)
            {
                error = ex;
            }// try

            if (Global.DebugLevel > 1) Logger.WriteMethodResult(mb, result.Success());
            return result.Success();
        }
        public virtual int Save(out Exception error)
        {
            return (int)InternalSave(out error);
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

        protected abstract int InternalReadList(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack CB);
        protected abstract int InternalSaveList(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, SaveListCallBack CB);

        public int ReadList(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack CB)
        {
            if (Running) return -1;
            try
            {
                Running = true;
                return InternalReadList(worker, info, CB);
            }
            finally
            {
                Running = false;
            }
        }
        public int SaveList(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, SaveListCallBack CB)
        {
            if (Running) return -1;
            try
            {
                Running = true;
                return InternalSaveList(worker, info, CB);
            }
            finally
            {
                Running = false;
            }
        }

        public abstract bool ApplyGlobalSettings(IWebSiteRepository repository);

        public void LogProperties()
        {
            Logger.WriteDebug("{0}.Properties[{1}]", this.Name, this.Properties.Count);
            foreach (var p in this.Properties.AllKeys().OrderBy(p => p))
            {
                Logger.WriteDebug(">>Property[{0}]={1}", p, this.Properties.GetPropertyValue(null, p));
            }
        }

        public void LogConfigurations()
        {
            Logger.WriteDebug("{0}.Configurations[{1}]", this.Name, this.Config.Count);
            foreach (var c in this.Config)
            {
                Logger.WriteDebug(">>Configuration[{0}]={1}", c.Key, c.Value);
            }
        }

        public void LogFeatures()
        {
            Logger.WriteDebug("{0}.Features[{1}]", this.Name, this.Features.Count);
            foreach (FeatureElement f in this.Features)
            {
                Logger.WriteDebug(">>Feature[{0}]={1}", f.Name, f.XPath);
            }
        }

        protected void LogAll()
        {
            LogProperties();
            LogConfigurations();
            LogFeatures();
        }
    }
}
