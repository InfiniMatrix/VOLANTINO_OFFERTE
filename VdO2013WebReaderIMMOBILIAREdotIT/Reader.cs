using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;

using MPExtensionMethods;
using VdO2013WebReaderIMMOBILIAREdotIT;

using RS = VdO2013RS;
using VdO2013SR;
using VdO2013SRCore;
using VdO2013SRCore.Specialized;
using VdO2013Data;
using VdO2013DataCore;
using VdO2013Core;

namespace VdO2013WebReaderIMMOBILIAREdotIT
{
    [Obsolete("Please use " + nameof(SiteReader) + " class")]
    public class Reader : SiteReader2, ISiteReader2
    {
        internal static bool Register(ref SiteReader2Info info)
        {
            info.Type = typeof(Reader);
            info.ReaderName = ReaderSettings.KREADERNAMEVALUE;
            info.Title = ReaderSettings.KTITLEVALUE;
            info.Description = ReaderSettings.KDESCRIPTIONVALUE;
            info.Logo = ReaderSettings.KLOGOVALUE;
            info.FullLogo = ReaderSettings.KLOGOFULLVALUE;

            return true;
        }

        //private ReaderMode _Mode = ReaderMode.Aggiungi;

        private ISiteReaderValueList<SettingWrapper> _Settings = new ReaderSettings();
        private ISiteReaderValueList<FeatureWrapper> _Features = new ReaderFeatures();

        public override int Reload(out Exception error)
        {
            var result = base.Reload(out error);

            foreach (var ss in this._Settings.List)
            {
                //Verificare
                ss.Value.Value = this.Config.Get<string>(ss.Key);
                result++;
            }
            return result;
        }

        protected override string GetName()
        {
            return ReaderSettings.KNAMEVALUE;
        }
        protected override string GetReaderName()
        {
            return ReaderSettings.KREADERNAMEVALUE;
        }
        protected override string GetReaderVersion()
        {
            return ReaderSettings.KREADERVERSIONVALUE;
        }

        protected override string GetTitle()
        {
            return ReaderSettings.KTITLEVALUE;
        }

        protected override string GetDescription()
        {
            return ReaderSettings.KDESCRIPTIONVALUE;
        }

        protected override Image GetLogo()
        {
            return ReaderSettings.KLOGOVALUE;
        }

        protected override Image GetFullLogo()
        {
            return ReaderSettings.KLOGOFULLVALUE;
        }

        //protected override ReaderMode GetReaderMode()
        //{
        //    return _Mode;
        //}
        //protected override Boolean SetReaderMode(ReaderMode value)
        //{
        //    _Mode = value;
        //    return true;
        //}

        protected override string GetNormalSiteRootUrl() { return ReaderSettings.KURLNORMALVALUE; }

        protected override string GetMobileSiteRootUrl() { return ReaderSettings.KURLMOBILEVALUE; }

        protected override string GetPageListUrlDefault() { return ReaderSettings.KPAGELIST_URLVALUE; }

        protected override string GetPageListListXPath() { return ReaderSettings.KPAGELIST_LISTXPATHVALUE; }

        protected override int InitDefaultSettings()
        {
            // Rivedere
            var result = base.InitDefaultSettings();
            foreach (var ss in this._Settings.List)
                if (!this.Config.Exists(ss.Key))
                    this.Config.Add(ss);
                else
                    ss.Value.Value = this.Config.Get<string>(ss.Key);
            result += this._Settings.Count;
            return result;
        }

        protected override int GetDefaultSettingCount()
        {
            var result = base.GetDefaultSettingCount();
            result += this._Settings.Count;
            return result;
        }

        protected override int InitDefaultFeatures()
        {
            var result = 0;
            foreach (var ff in this._Features.List)
                this.FeatureAdd(ff);
            result += this._Features.Count;
            return result;
        }

        protected override int GetDefaultFeatureCount()
        {
            var result = 0;
            result += _Features.Count;
            return result;
        }

        #region StaticReadList
        //protected override Boolean StaticReadListInitialize()
        //{
        //    StaticReadList = new ReadListDelegate(DoStaticReadList);
        //    return true;
        //}

        private static int DoStaticReadListItemFeatures(ISiteReader2 reader
            , BackgroundWorker worker
            , ISiteReaderOnLineCheckDataList info
            , ReadListCallBack CB
            , List<HtmlAgilityPack.HtmlNode> features
            , Dictionary<string, string> featureList
            , string annuncio)
        {
            var result = 0;
            if (features == null || features.Count == 0)
            {
                info.AddInfo("Warning: NO FEATURES FOUND", "Reader:{0}, Worker:{1}, Item:{2}".FormatWith(reader, worker, annuncio));
                return -1;
            }
#if true
            else
            {
                info.AddInfo("Information: {0} FEATURES FOUND".FormatWith(features.Count), "Reader:{0}, Worker:{1}, Item:{2}".FormatWith(reader, worker, annuncio));
            }
#endif
            var rows = features[0].ChildNodes;
            // Salto header e footer della tablella
            for (var iRow = 1; iRow < rows.Count - 1; iRow++)
            {
                var cols = rows[iRow].ChildNodes;
                // 2016-08-28: precedentemente avevo una tabella da leggere
                if (cols.Count > 1)
                {
                    for (var iCol = 1; iCol < cols.Count - 2; iCol += 4)
                    {
                        var title = "{" + HtmlHelper.HtmlDecode(cols[iCol].InnerText).ToLower().Replace(':', ' ').Trim().Replace(' ', '_') + "}";
                        var value = HtmlHelper.HtmlDecode(cols[iCol + 2].InnerText).Trim();

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
                    if ((iRow - 1) % 4 == 0 && (iRow + 2 < rows.Count))
                    {
                        var title = "{" + HtmlHelper.HtmlDecode(rows[iRow].InnerText).ToLower().Replace(':', ' ').Trim().Replace(' ', '_') + "}";
                        var value = HtmlHelper.HtmlDecode(rows[iRow + 2].InnerText).Trim();

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

                info.AddInfo(info.LastItem.Percentage, string.Format(RS.TextRes.Reader.ReadLista.ITEMFEATURESFORMAT, result));
                reader.InvokeCB(worker, info, CB);
                result++;
            }

            return result;
        }

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

        private static int DoStaticReadListItemImages(ISiteReader2 reader, BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack CB, List<HtmlAgilityPack.HtmlNode> images, Dictionary<string, string> imageList, string annuncio)
        {
            var result = 0;
            Exception error = null;
            if (images == null) return -1;

            var maxImageCount = reader.Config.Get<int>(ReaderSettings.KPAGELIST_IMAGEMAXCOUNTNAME);
            if (maxImageCount == -1) maxImageCount = int.MaxValue;

            foreach (var n in images)
            {
                var fImageUrl = n.Attributes["src"].Value;
                var fImageUrlParts = fImageUrl.Split('/');
                //Aggiusto il doppio slash
                fImageUrlParts[WebImageAddressParts.filler] = @"/";
                //Ricostruisco il percorso
                fImageUrl = string.Empty;
                for (var j = 0; j < fImageUrlParts.Length; j++)
                {
                    fImageUrl += (string.IsNullOrEmpty(fImageUrl) ? "" : @"/") + fImageUrlParts[j];
                }

                if (!fImageUrl.StartsWith("http"))
                    continue;

                info.AddInfo(info.LastItem.Percentage, string.Format(RS.TextRes.Reader.ReadLista.ITEMIMAGESDOWNLOAD, VdO2013SRCore.TextRes.tagImmagine, result));
                reader.InvokeCB(worker, info, CB);

                var fAnnuncio = string.Empty;
                var fIndice = string.Empty;
                var fFormato = string.Empty;
                var fNomeFile = string.Empty;
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
                            fAnnuncio = fImageFileNameParts[j] + (fAnnuncio.Length > 0 ? "_" : string.Empty) + fAnnuncio;
                            break;
                    }

                    i++;
                }

                fImageUrl = string.Format(@"{0}//{1}/{2}/"
                    , fImageUrlParts[WebImageAddressParts.root]
                    , fImageUrlParts[WebImageAddressParts.url]
                    , fImageUrlParts[WebImageAddressParts.url2]);
                fImageUrl += string.Format(@"{0}_{1}_{2}", fAnnuncio, fIndice, fNomeFile);

                var fImageFileName = System.IO.Path.Combine(info.Reader.ImagesPath, reader.Agenzia, annuncio, fNomeFile);
                var fImageDirectory = System.IO.Path.GetDirectoryName(fImageFileName);

                if (!System.IO.Directory.Exists(fImageDirectory))
                    System.IO.Directory.CreateDirectory(fImageDirectory);

                if (System.IO.File.Exists(fImageFileName))
                    System.IO.File.Delete(fImageFileName);


                fImageUrl = fImageUrl.Replace(@"///", @"//").Replace(@"///", @"//");
                var fImageUri = new Uri(fImageUrl);
                //fImageFileName = System.IO.Path.ChangeExtension(fImageFileName, reader.QRCode.ImageFormat.GetImageFormatFileExtension());

                var fImageFileNameExtension = System.IO.Path.GetExtension(fImageFileName);
                if (fImageUrl.DownloadImage(fImageFileName, out error, fImageFileNameExtension.GetImageClassFromFileExtension(), true))
                    info.AddInfo(info.LastItem.Percentage, string.Format("{0} {1} {2}.", VdO2013SRCore.TextRes.tagImmagine, result, RS.TextRes.Reader.ReadLista.ITEMIMAGESDOWNLOADOK));
                else
                    info.AddInfo(info.LastItem.Percentage, string.Format("{0} {1} {2}.", VdO2013SRCore.TextRes.tagImmagine, result, RS.TextRes.Reader.ReadLista.ITEMIMAGESDOWNLOADKO));
                reader.InvokeCB(worker, info, CB);

                //using (ImageDownloader id = new ImageDownloader(fImageUrl.Replace(@"///", @"//")))
                //{
                //    id.Download(out error);
                //    info.AddInfo(String.Format("{0} {1} {2}.", SiteReader2Resources.tagImmagine, result, error == null ? TextRes.Reader.ReadLista.ItemImagesDownloadOK : TextRes.Reader.ReadLista.ItemImagesDownloadKO), info.LastItem.Step, info.LastItem.Percentage + 0.1);
                //    DoCB(worker, info, CB);

                //    fImageFileName = System.IO.Path.ChangeExtension(fImageFileName, QRCodeHelper.ImageFormatFileExtension(reader.QRCode.ImageFormat));
                //    id.Save(fImageFileName, reader.QRCode.ImageFormat, out error);
                //}

                info.AddInfo(string.Format("{0} {1}.", VdO2013SRCore.TextRes.tagImmagine, fImageUrlParts[WebImageAddressParts.WebImageFileNameAddressParts.indice]));
                info.AddError(error);
                reader.InvokeCB(worker, info, CB);
                if (error != null) break;

                var sImageKey = string.Format("{0}{1}{2}", VdO2013SRCore.TextRes.tagImmagineB, (result + 1).ToString().PadLeft(2, '0'), VdO2013SRCore.TextRes.tagImmagineE);
                if (!imageList.ContainsKey(sImageKey))
                {
                    imageList.Add(sImageKey, fImageFileName);
                }
                else
                {
                    info.AddInfo(string.Format("--> Image '{0}' already added: (Value:{1}; NewValue:{2})", sImageKey, imageList[sImageKey], fImageFileName));
                    reader.InvokeCB(worker, info, CB);
                }
                result++;
                if (maxImageCount > 0 && result > maxImageCount) break;
            }

            return result;
        }

        private static int DoStaticReadListItemVideos(ISiteReader2 reader, BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack CB, List<HtmlAgilityPack.HtmlNode> videos, Dictionary<string, string> videoList, string annuncio)
        {
            var result = 0;
            Exception error = null;
            if (videos == null || videos.Count == 0) return -1;

            var maxVideoCount = reader.Config.Get<int>(ReaderSettings.KPAGELIST_VIDEOMAXCOUNTNAME);
            if (maxVideoCount == -1) maxVideoCount = int.MaxValue;

            foreach (var n in videos)
            {
                string videoUrl = null;
                
                // Extract video URL based on element type
                if (n.Name.ToLower() == "source" && n.Attributes["src"] != null)
                {
                    videoUrl = n.Attributes["src"].Value;
                }
                else if (n.Name.ToLower() == "video" && n.Attributes["src"] != null)
                {
                    videoUrl = n.Attributes["src"].Value;
                }
                else if (n.Name.ToLower() == "iframe" && n.Attributes["src"] != null)
                {
                    // For iframe videos (e.g., YouTube, Vimeo), we store the embed URL
                    videoUrl = n.Attributes["src"].Value;
                }
                
                if (string.IsNullOrEmpty(videoUrl))
                    continue;
                
                // Ensure URL is absolute
                if (!videoUrl.StartsWith("http"))
                {
                    var baseUrl = reader.Config.Get<string>(ReaderSettings.KURLNORMALVALUE);
                    videoUrl = new Uri(new Uri(baseUrl), videoUrl).ToString();
                }

                info.AddInfo(info.LastItem.Percentage, string.Format(RS.TextRes.Reader.ReadLista.ITEMIMAGESDOWNLOAD, "Video", result));
                reader.InvokeCB(worker, info, CB);

                // For iframe videos, we just store the URL
                if (n.Name.ToLower() == "iframe")
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
                    var videoFileName = System.IO.Path.Combine(info.Reader.ImagesPath, reader.Agenzia, annuncio, "videos", $"video_{result + 1}{videoExtension}");
                    var videoDirectory = System.IO.Path.GetDirectoryName(videoFileName);

                    if (!System.IO.Directory.Exists(videoDirectory))
                        System.IO.Directory.CreateDirectory(videoDirectory);

                    if (System.IO.File.Exists(videoFileName))
                        System.IO.File.Delete(videoFileName);

                    if (videoUrl.DownloadVideo(videoFileName, out error, true))
                        info.AddInfo(info.LastItem.Percentage, string.Format("Video {0} download OK.", result));
                    else
                        info.AddInfo(info.LastItem.Percentage, string.Format("Video {0} download FAILED.", result));
                    
                    reader.InvokeCB(worker, info, CB);

                    var sVideoKey = string.Format("{0}{1}{2}", "{video", (result + 1).ToString().PadLeft(2, '0'), "}");
                    if (!videoList.ContainsKey(sVideoKey))
                    {
                        videoList.Add(sVideoKey, videoFileName);
                    }
                    else
                    {
                        info.AddInfo(string.Format("--> Video '{0}' already added: (Value:{1}; NewValue:{2})", sVideoKey, videoList[sVideoKey], videoFileName));
                        reader.InvokeCB(worker, info, CB);
                    }
                }
                
                info.AddError(error);
                reader.InvokeCB(worker, info, CB);
                if (error != null) break;
                
                result++;
                if (maxVideoCount > 0 && result >= maxVideoCount) break;
            }

            return result;
        }

        private static Dictionary<string, string> DoStaticReadListItem(ISiteReader2 reader
            , BackgroundWorker worker
            , ISiteReaderOnLineCheckDataList info
            , ReadListCallBack CB
            , string aPaginaPropostaUrl)
        {
            var result = new Dictionary<string, string>();

            info.AddInfo("Start", RS.TextRes.Reader.ReadLista.ITEMSTEP);
            var fPaginaPropostaUrlText = aPaginaPropostaUrl;

            Exception error = null;
            var xpath = string.Empty;
            List<HtmlAgilityPack.HtmlNode> xnodes = null;
            try
            {
                var fPaginaPropostaUrl = new Uri(fPaginaPropostaUrlText.ToLower());

                var canRedirect = false;
                var allowedRedirections = reader.Config.Get<string>(ReaderSettings.KPAGELIST_URLREDIRECTSNAME).Split('|', StringSplitOptions.RemoveEmptyEntries);
                foreach (var r in allowedRedirections)
                {
                    canRedirect = fPaginaPropostaUrlText.Contains(r);
                    if (canRedirect) break;
                }

                if (HtmlHelper.OpenUrl(fPaginaPropostaUrl, out error, canRedirect ? HtmlHelper.UriPart.Path | HtmlHelper.UriPart.Query : HtmlHelper.UriPart.None) != System.Net.HttpStatusCode.OK || error != null)
                {
                    info.AddError(error);
                    return result;
                }

                var annuncio = fPaginaPropostaUrlText.Replace(reader.Config.Get<string>(ReaderSettings.KPAGELIST_URLNAME), string.Empty);
                if (annuncio.StartsWith(@"/")) annuncio = annuncio.Right(annuncio.Length - 1);
                annuncio = annuncio.Left(annuncio.IndexOf('-'));

                if (annuncio.Contains('/')) annuncio = annuncio.Right(annuncio.ReverseIndexOf('/'));

                foreach (VdO2013Core.Config.FeatureElement fe in reader.Features)
                {
                    if (!string.IsNullOrEmpty(fe.XPath))
                    {
                        var webItem = fe.Name;
                        var webPart = HtmlHelper.FindNode_InnerText(HtmlHelper.HtmlDocument.DocumentNode, fe.XPath, true, 0, "null", out error);
                        if (error != null)
                        {
                            info.AddError(error);
                            reader.InvokeCB(worker, info, CB);
                        }

                        if (!result.ContainsKey(webItem))
                        {
                            result.Add(webItem, webPart);
                            info.AddInfo(string.Format("--> Feature '{0}' added: (Value:{1})", webItem, webPart));
                        }
                        else
                        {
                            var oldValue = result[webItem];
                            result[webItem] = webPart;
                            info.AddInfo(string.Format("--> Feature '{0}' updated: (Value:{1}; NewValue:{2})", webItem, oldValue, webPart));
                        }
                        reader.InvokeCB(worker, info, CB);
                    }
                }

                if (reader.Config.Get<bool>(ReaderSettings.KPAGELIST_FEATURESENABLEDNAME))
                {
                    xpath = reader.Config.Get<string>(ReaderSettings.KPAGELIST_FEATURESXPATHNAME);
                    xnodes = HtmlHelper.FindNode(HtmlHelper.HtmlDocument.DocumentNode, xpath, true, out error);
                    DoStaticReadListItemFeatures(reader, worker, info, CB, xnodes, result, annuncio);
                }

                xpath = reader.Config.Get<string>(ReaderSettings.KPAGELIST_IMAGEXPATHNAME);
                var maxImageCount = reader.Config.Get<int>(ReaderSettings.KPAGELIST_IMAGEMAXCOUNTNAME);
                if (maxImageCount == -1) maxImageCount = int.MaxValue;

                var containers = HtmlHelper.FindNode(HtmlHelper.HtmlDocument.DocumentNode, xpath, true, out error);
                if (error != null)
                {
                    info.AddError(error);
                    reader.InvokeCB(worker, info, CB);
                }

                foreach (var container in containers)
                {
                    if (maxImageCount >= 1)
                    {
                        xpath = reader.Config.Get<string>(ReaderSettings.KPAGELIST_IMAGETHUMBLISTXPATHNAME);
                        xnodes = HtmlHelper.FindNode(container, xpath, true, out error);
                        if (error != null)
                        {
                            info.AddError(error);
                            reader.InvokeCB(worker, info, CB);
                        }
                        DoStaticReadListItemImages(reader, worker, info, CB, xnodes, result, annuncio);
                    }
                }
                
                // Download videos if enabled
                if (reader.Config.Get<bool>(ReaderSettings.KPAGELIST_VIDEOENABLEDNAME))
                {
                    xpath = reader.Config.Get<string>(ReaderSettings.KPAGELIST_VIDEOXPATHNAME);
                    var videoNodes = HtmlHelper.FindNode(HtmlHelper.HtmlDocument.DocumentNode, xpath, true, out error);
                    if (error != null)
                    {
                        info.AddError(error);
                        reader.InvokeCB(worker, info, CB);
                    }
                    
                    if (videoNodes != null && videoNodes.Count > 0)
                    {
                        info.AddInfo("Found {0} video(s) on page".FormatWith(videoNodes.Count));
                        DoStaticReadListItemVideos(reader, worker, info, CB, videoNodes, result, annuncio);
                    }
                }
            }
            catch (Exception Ex)
            {
                error = new Exception(string.Format(RS.TextRes.Reader.ReadLista.ITEMERRORFORMAT, fPaginaPropostaUrlText, xpath), Ex);
            }

            return result;
        }

        /// <summary>
        /// Formato supportato "Pag. 9 di 10"
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="htmlText"></param>
        private static void DoStaticReadListParsePagine(ISiteReader2 reader, string htmlText)
        {
            if (string.IsNullOrEmpty(htmlText)) return;

            var htmlParts = htmlText.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var daPaginaN = -1;
            var aPaginaN = -1;

            int.TryParse(htmlText.Split(' ')[1], out daPaginaN);
            int.TryParse(htmlText.Split(' ')[3], out aPaginaN);

            reader.Pagine = aPaginaN;
        }

        private static bool DoStaticInitReader(ISiteReader2 reader, BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack CB)
        {
            Exception error = null;
            try
            {
                reader.Tipologia = RS.TextRes.Reader.ReadLista.TIPOLOGIATUTTE; //FindNode(HtmlDocument.DocumentNode, fXPathPaginaLista_TipologiaProposta, true, out error)[0].InnerText;
                var xRagioneSociale = reader.Config.Get<string>(ReaderSettings.KPAGELIST_RAGIONESOCIALEXPATHNAME);
                var nRagioneSociale = HtmlHelper.FindNode(HtmlHelper.HtmlDocument.DocumentNode, xRagioneSociale, true, out error);
                if (nRagioneSociale == null || nRagioneSociale.Count == 0)
                {
                    var m = "Elemento {0}(xpath: {1}) non trovato.".FormatWith(ReaderSettings.KPAGELIST_RAGIONESOCIALEXPATHNAME, xRagioneSociale);
                    info.AddError(m);
                    reader.InvokeCB(worker, info, CB);
                    throw new InvalidOperationException(m);
                }
                reader.RagioneSociale = nRagioneSociale[0].InnerText;

                info.AddInfo(info.LastItem.Percentage + 0.1, string.Format(RS.TextRes.Reader.ReadLista.PROPOSTECOUNTFORMAT, reader.Elementi));
                reader.InvokeCB(worker, info, CB);
                info.AddError(error);
                reader.InvokeCB(worker, info, CB);
                return true;
            }
            catch (Exception ex)
            {
                info.AddInfo(info.LastItem.Percentage + 0.1, string.Format(RS.TextRes.Reader.ReadLista.PROPOSTECOUNTFORMAT, reader.Elementi), info.LastItem.Step);
                info.AddError(error);
                info.AddError(ex);
                reader.InvokeCB(worker, info, CB);
            }
            return false;
        }

        private static int DoStaticGetTotalCount(ISiteReader2 reader, BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack CB, out string[] activeListPages)
        {
            Exception error = null;
            activeListPages = null;
            var _activePages = new List<string>();

            info.AddInfo("Conteggio");
            reader.InvokeCB(worker, info, CB);

            if (!reader.Initialized) return -1;
            if (reader.Agenzia.Trim().Length <= 0) return -2;
            reader.Pagine = 0;
            reader.Elementi = 0;

            try
            {
                var readerPageListMaxCount = reader.Config.Get<int>(ReaderSettings.KPAGELIST_MAXCOUNTNAME);
                // Posso avere più pagine lista, ogni url di pagina è separata da KPageListUrlDefault
                var urlListItems = reader.Config.Get<string>(ReaderSettings.KPAGELIST_URLNAME).Split(ReaderSettings.KPageListUrlSeparator, StringSplitOptions.RemoveEmptyEntries);
                for (var urlListIndex = 0; urlListIndex < urlListItems.Length; urlListIndex++)
                {
                    // Scorro le pagine della lista
                    for (var pageListIndex = 1; pageListIndex < readerPageListMaxCount; pageListIndex++)
                    {
                        var fPaginaListaUrlString = (reader.StandardSiteRootUrl + urlListItems[urlListIndex])
                            .Replace(VdO2013SRCore.TextRes.WebSite.KTAGCODICEAGENZIA, reader.Agenzia)
                            //.Replace(VdO2013SRCore.TextRes.WebSite.KTAGNUMEROAGENZIA, reader.Numero)
                            .Replace(VdO2013SRCore.TextRes.WebSite.KTAGINDICELISTA, pageListIndex.ToString());
                        var fPaginaListaUrl = new Uri(fPaginaListaUrlString);

                        info.AddInfo(string.Format(RS.TextRes.Reader.ReadLista.URLOPENING, fPaginaListaUrlString));
                        reader.InvokeCB(worker, info, CB);

                        if (HtmlHelper.OpenUrl(fPaginaListaUrl, out error, HtmlHelper.UriPart.Path | HtmlHelper.UriPart.Query) != System.Net.HttpStatusCode.OK || error != null)
                        {
                            if (error is HttpRedirectNotAllowedException)
                            {
                                info.AddWarning(error.Message);
                                error = null;
                            }
                            else
                                info.AddError(error);

                            reader.InvokeCB(worker, info, CB);
                            break; // Esco dal ciclo delle pagine
                        }

                        var pageListXPath = reader.Config.Get<string>(ReaderSettings.KPAGELIST_LISTXPATHNAME);
                        var pageListItems = HtmlHelper.FindNode(HtmlHelper.HtmlDocument.DocumentNode, pageListXPath, true, out error);
                        info.AddError(error);
                        reader.InvokeCB(worker, info, CB);

                        if (pageListItems != null && pageListItems.Count == 0)
                        {
                            info.AddWarning("Nessuna pagina trovata per {0}".FormatWith(pageListXPath));
                            reader.InvokeCB(worker, info, CB);
                        }
                        if (error is HttpRedirectNotAllowedException || pageListItems == null || pageListItems.Count == 0)
                            break; // Esco dal ciclo delle pagine

                        _activePages.Add(fPaginaListaUrlString);
                        reader.Pagine++;
                        reader.Elementi += pageListItems.Count;

                    }//for (int pageListIndex = 1; ...
                }//for (int urlListIndex = 0; ...
            }
            catch (Exception ex)
            {
                info.AddError(ex);
                reader.InvokeCB(worker, info, CB);
            }

            info.AddInfo("Numero pagine trovate: {0}".FormatWith(reader.Pagine));
            reader.InvokeCB(worker, info, CB);
            info.AddInfo("Numero elementi trovati: {0}".FormatWith(reader.Elementi));
            reader.InvokeCB(worker, info, CB);

            activeListPages = _activePages.ToArray();
            return reader.Elementi;
        }

        private static int DoStaticReadList(ISiteReader2 reader, BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack CB)
        {
            //int result = -1;
            //Exception error = null;
            //if (!reader.Initialized)
            //    return result;

            //info.AddInfo("Start", TextRes.Reader.ReadLista.Step);
            //List<HtmlAgilityPack.HtmlNode> pageListItems = null;
            //if (reader.Agenzia.Trim().Length <= 0) return -2;
            //reader.Pagine = 99;
            //reader.Elementi = 0;

            //try
            //{
            //    int readerPageListMaxCount = getIntSetting(reader, ReaderSettings.KPageListMaxCount);
            //    // Posso avere più pagine lista, ogni url di pagina è separata da KPageListUrlDefault
            //    String[] urlListItems = getStringSetting(reader, ReaderSettings.KPageListUrl).Split(ReaderSettings.KPageListUrlSeparator, StringSplitOptions.RemoveEmptyEntries);
            //    for (int urlListIndex = 0; urlListIndex < urlListItems.Length; urlListIndex++)
            //    {
            //        // Scorro le pagine della lista
            //        for (int pageListIndex = 1; pageListIndex < readerPageListMaxCount; pageListIndex++)
            //        {
            //            String fPaginaListaUrlString = (reader.StandardSiteRootUrl + urlListItems[urlListIndex]).Replace(VdO2013SRCore.TextRes.tagCodiceAgenzia, reader.Agenzia).Replace(VdO2013SRCore.TextRes.tagIndiceLista, pageListIndex.ToString());
            //            Uri fPaginaListaUrl = new Uri(fPaginaListaUrlString);

            //            info.AddInfo(info.LastItem.Percentage + 0.1, String.Format(TextRes.Reader.ReadLista.UrlOpening, fPaginaListaUrlString));
            //            DoCB(worker, info, CB);

            //            if (!HtmlHelper.OpenUrl(fPaginaListaUrl, out error))
            //            {
            //                info.AddError(error);
            //                if (error is HttpRedirectNotAllowedException)
            //                    error = null;
            //                break; // Esco dal ciclo delle pagine
            //            }

            //            // Dalla prima pagina di ogni url ricavo la paginazione
            //            if (pageListIndex == 1)
            //            {
            //                String sPagination = HtmlHelper.FindNode(HtmlHelper.HtmlDocument.DocumentNode, getStringSetting(reader, ReaderSettings.KPageListItemCountXPath), true, out error)[0].InnerText;
            //                DoStaticReadListParsePagine(reader, sPagination);

            //                // Solo dalla prima pagina della prima url ricavo le informazioni del reader
            //                if (urlListIndex == 0)
            //                    if (!DoStaticInitReader(reader, worker, info, CB))
            //                        break; // Esco dal ciclo delle pagine
            //            }

            //            pageListItems = HtmlHelper.FindNode(HtmlHelper.HtmlDocument.DocumentNode, getStringSetting(reader, ReaderSettings.KPageListListXPath), true, out error);
            //            info.AddError(error);
            //            if (error is HttpRedirectNotAllowedException || pageListItems == null)
            //                break; // Esco dal ciclo delle pagine

            //            // Scorro gli elementi della lista corrente
            //            for (int pageListItemIndex = 0; pageListItemIndex < pageListItems.Count; pageListItemIndex++)
            //            {
            //                info.AddInfo(info.LastItem.Percentage + 0.1, String.Format(TextRes.Reader.ReadLista.PropostaFormat, pageListItemIndex));
            //                DoCB(worker, info, CB);

            //                String fPaginaListaItemLink = pageListItems[pageListItemIndex].Attributes[TextRes.domSearchTagAAttr].Value.ToString();
            //                reader.PageItems.Add(fPaginaListaItemLink, DoStaticReadListItem(reader, worker, info, CB, fPaginaListaItemLink));
            //                reader.Elementi++;
            //            }//for (int pageListItemIndex = 0; ...

            //            if (reader.Pagine <= 0 || pageListIndex >= reader.Pagine)
            //                break; // Esco dal ciclo delle pagine
            //        }//for (int pageListIndex = 1; ...
            //    }//for (int urlListIndex = 0; ...

            //    result = reader.PageItems.Count;
            //}
            //catch (Exception ex)
            //{
            //    info.AddError(ex);
            //}

            //return result;
            var result = -1;
            Exception error = null;
            if (!reader.Initialized)
                return result;

            info.AddInfo("Start", RS.TextRes.Reader.ReadLista.STEP);
            reader.InvokeCB(worker, info, CB);

            // Se info.List contiene valori, allora leggo direttamente le offerte specificate dall'elenco nella lista
            // in questo modo è possibile (ri)leggere anche solo impostazioni di una o più offerte specifiche
            var elementZero = info.List != null && info.List.Count > 0 ? info.List.ElementAt(0).Key.ToString() : string.Empty;
            if (info.List != null && info.List.Count > 0 && !elementZero.Equals(info.Reader.StandardSiteRootUrl) && !elementZero.Equals(info.Reader.StandardSiteRootUrl + @"/"))
            {
                var itemProgressPercentage = 33.0 / info.List.Count; //Metto 33 invece che 100 perché lo scaricamento è 1/3 del lavoro da fare

                info.AddInfo("Lettura lista statica ({0} elementi).".FormatWith(info.List.Count));
                reader.InvokeCB(worker, info, CB);

                for (var i = 0; i < info.List.Count; i++)
                {
                    info.AddInfo(info.LastItem.Percentage + itemProgressPercentage, string.Format(RS.TextRes.Reader.ReadLista.PROPOSTAFORMAT, i));
                    reader.InvokeCB(worker, info, CB);

                    var url = info.List.Keys.ElementAt(i);
                    var sts = info.List[url];

                    try
                    {
                        var redirText = @"Siamo spiacenti. La pagina che stai cercando non &egrave; presente sul nostro sito o non &egrave; pi&ugrave; disponibile.";
                        if (HtmlHelper.OpenUrl(url, out error, HtmlHelper.UriPart.Path | HtmlHelper.UriPart.Query, redirText) != System.Net.HttpStatusCode.OK || error != null)
                        {
                            if (error is HttpRedirectNotAllowedException)
                            {
                                info.AddWarning(error.Message);
                                sts = VdO2013Core.OnLineStatus.Redirected;
                                error = null;
                            }
                            else
                            {
                                info.AddError(error);
                                sts = VdO2013Core.OnLineStatus.Offline;
                            }
                            reader.InvokeCB(worker, info, CB);
                        }
                        else
                        {
                            reader.PageItems.Add(url.ToString(), DoStaticReadListItem(reader, worker, info, CB, url.ToString()));
                            sts = VdO2013Core.OnLineStatus.OnLine;
                        }

                    }
                    finally
                    {
                        info.List[url] = sts;
                    }
                }//for (int i = 0; i < info.List.Count; i++)

                reader.Elementi = info.List.Count;
                return info.List.Count;
            }

            double activeItems = DoStaticGetTotalCount(reader, worker, info, CB, out var activeListPages);
            if (reader.Agenzia.Trim().Length <= 0) return -2;

            try
            {
                var itemProgressPercentage = 33.0 / activeItems; //Metto 33 invece che 100 perché lo scaricamento è 1/3 del lavoro da fare

                // Scorro le pagine della lista
                for (var pageListIndex = 0; pageListIndex < activeListPages.Length; pageListIndex++)
                {
                    var fPaginaListaUrl = new Uri(activeListPages[pageListIndex]);

                    info.AddInfo(string.Format(RS.TextRes.Reader.ReadLista.URLOPENING, activeListPages[pageListIndex]));
                    reader.InvokeCB(worker, info, CB);

                    if (HtmlHelper.OpenUrl(fPaginaListaUrl, out error) != System.Net.HttpStatusCode.OK || error != null)
                    {
                        if (error is HttpRedirectNotAllowedException)
                        {
                            info.AddWarning(error.Message);
                            error = null;
                        }
                        else
                        {
                            info.AddError(error);
                        }
                        reader.InvokeCB(worker, info, CB);
                        break; // Esco dal ciclo delle pagine
                    }

                    ////// Dalla prima pagina di ogni url ricavo la paginazione
                    ////if (pageListIndex == 1)
                    ////{
                    ////    String sPagination = HtmlHelper.FindNode(HtmlHelper.HtmlDocument.DocumentNode, getStringSetting(reader, ReaderSettings.KPageListItemCountXPath), true, out error)[0].InnerText;
                    ////    DoStaticReadListParsePagine(reader, sPagination);
                    ////    // Solo dalla prima pagina della prima url ricavo le informazioni del reader
                    ////    if (pageListIndex == 1)
                    ////        if (!DoStaticInitReader(reader, worker, info, CB))
                    ////            break; // Esco dal ciclo delle pagine
                    ////}

                    // Solo dalla prima pagina della prima url ricavo le informazioni del reader
                    if (pageListIndex == 0)
                        if (!DoStaticInitReader(reader, worker, info, CB))
                            break; // Esco dal ciclo delle pagine

                    var search = reader.Config.Get<string>(ReaderSettings.KPAGELIST_LISTXPATHNAME);
                    var pageListItems = HtmlHelper.FindNode(HtmlHelper.HtmlDocument.DocumentNode, search, true, out error);
                    info.AddError(error);
                    if (error is HttpRedirectNotAllowedException || pageListItems == null || pageListItems.Count == 0)
                        break; // Esco dal ciclo delle pagine

                    // Scorro gli elementi della lista corrente
                    for (var pageListItemIndex = 0; pageListItemIndex < pageListItems.Count; pageListItemIndex++)
                    {
                        info.AddInfo(info.LastItem.Percentage + itemProgressPercentage, string.Format(RS.TextRes.Reader.ReadLista.PROPOSTAFORMAT, pageListItemIndex));
                        reader.InvokeCB(worker, info, CB);

                        var attrs = pageListItems[pageListItemIndex].Attributes;
                        if (!attrs.Contains(RS.TextRes.DomSearch.TagAAttrHREF))
                        {
                            info.AddError("Element {0} does not contain {1} attribute.".FormatWith(pageListItemIndex, RS.TextRes.DomSearch.TagAAttrHREF));
                            continue;
                        }
                        var fPaginaListaItemLink = attrs[RS.TextRes.DomSearch.TagAAttrHREF].Value.ToString();
                        reader.PageItems.Add(fPaginaListaItemLink, DoStaticReadListItem(reader, worker, info, CB, fPaginaListaItemLink));
                        reader.Elementi++;
                    }//for (int pageListItemIndex = 0; ...

                    if (reader.Pagine <= 0 || pageListIndex >= reader.Pagine)
                        break; // Esco dal ciclo delle pagine
                }//for (int pageListIndex = 1; ...

                result = reader.PageItems.Count;
            }
            catch (Exception ex)
            {
                info.AddError(ex);
                reader.InvokeCB(worker, info, CB);
            }

            return result;
        }
        #endregion StaticReadList

        #region StaticSaveList
        //protected override Boolean StaticSaveListInitialize()
        //{
        //    StaticSaveList = new SaveListDelegate(DoStaticSaveList);
        //    return true;
        //}

        private static int DoStaticSaveListWriteData(ISiteReader2 reader,
            BackgroundWorker worker,
            ISiteReaderOnLineCheckDataList info,
            //Boolean isDataBound,
            string sql,
            string sqlParam0,
            string sqlParam1,
            string sqlParam2,
            out Exception error)
        {
            //if (isDataBound)
            //    return info.DataInfo.ExecuteNonQuery(String.Format(sql, sqlParam0.Quote(), sqlParam1.Quote(), sqlParam2.Quote()), out error);
            //else
            return DownloadData.Add(info.DataInfo.ReportData, sqlParam0, sqlParam1, sqlParam2, out error);
        }

        private static int DoStaticSaveList(ISiteReader2 reader, BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, SaveListCallBack CB)
        {
            info.AddInfo(RS.TextRes.Reader.SaveLista.INFO, RS.TextRes.Reader.SaveLista.STEP);
            reader.InvokeCB(worker, info, CB);
            Exception error = null;
            var sql = string.Empty;
            var webPart = string.Empty;
            var webValue = string.Empty;
            try
            {
                info.AddInfo(RS.TextRes.Reader.SaveLista.STEPSAVE);
                reader.InvokeCB(worker, info, CB);
                sql = string.Format("INSERT INTO [{0}]([Job],[Description],[Value]) VALUES({1},{2},{3});", info.DataInfo.ReportData.Table.TableName, "{0}", "{1}", "{2}");
                // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
                webPart = RS.TextRes.Reader.SaveLista.CODICEAGENZIA;
                webValue = reader.Agenzia;
                info.AddInfo(string.Format(RS.TextRes.Reader.SaveLista.SAVEWRITEFORMAT, webPart, webValue));
                reader.InvokeCB(worker, info, CB);
                DoStaticSaveListWriteData(reader, worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, webPart, webValue, out error);

                info.AddError(error);
                reader.InvokeCB(worker, info, CB);
                if (error != null) throw error;
                // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
                //webPart = RS.TextRes.Reader.SaveLista.NUMEROAGENZIA;
                //webValue = reader.Numero;
                //info.AddInfo(string.Format(RS.TextRes.Reader.SaveLista.SAVEWRITEFORMAT, webPart, webValue));
                //reader.InvokeCB(worker, info, CB);
                //DoStaticSaveListWriteData(reader, worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, webPart, webValue, out error);

                //info.AddError(error);
                //reader.InvokeCB(worker, info, CB);
                //if (error != null) throw error;
                // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
                webPart = RS.TextRes.Reader.SaveLista.RAGIONESOCIALE;
                webValue = reader.RagioneSociale;
                info.AddInfo(string.Format(RS.TextRes.Reader.SaveLista.SAVEWRITEFORMAT, webPart, webValue));
                reader.InvokeCB(worker, info, CB);
                DoStaticSaveListWriteData(reader, worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, webPart, webValue, out error);

                info.AddError(error);
                reader.InvokeCB(worker, info, CB);
                if (error != null) throw error;
                // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
                webPart = RS.TextRes.Reader.SaveLista.TIPOLOGIA;
                webValue = reader.Tipologia;
                info.AddInfo(string.Format(RS.TextRes.Reader.SaveLista.SAVEWRITEFORMAT, webPart, webValue));
                reader.InvokeCB(worker, info, CB);
                DoStaticSaveListWriteData(reader, worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, webPart, webValue, out error);

                info.AddError(error);
                reader.InvokeCB(worker, info, CB);
                if (error != null) throw error;
                // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
                webPart = RS.TextRes.Reader.SaveLista.ELEMENTI;
                //webValue = info.DataInfo.IsDataBound ? reader.Pagine.ToString() : reader.Elementi.ToString();
                webValue = reader.Elementi.ToString();
                info.AddInfo(string.Format(RS.TextRes.Reader.SaveLista.SAVEWRITEFORMAT, webPart, webValue));
                reader.InvokeCB(worker, info, CB);
                DoStaticSaveListWriteData(reader, worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, webPart, webValue, out error);

                info.AddError(error);
                reader.InvokeCB(worker, info, CB);
                if (error != null) throw error;
                // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****

                if (reader.PageItems != null)
                {
                    var itemProgressPercentage = 33.0 / (double)reader.PageItems.Count; //Metto 33 invece che 100 perché lo scaricamento è 1/3 del lavoro da fare
                    var pageIndex = 1;
                    foreach (var pageItemKey in reader.PageItems.AllKeys)
                    {
                        info.AddInfo(info.LastItem.Percentage + itemProgressPercentage, string.Format(RS.TextRes.Reader.SaveLista.ITEMFORMAT, pageItemKey, pageIndex));
                        reader.InvokeCB(worker, info, CB);
                        // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
                        var description = VdO2013SRCore.TextRes.tagOfferta;
                        var value = pageIndex.ToString();
                        DoStaticSaveListWriteData(reader, worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, description, value, out error);

                        if (error != null)
                            throw error;
                        // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
                        description = string.Format(RS.TextRes.Reader.SaveLista.ITEMNAMEFORMAT, VdO2013SRCore.TextRes.tagOfferta, pageIndex, OfferteData.Columns.Codice);
                        value = pageItemKey.Replace(reader.StandardSiteRootUrl + @"/", string.Empty).Left(pageItemKey.IndexOf('-')); //!!
                        DoStaticSaveListWriteData(reader, worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, description, value, out error);

                        if (error != null)
                            throw error;
                        // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
                        description = string.Format(RS.TextRes.Reader.SaveLista.ITEMNAMEFORMAT, VdO2013SRCore.TextRes.tagOfferta, pageIndex, OfferteData.Columns.Ordinal);
                        value = pageIndex.ToString();
                        DoStaticSaveListWriteData(reader, worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, description, value, out error);

                        if (error != null)
                            throw error;
                        // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
                        description = string.Format(RS.TextRes.Reader.SaveLista.ITEMNAMEFORMAT, VdO2013SRCore.TextRes.tagOfferta, pageIndex, OfferteData.Columns.Link);
                        value = pageItemKey.Replace(reader.StandardSiteRootUrl + @"/", string.Empty); //!!
                        DoStaticSaveListWriteData(reader, worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, description, value, out error);

                        if (error != null)
                            throw error;
                        // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
                        var features = reader.PageItems[pageItemKey] as Dictionary<string, string>;
                        foreach (var key in features.Keys)
                        {
                            info.AddInfo(string.Format(RS.TextRes.Reader.SaveLista.ITEMFEATUREFORMAT, key));
                            reader.InvokeCB(worker, info, CB);
                            // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
                            description = string.Format(RS.TextRes.Reader.SaveLista.ITEMFEATURENAMEFORMAT, VdO2013SRCore.TextRes.tagOfferta, pageIndex, VdO2013SRCore.TextRes.tagDettaglioB, key, VdO2013SRCore.TextRes.tagDettaglioE);
                            value = HtmlHelper.HtmlDecode(features[key]);//!!
                            DoStaticSaveListWriteData(reader, worker, info/*, info.DataInfo.IsDataBound*/, sql, info.JobName, description, value, out error);

                            if (error != null)
                                throw error;
                            // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
                        }

                        pageIndex++;
                    }
                }

                if (error != null) throw error;
            }
            catch (Exception ex)
            {
                info.AddError(ex);
                reader.InvokeCB(worker, info, CB);
            }

            return (int)info.LastItem.Percentage;
        }
        #endregion StaticSaveList

        public bool Running { get; private set; } = false;
        public override int ReadList(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, ReadListCallBack CB)
        {
            if (Running) return 0;
            Running = true;
            try
            {
                return DoStaticReadList(this, worker, info, CB);
            }
            finally
            {
                Running = false;
            }
        }

        public override int SaveList(BackgroundWorker worker, ISiteReaderOnLineCheckDataList info, SaveListCallBack CB)
        {
            if (Running) return 0;
            Running = true;
            try
            {
                return DoStaticSaveList(this, worker, info, CB);
            }
            finally
            {
                Running = false;
            }
        }

        #region Merge
        protected override string DefaultMergeIVDefinitionTypeName() { return "VdO2013Data.OfferteData, VdO2013Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"; }
        protected override MergeSettings DefaultMergeKeyFields()
        {
            var v = new MergeSettings(this) { Kind = MergeKind.IsKey, Context = MergeContext.Column, IVDefinitionTypeName = DefaultMergeIVDefinitionTypeName() };

            var cReaderName = new MergeSettings.MergeSetting() { ColumnName = OfferteData.Columns.ReaderName.ColumnName };
            v.Add(cReaderName);

            var cCodice = new MergeSettings.MergeSetting() { ColumnName = OfferteData.Columns.Codice.ColumnName };
            v.Add(cCodice);

            return v;
        }
        protected override MergeSettings DefaultMergeColExclude()
        {
            var v = new MergeSettings(this) { Kind = MergeKind.Exclude, Context = MergeContext.Column, IVDefinitionTypeName = DefaultMergeIVDefinitionTypeName() };

            var cOrdinal = new MergeSettings.MergeSetting() { ColumnName = OfferteData.Columns.Ordinal.ColumnName };
            v.Add(cOrdinal);

            return v;
        }
        protected override MergeSettings DefaultMergeRowExclude()
        {
            var v = new MergeSettings(this) { Kind = MergeKind.Exclude, Context = MergeContext.Row, IVDefinitionTypeName = DefaultMergeIVDefinitionTypeName() };

            var cStatus = new MergeSettings.MergeSetting() { ColumnName = OfferteData.Columns.Status.ColumnName, Value = OfferteItemStatus.Locked.ToString() };
            v.Add(cStatus);

            return v;
        }
        #endregion

        public override bool ApplyGlobalSettings(IWebSiteRepository repository)
        {
            try
            {
                this.Config.Set(VdO2013SR.TextRes.DefaultSettingsNames.KNAME, repository.Name);
                this.Config.Set(VdO2013SR.TextRes.DefaultSettingsNames.KCODE, repository.Guid.ToString());
                this.Config.Set(VdO2013SR.TextRes.DefaultSettingsNames.KNormalSiteUrl, repository.NormalUrl);
                this.Config.Set(VdO2013SR.TextRes.DefaultSettingsNames.KMobileSiteUrl, repository.MobileUrl);
                this.Config.Set(VdO2013SR.TextRes.DefaultSettingsNames.KVersion, repository.Version);
                this.Config.Set(VdO2013SR.TextRes.DefaultSettingsNames.KReleaseDate, repository.ReleaseDate);
                this.Config.Set(VdO2013SR.TextRes.DefaultSettingsNames.KRagioneSociale, repository.PageListRagioneSocialeXPath);

                this.Config.Set(ReaderSettings.KPAGELIST_URLNAME, repository.PageListUrl);
                this.Config.Set(ReaderSettings.KPAGELIST_URLREDIRECTSNAME, repository.PageListUrlRedirects);
                this.Config.Set(ReaderSettings.KPAGELIST_MAXCOUNTNAME, repository.PageListMaxCount);

                this.Config.Set(ReaderSettings.KPAGELIST_ITEMCOUNTXPATHNAME, repository.PageListItemCountXPath);
                this.Config.Set(ReaderSettings.KPAGELIST_LISTXPATHNAME, repository.PageListListXPath);
                this.Config.Set(ReaderSettings.KPAGELIST_PAGECOUNTXPATHNAME, repository.PageListPageCountXPath);
                this.Config.Set(ReaderSettings.KPAGELIST_PAGENUMBERXPATHNAME, repository.PageListPageNumberXPath);
                this.Config.Set(ReaderSettings.KPAGELIST_FEATURESENABLEDNAME, repository.PageListFeaturesEnabled);
                this.Config.Set(ReaderSettings.KPAGELIST_FEATURESXPATHNAME, repository.PageListFeaturesXPath);
                this.Config.Set(ReaderSettings.KPAGELIST_IMAGEMAXCOUNTNAME, repository.PageListMaxImageCount);
                this.Config.Set(ReaderSettings.KPAGELIST_IMAGEXPATHNAME, repository.PageListImageXPath);
                this.Config.Set(ReaderSettings.KPAGELIST_IMAGETHUMBLISTXPATHNAME, repository.PageListImageThumbNormalXPath);

                this.QRCode.Encoding = (QREncoding)repository.QRCodeEncoding;
                this.QRCode.Scale = repository.QRCodeScale;
                this.QRCode.Version = (QRVersion)repository.QRCodeVersion;
                this.QRCode.Correction = (QRErrorCorrection)repository.QRCodeCorrection;
                this.QRCode.CharacterSet = repository.QRCodeCharacterSet;
                this.QRCode.BackColor = Color.FromName(repository.QRCodeBackColor);
                this.QRCode.ForeColor = Color.FromName(repository.QRCodeForeColor);

                this.Config.Set(VdO2013SR.TextRes.DefaultSettingsNames.KMergeDefinitionName, repository.MergeDefinitionName);
                this.Config.Set(VdO2013SR.TextRes.DefaultSettingsNames.KMergeDefinitionKeyFields, repository.MergeDefinitionKeyFields);
                this.Config.Set(VdO2013SR.TextRes.DefaultSettingsNames.KMergeDefinitionColExclude, repository.MergeDefinitionColExclude);
                this.Config.Set(VdO2013SR.TextRes.DefaultSettingsNames.KMergeDefinitionRowExclude, repository.MergeDefinitionRowExclude);

#if DEBUG
                Logger.WriteDebug("{0}.Config[{1}]", this.Name, this.Config.Count);
                foreach (var c in this.Config)
                {
                    Logger.WriteDebug(">>Config[{0}]={1}", c.Key, c.Value);
                }
#endif
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return false;
            }
        }

        protected override bool CanUpdate => !Running;
    }
}
