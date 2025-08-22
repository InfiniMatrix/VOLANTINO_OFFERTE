using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;

namespace VdO2013LicenseProviderClasses
{
    /// <summary>
    /// Gestisce la creazione id numeri progressivi per tutte le entità create
    /// Implementa alcuni metodi di supporto
    /// </summary>
    internal class LPCommon
    {
        #region Identities
        private static Dictionary<LPIdentityKind, Int64> _identities;

        public static Dictionary<LPIdentityKind, Int64> Identities
        {
            get
            {
                if (_identities == null)
                {
                    _identities = new Dictionary<LPIdentityKind, Int64>();

                    foreach (LPIdentityKind k in Enum.GetValues(typeof(LPIdentityKind)))
                        _identities.Add(k, 0);
                }
                return _identities;
            }
        }
        
        public static String GetLastID(LPIdentityKind forKind)
        {
            return Identities.ContainsKey(forKind) ? _identities[forKind].ToString() : "-1";
        }
        
        public static String GetNewID(LPIdentityKind forKind)
        {
            Int64 newId = Int64.Parse(GetLastID(forKind)) + 1;
            Identities[forKind] = newId;
            return newId.ToString();
        }
        #endregion Identities

        public static String CurrentRequestServerVariablesHost(HttpContext current)
        {
            return current != null ? @"http://" + current.Request.ServerVariables["HTTP_HOST"] : String.Empty;
        }
        
        public static String GetRelativeUri(HttpContext current, String absoluteUri)
        {
            String host = CurrentRequestServerVariablesHost(current);
            return !String.IsNullOrEmpty(host) ? absoluteUri.Replace(host, "~") : absoluteUri;
        }

        public static String GetAbsoluteUri(HttpContext current, String relativeUri)
        {
            String host = CurrentRequestServerVariablesHost(current);
            return !String.IsNullOrEmpty(host) ? relativeUri.Replace("~", host) : relativeUri;
        }
    }
}
