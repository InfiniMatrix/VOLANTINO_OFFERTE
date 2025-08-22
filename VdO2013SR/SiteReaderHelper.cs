using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

using VdO2013Core;
using VdO2013Core.Config;
using VdO2013SRCore;

using VdO2013WS;

namespace VdO2013SR
{
    public static class SiteReaderHelper
    {
        private static MPLogHelper.FileLog Logger = new MPLogHelper.FileLog(System.Reflection.MethodBase.GetCurrentMethod());
        static SiteReaderHelper()
        {
            MPLogHelper.FileLog.Default.WriteCtor(System.Reflection.MethodBase.GetCurrentMethod());
        }
        
        //public static String GetLogoFileName(Type siteReaderType)
        //{
        //    return System.IO.Path.ChangeExtension(ConfigSettingsHelper.GetFileName(siteReaderType), ".png");
        //}

        public static PropertyInfo[] GetPropertyInfos(Type type, BindingFlags flags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            return type.GetProperties(flags);
        }

        public static CustomAttributeData[] GetPropertyCustomAttributes(PropertyInfo propInfo)
        {
            return propInfo.GetCustomAttributesData().ToArray();
        }

        public static int DisplayProperties(ISiteReader2 reader, BindingFlags flags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            PropertyInfo[] pInfos = GetPropertyInfos(reader.GetType(), flags);

            // sort properties by name
            Array.Sort(pInfos, delegate(PropertyInfo pi1, PropertyInfo pi2) { return pi1.Name.CompareTo(pi2.Name); });

            Logger.WriteInfo("Type: {0}", reader.GetType().AssemblyQualifiedName);
            // write property names
            foreach (PropertyInfo pInfo in pInfos)
            {
                Boolean isPropStatic = (pInfo.GetGetMethod() != null && pInfo.GetGetMethod().IsStatic) || (pInfo.GetSetMethod() != null && pInfo.GetSetMethod().IsStatic);
                Logger.WriteInfo("\tProperty: {0}\tIsStatic: {1}\tCanRead: {2}\tCanWrite: {3}\tPropertyType: {4}\t", MPLogHelper.LogKind.Information, pInfo.Name, isPropStatic, pInfo.CanRead, pInfo.CanWrite, pInfo.PropertyType);
            }

            return pInfos.Count();
        }

        public static Boolean HasMethod(this Type readerType, String methodName)
        {
            //if (!readerType.IsSubclassOf(typeof(SiteReader2)))
            //    throw new InvalidCastException("{0} is not a {1} descendant.".FormatWith(readerType.FullName, typeof(SiteReader2)));

            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            return readerType.GetMethod(methodName, flags) != null;
        }
        public static Boolean HasMethod(this ISiteReader2 reader, String methodName)
        {
            return HasMethod(reader.GetType(), methodName);
        }

        public static Boolean CanRegister(this Type readerType)
        {
            return HasMethod(readerType, TextRes.KRegisterMethodName);
        }
        public static Boolean CanRegister(this ISiteReader2 reader)
        {
            return HasMethod(reader, TextRes.KRegisterMethodName);
        }
    }
}
