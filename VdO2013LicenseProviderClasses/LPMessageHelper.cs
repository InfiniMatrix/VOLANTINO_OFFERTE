using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.IO;

namespace VdO2013LicenseProviderClasses
{
    internal class LPMessageHelper
    {
        public static String GetTypeName(Type type, String emptyTypeName) { return type != null ? type.FullName : emptyTypeName; }
        public static TypeCode GetTypeCode(Type type, TypeCode emptyTypeCode) { return type != null ? Type.GetTypeCode(type) : emptyTypeCode; }

        public static Boolean ValueTryReadXml(Object value, out String result)
        {
            result = null;
            Boolean canSerialize = value != null && ValueIsXmlData(value.GetType());
            if (!canSerialize) return false;

            if (value.GetType().Equals(typeof(DataSet)))
            {
                var ds = value as DataSet;
                using (MemoryStream ms = new MemoryStream())
                {
                    ds.WriteXml(ms, XmlWriteMode.WriteSchema);
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        ms.Position = 0;
                        result = sr.ReadToEnd();
                    }
                }
            }
            else
                if (value.GetType().Equals(typeof(DataTable)))
                {
                    var dt = value as DataTable;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        dt.WriteXml(ms, XmlWriteMode.WriteSchema);
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            ms.Position = 0;
                            result = sr.ReadToEnd();
                        }
                    }
                }
                else
                    if (value.GetType().Equals(typeof(DataView)))
                    {
                        var dv = value as DataView;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            dv.ToTable().WriteXml(ms, XmlWriteMode.WriteSchema);
                            using (StreamReader sr = new StreamReader(ms))
                            {
                                ms.Position = 0;
                                result = sr.ReadToEnd();
                            }
                        }
                    }
                    else
                        return false;

            return result != null;
        }

        public static Boolean ValueIsXmlData(Type valueType)
        {
            return valueType == null ? false : (valueType.Equals(typeof(DataTable)) || valueType.Equals(typeof(DataSet))) || valueType.Equals(typeof(DataView));
        }
    }
}
