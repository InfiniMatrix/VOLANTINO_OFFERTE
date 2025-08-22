using System;

namespace VdO2013SRCore
{
  using System.Net;

  public interface IHtmlWeb
  {
    HttpStatusCode StatusCode { get; }
    IHtmlDocument Load(string url, string proxyHost, int proxyPort, string userId, string password);
    IHtmlDocument Load(Uri uri);
    IHtmlDocument Load(string url);
    string Host { get; }
    Uri ResponseUri { get; }
  }
}
