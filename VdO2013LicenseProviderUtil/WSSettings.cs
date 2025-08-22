#if DEBUG
#define LOCALWEBSERVICE
#endif

namespace VdO2013LicenseProviderUtil
{
    internal static class WSSettings
    {

#if LOCALWEBSERVICE
#if LICENSEPROVIDER_VER1
        public static string WSActivationUri = global::VdO2013LicenseProviderUtil.Properties.Settings.Default.WSActivation;
#elif LICENSEPROVIDER_VER2
        public static string WSActivationUri = global::VdO2013LicenseProviderUtil.Properties.Settings.Default.WSActivation2;
#elif LICENSEPROVIDER_VER3
        public static string WSActivationUri = global::VdO2013LicenseProviderUtil.Properties.Settings.Default.WSActivation3;
#endif
#else
#if LICENSEPROVIDER_VER1
        public static string WSActivationUri = global::VdO2013LicenseProviderUtil.Properties.Settings.Default.WSActivationOnLine;
#elif LICENSEPROVIDER_VER2
        public static string WSActivationUri = global::VdO2013LicenseProviderUtil.Properties.Settings.Default.WSActivationOnLine2;
#elif LICENSEPROVIDER_VER3
        public static string WSActivationUri = global::VdO2013LicenseProviderUtil.Properties.Settings.Default.WSActivationOnLine3;
#endif
#endif
    }

}