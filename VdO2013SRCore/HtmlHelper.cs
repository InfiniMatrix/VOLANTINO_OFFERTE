using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace VdO2013SRCore
{
  public static class HtmlHelper
  {
    private static readonly MPLogHelper.FileLog Log = new MPLogHelper.FileLog(typeof(HtmlHelper));
    //public static readonly HtmlAgilityPack.HtmlWeb __html = new HtmlAgilityPack.HtmlWeb() { UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36", AutoDetectEncoding = true };
    public static readonly HtmlAgilityPack.HtmlWeb __html = new HtmlAgilityPack.HtmlWeb() { UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:x.x.x) Gecko/20041107 Firefox/x.x", AutoDetectEncoding = true };
    private static IHtmlDocument __document = null;

    public static IHtmlWeb Html { get => __html.GetWeb(); }
    public static IHtmlDocument Document { get => __document; }
    public static bool HasDocument { get => __document != null; }

    public static IHtmlNode DocumentNode => HasDocument ? __document.DocumentNode : null;
    public static bool HasDocumentNode { get => DocumentNode != null; }

    public static IHtmlNode HtmlNode => HasDocumentNode ? DocumentNode.ChildNodes.FirstOrDefault(n => n.Id == "html") : null;
    public static bool HasHtmlNode { get => HtmlNode != null; }

    public static IHtmlNode BodyNode => HasHtmlNode ? HtmlNode?.ChildNodes.FirstOrDefault(n => n.Id == "body") : null;
    public static bool HasHtmlBodyNode { get => BodyNode != null; }

    /// <summary>
    /// Mitigate error message: The underlying connection was closed: An unexpected error occurred on a send.
    /// If it only happens with HTTPS resources, you are targeting .Net 4, then it could have to do with the default SSL/TLS support. Try the following:
    /// </summary>
    static HtmlHelper()
    {
      //place this anywhere in your code prior to invoking the Web request
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
    }

    public static string HtmlDecode(string htmlText)
    {
      if (string.IsNullOrEmpty(htmlText))
        return null;
      
      var result = System.Web.HttpUtility.HtmlDecode(htmlText);
      foreach (TextRes.htmlDecodePart replace in TextRes.htmlDecodeParts)
        result = result.Replace(replace.htmlText, replace.plainText);
      
      return result;
    }

    public static string HtmlEncode(string simpleText)
    {
      if (string.IsNullOrEmpty(simpleText))
        return null;
      return System.Web.HttpUtility.HtmlEncode(simpleText);
    }

    public static string StaticDoGetHtml(Uri uri)
    {
      var req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
      using (var resp = req.GetResponse().GetResponseStream())
      using (var rdr = new System.IO.StreamReader(resp))
      {
        return rdr.ReadToEnd();
      }
    }

    public static string StaticDoGetHtml(string uri)
    {
      var req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
      using (var resp = req.GetResponse().GetResponseStream())
      using (var rdr = new System.IO.StreamReader(resp))
      {
        return rdr.ReadToEnd();
      }
    }

    public static string StaticDoGetHtmlNodeAttributesText(IHtmlNode nodeHtml)
    {
      string result = string.Empty;
      if (nodeHtml == null) return result;

      foreach (var a in nodeHtml.Attributes)
      {
        result += (string.IsNullOrEmpty(result) ? "" : " ") + a.Key + "='" + a.Value + "'";
      }
      return result;
    }

    [Flags]
    public enum UriPart : short
    {
      None = 0,
      Host = 1,
      Path = 2,
      Query = 4,
      Any = Host + Path + Query,
    }

    public static HttpStatusCode OpenUrl(Uri requestUri, out Exception error, UriPart allowedUriRedirectedParts = UriPart.None, string findCustomRedirectedText = null, bool allowHttps = true)
    {
      error = null;
      var me = System.Reflection.MethodBase.GetCurrentMethod();
      Log.WriteMethod(me, requestUri, error, allowedUriRedirectedParts, findCustomRedirectedText, allowHttps);

      if (requestUri is null)
      {
        throw new ArgumentNullException(nameof(requestUri));
      }

      error = null;
      var result = HttpStatusCode.Unused;
      try
      {
        if (Html == null) return result;
        __document = null;
        __document = Html.Load(requestUri.AbsoluteUri);

        if (HttpNotFoundException.ErrorStatuses.Contains(Html.StatusCode))
        {
          throw new HttpNotFoundException(Html.StatusCode, requestUri);
        }

        if (HttpTimeoutException.ErrorStatuses.Contains(Html.StatusCode))
        {
          throw new HttpTimeoutException(Html.StatusCode, requestUri);
        }

        if (VdO2013Core.Global.DebugLevel > 0 && __document != null)
        {
          var htmlFilePath = Path.Combine(VdO2013Core.Global.FileLogPath, DateTime.Now.ToString("yyyyMMdd_HHmmssFFF") + ".html");
          File.WriteAllText(htmlFilePath, __document.DocumentNode.OuterHtml);
        }

        var ok = Html.StatusCode == System.Net.HttpStatusCode.OK;
        var redirectedHost = !Html.ResponseUri.Host.ToLower().Equals(requestUri.Host.ToLower());
        var redirectedPath = !Html.ResponseUri.AbsolutePath.Replace("/", "").ToLower().Equals(requestUri.AbsolutePath.Replace("/", "").ToLower());
        var redirectedQuery = !Html.ResponseUri.Query.ToLower().Equals(requestUri.Query.ToLower());
        if (ok && !redirectedHost)
        {
          redirectedHost = HttpRedirectNotAllowedException.ErrorStatuses.Contains(Html.StatusCode);
        }

        if (redirectedHost && !string.IsNullOrEmpty(findCustomRedirectedText))
        {
          var htmlText = __document.DocumentNode.OuterHtml.ToLower();
          redirectedHost = htmlText.Contains(findCustomRedirectedText.ToLower());
        }

        if ((redirectedHost && !allowedUriRedirectedParts.HasFlag(UriPart.Host))
           || (redirectedPath && !allowedUriRedirectedParts.HasFlag(UriPart.Path))
           || (redirectedQuery && !allowedUriRedirectedParts.HasFlag(UriPart.Query)))
        {
          throw new HttpRedirectNotAllowedException(Html.StatusCode
            , requestUri
            , new Exception($"Redirect not allowed: url '{requestUri.AbsoluteUri}' was requested but '{Html.ResponseUri}' was returned."));
        }

        if (Html != null)
        {
          result = Html.StatusCode;
        }
      }
#pragma warning disable CA1031 // Do not catch general exception types
      catch (Exception ex)
      {
        error = ex;
      }
#pragma warning restore CA1031 // Do not catch general exception types
      return result;
    }

    public static IEnumerable<IHtmlNode> FindNode(IHtmlNode fromNode, string searchText, bool xpath, out Exception error, bool throwIfEmpty = true)
    {
      error = null;
      if (__document == null)
      {
        error = new Exception(TextRes.htmlExceptionNoDocument);
        return null;
      }

      try
      {
        var node = fromNode ?? __document.DocumentNode;
        var nodes = xpath ? node.SelectNodes(searchText) : node.Descendants(searchText);

        if (throwIfEmpty && (nodes == null || !nodes.Any()))
        {
          throw new InvalidOperationException($"Search text '{searchText}'(xpath={xpath}) did not return any node.");
        }
        return nodes;
      }
      catch (System.Xml.XPath.XPathException Ex)
      {
        error = new System.Xml.XPath.XPathException("Error finding '{0}'".FormatWith(searchText), Ex);
      }
#pragma warning disable CA1031 // Do not catch general exception types
      catch (Exception Ex)
      {
        error = Ex;
      }
#pragma warning restore CA1031 // Do not catch general exception types
      return null;
    }

    public static string FindNode_InnerText(IHtmlNode fromNode, string searchText, bool xpath, int itemIndex, string nullText, out Exception error)
    {
      try
      {
        var ns = FindNode(fromNode, searchText, xpath, out error);
        if (error == null && ns != null && ns.Count() > itemIndex)
          return ns.ElementAt(itemIndex).InnerText;
        else
          return nullText;
      }
#pragma warning disable CA1031 // Do not catch general exception types
      catch (Exception ex)
      {
        error = ex;
        return nullText;
      }
#pragma warning restore CA1031 // Do not catch general exception types
    }
  }
}