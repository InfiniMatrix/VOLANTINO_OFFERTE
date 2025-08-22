//using VdO2013Main.SiteReader;

using SR = VdO2013SRCore;
using System.Drawing;

namespace VdO2013WebReaderIMMOBILIAREdotIT
{
    public static class ReaderSettingsDefaults
    {
        internal const string KURLNORMALVALUE = @"https://www.immobiliare.it";
        internal const string KURLMOBILEVALUE = @"https://m.immobiliare.it";
        internal static readonly Image KLOGOVALUE = Properties.Resources.IMMOBILIAREdotITReader;
        internal static readonly Image KLOGOFULLVALUE = Properties.Resources.IMMOBILIAREdotITReaderFull;
        internal const string KPAGELIST_URLVALUE = @"/agenzie_immobiliari/" + SR.TextRes.WebSite.KTAGNUMEROAGENZIA + @"/" + SR.TextRes.WebSite.KTAGCODICEAGENZIA + @"/?pag=" + SR.TextRes.WebSite.KTAGINDICELISTA;
        internal const string KPAGELIST_LISTXPATHVALUE = @"//*[@id='risultati']/div//*[@class='wrapper_riga_annuncio']//*[@class='annuncio_title']//a";
    }
}
