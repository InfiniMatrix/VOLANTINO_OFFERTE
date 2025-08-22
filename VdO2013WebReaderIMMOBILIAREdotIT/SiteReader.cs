using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPUtils.PluginFactory;
using VdO2013Core;
using VdO2013SR;
using VdO2013SRCore;
using VdO2013SRCore.Specialized;

using MPExtensionMethods;
using VdO2013Core.Config;
using VdO2013Data;
using VdO2013WebReaderIMMOBILIAREdotIT;
using TextRes = MPCommonRes.TextRes;

namespace VdO2013WebReaderIMMOBILIAREdotIT
{
  [Serializable]
  public class SiteReader : SiteReaderBase, ISiteReader3
  {
    public SiteReader() : base()
    {
      Logger.WriteCtor(this.GetType());
      StandardSiteRootUrl = ReaderSettingsDefaults.KURLNORMALVALUE;
      MobileSiteRootUrl = ReaderSettingsDefaults.KURLMOBILEVALUE;
    }

    private int _initialized = 0;
#pragma warning disable CS0672 // Member overrides obsolete member
    public override object this[string property, string key] => throw new NotImplementedException();
#pragma warning restore CS0672 // Member overrides obsolete member

    public override bool Initialized => _initialized > 0;

    public override IElementCollection Scripts => GetDefaultScripts();
    public override IElementCollection Options => GetDefaultOptions();

    //public string UrlText { get => Config.Get<string>(ReaderSettings.KPAGELIST_URLNAME); set => Config.Set(ReaderSettings.KPAGELIST_URLNAME, value); }
    //public string UrlRedirectsText { get => Config.Get<string>(ReaderSettings.KPAGELIST_URLREDIRECTSNAME); set => Config.Set(ReaderSettings.KPAGELIST_URLREDIRECTSNAME, value); }
    //public int MaxCount { get => Config.Get<int>(ReaderSettings.KPAGELIST_MAXCOUNTNAME); set => Config.Set(ReaderSettings.KPAGELIST_MAXCOUNTNAME, value); }

    //public string RagioneSocialeXPath { get => Config.Get<string>(ReaderSettings.KPAGELIST_RAGIONESOCIALEXPATHNAME); }
    //public string ItemCountXPath { get => Config.Get<string>(ReaderSettings.KPAGELIST_ITEMCOUNTXPATHNAME); set => Config.Set(ReaderSettings.KPAGELIST_ITEMCOUNTXPATHNAME, value); }

    //public string ListXPath { get => Config.Get<string>(ReaderSettings.KPAGELIST_LISTXPATHNAME); set => Config.Set(ReaderSettings.KPAGELIST_LISTXPATHNAME, value); }
    //public bool FeaturesEnabled { get => Config.Get<bool>(ReaderSettings.KPAGELIST_FEATURESENABLEDNAME); set => Config.Set(ReaderSettings.KPAGELIST_FEATURESENABLEDNAME, value); }
    //public string FeaturesXPath { get => Config.Get<string>(ReaderSettings.KPAGELIST_FEATURESXPATHNAME); set => Config.Set(ReaderSettings.KPAGELIST_FEATURESXPATHNAME, value); }
    //public int MaxImageCount { get => Config.Get<int>(ReaderSettings.KPAGELIST_IMAGEMAXCOUNTNAME); set => Config.Set(ReaderSettings.KPAGELIST_IMAGEMAXCOUNTNAME, value); }
    //public string ImageFormatName { get => Config.Get<string>(ReaderSettings.KPAGELIST_IMAGEFORMATNAME); set => Config.Set(ReaderSettings.KPAGELIST_IMAGEFORMATNAME, value); }
    //public string ImageXPath {  get => Config.Get<string>(ReaderSettings.KPAGELIST_IMAGEXPATHNAME); set => Config.Set(ReaderSettings.KPAGELIST_IMAGEXPATHNAME, value); }
    //public string ImageActiveXPath { get => Config.Get<string>(ReaderSettings.KPAGELIST_IMAGETHUMBACTIVEXPATHNAME); set => Config.Set(ReaderSettings.KPAGELIST_IMAGETHUMBACTIVEXPATHNAME, value); }
    //public string ImageThumbsXPath { get => Config.Get<string>(ReaderSettings.KPAGELIST_IMAGETHUMBLISTXPATHNAME); set => Config.Set(ReaderSettings.KPAGELIST_IMAGETHUMBLISTXPATHNAME, value); }

    //public string MergeDefinitionName { get => Config.Get<string>(VdO2013SR.TextRes.DefaultSettingsNames.KMergeDefinitionName); set => Config.Set(VdO2013SR.TextRes.DefaultSettingsNames.KMergeDefinitionName, value); }
    //public string MergeDefinitionKeyFields { get => Config.Get<string>(VdO2013SR.TextRes.DefaultSettingsNames.KMergeDefinitionKeyFields); set => Config.Set(VdO2013SR.TextRes.DefaultSettingsNames.KMergeDefinitionKeyFields, value); }
    //public string MergeDefinitionColExclude { get => Config.Get<string>(VdO2013SR.TextRes.DefaultSettingsNames.KMergeDefinitionColExclude); set => Config.Set(VdO2013SR.TextRes.DefaultSettingsNames.KMergeDefinitionColExclude, value); }
    //public string MergeDefinitionRowExclude { get => Config.Get<string>(VdO2013SR.TextRes.DefaultSettingsNames.KMergeDefinitionRowExclude); set => Config.Set(VdO2013SR.TextRes.DefaultSettingsNames.KMergeDefinitionRowExclude, value); }

    //public string RagioneSocialeXPath { get => Config.Get<string>(ReaderSettings.KPAGELIST_RAGIONESOCIALEXPATHNAME); set => Config.Set(ReaderSettings.KPAGELIST_RAGIONESOCIALEXPATHNAME, value); }
    //public string ItemCountXPath { get => Config.Get<string>(ReaderSettings.KPAGELIST_ITEMCOUNTXPATHNAME); set => Config.Set(ReaderSettings.KPAGELIST_ITEMCOUNTXPATHNAME, value); }

    public string RagioneSocialeXPath { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }
    public string ItemCountXPath { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }

    //public bool FeaturesEnabled { get => Config.Get<bool>(ReaderSettingsNames.KPAGELIST_FEATURESENABLEDNAME); set => Config.Set(ReaderSettingsNames.KPAGELIST_FEATURESENABLEDNAME, value); }
    public int MaxItemCount { get => Config.Get<int>(ReaderSettingsNames.KPAGELIST_MAXCOUNTNAME); set => Config.Set(ReaderSettingsNames.KPAGELIST_MAXCOUNTNAME, value); }
    public int MaxImageCount { get => Config.Get<int>(ReaderSettingsNames.KPAGELIST_IMAGEMAXCOUNTNAME); set => Config.Set(ReaderSettingsNames.KPAGELIST_IMAGEMAXCOUNTNAME, value); }

    public string UrlText { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }
    public string UrlRedirectsText { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }

    public string ListXPath { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }

    protected override string GetUrlNormal() => Properties.GetPropertyValue(NULLSTRING, VdO2013SR.TextRes.DefaultSettingsNames.KNormalSiteUrl);
    protected override void SetUrlNormal(string value) => Properties.SetPropertyValue(value, VdO2013SR.TextRes.DefaultSettingsNames.KNormalSiteUrl);

    protected override string GetUrlMobile() => Properties.GetPropertyValue(NULLSTRING, VdO2013SR.TextRes.DefaultSettingsNames.KMobileSiteUrl);
    protected override void SetUrlMobile(string value) => Properties.SetPropertyValue(value, VdO2013SR.TextRes.DefaultSettingsNames.KMobileSiteUrl);

    protected override string GetPageListFeaturesXPath() => Properties.GetPropertyValue(NULLSTRING, VdO2013SR.TextRes.DefaultSettingsNames.KFeaturesXPath);
    protected override void SetPageListFeaturesXPath(string value) => Properties.SetPropertyValue(value, VdO2013SR.TextRes.DefaultSettingsNames.KFeaturesXPath);

    protected override bool GetPageListFeaturesEnabled() => Properties.GetPropertyValue(true, VdO2013SR.TextRes.DefaultSettingsNames.KFeaturesEnabled);
    protected override void SetPageListFeaturesEnabled(bool value) => Properties.SetPropertyValue(value, VdO2013SR.TextRes.DefaultSettingsNames.KFeaturesEnabled);

    public string ImageFormatName { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }
    public string ImageXPath { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }
    public string ImageActiveXPath { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }
    public string ImageThumbsXPath { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }

    public string MergeDefinitionName { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }
    //public string MergeDefinitionKeyFields { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }
    //public string MergeDefinitionColExclude { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }
    //public string MergeDefinitionRowExclude { get => Properties.GetPropertyValue(NULLSTRING); set => Properties.SetPropertyValue(value); }

    public override bool ApplyGlobalSettings(IWebSiteRepository repository)
    {
      try
      {
        this.Config.Upsert(VdO2013SR.TextRes.DefaultSettingsNames.KNAME, repository.Name);
        this.Config.Upsert(VdO2013SR.TextRes.DefaultSettingsNames.KCODE, repository.Guid.ToString());
        this.Config.Upsert(VdO2013SR.TextRes.DefaultSettingsNames.KNormalSiteUrl, repository.NormalUrl);
        this.Config.Upsert(VdO2013SR.TextRes.DefaultSettingsNames.KMobileSiteUrl, repository.MobileUrl);
        this.Config.Upsert(VdO2013SR.TextRes.DefaultSettingsNames.KRagioneSociale, repository.PageListRagioneSocialeXPath);

        StandardSiteRootUrl = repository.NormalUrl;
        MobileSiteRootUrl = repository.MobileUrl;

        SettingsVersion = repository.Version;
        SettingsReleaseDate = repository.ReleaseDate;
        MaxPageCount = repository.MaxPageCount;

        UrlText = repository.PageListUrl;
        UrlRedirectsText = repository.PageListUrlRedirects;
        MaxItemCount = repository.PageListMaxCount;

        ItemCountXPath = repository.PageListItemCountXPath;
        RagioneSocialeXPath = repository.PageListRagioneSocialeXPath;
        ListXPath = repository.PageListListXPath;
        //repository.PageListPageCountXPath;
        //repository.PageListPageNumberXPath;
        PageListFeaturesEnabled = repository.PageListFeaturesEnabled;
        PageListFeaturesXPath = repository.PageListFeaturesXPath;
        MaxImageCount = repository.PageListMaxImageCount;
        ImageFormatName = repository.PageListImageFormat;
        ImageXPath = repository.PageListImageXPath;
        //this.Config.Set(ReaderSettings.KPAGELIST_IMAGETHUMBLISTXPATHNAME, repository.PageListImageThumbListXPath);
        ImageActiveXPath = repository.PageListImageThumbActiveXPath;
        ImageThumbsXPath = repository.PageListImageThumbNormalXPath;

        this.QRCode.Encoding = (QREncoding)repository.QRCodeEncoding;
        this.QRCode.Scale = repository.QRCodeScale;
        this.QRCode.Version = (QRVersion)repository.QRCodeVersion;
        this.QRCode.Correction = (QRErrorCorrection)repository.QRCodeCorrection;
        this.QRCode.CharacterSet = repository.QRCodeCharacterSet;
        this.QRCode.BackColor = Color.FromName(repository.QRCodeBackColor);
        this.QRCode.ForeColor = Color.FromName(repository.QRCodeForeColor);

        MergeDefinitionName = repository.MergeDefinitionName;
        MergeDefinitionKeyFields = VdO2013Data.MergeSettings.FromText(this, repository.MergeDefinitionKeyFields);
        MergeDefinitionColExclude = VdO2013Data.MergeSettings.FromText(this, repository.MergeDefinitionColExclude);
        MergeDefinitionRowExclude = VdO2013Data.MergeSettings.FromText(this, repository.MergeDefinitionRowExclude);

        foreach (var fromFeature in repository.Features)
        {
          var toFeature = this.Features.HasKey(fromFeature.Name) ? this.Features[fromFeature.Name] : new FeatureElement();
          toFeature.XPath = fromFeature.XPath;
          toFeature.Mapping = fromFeature.Caption;
          if (!this.Features.HasKey(fromFeature.Name))
          {
            toFeature.Name = fromFeature.Name;
            this.FeatureAdd(toFeature);
          }
        }

        Save();
#if DEBUG
        LogAll();
#endif

        return true;
      }
      catch (Exception ex)
      {
        Logger.WriteError(ex);
        return false;
      }
    }

    //protected override IEnumerable<FeatureWrapper> GetDefaultFeatures()
    //{
    //    if (Global.DebugLevel > 1) Logger.WriteMethod(System.Reflection.MethodBase.GetCurrentMethod());
    //    var result = new ReaderFeatures();
    //    if (Global.DebugLevel > 1) Logger.WriteMethodResult(System.Reflection.MethodBase.GetCurrentMethod(), result);
    //    return result;
    //}

    protected override int InternalGetDefaultReaderFeatures(out IEnumerable<FeatureWrapper> features)
    {
      var mb = System.Reflection.MethodBase.GetCurrentMethod();
      if (Global.DebugLevel > 1) Logger.WriteMethod(mb);
      int result = 0;
      features = new ReaderFeatures();
      result += features.Count();
      if (Global.DebugLevel > 1) Logger.WriteMethodResult(mb, result);
      return result;
    }

    //protected override int AppendDefaultFeatures()
    //{
    //    var mb = System.Reflection.MethodBase.GetCurrentMethod();
    //    if (Global.DebugLevel > 1) Logger.WriteMethod(mb);
    //    int result = 0;
    //    var defaultFeatures = new ReaderFeatures();
    //    if (defaultFeatures == null)
    //    {
    //        Logger.WriteDebug("{0}.{1} is returning null.".FormatWith(mb.ReflectedType.Name, mb.Name));
    //    }
    //    else if (defaultFeatures.Count == 0)
    //    {
    //        Logger.WriteDebug("{0}.{1} is returning {2} elements.".FormatWith(mb.ReflectedType.Name, mb.Name, defaultFeatures.Count));
    //    }

    //    foreach (var ff in defaultFeatures)
    //        if (!this.FeatureExists(ff.Name))
    //            this.FeatureAdd(ff);

    //    result += this.Features.Count;
    //    if (Global.DebugLevel > 1) Logger.WriteMethodResult(mb, result);
    //    return result;
    //}

    public override void Initialize(string xmlFileName)
    {
      base.Initialize(xmlFileName);
      if (FeatureCount > 0) _initialized++;
    }

    #region WebImageAddressParts
    /* L'indirizzo dell'immagine dovrebbe essere di questo formato...
    ** http://media.immobiliare.it/image/Appartamento_vendita_Roma_foto1_thumb_331059211.jpg
    ** Scomponendolo tramite fImageUrl.Split('/')
    ** ottengo
    **
    ** ID   Testo                                                   Descrizione
    **
    ** 00   "http:"                                                 url_part
    ** 01   ""                                                      url_part
    ** 02   "media.immobiliare.it"                                  url_part
    ** 03   "image"                                                 url_part
    ** 04   "Appartamento_vendita_Roma_foto1_thumb_331059211.jpg"   {nomefile}
    **  00   "Appartamento_vendita_Roma"                            {annuncio}
    **  01   "foto1"                                                {indice-immagine}
    **  02   "thumb"                                                {formato}
    **  03   "331059211.jpg"                                        {nome-file}
    */
    private static class WebImageAddressParts
    {
      public const int root = 0;
      public const int filler = 1;
      public const int url = 2;
      public const int url2 = 3;
      public const int filename = 4;

      public static class WebImageFileNameAddressParts
      {
        public const int annuncio = -1;
        public const int indice = 2;
        public const int formato = 1;
        public const int nomefile = 0;
      }
    }
    #endregion WebImageAddressParts

    #region ReadList
    private bool DoReadListInitReader(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack CB)
    {
      var mi = System.Reflection.MethodBase.GetCurrentMethod();
      Logger.WriteMethod(mi, worker, info, CB);
      bool exit(bool _result)
      {
        Logger.WriteMethodResult(mi, _result, worker, info, CB);
        return _result;
      }

      Exception error = null;
      try
      {
        this.Tipologia = VdO2013RS.TextRes.Reader.ReadLista.TIPOLOGIATUTTE; //FindNode(HtmlDocument.DocumentNode, fXPathPaginaLista_TipologiaProposta, true, out error)[0].InnerText;
        var xRagioneSociale = RagioneSocialeXPath;
        var nRagioneSociale = HtmlHelper.FindNode(HtmlHelper.Document.DocumentNode, xRagioneSociale, true, out error);
        if (nRagioneSociale == null || nRagioneSociale.Count() == 0)
        {
          var m = "Elemento {0}(xpath: {1}) non trovato.".FormatWith(ReaderSettingsNames.KPAGELIST_RAGIONESOCIALEXPATHNAME, xRagioneSociale);
          info.AddError(m);
          InvokeCB(worker, info, CB);
          throw new InvalidOperationException(m);
        }
        this.RagioneSociale = nRagioneSociale.First().InnerText;

        info.AddInfo(info.LastItem.Percentage + 0.1, string.Format(VdO2013RS.TextRes.Reader.ReadLista.PROPOSTECOUNTFORMAT, this.Elementi));
        InvokeCB(worker, info, CB);
        info.AddError(error);
        InvokeCB(worker, info, CB);
        return exit(true);
      }
      catch (Exception ex)
      {
        info.AddInfo(info.LastItem.Percentage + 0.1, string.Format(VdO2013RS.TextRes.Reader.ReadLista.PROPOSTECOUNTFORMAT, this.Elementi), info.LastItem.Step);
        info.AddError(error);
        info.AddError(ex);
        InvokeCB(worker, info, CB);
      }

      return exit(false);
    }

    private int DoReadListGetTotalCount(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack CB, out string[] activeListPages)
    {
      activeListPages = null;
      Exception error = null;
      var mi = System.Reflection.MethodBase.GetCurrentMethod();
      Logger.WriteMethod(mi, worker, info, CB, activeListPages);
      int exit(int _result)
      {
        Logger.WriteMethodResult(mi, _result, worker, info, CB);
        return _result;
      }

      if (!Initialized)
      {
        info.AddError("{0}: {1} NOT INITIALIZED!".FormatWith(mi.Name, this));
        return exit(-1);
      }

      var _activePages = new List<string>();

      info.AddInfo("Conteggio");
      InvokeCB(worker, info, CB);

      if (this.Agenzia.Trim().Length <= 0)
        return exit(-2);
      this.Pagine = 0;
      this.Elementi = 0;

      try
      {
        var loadedPages = new Dictionary<Uri, System.Net.HttpStatusCode>();

        var maxPageCount = MaxPageCount <= 0 ? info.Reader.MAXPAGEDEFAULT : MaxPageCount;
        // Posso avere più pagine lista, ogni url di pagina è separata da KPageListUrlDefault
        var urlListItems = UrlText.Split(ReaderSettingsConsts.KPageListUrlSeparator, StringSplitOptions.RemoveEmptyEntries);
        for (var urlListIndex = 0; urlListIndex < urlListItems.Length; urlListIndex++)
        {
          // Scorro le pagine della lista
          for (var pageIndex = 1; pageIndex < maxPageCount; pageIndex++)
          {
            var fPaginaListaUrlString = (this.StandardSiteRootUrl + urlListItems[urlListIndex])
                .Replace(VdO2013SRCore.TextRes.WebSite.KTAGCODICEAGENZIA, this.Agenzia)
                .Replace(VdO2013SRCore.TextRes.WebSite.KTAGNUMEROAGENZIA, this.Numero)
                .Replace(VdO2013SRCore.TextRes.WebSite.KTAGINDICELISTA, pageIndex.ToString())
                .Replace("?pag=1", ""); //test: tolgo prima pagina ed evito redirect

            var fPaginaListaUrl = new Uri(fPaginaListaUrlString.ToLower());
            
            if (loadedPages.Any(k => k.Key.Equals(fPaginaListaUrl) && k.Value.Equals(System.Net.HttpStatusCode.OK)))
              continue;

            info.AddInfo(string.Format(VdO2013RS.TextRes.Reader.ReadLista.URLOPENING, fPaginaListaUrlString));
            InvokeCB(worker, info, CB);

            // la prima pagina - soprattutto se ci sono pochi elementi -
            // subisce redirect verso la pagina principale
            var open = HtmlHelper.OpenUrl(fPaginaListaUrl, out error, pageIndex == 1 ? HtmlHelper.UriPart.Query : HtmlHelper.UriPart.None);

            if (loadedPages.ContainsKey(fPaginaListaUrl))
              loadedPages[fPaginaListaUrl] = open;
            else
              loadedPages.Add(fPaginaListaUrl, open);

            if (open != System.Net.HttpStatusCode.OK || error != null)
            {
              if (error is HttpRedirectNotAllowedException)
              {
                info.AddWarning(error.Message);
                InvokeCB(worker, info, CB);
                break; // Esco dal ciclo delle pagine
              }
              else
              {
                info.AddError(error ?? new Exception($"{nameof(HtmlHelper)}.{nameof(HtmlHelper.OpenUrl)} returned {open}."));
                InvokeCB(worker, info, CB);
                break; // Esco dal ciclo delle pagine
              }
            }

            if (Global.DebugLevel > 1)
              Logger.WriteInfo(HtmlHelper.Document != null ? HtmlHelper.Document.DocumentNode.OuterHtml : "<null>");
            var pageListXPath = ListXPath;
            var pageListItems = HtmlHelper.FindNode(HtmlHelper.Document.DocumentNode, pageListXPath, true, out error);
            info.AddError(error);
            InvokeCB(worker, info, CB);

            if (Pagine == 0 && (pageListItems == null || pageListItems.Count() == 0))
            {
              info.AddWarning("Nessuna pagina trovata per {0}".FormatWith(pageListXPath));
              InvokeCB(worker, info, CB);
              break; // Esco dal ciclo delle pagine
            }

            _activePages.Add(fPaginaListaUrlString.ToLower());
            Pagine++;
            Elementi += pageListItems.Count();
          }//for (int pageListIndex = 1; pageListIndex < readerPageListMaxCount; pageListIndex++)

        }//for (int urlListIndex = 0; urlListIndex < urlListItems.Length; urlListIndex++)
      }
      catch (Exception ex)
      {
        info.AddError(ex);
        InvokeCB(worker, info, CB);
      }

      var m = "Numero pagine trovate: {0}".FormatWith(Pagine);
      if (Pagine > 0)
        info.AddInfo(m);
      else
        info.AddError(m);
      InvokeCB(worker, info, CB);

      m = "Numero elementi trovati: {0}".FormatWith(Elementi);
      if (Elementi > 0)
        info.AddInfo(m);
      else
        info.AddError(m);
      InvokeCB(worker, info, CB);

      activeListPages = _activePages.ToArray();

      Logger.WriteMethodResult(mi, Elementi, worker, info, CB, activeListPages);
      return exit(Elementi);
    }
    
    private Dictionary<string, string> DoReadListItem(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack CB, string aPaginaPropostaUrl)
    {
      var result = new Dictionary<string, string>();

      info.AddInfo("Start", VdO2013RS.TextRes.Reader.ReadLista.ITEMSTEP);
      var fPaginaPropostaUrlText = aPaginaPropostaUrl;

      Exception error = null;
      var xpath = string.Empty;
      IEnumerable<IHtmlNode> xnodes = null;
      try
      {
        var fPaginaPropostaUrl = new Uri(fPaginaPropostaUrlText);

        var canRedirect = false;
        var allowedRedirections = UrlRedirectsText.Split('|', StringSplitOptions.RemoveEmptyEntries);
        foreach (var r in allowedRedirections)
        {
          canRedirect = fPaginaPropostaUrlText.Contains(r);
          if (canRedirect) break;
        }

        var open = HtmlHelper.OpenUrl(fPaginaPropostaUrl, out error, canRedirect ? HtmlHelper.UriPart.Path | HtmlHelper.UriPart.Query : HtmlHelper.UriPart.None);
        if (open != System.Net.HttpStatusCode.OK || error != null)
        {
          info.AddError(error ?? new Exception($"{nameof(HtmlHelper)}.{nameof(HtmlHelper.OpenUrl)} returned {open}."));
          return result;
        }

        if (Global.DebugLevel > 1) Logger.WriteInfo(HtmlHelper.Document != null ? HtmlHelper.Document.DocumentNode.OuterHtml : "<null>");
        var fromNode = HtmlHelper.HasHtmlBodyNode ? HtmlHelper.BodyNode : HtmlHelper.HasHtmlNode ? HtmlHelper.HtmlNode : HtmlHelper.Document.DocumentNode;

        //
        // cerco di ricavare il codice dell'annuncio dalla Url di partenza
        //
        var annuncio = fPaginaPropostaUrlText.Replace(this.PageListUrlDefault /*this.Config.Get<string>(ReaderSettings.KPAGELIST_URLNAME)*/, string.Empty);
        if (annuncio.StartsWith(@"/")) annuncio = annuncio.Right(annuncio.Length - 1);

        if (annuncio.Contains('-')) annuncio = annuncio.Left(annuncio.IndexOf('-'));

        // Attenzione: a partire dall'ultima versione del sito la url che viene restituita contiene uno / finale
        // questo comporta che non è possibile prelevare l'ultima parte del in base alla posizione /
        //if (annuncio.Contains('/')) annuncio = annuncio.Right(annuncio.ReverseIndexOf('/'));
        // utilizzo quindi il metodo Split per prelevare l'ultima porzione
        if (annuncio.Contains('/'))
        {
          annuncio = annuncio.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
        }

        DoReadListItemWriteFeatures(info, worker, CB, result, out error);

        if (error == null && this.PageListFeaturesEnabled)
        {
          xpath = this.PageListFeaturesXPath;
          xnodes = HtmlHelper.FindNode(fromNode, xpath, true, out error);
          if (xnodes == null || xnodes.Count() == 0)
          {
            info.AddWarning("The xpath query for '{0}' returned zero nodes.".FormatWith(xpath));
            InvokeCB(worker, info, CB);
          }
          else
          {
            DoReadListItemFeatures(worker, info, CB, xnodes, result, annuncio);
          }
        }

        xpath = this.ImageXPath;
        var maxImageCount = MaxImageCount == 0 ? 10 : MaxImageCount;

        Func<IEnumerable<IHtmlNode>, IHtmlNode, string, int> searchImages = (_nodes, _root, _xpath) =>
        {
          const string _msg = "The xpath query on {0} for '{1}' returned {2} nodes.";
          if (_nodes == null)
          {
            info.AddWarning(_msg.FormatWith(_root, _xpath, "no"));
            InvokeCB(worker, info, CB);
            return 0;
          }

          if (_nodes.Count() == 0)
          {
            info.AddWarning(_msg.FormatWith(_root, _xpath, "zero"));
            InvokeCB(worker, info, CB);
            return 0;
          }

          info.AddInfo(_msg.FormatWith(_root, _xpath, _nodes.Count()));
          InvokeCB(worker, info, CB);
          return DoReadListItemImages(worker, info, CB, _nodes, result, annuncio);
        };

        var containers = HtmlHelper.FindNode(fromNode, xpath, true, out error);
        if (error != null)
        {
          info.AddError(error);
          InvokeCB(worker, info, CB);
        }
        else
        {
          if (searchImages(containers, fromNode, xpath) == 0)
          {
            xpath = this.ImageXPath;
            containers = containers.SelectMany(c => HtmlHelper.FindNode(c, xpath, true, out error)).ToList();
            searchImages(containers, fromNode, xpath);
          }
        }
        
        // Download videos if enabled
        if (Config.Get<bool>(ReaderSettings.KPAGELIST_VIDEOENABLEDNAME))
        {
          xpath = Config.Get<string>(ReaderSettings.KPAGELIST_VIDEOXPATHNAME);
          var videoNodes = HtmlHelper.FindNode(fromNode, xpath, true, out error);
          if (error != null)
          {
            info.AddError(error);
            InvokeCB(worker, info, CB);
          }
          
          if (videoNodes != null && videoNodes.Count() > 0)
          {
            info.AddInfo("Found {0} video(s) on page".FormatWith(videoNodes.Count()));
            DoReadListItemVideos(worker, info, CB, videoNodes, result, annuncio);
          }
        }
      }
      catch (Exception Ex)
      {
        error = new Exception(string.Format(VdO2013RS.TextRes.Reader.ReadLista.ITEMERRORFORMAT, fPaginaPropostaUrlText, xpath), Ex);
      }

      return result;
    }

    private int DoReadListItemFeatures(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack CB, IEnumerable<IHtmlNode> features, Dictionary<string, string> featureList, string annuncio)
    {
      int result = 0;
      if (features == null || features.Count() == 0)
      {
        info.AddInfo("Warning: NO FEATURES FOUND", "Reader:{0}, Worker:{1}, Item:{2}".FormatWith(this, worker, annuncio));
        return -1;
      }
#if true
      else
      {
        info.AddInfo("Information: {0} FEATURES FOUND".FormatWith(features.Count()), "Reader:{0}, Worker:{1}, Item:{2}".FormatWith(this, worker, annuncio));
      }
#endif
      var rows = features.First().ChildNodes;
      // Salto header e footer della tabella
      for (int iRow = 1; iRow < rows.Count() - 1; iRow++)
      {
        var cols = rows.ElementAt(iRow).ChildNodes;
        // 2016-08-28: precedentemente avevo una tabella da leggere
        if (cols.Count() > 1)
        {
          for (int iCol = 1; iCol < cols.Count() - 2; iCol += 4)
          {
            string title = "{" + HtmlHelper.HtmlDecode(cols.ElementAt(iCol).InnerText).ToLower().Replace(':', ' ').Trim().Replace(' ', '_') + "}";
            string value = HtmlHelper.HtmlDecode(cols.ElementAt(iCol + 2).InnerText).Trim();

            if (!string.IsNullOrEmpty(title))
            {
              if (!featureList.ContainsKey(title))
                featureList.Add(title, value);
              else
                  if (string.IsNullOrEmpty(featureList[title]))
                featureList[title] = value;
            }
          }
        }
        else // 2016-08-28: ora c'è una lista di righe: riga1=titolo, riga3=valore
        {
          if ((iRow - 1) % 4 == 0 && (iRow + 2 < rows.Count()))
          {
            string title = "{" + HtmlHelper.HtmlDecode(rows.ElementAt(iRow).InnerText).ToLower().Replace(':', ' ').Trim().Replace(' ', '_') + "}";
            string value = HtmlHelper.HtmlDecode(rows.ElementAt(iRow + 2).InnerText).Trim();

            if (!string.IsNullOrEmpty(title))
            {
              if (!featureList.ContainsKey(title))
                featureList.Add(title, value);
              else
                  if (string.IsNullOrEmpty(featureList[title]))
                featureList[title] = value;
            }
          }
        }

        info.AddInfo(info.LastItem.Percentage, string.Format(VdO2013RS.TextRes.Reader.ReadLista.ITEMFEATURESFORMAT, result));
        InvokeCB(worker, info, CB);
        result++;
      }

      return result;
    }

    private int DoReadListItemImages(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack CB, IEnumerable<IHtmlNode> images, Dictionary<string, string> imageList, string annuncio)
    {
      var result = 0;
      Exception error = null;
      if (images == null) return -1;

      var maxImageCount = MaxImageCount;
      if (maxImageCount == -1) maxImageCount = int.MaxValue;

      foreach (var n in images)
      {
        var fImageUrl = n.Attributes.Any(a => a.Key == TextRes.DomSearch.TagIMGAttrSRC) ? n.Attributes[VdO2013RS.TextRes.DomSearch.TagIMGAttrSRC] : string.Empty;
        if (string.IsNullOrEmpty(fImageUrl) && n.HasChildNodes && n.FirstChild.Attributes.Any(a => a.Key == TextRes.DomSearch.TagIMGAttrSRC))
          fImageUrl = n.FirstChild.Attributes[VdO2013RS.TextRes.DomSearch.TagIMGAttrSRC];
        if (string.IsNullOrEmpty(fImageUrl)) break;

        var fImageUrlParts = fImageUrl.Split('/');
        var fNomeFile = string.Empty;
        var fImageFileName = string.Empty;
        var is2018Onward = fImageUrl.EndsWith("m-c.jpg");

        if (!is2018Onward)
        {
          //Aggiusto il doppio slash
          fImageUrlParts[WebImageAddressParts.filler] = @"/";

          //Ricostruisco il percorso
          //for (var j = 0; j < fImageUrlParts.Length; j++)
          //{
          //    fImageUrl += (string.IsNullOrEmpty(fImageUrl) ? "" : @"/") + fImageUrlParts[j];
          //}
          fImageUrl = fImageUrlParts.Aggregate(string.Empty, (current, t) => current + ((string.IsNullOrEmpty(current) ? "" : @"/") + t));

          if (!fImageUrl.StartsWith("http"))
            continue;

          info.AddInfo(info.LastItem.Percentage, string.Format(VdO2013RS.TextRes.Reader.ReadLista.ITEMIMAGESDOWNLOAD, VdO2013SRCore.TextRes.tagImmagine, result));
          InvokeCB(worker, info, CB);

          var fAnnuncio = string.Empty;
          var fIndice = string.Empty;
          var fFormato = string.Empty;
          var fImageFileNameParts = fImageUrlParts[WebImageAddressParts.filename].Split('_');
          var i = 0;
          for (var j = fImageFileNameParts.Length - 1; j >= 0; j--)
          {
            switch (i)
            {
              case WebImageAddressParts.WebImageFileNameAddressParts.annuncio:
                //
                break;
              case WebImageAddressParts.WebImageFileNameAddressParts.indice:
                fIndice = fImageFileNameParts[j];
                break;
              case WebImageAddressParts.WebImageFileNameAddressParts.formato:
                fFormato = fImageFileNameParts[j];
                break;
              case WebImageAddressParts.WebImageFileNameAddressParts.nomefile:
                fNomeFile = fImageFileNameParts[j];
                break;
              default:
                fAnnuncio = fImageFileNameParts[j] + (fAnnuncio.Length > 0 ? "_" : string.Empty) +
                            fAnnuncio;
                break;
            }

            i++;
          }

          fImageUrl = $@"{fImageUrlParts[WebImageAddressParts.root]}//{fImageUrlParts[WebImageAddressParts.url]}/{fImageUrlParts[WebImageAddressParts.url2]}/";
          fImageUrl += $@"{fAnnuncio}_{fIndice}_{fNomeFile}";
          fImageFileName = System.IO.Path.Combine(info.Reader.ImagesPath, this.Agenzia, annuncio, fNomeFile);
        }
        else
        {
          fNomeFile = fImageUrlParts[4] + ".jpg";
          fImageFileName = System.IO.Path.Combine(info.Reader.ImagesPath, this.Agenzia, annuncio, fNomeFile);
        }

        var fImageDirectory = System.IO.Path.GetDirectoryName(fImageFileName);

        if (!string.IsNullOrEmpty(fImageDirectory) && !System.IO.Directory.Exists(fImageDirectory))
          System.IO.Directory.CreateDirectory(fImageDirectory);

        if (System.IO.File.Exists(fImageFileName))
          System.IO.File.Delete(fImageFileName);


        fImageUrl = fImageUrl.Replace(@"///", @"//").Replace(@"///", @"//");
        var fImageUri = new Uri(fImageUrl);

        if (!fImageUri.IsWellFormedOriginalString())
        {
          info.AddWarning(info.LastItem.Percentage, $"Url {fImageUrl} is not well formed.");
        }

        info.AddInfo(string.Format(VdO2013RS.TextRes.Reader.ReadLista.ITEMIMAGEDOWNLOADSTART, VdO2013SRCore.TextRes.tagImmagine, string.Empty, fImageFileName, fImageUrl));
        var fImageFileNameExtension = System.IO.Path.GetExtension(fImageFileName);
        var fImageFileClass = fImageFileNameExtension.GetImageClassFromFileExtension();

        info.AddInfo(string.Format(VdO2013RS.TextRes.Reader.ReadLista.ITEMIMAGEDOWNLOADSTART, VdO2013SRCore.TextRes.tagImmagine, result, fImageFileName, fImageUrl));
        if (fImageUrl.DownloadImage(fImageFileName, out error, fImageFileClass, true))
          info.AddInfo(info.LastItem.Percentage, $"{VdO2013SRCore.TextRes.tagImmagine} {result} {VdO2013RS.TextRes.Reader.ReadLista.ITEMIMAGESDOWNLOADOK}.");
        else
          info.AddWarning(info.LastItem.Percentage, $"{VdO2013SRCore.TextRes.tagImmagine} {result} {VdO2013RS.TextRes.Reader.ReadLista.ITEMIMAGESDOWNLOADKO}.");
        InvokeCB(worker, info, CB);

        info.AddInfo($"{VdO2013SRCore.TextRes.tagImmagine} {fImageUrlParts[WebImageAddressParts.WebImageFileNameAddressParts.indice]}.");
        info.AddError(error);
        InvokeCB(worker, info, CB);
        if (error != null) break;

        var sImageKey = $"{VdO2013SRCore.TextRes.tagImmagineB}{(result + 1).ToString().PadLeft(2, '0')}{VdO2013SRCore.TextRes.tagImmagineE}";
        if (!imageList.ContainsKey(sImageKey))
        {
          imageList.Add(sImageKey, fImageFileName);
        }
        else
        {
          info.AddInfo($"--> Image '{sImageKey}' already added: (Value:{imageList[sImageKey]}; NewValue:{fImageFileName})");
          InvokeCB(worker, info, CB);
        }
        result++;
        if (maxImageCount > 0 && result > maxImageCount) break;
      }

      return result;
    }
    
    private int DoReadListItemVideos(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack CB, IEnumerable<IHtmlNode> videos, Dictionary<string, string> videoList, string annuncio)
    {
      var result = 0;
      Exception error = null;
      if (videos == null || videos.Count() == 0) return -1;

      var maxVideoCount = Config.Get<int>(ReaderSettings.KPAGELIST_VIDEOMAXCOUNTNAME);
      if (maxVideoCount == -1) maxVideoCount = int.MaxValue;

      foreach (var n in videos)
      {
        string videoUrl = null;
        
        // Extract video URL based on element type
        if (n.NodeName.ToLower() == "source" && n.Attributes.Any(a => a.Key == "src"))
        {
          videoUrl = n.Attributes["src"];
        }
        else if (n.NodeName.ToLower() == "video" && n.Attributes.Any(a => a.Key == "src"))
        {
          videoUrl = n.Attributes["src"];
        }
        else if (n.NodeName.ToLower() == "iframe" && n.Attributes.Any(a => a.Key == "src"))
        {
          // For iframe videos (e.g., YouTube, Vimeo), we store the embed URL
          videoUrl = n.Attributes["src"];
        }
        
        if (string.IsNullOrEmpty(videoUrl))
          continue;
        
        // Ensure URL is absolute
        if (!videoUrl.StartsWith("http"))
        {
          var baseUrl = StandardSiteRootUrl;
          videoUrl = new Uri(new Uri(baseUrl), videoUrl).ToString();
        }

        info.AddInfo(info.LastItem.Percentage, string.Format(VdO2013RS.TextRes.Reader.ReadLista.ITEMIMAGESDOWNLOAD, "Video", result));
        InvokeCB(worker, info, CB);

        // For iframe videos, we just store the URL
        if (n.NodeName.ToLower() == "iframe")
        {
          var sVideoKey = string.Format("{0}{1}{2}", "{video", (result + 1).ToString().PadLeft(2, '0'), "}");
          if (!videoList.ContainsKey(sVideoKey))
          {
            videoList.Add(sVideoKey, videoUrl);
            info.AddInfo(string.Format("--> Video iframe URL '{0}' added: {1}", sVideoKey, videoUrl));
          }
        }
        else
        {
          // For direct video files, download them
          var videoExtension = videoUrl.GetVideoFileExtension();
          var videoFileName = System.IO.Path.Combine(ImagesPath, Agenzia, annuncio, "videos", $"video_{result + 1}{videoExtension}");
          var videoDirectory = System.IO.Path.GetDirectoryName(videoFileName);

          if (!System.IO.Directory.Exists(videoDirectory))
            System.IO.Directory.CreateDirectory(videoDirectory);

          if (System.IO.File.Exists(videoFileName))
            System.IO.File.Delete(videoFileName);

          if (videoUrl.DownloadVideo(videoFileName, out error, true))
            info.AddInfo(info.LastItem.Percentage, string.Format("Video {0} download OK.", result));
          else
            info.AddInfo(info.LastItem.Percentage, string.Format("Video {0} download FAILED.", result));
          
          InvokeCB(worker, info, CB);

          var sVideoKey = string.Format("{0}{1}{2}", "{video", (result + 1).ToString().PadLeft(2, '0'), "}");
          if (!videoList.ContainsKey(sVideoKey))
          {
            videoList.Add(sVideoKey, videoFileName);
          }
          else
          {
            info.AddInfo(string.Format("--> Video '{0}' already added: (Value:{1}; NewValue:{2})", sVideoKey, videoList[sVideoKey], videoFileName));
            InvokeCB(worker, info, CB);
          }
        }
        
        info.AddError(error);
        InvokeCB(worker, info, CB);
        if (error != null) break;
        
        result++;
        if (maxVideoCount > 0 && result >= maxVideoCount) break;
      }

      return result;
    }
    
    protected override int InternalReadList(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack CB)
    {
      var mi = System.Reflection.MethodBase.GetCurrentMethod();
      Logger.WriteMethod(mi, worker, info, CB);
      int exit(int _result)
      {
        Logger.WriteMethodResult(mi, _result, worker, info, CB);
        return _result;
      }

      Exception error = null;

      if (!Initialized)
      {
        info.AddError("{0}: {1} NOT INITIALIZED!".FormatWith(mi.Name, this));
        return exit(-1);
      }

      info.AddInfo("Start", VdO2013RS.TextRes.Reader.ReadLista.STEP);
      InvokeCB(worker, info, CB);

      // Se info.List contiene valori, allora leggo direttamente le offerte specificate dall'elenco nella lista
      // in questo modo è possibile (ri)leggere anche solo impostazioni di una o più offerte specifiche
      var elementZero = info.List != null && info.List.Any() ? info.List.First().Key.ToString() : string.Empty;
      if (info.List != null && info.List.Count > 0 && !elementZero.Equals(info.Reader.StandardSiteRootUrl) && !elementZero.Equals(info.Reader.StandardSiteRootUrl + @"/"))
      {
        double itemProgressPercentage = 33.0 / info.List.Count; //Metto 33 invece che 100 perché lo scaricamento è 1/3 del lavoro da fare

        info.AddInfo("Lettura lista statica ({0} elementi).".FormatWith(info.List.Count));
        InvokeCB(worker, info, CB);

        for (int i = 0; i < info.List.Count; i++)
        {
          info.AddInfo(info.LastItem.Percentage + itemProgressPercentage, string.Format(VdO2013RS.TextRes.Reader.ReadLista.PROPOSTAFORMAT, i));
          InvokeCB(worker, info, CB);

          var url = info.List.Keys.ElementAt(i);
          var sts = info.List[url];

          try
          {
            var redirText = @"Siamo spiacenti. La pagina che stai cercando non &egrave; presente sul nostro sito o non &egrave; pi&ugrave; disponibile.";

            var open = HtmlHelper.OpenUrl(url, out error, HtmlHelper.UriPart.None, redirText);
            if (open != System.Net.HttpStatusCode.OK || error != null)
            {
              if (error is HttpRedirectNotAllowedException)
              {
                info.AddWarning(error.Message);
                sts = VdO2013Core.OnLineStatus.Redirected;
                error = null;
              }
              else
              {
                info.AddError(error ?? new Exception($"{nameof(HtmlHelper)}.{nameof(HtmlHelper.OpenUrl)} returned {open}."));
                sts = VdO2013Core.OnLineStatus.Offline;
              }

              if (Global.DebugLevel > 1)
                Logger.WriteInfo(HtmlHelper.Document != null ? HtmlHelper.Document.DocumentNode.OuterHtml : "<null>");
              InvokeCB(worker, info, CB);
            }
            else
            {
              this.PageItems.Add(url.ToString(), DoReadListItem(worker, info, CB, url.ToString()));
              sts = VdO2013Core.OnLineStatus.OnLine;
            }

          }
          finally
          {
            info.List[url] = sts;
          }
        }//for (int i = 0; i < info.List.Count; i++)

        this.Elementi = info.List.Count;

        return exit(this.Elementi);
      }

      var activeItems = DoReadListGetTotalCount(worker, info, CB, out string[] activeListPages);
      if (this.Agenzia.Trim().Length <= 0)
        return exit(-2);

      try
      {
        var itemProgressPercentage = 33.0 / activeItems; //Metto 33 invece che 100 perché lo scaricamento è 1/3 del lavoro da fare

        // Scorro le pagine della lista
        for (var pageListIndex = 0; pageListIndex < activeListPages.Length; pageListIndex++)
        {
          var fPaginaListaUrl = new Uri(activeListPages[pageListIndex]);

          info.AddInfo(string.Format(VdO2013RS.TextRes.Reader.ReadLista.URLOPENING, activeListPages[pageListIndex]));
          InvokeCB(worker, info, CB);

          var open = HtmlHelper.OpenUrl(fPaginaListaUrl, out error, pageListIndex == 0 ? HtmlHelper.UriPart.Query : HtmlHelper.UriPart.None);
          if (open != System.Net.HttpStatusCode.OK || error != null)
          {
            if (error is HttpRedirectNotAllowedException)
            {
              info.AddWarning(error.Message);
              error = null;
            }
            else
            {
              info.AddError(error ?? new Exception($"{nameof(HtmlHelper)}.{nameof(HtmlHelper.OpenUrl)} returned {open}."));
            }

            if (Global.DebugLevel > 1)
              Logger.WriteInfo(HtmlHelper.Document != null ? HtmlHelper.Document.DocumentNode.OuterHtml : "<null>");
            
            InvokeCB(worker, info, CB);
            break; // Esco dal ciclo delle pagine
          }

          // Solo dalla prima pagina della prima url ricavo le informazioni del reader
          if (pageListIndex == 0)
            if (!DoReadListInitReader(worker, info, CB))
              break; // Esco dal ciclo delle pagine

          var search = ListXPath; // PageListListXPath;
          var pageListItems = HtmlHelper.FindNode(HtmlHelper.Document.DocumentNode, search, true, out error).ToArray();
          
          if (error != null)
            info.AddError(error);
          
          if (error is HttpRedirectNotAllowedException || pageListItems == null || pageListItems.Count() == 0)
            break; // Esco dal ciclo delle pagine

          // Scorro gli elementi della lista corrente
          for (int pageListItemIndex = 0; pageListItemIndex < pageListItems.Count(); pageListItemIndex++)
          {
            info.AddInfo(info.LastItem.Percentage + itemProgressPercentage, string.Format(VdO2013RS.TextRes.Reader.ReadLista.PROPOSTAFORMAT, pageListItemIndex));
            InvokeCB(worker, info, CB);

            var attrs = pageListItems.ElementAt(pageListItemIndex).Attributes;
            if (!attrs.ContainsKey(VdO2013RS.TextRes.DomSearch.TagAAttrHREF))
            {
              //TM20231105: se l'elemento non contiene un href come attributo allora versione aggiornata di immobiliare.
              //L'href si trova in un tag A interno a dei div dell'elemento
              var currItem = pageListItems.ElementAt(pageListItemIndex);
              var tagAItem = currItem.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1).ChildNodes.ElementAt(1); //Fa vomitare, da riscrivere
              attrs = tagAItem.Attributes;
              if (!attrs.ContainsKey(VdO2013RS.TextRes.DomSearch.TagAAttrHREF)) { 
                info.AddError("Element {0} does not contain {1} attribute.".FormatWith(pageListItemIndex, VdO2013RS.TextRes.DomSearch.TagAAttrHREF));
                continue;
              }
            }
            var fPaginaListaItemLink = attrs[VdO2013RS.TextRes.DomSearch.TagAAttrHREF].ToString();
            var fPaginaListaItems = DoReadListItem(worker, info, CB, fPaginaListaItemLink);
            this.PageItems.Add(fPaginaListaItemLink, fPaginaListaItems);
            
            //this.Elementi++;
          
          }//for (int pageListItemIndex = 0; ...

          if (this.Pagine <= 0 || pageListIndex >= this.Pagine)
            break; // Esco dal ciclo delle pagine
        }//for (int pageListIndex = 1; ...

        return exit(this.PageItems.Count);
      }
      catch (Exception ex)
      {
        info.AddError(ex);
        InvokeCB(worker, info, CB);
      }

      return exit(-3);
    }
    #endregion ReadList

    #region SaveList
    private static int DoSaveListWriteData(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, string sql, string sqlParam0, string sqlParam1, string sqlParam2, out Exception error)
    {
      return DownloadData.Add(info.DataInfo.ReportData, sqlParam0, sqlParam1, sqlParam2, out error);
    }
    protected override int InternalSaveList(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, SaveListCallBack CB)
    {
      var mi = System.Reflection.MethodBase.GetCurrentMethod();
      Logger.WriteMethod(mi, worker, info, CB);
      int exit(int _result)
      {
        Logger.WriteMethodResult(mi, _result, worker, info, CB);
        return _result;
      }

      info.AddInfo(VdO2013RS.TextRes.Reader.SaveLista.INFO, VdO2013RS.TextRes.Reader.SaveLista.STEP);
      InvokeCB(worker, info, CB);
      Exception error = null;
      string sql = string.Empty;
      string webPart = string.Empty;
      string webValue = string.Empty;
      try
      {
        info.AddInfo(VdO2013RS.TextRes.Reader.SaveLista.STEPSAVE);
        InvokeCB(worker, info, CB);
        sql = string.Format("INSERT INTO [{0}]([Job],[Description],[Value]) VALUES({1},{2},{3});", info.DataInfo.ReportData.Table.TableName, "{0}", "{1}", "{2}");
        // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
        webPart = VdO2013RS.TextRes.Reader.SaveLista.CODICEAGENZIA;
        webValue = this.Agenzia;
        info.AddInfo(string.Format(VdO2013RS.TextRes.Reader.SaveLista.SAVEWRITEFORMAT, webPart, webValue));
        InvokeCB(worker, info, CB);
        DoSaveListWriteData(worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, webPart, webValue, out error);

        info.AddError(error);
        InvokeCB(worker, info, CB);
        if (error != null) throw error;
        // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
        webPart = VdO2013RS.TextRes.Reader.SaveLista.RAGIONESOCIALE;
        webValue = this.RagioneSociale;
        info.AddInfo(string.Format(VdO2013RS.TextRes.Reader.SaveLista.SAVEWRITEFORMAT, webPart, webValue));
        InvokeCB(worker, info, CB);
        DoSaveListWriteData(worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, webPart, webValue, out error);

        info.AddError(error);
        InvokeCB(worker, info, CB);
        if (error != null) throw error;
        // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
        webPart = VdO2013RS.TextRes.Reader.SaveLista.TIPOLOGIA;
        webValue = this.Tipologia;
        info.AddInfo(string.Format(VdO2013RS.TextRes.Reader.SaveLista.SAVEWRITEFORMAT, webPart, webValue));
        InvokeCB(worker, info, CB);
        DoSaveListWriteData(worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, webPart, webValue, out error);

        info.AddError(error);
        InvokeCB(worker, info, CB);
        if (error != null) throw error;
        // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
        webPart = VdO2013RS.TextRes.Reader.SaveLista.ELEMENTI;
        //webValue = info.DataInfo.IsDataBound ? reader.Pagine.ToString() : reader.Elementi.ToString();
        webValue = this.Elementi.ToString();
        info.AddInfo(string.Format(VdO2013RS.TextRes.Reader.SaveLista.SAVEWRITEFORMAT, webPart, webValue));
        InvokeCB(worker, info, CB);
        DoSaveListWriteData(worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, webPart, webValue, out error);

        info.AddError(error);
        InvokeCB(worker, info, CB);
        if (error != null) throw error;
        // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****

        if (this.PageItems != null)
        {
          double itemProgressPercentage = 33.0 / (double)this.PageItems.Count; //Metto 33 invece che 100 perché lo scaricamento è 1/3 del lavoro da fare
          int pageIndex = 1;
          foreach (string pageItemKey in this.PageItems.AllKeys)
          {
            info.AddInfo(info.LastItem.Percentage + itemProgressPercentage, string.Format(VdO2013RS.TextRes.Reader.SaveLista.ITEMFORMAT, pageItemKey, pageIndex));
            InvokeCB(worker, info, CB);
            // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
            string description = VdO2013SRCore.TextRes.tagOfferta;
            string value = pageIndex.ToString();
            DoSaveListWriteData(worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, description, value, out error);

            if (error != null)
              throw error;
            // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
            description = string.Format(VdO2013RS.TextRes.Reader.SaveLista.ITEMNAMEFORMAT, VdO2013SRCore.TextRes.tagOfferta, pageIndex, OfferteData.Columns.Codice);
            value = pageItemKey.Replace(this.StandardSiteRootUrl + @"/", string.Empty);
            if (value.Contains('-')) value = value.Left(pageItemKey.IndexOf('-')); //!!
            DoSaveListWriteData(worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, description, value, out error);

            if (error != null)
              throw error;
            // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
            description = string.Format(VdO2013RS.TextRes.Reader.SaveLista.ITEMNAMEFORMAT, VdO2013SRCore.TextRes.tagOfferta, pageIndex, OfferteData.Columns.Ordinal);
            value = pageIndex.ToString();
            DoSaveListWriteData(worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, description, value, out error);

            if (error != null)
              throw error;
            // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
            description = string.Format(VdO2013RS.TextRes.Reader.SaveLista.ITEMNAMEFORMAT, VdO2013SRCore.TextRes.tagOfferta, pageIndex, OfferteData.Columns.Link);
            value = pageItemKey.Replace(this.StandardSiteRootUrl + @"/", string.Empty); //!!
            DoSaveListWriteData(worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, description, value, out error);

            if (error != null)
              throw error;
            // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
            Dictionary<string, string> features = this.PageItems[pageItemKey] as Dictionary<string, string>;
            foreach (string key in features.Keys)
            {
              info.AddInfo(string.Format(VdO2013RS.TextRes.Reader.SaveLista.ITEMFEATUREFORMAT, key));
              InvokeCB(worker, info, CB);
              // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
              description = string.Format(VdO2013RS.TextRes.Reader.SaveLista.ITEMFEATURENAMEFORMAT, VdO2013SRCore.TextRes.tagOfferta, pageIndex, VdO2013SRCore.TextRes.tagDettaglioB, key, VdO2013SRCore.TextRes.tagDettaglioE);
              value = HtmlHelper.HtmlDecode(features[key]);//!!
              DoSaveListWriteData(worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, description, value, out error);

              if (error != null)
                throw error;
              // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
            }

            pageIndex++;
          }
        }

        if (error != null)
          throw error;
      }
      catch (Exception ex)
      {
        info.AddError(ex);
        InvokeCB(worker, info, CB);
      }

      return exit((int)info.LastItem.Percentage);
    }
    #endregion SaveList

    protected override string GetReaderName() => "Immobiliare";
    protected override string GetAuthor() => "Paolo Mancini";
    protected override string GetCopyright() => "LogicNet 2017";
    protected override string GetDescription() => "Lettore per immobiliare.it";
    protected override DateTime GetReleaseDate() => new DateTime(2020, 06, 01);
    protected override string GetUrl() => "http://www.logicnet.it";
    protected override string GetVersion() => MPCommonRes.TextRes.Versioning.GetVersionString(MPCommonRes.TextRes.Versioning.Majors.Two);

    protected override Image GetFullLogo() => ReaderSettingsDefaults.KLOGOFULLVALUE;
    protected override Image GetLogo() => ReaderSettingsDefaults.KLOGOVALUE;

    protected override string GetMergeIVDefinitionTypeName() => VdO2013SR.TextRes.DefaultSettingsNames.KMergeDefinitionTypeName;

    protected override string GetPageListListXPath() => ReaderSettingsDefaults.KPAGELIST_LISTXPATHVALUE;

    protected override string GetPageListUrlDefault() => ReaderSettingsDefaults.KPAGELIST_URLVALUE;

    public override Type GetSiteReader2Type() => typeof(SiteReader);
  }
}
