using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VdO2013SRCore
{
  using HtmlAgilityPack;

  public interface IHtmlNode
  {
    IDictionary<string, string> Attributes { get; }
    string InnerText { get; }
    string InnerHtml { get; }
    string Id { get; }
    bool Closed { get; }
    int Depth { get; }
    IHtmlNode FirstChild { get; }
    bool HasAttributes { get; }
    bool HasChildNodes { get; }
    int Line { get; }
    string Name { get; }
    string OuterHtml { get; }
    string XPath { get; }
    IHtmlNode ParentNode { get; }
    IEnumerable<IHtmlNode> SelectNodes(string xpath);
    IEnumerable<IHtmlNode> Descendants(string name);
    IEnumerable<IHtmlNode> ChildNodes { get; }
  }

  internal static class AgilityExtensions
  {
    public static IHtmlWeb GetWeb(this HtmlWeb value) => value != null ? new HtmlWebWrapper(value) : null;
    public static IHtmlDocument GetDocument(this HtmlDocument value) => value != null ? new HtmlDocumentWrapper(value) : null;
    public static IHtmlNode GetNode(this HtmlNode value) => value != null ? new HtmlNodeWrapper(value) : null;
  }
}
