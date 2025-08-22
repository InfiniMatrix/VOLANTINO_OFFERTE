using System;

namespace VdO2013SRCore
{
  using HtmlAgilityPack;

  internal class HtmlDocumentWrapper : IHtmlDocument
  {
    private readonly HtmlDocument _document;

    public HtmlDocumentWrapper(HtmlDocument document)
    {
      _document = document ?? throw new ArgumentNullException(nameof(document));
    }
    public IHtmlNode DocumentNode { get => _document.DocumentNode?.GetNode(); }
    public string Text { get => _document.Text; }

    public IHtmlNode GetElementbyId(string id) => _document.GetElementbyId(id)?.GetNode();
  }
}
