//#define SiteReader2Factory

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections.ObjectModel;
using System.Reflection;

using System.IO;
using System.Data;

using MPUtils;

using VdO2013Core;
using VdO2013SRCore;
using VdO2013THCore;
using VdO2013TH;
using VdO2013WS;

namespace VdO2013SR
{
#if SiteReader2Factory
    /// <summary>
    /// This class cannot be created
    /// </summary>
    public abstract class SiteReader2Factory : FactoryBase<ISiteReader2, SiteReader2Factory>, ISiteReader2Factory
    {
        private const String RegisterMethodMissing = @"Static method 'bool {0}(ref {1} info)' is missing or not correctly declared in type {2}.";
        private const String RegisterMethodFailed = @"Static method 'bool {0}(ref {1} info)' returned false for {2}.";
        private const String RegisterMethodSuccess = @"'{0}' infos were registered.";

        public new static String AssemblyFilesPattern = @"VdO*Web*.dll";
        private static Dictionary<Type, ISiteReader2Info> siteReaderInfos = new Dictionary<Type, ISiteReader2Info>();
        //private static Guid _customerCode;

        /// <summary>
        /// This is the main static constructor for the factory: please consider it occours ONLY one time a this
        /// class level
        /// Put your Factory initialization code here
        /// </summary>
        static SiteReader2Factory()
        {
#if DEBUG
            MPLogHelper.FileLog.Default.WriteCtor(System.Reflection.MethodBase.GetCurrentMethod());
#endif
            try
            {
                FactoryBase<ISiteReader2, SiteReader2Factory>.AfterRegisterFuncs.Add(InternalReaderAfterRegister);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }// try
        }

        public static void ClearTypes()
        {
            if (ProtectedClearTypes() == 0)
                siteReaderInfos.Clear();
        }

        private static int InternalReaderAfterRegister(Type readerType)
        {
            ISiteReader2Info info = new SiteReader2Info() { Type = readerType };
            BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod;
            MethodInfo mi = readerType.GetMethod(TextRes.KRegisterMethodName, flags);
            if (mi == null)
            {
                Logger.WriteWarn(RegisterMethodMissing, TextRes.KRegisterMethodName, info.GetType(), readerType);
                return -1;
            }

            ParameterInfo[] mp = mi.GetParameters();
            if (mi != null && mp.Length == 1 && mp[0].ParameterType.IsByRef && mp[0].ParameterType.GetElementType().Equals(info.GetType()))
            {
                Object[] args = new Object[] { info };
                Object result = mi.Invoke(readerType, args);
                if (result is Boolean && !(Boolean)result)
                {
                    Logger.WriteWarn(RegisterMethodFailed, TextRes.KRegisterMethodName, info.GetType(), readerType);
                    return -2;
                }
                siteReaderInfos.Add(readerType, (ISiteReader2Info)args[0]);
                Logger.WriteInfo(RegisterMethodSuccess, readerType);
            }
            else
            {
                Logger.WriteWarn(RegisterMethodMissing, TextRes.KRegisterMethodName, info.GetType(), readerType);
            }

            return 0;
        }

        protected SiteReader2Factory() : base() { }

        public static void Register()
        {
            Logger.WriteWarn("Register {0} started:", typeof(SiteReader2Factory).AssemblyQualifiedName);

            Logger.WriteWarn("\tRegistering {0} ...", typeof(ISiteReader2).AssemblyQualifiedName);
            RegisterTypes(System.Reflection.Assembly.GetAssembly(typeof(ISiteReader2))); // Registers the default/empty T as the Default element

            Logger.WriteWarn("\tRegistering all '{0}' assembly in {1} ...", SiteReader2Factory.AssemblyFilesPattern, Global.ConfigPath);
            RegisterTypes(Global.ConfigPath, SiteReader2Factory.AssemblyFilesPattern);

            Logger.WriteWarn("Register {0} completed.", typeof(SiteReader2Factory).AssemblyQualifiedName);
        }

        public ISiteReader2 CreateReader(String assemblyQualifiedName, params Object[] args)
        {
            return CreateObject(assemblyQualifiedName, args);
        }

        public static ISiteReader2Info GetInfo(Type type)
        {
            siteReaderInfos.TryGetValue(type, out ISiteReader2Info result);
            return result;
        }

        public static ISiteReader2Info GetInfo(String assemblyQualifiedName)
        {
            var result = default(ISiteReader2Info);
            var infos = (from i in siteReaderInfos where i.Key.AssemblyQualifiedName.Equals(assemblyQualifiedName) select i.Value).ToList();
            if (infos != null && infos.Count > 0) result = infos[0];
            return result;
        }

        public static KeyValuePair<Type, ISiteReader2Info> GetReaderInfo(String assemblyQualifiedName)
        {
            return new KeyValuePair<Type, ISiteReader2Info>(RegisteredTypes[RegisteredTypeNames.IndexOf(assemblyQualifiedName)], GetInfo(assemblyQualifiedName));
        }
    }
#endif
}
