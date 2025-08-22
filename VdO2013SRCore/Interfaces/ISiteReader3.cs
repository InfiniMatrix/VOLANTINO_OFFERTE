using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using MPUtils;
using VdO2013Core;
using VdO2013Core.Config;
using VdO2013SRCore;
using VdO2013SRCore.Specialized;
using MPUtils.PluginFactory;

namespace VdO2013SRCore
{
    public interface ISiteReader3 : ISiteReader2, IPlugin
    {
        int MAXPAGEDEFAULT { get; }

        Guid Code { get; }
        string Numero { get; set; }
        string ConfigPath { get; }
        string DefaultConfigPath { get; }
        string DefaultDataPath { get; }
        string DefaultImagesPath { get; }
        string DefaultReportPath { get; }
        DateTime LoadTimeStamp { get; }
        DateTime SaveTimeStamp { get; }
        DateTime SettingsReleaseDate { get; }
        string SettingsVersion { get; }

        string PageListFeaturesXPath { get; set; }
        bool PageListFeaturesEnabled { get; set; }

        string PropertiesFile { get; }

        bool Running { get; }
        Type GetSiteReader2Type();
    }
}