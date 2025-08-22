using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VdO2013SRCore
{
  public interface ISiteCrawlerSettings : ICollection, IEnumerable
  {
  }

  public abstract class SiteCrawlerSettings : NameValueCollection, ISiteCrawlerSettings
  {
    public SiteCrawlerSettings()
    {

    }
  }

  public interface ISiteCrawler : IDisposable
  {
    ISiteCrawlerSettings GetSettings();
    bool Initialized { get; }
    
    event EventHandler OnCreating;
    event EventHandler OnDisposing;
  }

  public static class SiteCrawlerFactory<TSiteCrawler> where TSiteCrawler : ISiteCrawler
  {
    readonly static IDictionary<Type, ISiteCrawler> _site_crawlers = new Dictionary<Type, ISiteCrawler>();
    static SiteCrawlerFactory()
    {
    }

    public static ISiteCrawler Register(Type siteCrawlerType, params object[] siteCrawlerCtorParameters)
    {
      if (typeof(ISiteCrawler).IsAssignableFrom(siteCrawlerType ?? throw new ArgumentNullException(nameof(siteCrawlerType))))
        throw new InvalidOperationException($"Type {siteCrawlerType} is not a {typeof(ISiteCrawler)}.");

      if (IsRegistered(siteCrawlerType))
        throw new InvalidOperationException($"Type {siteCrawlerType} is already registered.");

      ISiteCrawler instance;
      var ctor = siteCrawlerType.GetConstructor(siteCrawlerCtorParameters);
      if (ctor != null)
      {
        instance = (ISiteCrawler)ctor.Invoke(siteCrawlerCtorParameters);
      }
      else
        instance = (ISiteCrawler)Activator.CreateInstance(siteCrawlerType);

      _site_crawlers.Add(siteCrawlerType, instance);

      return instance;
    }

    public static bool Unregister(Type siteCrawlerType, bool throwIfMissing = false)
    {
      if (!IsRegistered(siteCrawlerType) && throwIfMissing)
        throw new InvalidOperationException($"Type {siteCrawlerType} is not registered.");

      var instance = _site_crawlers[siteCrawlerType];
      _site_crawlers.Remove(siteCrawlerType);
      instance.Dispose();
      instance = null;
      return instance == null;
    }

    public static IEnumerable<Type> RegisteredTypes() => _site_crawlers.Keys;
    public static IEnumerable<ISiteCrawler> RegisteredCrawlers() => _site_crawlers.Values;
    public static bool IsRegistered(Type siteCrawlerType) => _site_crawlers.ContainsKey(siteCrawlerType);
    public static bool IsRegistered(ISiteCrawler siteCrawler) => _site_crawlers.Values.Contains(siteCrawler);
    public static ISiteCrawler Get(Type siteCrawlerType) => _site_crawlers.ContainsKey(siteCrawlerType) ? _site_crawlers[siteCrawlerType] : default;
    public static int Count() => _site_crawlers.Count();
  }
}
