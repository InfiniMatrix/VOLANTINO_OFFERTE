using System;
using System.Collections.Generic;
using System.Linq;

namespace VdO2013SRCore
{
  using HtmlAgilityPack;

  internal class HtmlNodeWrapper : IHtmlNode
  {
    private readonly HtmlNode _node;

    public HtmlNodeWrapper(HtmlNode node)
    {
      _node = node ?? throw new ArgumentNullException(nameof(node));
    }
    public int Depth { get => _node.Depth; }
    public string InnerText => _node.InnerText;
    public string InnerHtml => _node.InnerHtml;
    public string Id => _node.Id;
    public bool Closed => _node.Closed;

    public IHtmlNode FirstChild => _node.FirstChild.GetNode();
    public bool HasAttributes => _node.HasAttributes;
    public bool HasChildNodes => _node.HasChildNodes;
    public int Line => _node.Line;
    public string Name => _node.Name;
    public IHtmlNode ParentNode => _node.ParentNode.GetNode();
    public string OuterHtml => _node.OuterHtml;
    public string XPath => _node.XPath;
    public IDictionary<string, string> Attributes => _node.Attributes.ToDictionary(k => k.Name, v => v.Value);

    public IEnumerable<IHtmlNode> SelectNodes(string xpath) => from n in _node.SelectNodes(xpath) select n.GetNode();
    public IEnumerable<IHtmlNode> Descendants(string name) => from n in _node.Descendants(name) select n.GetNode();
    public IEnumerable<IHtmlNode> ChildNodes => from n in _node.ChildNodes select n.GetNode();
  }
}
