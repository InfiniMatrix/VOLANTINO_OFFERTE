namespace VdO2013SRCore
{
  public interface IHtmlDocument
  {
    IHtmlNode DocumentNode { get; }
    string Text { get; }
    IHtmlNode GetElementbyId(string id);
  }
}
