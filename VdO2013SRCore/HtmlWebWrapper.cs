using System;

namespace VdO2013SRCore
{
  using HtmlAgilityPack;
  using System.Net;

  internal class HtmlWebWrapper : IHtmlWeb
  {
    private readonly HtmlWeb _web;
    public HtmlWebWrapper(HtmlWeb web)
    {
      _web = web;
    }

    public string AbsoluteUri => ResponseUri?.AbsoluteUri;
    public string Host => ResponseUri.Host;
    public Uri ResponseUri => _web.ResponseUri;

    public HttpStatusCode StatusCode => _web.StatusCode;
    public IHtmlDocument Load(string url, string proxyHost, int proxyPort, string userId, string password)
      => _web.Load(url, proxyHost, proxyPort, userId, password).GetDocument();
    public IHtmlDocument Load(Uri uri)
      => _web.Load(uri).GetDocument();
    public IHtmlDocument Load(string url)
      => _web.Load(url).GetDocument();
  }
}
