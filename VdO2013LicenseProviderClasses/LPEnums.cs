using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.Serialization.Advanced;

namespace VdO2013LicenseProviderClasses
{
    [Serializable]
    public enum LPIdentityKind
    {
        Unknown,
        MagicCode,
        Credential,
        Result
    }

    [Serializable]
    public enum LPBoolean
    {
        Unassigned = 0,
        False = -1,
        True = 1
    }

    public class SerializableEnums
    {
        public LPIdentityKind Kind;
        public LPBoolean Bool;
    }
}