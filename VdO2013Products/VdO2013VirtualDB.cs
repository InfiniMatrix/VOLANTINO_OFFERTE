using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml.Serialization.Advanced;

using MPCommonRes;
using MPFingerPrint;

namespace VdO2013Products
{
    using LPConsts = MPCommonRes.TextRes.LicenseProvider;
    public class VdO2013
    {
        /// <summary>
        /// Encrypts the value: if error is generated, the message is returned into result parameter
        /// </summary>
        /// <param name="value">value to encrypt</param>
        /// <param name="result">encrypted value or error message</param>
        /// <returns>returns true if encryption was succesfull</returns>
        public static bool Encrypt(string value, out string result)
        {
            result = string.Empty;
            try
            {
                result = FingerPrint.Encrypt(value, LPConsts.PasswordHash, LPConsts.SaltKey, LPConsts.VIKey);
                return true;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Decrypts the value: if error is generated, the message is returned into result parameter
        /// </summary>
        /// <param name="value">value to decrypt</param>
        /// <param name="result">plain value or error message</param>
        /// <returns>returns true if decryption was succesfull</returns>
        public static bool Decrypt(string value, out string result)
        {
            result = string.Empty;
            try
            {
                result = FingerPrint.Decrypt(value, LPConsts.PasswordHash, LPConsts.SaltKey, LPConsts.VIKey);
                return true;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }

        public static object ParseNull(object value)
        {
            if (value == null || DBNull.Value.Equals(value))
                return DBNull.Value;
            return value;
        }

        public static bool IsNullOrDBNull(object value)
        {
            return value == null || DBNull.Value.Equals(value);
        }

        public static string PluralizeName(string singularName)
        {
            string prefix = singularName.Left(singularName.Length - 1);
            if (singularName.EndsWith("a"))
                return prefix + "e";
            else
                if (singularName.EndsWith("o") || singularName.EndsWith("e"))
                return prefix.EndsWith("i") ? prefix : prefix + "i";

            return singularName;
        }

        #region DataSet
        public class DataSet
        {
            public const string Name = LPConsts.DataSet.Name;

            private static Dictionary<Type, string> _tables;
            public static Dictionary<Type, string> TABLES
            {
                get
                {
                    if (_tables == null)
                        _tables = new Dictionary<Type, string>()
                        {
                            { typeof(Privilege), Privilege.Name }
                            , { typeof(Context), Context.Name }
                            , { typeof(User), User.Name }
                            , { typeof(Suite), Suite.Name }
                            , { typeof(Product), Product.Name }
                            , { typeof(Customer), Customer.Name }
                            , { typeof(Activation), Activation.Name }
                            , { typeof(Key), Key.Name }
                            , { typeof(WebSite), WebSite.Name }
                            , { typeof(WebSiteFeature), WebSiteFeature.Name }
                            , { typeof(WebSiteScript), WebSiteScript.Name }
                        };
                    return _tables;
                }
            }

            public static TableInfo Populate(Type tableType)
            {
                if (TABLES.ContainsKey(tableType))
                {
                    BindingFlags bf = BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod;
                    MethodInfo mi = tableType.GetMethod(LPConsts.DataSet.PopulateMethodName, bf);
                    if (mi != null)
                    {
                        object result = tableType.InvokeMember(LPConsts.DataSet.PopulateMethodName, bf, null, tableType, null);
                        if (result != null && result is TableInfo)
                        {
                            return result as TableInfo;
                        }
                    }
                }
                return null;
            }

            public static ColumnInfo[] COLUMNS(Type tableType)
            {
                if (TABLES.ContainsKey(tableType))
                {
                    BindingFlags bf = BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty;
                    PropertyInfo pi = tableType.GetProperty(LPConsts.DataSet.ColumnsPropertyName, bf);
                    if (pi != null)
                    {
                        object result = tableType.InvokeMember(LPConsts.DataSet.ColumnsPropertyName, bf, null, tableType, null);
                        if (result != null && result is ColumnInfo[])
                        {
                            return result as ColumnInfo[];
                        }
                    }
                }
                return null;
            }

            public static string[] COLUMNNAMES(Type tableType)
            {
                if (TABLES.ContainsKey(tableType))
                {
                    var names = from ci in COLUMNS(tableType) select ci.Name;
                    return names.ToArray();
                }
                return null;
            }
        }
        #endregion DataSet

        #region Context
        public class Context
        {
            public const string Name = LPConsts.DataSet.Context.Name;

            public class Columns : ColumnsBase
            {
                public const string Description = LPConsts.DataSet.Context.Columns.Description;
                public const string ActiveFrom = LPConsts.DataSet.Context.Columns.ActiveFrom;
                public const string ActiveUntil = LPConsts.DataSet.Context.Columns.ActiveUntil;
                public const string Notes = LPConsts.DataSet.Context.Columns.Notes;

                private static string[] _names;
                public static string[] NAMES
                {
                    get
                    {
                        if (_names == null)
                            _names = new string[] { ID
                            , Code
                            , Description
                            , ActiveFrom
                            , ActiveUntil
                            , Notes
                        };
                        return _names;
                    }
                }
            }

            private static ColumnInfo[] _columns;
            public static ColumnInfo[] COLUMNS
            {
                get
                {
                    if (_columns == null) _columns = new ColumnInfo[] {
                        ColumnsBase.CreateID()
                        , ColumnsBase.CreateCode()
                        , ColumnInfo.CreateString(Columns.Description, 25, true)
                        , ColumnInfo.CreateDateTime(Columns.ActiveFrom, false)
                        , ColumnInfo.CreateDateTime(Columns.ActiveUntil, true)
                        , ColumnInfo.CreateString(Columns.Notes, 4096, true)
                    };
                    return _columns;
                }
            }

            public static RowInfo PopulateRow(long id, string description, DateTime activeFrom, DateTime? activeUntil = null, string notes = null)
            {
                var result = new RowInfo();
                foreach (var c in COLUMNS)
                {
                    switch (c.Name)
                    {
                        case Columns.ID:
                            result.Add(c, id);
                            break;
                        case Columns.Code:
                            result.Add(c, ColumnInfo.GetDefaultForGuid());
                            break;
                        case Columns.Description:
                            result.Add(c, description);
                            break;
                        case Columns.ActiveFrom:
                            result.Add(c, activeFrom);
                            break;
                        case Columns.ActiveUntil:
                            if (activeUntil != null) result.Add(c, (DateTime)activeUntil);
                            break;
                        case Columns.Notes:
                            result.Add(c, notes);
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }

            private static TableInfo _population;

            public static TableInfo Populate()
            {
                if (_population == null)
                {
                    _population = new TableInfo
                    {
                        PopulateRow(LPConsts.DataSet.Context.DefaultAdmins.id
                        , LPConsts.DataSet.Context.DefaultAdmins.description
                        , ColumnInfo.GetDefaultForDate()),

                        PopulateRow(LPConsts.DataSet.Context.DefaultCustomers.id
                        , LPConsts.DataSet.Context.DefaultCustomers.description
                        , ColumnInfo.GetDefaultForDate())
                    };
                }
                return _population;
            }

            public static TableInfo GetContext(TableInfo Context, string description)
            {
                var customerProducts = from RowInfo s in Context
                                       where s.GetValueIsEqualTo(Columns.Description, description)
                                       select s;

                return customerProducts.ToList<RowInfo>() as TableInfo;
            }
        }
        #endregion Context

        #region User
        public class User
        {
            public const string Name = LPConsts.DataSet.User.Name;

            public class Columns : ColumnsBase
            {
                public const string Login = LPConsts.DataSet.User.Columns.Login;
                public const string Password = LPConsts.DataSet.User.Columns.Password;
                public const string Context = LPConsts.DataSet.User.Columns.Context;
                public const string Email = LPConsts.DataSet.User.Columns.Email;
                public const string ActiveFrom = LPConsts.DataSet.User.Columns.ActiveFrom;
                public const string ActiveUntil = LPConsts.DataSet.User.Columns.ActiveUntil;
                public const string Notes = LPConsts.DataSet.User.Columns.Notes;

                private static string[] _names;
                public static string[] NAMES
                {
                    get
                    {
                        if (_names == null)
                            _names = new string[] { ID
                            , Code
                            , Login
                            , Password
                            , Context
                            , Email
                            , ActiveFrom
                            , ActiveUntil
                            , Notes
                        };
                        return _names;
                    }
                }
            }

            private static ColumnInfo[] _columns;
            public static ColumnInfo[] COLUMNS
            {
                get
                {
                    if (_columns == null) _columns = new ColumnInfo[] {
                        ColumnsBase.CreateID()
                        , ColumnsBase.CreateCode()
                        , ColumnInfo.CreateString(Columns.Login, 25, false, true)
                        , ColumnInfo.CreateString(Columns.Password, 25, true)
                        , ColumnInfo.CreateString(Columns.Context, 25, true, false, string.Format(ColumnInfo.ForeignKeyReferenceFormat, Context.Name, Context.Columns.Description), Context.Columns.Description)
                        , ColumnInfo.CreateString(Columns.Email, 255, false, true)
                        , ColumnInfo.CreateDateTime(Columns.ActiveFrom, false)
                        , ColumnInfo.CreateDateTime(Columns.ActiveUntil, true)
                        , ColumnInfo.CreateString(Columns.Notes, 4096, true)
                    };
                    return _columns;
                }
            }

            public static RowInfo PopulateRow(long id, string login, string password, string context, string email, DateTime activeFrom, DateTime? activeUntil = null, string notes = null)
            {
                var result = new RowInfo();
                foreach (var c in COLUMNS)
                {
                    switch (c.Name)
                    {
                        case Columns.ID:
                            result.Add(c, id);
                            break;
                        case Columns.Code:
                            result.Add(c, ColumnInfo.GetDefaultForGuid());
                            break;
                        case Columns.Login:
                            result.Add(c, login);
                            break;
                        case Columns.Password:
                            result.Add(c, password);
                            break;
                        case Columns.Context:
                            result.Add(c, context);
                            break;
                        case Columns.Email:
                            result.Add(c, email);
                            break;
                        case Columns.ActiveFrom:
                            result.Add(c, activeFrom);
                            break;
                        case Columns.ActiveUntil:
                            if (activeUntil != null) result.Add(c, (DateTime)activeUntil);
                            break;
                        case Columns.Notes:
                            result.Add(c, notes);
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }

            private static TableInfo _population;
            public static TableInfo Populate()
            {
                if (_population == null)
                {
                    _population = new TableInfo
                    {
                        PopulateRow(LPConsts.DataSet.User.DefaultAdmin.id
                        , LPConsts.DataSet.User.DefaultAdmin.login
                        , LPConsts.DataSet.User.DefaultAdmin.password
                        , LPConsts.DataSet.User.DefaultAdmin.context
                        , LPConsts.DataSet.User.DefaultAdmin.email
                        , ColumnInfo.GetDefaultForDate()),

                        PopulateRow(LPConsts.DataSet.User.DefaultCustomer.id
                        , LPConsts.DataSet.User.DefaultCustomer.login
                        , LPConsts.DataSet.User.DefaultCustomer.password
                        , LPConsts.DataSet.User.DefaultCustomer.context
                        , LPConsts.DataSet.User.DefaultCustomer.email
                        , ColumnInfo.GetDefaultForDate())
                    };
                }
                return _population;
            }

            public static TableInfo GetUser(TableInfo Users, string login)
            {
                var customerProducts = from RowInfo s in Users
                                       where s.GetValueIsEqualTo(Columns.Login, login)
                                       select s;

                return customerProducts.ToList<RowInfo>() as TableInfo;
            }
        }
        #endregion User

        #region Suite
        public class Suite
        {
            public const string Name = LPConsts.DataSet.Suite.Name;

            public class Columns : ColumnsBase
            {
                public const string Description = LPConsts.DataSet.Suite.Columns.Description;
                public const string Version = LPConsts.DataSet.Suite.Columns.Version;
                public const string Build = LPConsts.DataSet.Suite.Columns.Build;
                public const string ReleaseDate = LPConsts.DataSet.Suite.Columns.ReleaseDate;
                public const string ExpireDate = LPConsts.DataSet.Suite.Columns.ExpireDate;

                private static string[] _names;
                public static string[] NAMES
                {
                    get
                    {
                        if (_names == null)
                            _names = new string[] { ID
                            , Code
                            , Description
                            , Version
                            , Build
                            , ReleaseDate
                            , ExpireDate
                        };
                        return _names;
                    }
                }
            }

            private static ColumnInfo[] _columns;
            public static ColumnInfo[] COLUMNS
            {
                get
                {
                    if (_columns == null) _columns = new ColumnInfo[] {
                        ColumnsBase.CreateID()
                        , ColumnsBase.CreateCode()
                        , ColumnInfo.CreateString(Columns.Version, 25, false)
                        , ColumnInfo.CreateString(Columns.Build, 15, false)
                        , ColumnInfo.CreateString(Columns.Description, 1024, true)
                        , ColumnInfo.CreateDateTime(Columns.ReleaseDate, true)
                        , ColumnInfo.CreateDateTime(Columns.ExpireDate, true)
                    };
                    return _columns;
                }
            }

            public static RowInfo PopulateRow(long id, string description, string version, string build, DateTime releaseDate, DateTime? expireDate = null)
            {
                var result = new RowInfo();
                foreach (var c in COLUMNS)
                {
                    switch (c.Name)
                    {
                        case Columns.ID:
                            result.Add(c, id);
                            break;
                        case Columns.Code:
                            result.Add(c, ColumnInfo.GetDefaultForGuid());
                            break;
                        case Columns.Version:
                            result.Add(c, version);
                            break;
                        case Columns.Build:
                            result.Add(c, build);
                            break;
                        case Columns.Description:
                            result.Add(c, description);
                            break;
                        case Columns.ReleaseDate:
                            result.Add(c, releaseDate);
                            break;
                        case Columns.ExpireDate:
                            if (expireDate != null) result.Add(c, (DateTime)expireDate);
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }

            private static TableInfo _population;

            public static TableInfo Populate()
            {
                if (_population == null)
                {
                    _population = new TableInfo
                    {
                        PopulateRow(LPConsts.DataSet.Suite.DefaultVetrina.id
                        , LPConsts.DataSet.Suite.DefaultVetrina.description
                        , LPConsts.DataSet.Suite.DefaultVetrina.version
                        , LPConsts.DataSet.Suite.DefaultVetrina.build
                        , ColumnInfo.GetDefaultForDate()),

                        PopulateRow(LPConsts.DataSet.Suite.DefaultVolantino.id
                        , LPConsts.DataSet.Suite.DefaultVolantino.description
                        , LPConsts.DataSet.Suite.DefaultVolantino.version
                        , LPConsts.DataSet.Suite.DefaultVolantino.build
                        , ColumnInfo.GetDefaultForDate())
                    };
                }
                return _population;
            }

            public static RowInfo GetSuite(TableInfo Suites, string description, string version = null, string build = null)
            {
                var suites = from RowInfo s in Suites
                             where s.GetValueIsEqualTo(Columns.Description, description)
                                && (version != null ? s.GetValueIsEqualTo(Columns.Version, version) : true)
                                && (build != null ? s.GetValueIsEqualTo(Columns.Build, build) : true)
                             select s;

                return suites.Count() > 0 ? suites.First() : null;
            }
            public static List<RowInfo> GetProducts(RowInfo suite)
            {
                long? suiteID = suite.GetValueAsInt64(Suite.Columns.ID);
                if (suiteID == null) return null;
                return Product.Populate().Where(p => p.GetValueAsInt64(Product.Columns.SuiteID).Equals(suiteID)).ToList();
            }
        }
        #endregion Suite

        #region Privilege
        public class Privilege
        {
            public const string Name = LPConsts.DataSet.Privilege.Name;

            public class Columns : ColumnsBase
            {
                public const string Description = LPConsts.DataSet.Privilege.Columns.Description;
                public const string Notes = LPConsts.DataSet.Privilege.Columns.Notes;

                private static string[] _names;
                public static string[] NAMES
                {
                    get
                    {
                        if (_names == null)
                            _names = new string[] { ID
                            , Code
                            , Description
                            , Notes
                        };
                        return _names;
                    }
                }
            }

            private static ColumnInfo[] _columns;
            public static ColumnInfo[] COLUMNS
            {
                get
                {
                    if (_columns == null) _columns = new ColumnInfo[] {
                        ColumnsBase.CreateID()
                        , ColumnsBase.CreateCode()
                        , ColumnInfo.CreateString(Columns.Description, 25, true)
                        , ColumnInfo.CreateString(Columns.Notes, 4096, true)
                    };
                    return _columns;
                }
            }

            public static RowInfo PopulateRow(long id, string description, string notes = null)
            {
                var result = new RowInfo();
                foreach (var c in COLUMNS)
                {
                    switch (c.Name)
                    {
                        case Columns.ID:
                            result.Add(c, id);
                            break;
                        case Columns.Code:
                            result.Add(c, ColumnInfo.GetDefaultForGuid());
                            break;
                        case Columns.Description:
                            result.Add(c, description);
                            break;
                        case Columns.Notes:
                            result.Add(c, notes);
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }

            private static TableInfo _population;

            public static TableInfo Populate()
            {
                if (_population == null)
                {
                    _population = new TableInfo
                    {
                        PopulateRow(LPConsts.DataSet.Privilege.DefaultEmpty.id
                        , LPConsts.DataSet.Privilege.DefaultEmpty.description),

                        PopulateRow(LPConsts.DataSet.Privilege.DefaultAdmin.id
                        , LPConsts.DataSet.Privilege.DefaultAdmin.description),

                        PopulateRow(LPConsts.DataSet.Privilege.DefaultStandard.id
                        , LPConsts.DataSet.Privilege.DefaultStandard.description),

                        PopulateRow(LPConsts.DataSet.Privilege.DefaultAdvanced.id
                        , LPConsts.DataSet.Privilege.DefaultAdvanced.description),

                        PopulateRow(LPConsts.DataSet.Privilege.DefaultDemo.id
                        , LPConsts.DataSet.Privilege.DefaultDemo.description)
                    };
                }
                return _population;
            }

            public static TableInfo GetPrivilege(TableInfo privileges, string description)
            {
                var privilege = from RowInfo s in privileges
                                where s.GetValueIsEqualTo(Columns.Description, description)
                                select s;

                return privilege.ToList<RowInfo>() as TableInfo;
            }
        }
        #endregion Privilege

        #region Product
        public class Product
        {
            public const string Name = LPConsts.DataSet.Product.Name;

            public class Columns : ColumnsBase
            {
                public const string SuiteID = LPConsts.DataSet.Product.Columns.SuiteID;
                public const string Description = LPConsts.DataSet.Product.Columns.Description;
                public const string Version = LPConsts.DataSet.Product.Columns.Version;
                public const string Build = LPConsts.DataSet.Product.Columns.Build;
                public const string ReleaseDate = LPConsts.DataSet.Product.Columns.ReleaseDate;
                public const string ExpireDate = LPConsts.DataSet.Product.Columns.ExpireDate;

                private static string[] _names;
                public static string[] NAMES
                {
                    get
                    {
                        if (_names == null)
                            _names = new string[] { ID
                            , Code
                            , SuiteID
                            , Description
                            , Version
                            , Build
                            , ReleaseDate
                            , ExpireDate
                        };
                        return _names;
                    }
                }
            }

            private static ColumnInfo[] _columns;
            public static ColumnInfo[] COLUMNS
            {
                get
                {
                    if (_columns == null) _columns = new ColumnInfo[] {
                        ColumnsBase.CreateID()
                        , ColumnsBase.CreateCode()
                        , ColumnInfo.CreateInt64(Columns.SuiteID
                            , false
                            , string.Format(ColumnInfo.ForeignKeyReferenceFormat, Suite.Name, Suite.Columns.ID)
                            , Suite.Columns.Description
                                + LPConsts.ExtendedProperties.ForeignKeyReferenceDisplayMemberSeparator + Suite.Columns.Version
                                + LPConsts.ExtendedProperties.ForeignKeyReferenceDisplayMemberSeparator + Suite.Columns.Build)
                        , ColumnInfo.CreateString(Columns.Version, 25, false)
                        , ColumnInfo.CreateString(Columns.Build, 15, false)
                        , ColumnInfo.CreateString(Columns.Description, 1024, true)
                        , ColumnInfo.CreateDateTime(Columns.ReleaseDate, true)
                        , ColumnInfo.CreateDateTime(Columns.ExpireDate, true)
                    };
                    return _columns;
                }
            }

            public static RowInfo PopulateRow(long id, long? productSuiteID, string description, string version, string build, DateTime releaseDate, DateTime? expireDate = null)
            {
                var result = new RowInfo();
                if (productSuiteID == null) return result;
                foreach (var c in COLUMNS)
                {
                    switch (c.Name)
                    {
                        case Columns.ID:
                            result.Add(c, id);
                            break;
                        case Columns.SuiteID:
                            result.Add(c, productSuiteID);
                            break;
                        case Columns.Code:
                            result.Add(c, ColumnInfo.GetDefaultForGuid());
                            break;
                        case Columns.Version:
                            result.Add(c, version);
                            break;
                        case Columns.Build:
                            result.Add(c, build);
                            break;
                        case Columns.Description:
                            result.Add(c, description);
                            break;
                        case Columns.ReleaseDate:
                            result.Add(c, releaseDate);
                            break;
                        case Columns.ExpireDate:
                            if (expireDate != null) result.Add(c, (DateTime)expireDate);
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }
            private static TableInfo _population;

            public static TableInfo Populate()
            {
                if (_population == null)
                {
                    _population = new TableInfo();
                    foreach (RowInfo suite in Suite.Populate())
                    {
                        long? productSuiteID = suite.GetValueAsInt64(Suite.Columns.ID);
                        switch (productSuiteID)
                        {
                            case LPConsts.DataSet.Suite.DefaultVetrina.id:
                                _population.Add(PopulateRow(LPConsts.DataSet.Product.DefaultVetrina.id
                                    , productSuiteID
                                    , LPConsts.DataSet.Product.DefaultVetrina.description
                                    , LPConsts.DataSet.Product.DefaultVetrina.version
                                    , LPConsts.DataSet.Product.DefaultVetrina.build
                                    , ColumnInfo.GetDefaultForDate()));
                                break;
                            case LPConsts.DataSet.Suite.DefaultVolantino.id:
                                _population.Add(PopulateRow(LPConsts.DataSet.Product.DefaultVolantino.id
                                    , productSuiteID
                                    , LPConsts.DataSet.Product.DefaultVolantino.description
                                    , LPConsts.DataSet.Product.DefaultVolantino.version
                                    , LPConsts.DataSet.Product.DefaultVolantino.build
                                    , ColumnInfo.GetDefaultForDate()));
                                break;
                            default:
                                break;
                        }
                    }
                }
                return _population;
            }

            public static RowInfo GetProduct(TableInfo Products, string description, string version = null, string build = null, int? productSuiteID = null)
            {
                var products = from RowInfo s in Products
                               where s.GetValueIsEqualTo(Columns.Description, description)
                                  && (version != null ? s.GetValueIsEqualTo(Columns.Version, version) : true)
                                  && (build != null ? s.GetValueIsEqualTo(Columns.Build, build) : true)
                                  && (productSuiteID != null ? s.GetValueIsEqualTo(Columns.SuiteID, productSuiteID) : true)
                               select s;

                return products.Count() > 0 ? products.First() : null;
            }
        }
        #endregion Product

        #region Customer
        public class Customer
        {
            public const string Name = LPConsts.DataSet.Customer.Name;

            public class Columns : ColumnsBase
            {
                public const string Description = LPConsts.DataSet.Customer.Columns.Description;
                public const string TaxIdentifier = LPConsts.DataSet.Customer.Columns.TaxIdentifier;
                public const string ActiveFrom = LPConsts.DataSet.Customer.Columns.ActiveFrom;
                public const string ActiveUntil = LPConsts.DataSet.Customer.Columns.ActiveUntil;
                public const string Address = LPConsts.DataSet.Customer.Columns.Address;
                public const string Town = LPConsts.DataSet.Customer.Columns.Town;
                public const string ZipCode = LPConsts.DataSet.Customer.Columns.ZipCode;
                public const string Email = LPConsts.DataSet.Customer.Columns.Email;
                public const string Phone = LPConsts.DataSet.Customer.Columns.Phone;
                public const string Fax = LPConsts.DataSet.Customer.Columns.Fax;
                public const string WebSite = LPConsts.DataSet.Customer.Columns.WebSite;
                public const string Notes = LPConsts.DataSet.Customer.Columns.Notes;

                private static string[] _names;
                public static string[] NAMES
                {
                    get
                    {
                        if (_names == null)
                            _names = new string[] { ID
                            , Code
                            , Description
                            , TaxIdentifier
                            , ActiveFrom
                            , ActiveUntil
                            , Address
                            , Town
                            , ZipCode
                            , Email
                            , Phone
                            , Fax
                            , WebSite
                            , Notes
                        };
                        return _names;
                    }
                }
            }

            private static ColumnInfo[] _columns;
            public static ColumnInfo[] COLUMNS
            {
                get
                {
                    if (_columns == null) _columns = new ColumnInfo[] {
                        ColumnsBase.CreateID()
                        , ColumnsBase.CreateCode()
                        , ColumnInfo.CreateString(Columns.Description, 1024, false, true)
                        , ColumnInfo.CreateString(Columns.TaxIdentifier, 20, false, true)
                        , ColumnInfo.CreateDateTime(Columns.ActiveFrom, false)
                        , ColumnInfo.CreateDateTime(Columns.ActiveUntil, true)
                        , ColumnInfo.CreateString(Columns.Address, 1024)
                        , ColumnInfo.CreateString(Columns.Town, 255)
                        , ColumnInfo.CreateString(Columns.ZipCode, 10)
                        , ColumnInfo.CreateString(Columns.Email, 255)
                        , ColumnInfo.CreateString(Columns.Phone, 128)
                        , ColumnInfo.CreateString(Columns.Fax, 128)
                        , ColumnInfo.CreateString(Columns.WebSite, 1024)
                        , ColumnInfo.CreateString(Columns.Notes, 4096, true)
                    };
                    return _columns;
                }
            }

            public static RowInfo PopulateRow(long? id
                , long? userID
                , string description
                , string taxIdentifier
                , string address
                , string town
                , string zipCode
                , string email
                , string phone
                , string fax
                , string webSite
                , DateTime? activeFrom = null
                , DateTime? activeUntil = null
                , string notes = null)
            {
                var result = new RowInfo();
                if (id == null) return result;
                foreach (var c in COLUMNS)
                {
                    switch (c.Name)
                    {
                        case Columns.ID:
                            if (id != null) result.Add(c, id);
                            break;
                        case Columns.Code:
                            result.Add(c, ColumnInfo.GetDefaultForGuid());
                            break;
                        case Columns.Description:
                            result.Add(c, description);
                            break;
                        case Columns.TaxIdentifier:
                            result.Add(c, taxIdentifier);
                            break;
                        case Columns.Address:
                            result.Add(c, address);
                            break;
                        case Columns.Town:
                            result.Add(c, town);
                            break;
                        case Columns.ZipCode:
                            result.Add(c, zipCode);
                            break;
                        case Columns.Email:
                            result.Add(c, email);
                            break;
                        case Columns.Phone:
                            result.Add(c, phone);
                            break;
                        case Columns.Fax:
                            result.Add(c, fax);
                            break;
                        case Columns.WebSite:
                            result.Add(c, webSite);
                            break;
                        case Columns.ActiveFrom:
                            result.Add(c, activeFrom ?? DateTime.Today);
                            break;
                        case Columns.ActiveUntil:
                            if (activeUntil != null) result.Add(c, (DateTime)activeUntil);
                            break;
                        case Columns.Notes:
                            result.Add(c, notes);
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }
            private static TableInfo _population;
            public static TableInfo Populate()
            {
                if (_population == null)
                {
                    _population = new TableInfo();
                    int id = 0;
                    foreach (RowInfo user in User.Populate())
                    {
                        _population.Add(PopulateRow(user.GetValueAsInt64(ColumnsBase.ID)
                            , user.GetValueAsInt64(User.Columns.ID)
                            , user.GetValueAsString(User.Columns.Login)
                            , user.GetValueAsString(User.Columns.Login).PadRight(20, '*')
                            , string.Empty //Address
                            , string.Empty //Town
                            , string.Empty //ZipCode
                            , string.Empty //email
                            , string.Empty //Phone
                            , string.Empty //Fax
                            , string.Empty //WebSite
                            , user.GetValueAsDateTime(User.Columns.ActiveFrom)
                            , user.GetValueAsDateTime(User.Columns.ActiveUntil)
                            , user.GetValueAsString(User.Columns.Notes)));
                        id = _population.LastID();
                    }
                }
                return _population;
            }

            public static RowInfo GetCustomer(TableInfo Suites, string description, string taxIdentifier = null)
            {
                var suites = from RowInfo s in Suites
                             where s.GetValueIsEqualTo(Columns.Description, description)
                                || (taxIdentifier != null ? s.GetValueIsEqualTo(Columns.TaxIdentifier, taxIdentifier) : true)
                             select s;

                return suites.Count() > 0 ? suites.First() : null;
            }
        }
        #endregion Customer

        #region Activation
        public class Activation
        {
            public const string Name = LPConsts.DataSet.Activation.Name;

            public class Columns : ColumnsBase
            {
                public const string CustomerID = LPConsts.DataSet.Activation.Columns.CustomerID;
                public const string ProductID = LPConsts.DataSet.Activation.Columns.ProductID;
                public const string PrivilegeID = LPConsts.DataSet.Activation.Columns.PrivilegeID;
                public const string Key = LPConsts.DataSet.Activation.Columns.Key;
                public const string ActiveFrom = LPConsts.DataSet.Activation.Columns.ActiveFrom;
                public const string ActiveUntil = LPConsts.DataSet.Activation.Columns.ActiveUntil;
                public const string Notes = LPConsts.DataSet.Activation.Columns.Notes;

                private static string[] _names;
                public static string[] NAMES
                {
                    get
                    {
                        if (_names == null)
                            _names = new string[] { ID
                            , Code
                            , CustomerID
                            , ProductID
                            , Key
                            , ActiveFrom
                            , ActiveUntil
                            , Notes
                        };
                        return _names;
                    }
                }
            }

            private static ColumnInfo[] _columns;
            public static ColumnInfo[] COLUMNS
            {
                get
                {
                    if (_columns == null) _columns = new ColumnInfo[] {
                        ColumnsBase.CreateID()
                        , ColumnsBase.CreateCode()
                        , ColumnInfo.CreateInt64(Columns.CustomerID
                            , false
                            , string.Format(ColumnInfo.ForeignKeyReferenceFormat, Customer.Name, Customer.Columns.ID)
                            , Customer.Columns.Description)
                        , ColumnInfo.CreateInt64(Columns.ProductID
                            , false
                            , string.Format(ColumnInfo.ForeignKeyReferenceFormat, Product.Name, Product.Columns.ID)
                            , Product.Columns.Description
                                + LPConsts.ExtendedProperties.ForeignKeyReferenceDisplayMemberSeparator + Product.Columns.Version
                                + LPConsts.ExtendedProperties.ForeignKeyReferenceDisplayMemberSeparator + Product.Columns.Build)
                        , ColumnInfo.CreateInt64(Columns.PrivilegeID
                            , false
                            , string.Format(ColumnInfo.ForeignKeyReferenceFormat, Privilege.Name, Privilege.Columns.ID)
                            , Privilege.Columns.Description)
                        , ColumnInfo.CreateString(Columns.Key, 4096, false)
                        , ColumnInfo.CreateDateTime(Columns.ActiveFrom, false)
                        , ColumnInfo.CreateDateTime(Columns.ActiveUntil, true)
                        , ColumnInfo.CreateString(Columns.Notes, 4096, true)
                    };
                    return _columns;
                }
            }

            public static RowInfo PopulateRow(long? id
                , long? customerID
                , string key
                , long? productID
                , long? privilegeID
                , DateTime? activeFrom
                , DateTime? activeUntil = null
                , string notes = null)
            {
                var result = new RowInfo();
                if (id == null || customerID == null || productID == null) return result;
                foreach (var c in COLUMNS)
                {
                    switch (c.Name)
                    {
                        case Columns.ID:
                            result.Add(c, id);
                            break;
                        case Columns.Code:
                            result.Add(c, ColumnInfo.GetDefaultForGuid());
                            break;
                        case Columns.CustomerID:
                            if (customerID != null) result.Add(c, customerID);
                            break;
                        case Columns.Key:
                            if (key != null) result.Add(c, key);
                            break;
                        case Columns.ProductID:
                            if (productID != null) result.Add(c, productID);
                            break;
                        case Columns.PrivilegeID:
                            if (privilegeID != null) result.Add(c, privilegeID);
                            break;
                        case Columns.ActiveFrom:
                            result.Add(c, activeFrom);
                            break;
                        case Columns.ActiveUntil:
                            if (activeUntil != null) result.Add(c, (DateTime)activeUntil);
                            break;
                        case Columns.Notes:
                            result.Add(c, notes);
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }
            private static TableInfo _population;
            public static TableInfo Populate()
            {
                if (_population == null)
                {
                    _population = new TableInfo();

                    //var cps = (from RowInfo c in Customer.Populate()
                    //           join RowInfo s in Suite.Populate() on 1 equals 1
                    //           join RowInfo p in Product.Populate() on s.GetValueAsInt64(Suite.Columns.ID) equals p.GetValueAsInt64(Product.Columns.ProductSuiteID)
                    //           select new { c, s, p });

                    //Int64 id = 0;
                    //foreach (var cp in cps)
                    //{
                    //    //Console.WriteLine(cp);
                    //    _population.Add(PopulateRow(id, cp.c.GetValueAsInt64(Customer.Columns.ID)
                    //        , cp.s.GetValueAsString(Suite.Columns.ID)
                    //        , cp.p.GetValueAsInt64(Product.Columns.ID)
                    //        , cp.c.GetValueAsDateTime(Customer.Columns.ActiveFrom)));
                    //    id++;
                    //}
                }
                return _population;
            }

            public static TableInfo GetCustomerActivations(TableInfo activations, int customerID, int? productID = null)
            {
                var customerActivations = from RowInfo s in activations
                                          where s.GetValueIsEqualTo(Columns.CustomerID, customerID)
                                             && (productID != null ? s.GetValueIsEqualTo(Columns.ProductID, productID) : true)
                                          select s;

                return customerActivations.ToList<RowInfo>() as TableInfo;
            }
        }
        #endregion Activation

        #region Key
        public class Key
        {
            public struct KeyData
            {
                public static FingerPrintParts Nullables = FingerPrintParts.BIOS | FingerPrintParts.DISK | FingerPrintParts.VIDEO;

                private string _machineCPU;
                private string _machineBASE;
                private string _machineMAC;
                private string _machineBIOS;
                private string _machineDISK;
                private string _machineVIDEO;

                public DateTime TimeStamp;
                public string Key;
                public Guid CustomerCode;
                public Guid ProductCode;
                public Guid PrivilegeCode;

                public DateTime ActiveFrom;
                public DateTime ActiveUntil;
                public string MachineCPU { get { return Nullables.HasFlag(FingerPrintParts.CPU) && string.IsNullOrEmpty(_machineCPU) ? LPConsts.DataSet.Key.KeyData.NullString : _machineCPU; } set { _machineCPU = value; } }
                public string MachineBASE { get { return Nullables.HasFlag(FingerPrintParts.BASE) && string.IsNullOrEmpty(_machineBASE) ? LPConsts.DataSet.Key.KeyData.NullString : _machineBASE; } set { _machineBASE = value; } }
                public string MachineMAC { get { return Nullables.HasFlag(FingerPrintParts.MAC) && string.IsNullOrEmpty(_machineMAC) ? LPConsts.DataSet.Key.KeyData.NullString : _machineMAC; } set { _machineMAC = value; } }
                public string MachineBIOS { get { return Nullables.HasFlag(FingerPrintParts.BIOS) && string.IsNullOrEmpty(_machineBIOS) ? LPConsts.DataSet.Key.KeyData.NullString : _machineBIOS; } set { _machineBIOS = value; } }
                public string MachineDISK { get { return Nullables.HasFlag(FingerPrintParts.DISK) && string.IsNullOrEmpty(_machineDISK) ? LPConsts.DataSet.Key.KeyData.NullString : _machineDISK; } set { _machineDISK = value; } }
                public string MachineVIDEO { get { return Nullables.HasFlag(FingerPrintParts.VIDEO) && string.IsNullOrEmpty(_machineVIDEO) ? LPConsts.DataSet.Key.KeyData.NullString : _machineVIDEO; } set { _machineVIDEO = value; } }

                public KeyData(Guid customerCode, Guid productCode, Guid privilegeCode, string infoCPU, string infoBASE, string infoMAC)
                {
                    this.TimeStamp = DateTime.UtcNow;
                    this.Key = string.Empty;
                    this.CustomerCode = customerCode;
                    this.ProductCode = productCode;
                    this.PrivilegeCode = privilegeCode;
                    this.ActiveFrom = DateTime.MinValue;
                    this.ActiveUntil = DateTime.MinValue;
                    this._machineCPU = infoCPU;
                    this._machineBASE = infoBASE;
                    this._machineMAC = infoMAC;
                    this._machineBIOS = string.Empty;
                    this._machineDISK = string.Empty;
                    this._machineVIDEO = string.Empty;
                }

                public bool IsValid
                {
                    get
                    {
                        return !CustomerCode.Equals(Guid.Empty)
                            && !ProductCode.Equals(Guid.Empty)
                            && !PrivilegeCode.Equals(Guid.Empty)
                            && !ActiveFrom.Equals(DateTime.MinValue)
                            && !ActiveUntil.Equals(DateTime.MinValue)
                            && (ActiveUntil > ActiveFrom)
                            && (Nullables.HasFlag(FingerPrintParts.CPU) || !string.IsNullOrEmpty(MachineCPU))
                            && (Nullables.HasFlag(FingerPrintParts.BASE) || !string.IsNullOrEmpty(MachineBASE))
                            && (Nullables.HasFlag(FingerPrintParts.MAC) || !string.IsNullOrEmpty(MachineMAC))
                            && (Nullables.HasFlag(FingerPrintParts.BIOS) || !string.IsNullOrEmpty(MachineBIOS))
                            && (Nullables.HasFlag(FingerPrintParts.DISK) || !string.IsNullOrEmpty(MachineDISK))
                            && (Nullables.HasFlag(FingerPrintParts.VIDEO) || !string.IsNullOrEmpty(MachineVIDEO));
                    }
                }

                private string _FormatPart(string column, object value)
                {
                    return string.Format(LPConsts.DataSet.Key.KeyData.ToStringFormat, column, LPConsts.DataSet.Key.PartIndicator, value.ToString()) + LPConsts.DataSet.Key.PartSeparator;
                }

                public override string ToString()
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(_FormatPart(Columns.TimeStamp, TimeStamp.ToString(LPConsts.DateTimeFormat)));
                    sb.Append(_FormatPart(Columns.Key, Key));
                    sb.Append(_FormatPart(Columns.CustomerCode, CustomerCode));
                    sb.Append(_FormatPart(Columns.ProductCode, ProductCode));
                    sb.Append(_FormatPart(Columns.PrivilegeCode, PrivilegeCode));
                    sb.Append(_FormatPart(Columns.ActiveFrom, ActiveFrom.ToString(LPConsts.DateFormat)));
                    sb.Append(_FormatPart(Columns.ActiveUntil, ActiveUntil.ToString(LPConsts.DateFormat)));
                    sb.Append(_FormatPart(Columns.MachineCPU, MachineCPU));
                    sb.Append(_FormatPart(Columns.MachineBASE, MachineBASE));
                    sb.Append(_FormatPart(Columns.MachineMAC, MachineMAC));
                    sb.Append(_FormatPart(Columns.MachineBIOS, MachineBIOS));
                    sb.Append(_FormatPart(Columns.MachineDISK, MachineDISK));
                    sb.Append(_FormatPart(Columns.MachineVIDEO, MachineVIDEO));
                    return sb.ToString();
                }

                public string ToString(bool encryped)
                {
                    string result = ToString();
                    if (encryped) Encrypt(result, out result);
                    return result;
                }

                public static KeyData Empty
                {
                    get
                    {
                        return new KeyData();
                    }
                }

                public static KeyData Build(string customerCode
                    , string productCode
                    , string privilegeCode
                    , string activeFrom
                    , string activeUntil
                    , string machineCPU
                    , string machineBASE
                    , string machineMAC
                    , string machineBIOS = null
                    , string machineDISK = null
                    , string machineVIDEO = null)
                {
                    DateTime from = DateTime.MinValue;
                    if (!DateTime.TryParse(activeFrom, out from)) return KeyData.Empty;
                    DateTime until = DateTime.MinValue;
                    if (!DateTime.TryParse(activeUntil, out until)) return KeyData.Empty;

                    return Build(new Guid(customerCode)
                        , new Guid(productCode)
                        , new Guid(privilegeCode)
                        , from
                        , until
                        , machineCPU
                        , machineBASE
                        , machineMAC
                        , machineBIOS
                        , machineDISK
                        , machineVIDEO
                    );
                }

                public static KeyData Build(string customerCode
                    , string productCode
                    , string privilegeCode
                    , DateTime activeFrom
                    , DateTime activeUntil
                    , string machineCPU
                    , string machineBASE
                    , string machineMAC
                    , string machineBIOS = null
                    , string machineDISK = null
                    , string machineVIDEO = null)
                {
                    return Build(new Guid(customerCode)
                        , new Guid(productCode)
                        , new Guid(privilegeCode)
                        , activeFrom
                        , activeUntil
                        , machineCPU
                        , machineBASE
                        , machineMAC
                        , machineBIOS
                        , machineDISK
                        , machineVIDEO
                    );
                }

                public static KeyData Build(Guid customerCode
                    , Guid productCode
                    , Guid privilegeCode
                    , DateTime activeFrom
                    , DateTime activeUntil
                    , string machineCPU
                    , string machineBASE
                    , string machineMAC
                    , string machineBIOS = null
                    , string machineDISK = null
                    , string machineVIDEO = null)
                {
                    return new KeyData()
                    {
                        CustomerCode = customerCode,
                        ProductCode = productCode,
                        PrivilegeCode = privilegeCode,
                        ActiveFrom = activeFrom,
                        ActiveUntil = activeUntil,
                        MachineCPU = machineCPU,
                        MachineBASE = machineBASE,
                        MachineMAC = machineMAC,
                        MachineBIOS = machineBIOS,
                        MachineDISK = machineDISK,
                        MachineVIDEO = machineVIDEO
                    };
                }

                public static KeyData FromString(string activationDataText)
                {
                    string[] plainActivationKeyParts = activationDataText.Split(LPConsts.DataSet.Key.PartSeparator);
                    KeyData result = new KeyData();

                    foreach (string part in plainActivationKeyParts)
                    {
                        if (string.IsNullOrEmpty(part)) continue;

                        string[] key_value = part.Split(LPConsts.DataSet.Key.PartIndicator, StringSplitOptions.RemoveEmptyEntries);

                        string partName = key_value[0];
                        string partValue = key_value.Length > 1 ? key_value[1] : string.Empty;

                        FingerPrintParts partEnum = FingerPrintParts.CUSTOM;
                        bool isFingerPrintParts = Enum.TryParse<FingerPrintParts>(partName, true, out partEnum);
                        if (!isFingerPrintParts) partEnum = FingerPrintParts.CUSTOM;

                        switch (partEnum)
                        {
                            case FingerPrintParts.CUSTOM:
                                switch (partName)
                                {
                                    case Columns.TimeStamp:
                                        result.TimeStamp = DateTime.Now;
                                        DateTime.TryParse(partValue, out result.TimeStamp);
                                        break;
                                    case Columns.Key:
                                        result.Key = partValue;
                                        break;
                                    case Columns.CustomerCode:
                                        result.CustomerCode = new Guid(partValue);
                                        break;
                                    case Columns.ProductCode:
                                        result.ProductCode = new Guid(partValue);
                                        break;
                                    case Columns.PrivilegeCode:
                                        result.PrivilegeCode = new Guid(partValue);
                                        break;
                                    case Columns.ActiveFrom:
                                        DateTime.TryParse(partValue, out result.ActiveFrom);
                                        break;
                                    case Columns.ActiveUntil:
                                        DateTime.TryParse(partValue, out result.ActiveUntil);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case FingerPrintParts.CPU:
                                result.MachineCPU = partValue;
                                break;
                            case FingerPrintParts.BIOS:
                                result.MachineBIOS = partValue;
                                break;
                            case FingerPrintParts.BASE:
                                result.MachineBASE = partValue;
                                break;
                            case FingerPrintParts.DISK:
                                result.MachineDISK = partValue;
                                break;
                            case FingerPrintParts.VIDEO:
                                result.MachineVIDEO = partValue;
                                break;
                            case FingerPrintParts.MAC:
                                result.MachineMAC = partValue;
                                break;
                            case FingerPrintParts.ALL:
                                //None
                                break;
                            default:
                                break;
                        }
                    }

                    return result;
                }

                public static KeyData FromString(string activationDataText, bool encrypted)
                {
                    if (encrypted) Decrypt(activationDataText, out activationDataText);
                    return FromString(activationDataText);
                }
            }
            public const string Name = LPConsts.DataSet.Key.Name;

            public class Columns : ColumnsBase
            {
                public const string TimeStamp = LPConsts.DataSet.Key.Columns.TimeStamp;
                public const string Key = LPConsts.DataSet.Key.Columns.Key;
                public const string CustomerCode = LPConsts.DataSet.Key.Columns.CustomerCode;
                public const string ProductCode = LPConsts.DataSet.Key.Columns.ProductCode;
                public const string PrivilegeCode = LPConsts.DataSet.Key.Columns.PrivilegeCode;
                public const string MachineCPU = LPConsts.DataSet.Key.Columns.MachineCPU;
                public const string MachineBASE = LPConsts.DataSet.Key.Columns.MachineBASE;
                public const string MachineMAC = LPConsts.DataSet.Key.Columns.MachineMAC;
                public const string MachineBIOS = LPConsts.DataSet.Key.Columns.MachineBIOS;
                public const string MachineDISK = LPConsts.DataSet.Key.Columns.MachineDISK;
                public const string MachineVIDEO = LPConsts.DataSet.Key.Columns.MachineVIDEO;
                public const string ActiveFrom = LPConsts.DataSet.Key.Columns.ActiveFrom;
                public const string ActiveUntil = LPConsts.DataSet.Key.Columns.ActiveUntil;
                public const string Notes = LPConsts.DataSet.Key.Columns.Notes;

                private static string[] _names;
                public static string[] NAMES
                {
                    get
                    {
                        if (_names == null)
                            _names = new string[] { ID
                            , Code
                            , TimeStamp
                            , Key
                            , CustomerCode
                            , ProductCode
                            , PrivilegeCode
                            , MachineCPU
                            , MachineBIOS
                            , MachineBASE
                            , MachineDISK
                            , MachineVIDEO
                            , MachineMAC
                            , ActiveFrom
                            , ActiveUntil
                            , Notes
                        };
                        return _names;
                    }
                }
            }

            private static ColumnInfo[] _columns;
            public static ColumnInfo[] COLUMNS
            {
                get
                {
                    if (_columns == null) _columns = new ColumnInfo[] {
                        ColumnsBase.CreateID()
                        , ColumnsBase.CreateCode()
                        , ColumnInfo.CreateDateTime(Columns.TimeStamp, false)
                        , ColumnInfo.CreateGuid(Columns.CustomerCode
                            , false
                            , string.Format(ColumnInfo.ForeignKeyReferenceFormat, Customer.Name, Customer.Columns.Code)
                            , Customer.Columns.Description)
                        , ColumnInfo.CreateGuid(Columns.ProductCode
                            , false
                            , string.Format(ColumnInfo.ForeignKeyReferenceFormat, Product.Name, Product.Columns.Code)
                            , Product.Columns.Description
                                + LPConsts.ExtendedProperties.ForeignKeyReferenceDisplayMemberSeparator
                                + Product.Columns.Version
                                + LPConsts.ExtendedProperties.ForeignKeyReferenceDisplayMemberSeparator
                                + Product.Columns.Build)
                        , ColumnInfo.CreateGuid(Columns.PrivilegeCode
                            , false
                            , string.Format(ColumnInfo.ForeignKeyReferenceFormat, Privilege.Name, Privilege.Columns.Code)
                            , Privilege.Columns.Description)
                        , ColumnInfo.CreateString(Columns.Key
                            , 4096
                            , false
                            , true
                            , string.Format(ColumnInfo.ForeignKeyReferenceFormat, Activation.Name, Activation.Columns.Key)
                            , Activation.Columns.Key
                                + LPConsts.ExtendedProperties.ForeignKeyReferenceDisplayMemberSeparator
                                + Activation.Columns.ActiveFrom
                                + LPConsts.ExtendedProperties.ForeignKeyReferenceDisplayMemberSeparator
                                + Activation.Columns.ActiveUntil)
                        , ColumnInfo.CreateString(Columns.MachineCPU, 256, false)
                        , ColumnInfo.CreateString(Columns.MachineBIOS, 256, false)
                        , ColumnInfo.CreateString(Columns.MachineBASE, 256, true)
                        , ColumnInfo.CreateString(Columns.MachineDISK, 256, true)
                        , ColumnInfo.CreateString(Columns.MachineVIDEO, 256, true)
                        , ColumnInfo.CreateString(Columns.MachineMAC, 256, false)
                        , ColumnInfo.CreateDateTime(Columns.ActiveFrom, false)
                        , ColumnInfo.CreateDateTime(Columns.ActiveUntil, true)
                        , ColumnInfo.CreateString(Columns.Notes, 4096, true)
                    };
                    return _columns;
                }
            }

            public static RowInfo PopulateRow(KeyData activationData, string notes = null)
            {
                var result = new RowInfo();
                if (activationData.Equals(KeyData.Empty)) return result;
                foreach (var c in COLUMNS)
                {
                    switch (c.Name)
                    {
                        case Columns.ID:

                            break;
                        case Columns.Code:
                            result.Add(c, ColumnInfo.GetDefaultForGuid());
                            break;
                        case Columns.TimeStamp:
                            result.Add(c, ColumnInfo.GetDefaultForDateTime());
                            break;
                        case Columns.Key:
                            result.Add(c, activationData.Key);
                            break;
                        case Columns.CustomerCode:
                            result.Add(c, activationData.CustomerCode);
                            break;
                        case Columns.ProductCode:
                            result.Add(c, activationData.ProductCode);
                            break;
                        case Columns.PrivilegeCode:
                            result.Add(c, activationData.PrivilegeCode);
                            break;
                        case Columns.MachineCPU:
                            result.Add(c, activationData.MachineCPU);
                            break;
                        case Columns.MachineBIOS:
                            result.Add(c, activationData.MachineBIOS);
                            break;
                        case Columns.MachineBASE:
                            result.Add(c, activationData.MachineBASE);
                            break;
                        case Columns.MachineDISK:
                            result.Add(c, activationData.MachineDISK);
                            break;
                        case Columns.MachineVIDEO:
                            result.Add(c, activationData.MachineVIDEO);
                            break;
                        case Columns.MachineMAC:
                            result.Add(c, activationData.MachineMAC);
                            break;
                        case Columns.ActiveFrom:
                            result.Add(c, activationData.ActiveFrom);
                            break;
                        case Columns.ActiveUntil:
                            result.Add(c, activationData.ActiveUntil);
                            break;
                        case Columns.Notes:
                            result.Add(c, notes);
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }

            public static string GetActivationKey(KeyData activationData)
            {
                return activationData.ToString(true);
            }

            public static string Deactivate(KeyData activationData)
            {
                //return encryptActivationData(compiledActivationData);
                return string.Empty;
            }

            private static TableInfo _population;
            public static TableInfo Populate()
            {
                if (_population == null)
                {
                    _population = new TableInfo();
                }
                return _population;
            }
        }
        #endregion Key

        #region WebSite
        public class WebSite
        {
            public const string Name = LPConsts.DataSet.WebSite.Name;

            public class Columns : ColumnsBase
            {
                public const string Name = LPConsts.DataSet.WebSite.Columns.Name;
                public const string NormalUrl = LPConsts.DataSet.WebSite.Columns.NormalUrl;
                public const string MobileUrl = LPConsts.DataSet.WebSite.Columns.MobileUrl;
                public const string Version = LPConsts.DataSet.WebSite.Columns.Version;
                public const string ReleaseDate = LPConsts.DataSet.WebSite.Columns.ReleaseDate;

                public const string MaxPageCount = LPConsts.DataSet.WebSite.Columns.MaxPageCount;
                public const string PageListUrl = LPConsts.DataSet.WebSite.Columns.PageListUrl;
                public const string PageListUrlRedirects = LPConsts.DataSet.WebSite.Columns.PageListUrlRedirects;
                public const string PageListMaxCount = LPConsts.DataSet.WebSite.Columns.PageListMaxCount;
                public const string PageListPageNumberXPath = LPConsts.DataSet.WebSite.Columns.PageListPageNumberXPath;
                public const string PageListPageCountXPath = LPConsts.DataSet.WebSite.Columns.PageListPageCountXPath;
                public const string PageListItemCountXPath = LPConsts.DataSet.WebSite.Columns.PageListItemCountXPath;
                public const string PageListRagioneSocialeXPath = LPConsts.DataSet.WebSite.Columns.PageListRagioneSocialeXPath;
                public const string PageListListXPath = LPConsts.DataSet.WebSite.Columns.PageListListXPath;
                public const string PageListFeaturesEnabled = LPConsts.DataSet.WebSite.Columns.PageListFeaturesEnabled;
                public const string PageListFeaturesXPath = LPConsts.DataSet.WebSite.Columns.PageListFeaturesXPath;
                public const string PageListMaxImageCount = LPConsts.DataSet.WebSite.Columns.PageListMaxImageCount;
                public const string PageListImageFormat = LPConsts.DataSet.WebSite.Columns.PageListImageFormat;
                public const string PageListImageXPath = LPConsts.DataSet.WebSite.Columns.PageListImageXPath;
                public const string PageListImageThumbListXPath = LPConsts.DataSet.WebSite.Columns.PageListImageThumbListXPath;
                public const string PageListImageThumbActiveXPath = LPConsts.DataSet.WebSite.Columns.PageListImageThumbActiveXPath;
                public const string PageListImageThumbNormalXPath = LPConsts.DataSet.WebSite.Columns.PageListImageThumbNormalXPath;

                public const string QRCodeEncoding = LPConsts.DataSet.WebSite.Columns.QRCodeEncoding;
                public const string QRCodeScale = LPConsts.DataSet.WebSite.Columns.QRCodeScale;
                public const string QRCodeVersion = LPConsts.DataSet.WebSite.Columns.QRCodeVersion;
                public const string QRCodeCorrection = LPConsts.DataSet.WebSite.Columns.QRCodeCorrection;
                public const string QRCodeCharacterSet = LPConsts.DataSet.WebSite.Columns.QRCodeCharacterSet;
                public const string QRCodeBackColor = LPConsts.DataSet.WebSite.Columns.QRCodeBackColor;
                public const string QRCodeForeColor = LPConsts.DataSet.WebSite.Columns.QRCodeForeColor;
                public const string QRCodeImageFormat = LPConsts.DataSet.WebSite.Columns.QRCodeImageFormat;

                public const string MergeDefinitionName = LPConsts.DataSet.WebSite.Columns.MergeDefinitionName;
                public const string MergeDefinitionKeyFields = LPConsts.DataSet.WebSite.Columns.MergeDefinitionKeyFields;
                public const string MergeDefinitionColExclude = LPConsts.DataSet.WebSite.Columns.MergeDefinitionColExclude;
                public const string MergeDefinitionRowExclude = LPConsts.DataSet.WebSite.Columns.MergeDefinitionRowExclude;

                private static string[] _names;
                public static string[] NAMES
                {
                    get
                    {
                        if (_names == null)
                            _names = new string[] { ID
                            , Code
                            , Name
                            , NormalUrl
                            , MobileUrl
                            , Version
                            , ReleaseDate
                            , MaxPageCount
                            , PageListUrl
                            , PageListUrlRedirects
                            , PageListMaxCount
                            , PageListPageNumberXPath
                            , PageListPageCountXPath
                            , PageListItemCountXPath
                            , PageListRagioneSocialeXPath
                            , PageListListXPath
                            , PageListFeaturesEnabled
                            , PageListFeaturesXPath
                            , PageListMaxImageCount
                            , PageListImageXPath
                            , QRCodeEncoding
                            , QRCodeScale
                            , QRCodeVersion
                            , QRCodeCorrection
                            , QRCodeCharacterSet
                            , QRCodeBackColor
                            , QRCodeForeColor
                            , QRCodeImageFormat
                            , MergeDefinitionName
                            , MergeDefinitionKeyFields
                            , MergeDefinitionColExclude
                            , MergeDefinitionRowExclude
                        };
                        return _names;
                    }
                }
            }

            private static ColumnInfo[] _columns;
            public static ColumnInfo[] COLUMNS
            {
                get
                {
                    if (_columns == null) _columns = new ColumnInfo[] {
                        ColumnsBase.CreateID()
                        , ColumnsBase.CreateCode()
                        , ColumnInfo.CreateString(Columns.Name, 1024, false, true)
                        , ColumnInfo.CreateString(Columns.NormalUrl, 1024, false, true)
                        , ColumnInfo.CreateString(Columns.MobileUrl, 1024, false, true)
                        , ColumnInfo.CreateString(Columns.Version, 25, false)
                        , ColumnInfo.CreateDateTime(Columns.ReleaseDate, false)
                        , ColumnInfo.CreateInt(Columns.MaxPageCount, false)
                        , ColumnInfo.CreateString(Columns.PageListUrl, 1024)
                        , ColumnInfo.CreateString(Columns.PageListUrlRedirects, 1024)
                        , ColumnInfo.CreateInt(Columns.PageListMaxCount, false)
                        , ColumnInfo.CreateString(Columns.PageListPageNumberXPath, 1024)
                        , ColumnInfo.CreateString(Columns.PageListPageCountXPath, 1024)
                        , ColumnInfo.CreateString(Columns.PageListItemCountXPath, 1024)
                        , ColumnInfo.CreateString(Columns.PageListRagioneSocialeXPath, 1024)
                        , ColumnInfo.CreateString(Columns.PageListListXPath, 1024, true)
                        , ColumnInfo.CreateBoolean(Columns.PageListFeaturesEnabled, false)
                        , ColumnInfo.CreateString(Columns.PageListFeaturesXPath, 1024, true)
                        , ColumnInfo.CreateInt(Columns.PageListMaxImageCount, false)
                        , ColumnInfo.CreateString(Columns.PageListImageFormat, 1024, true)
                        , ColumnInfo.CreateString(Columns.PageListImageXPath, 1024, true)
                        , ColumnInfo.CreateString(Columns.PageListImageThumbListXPath, 1024, true)
                        , ColumnInfo.CreateString(Columns.PageListImageThumbActiveXPath, 1024, true)
                        , ColumnInfo.CreateString(Columns.PageListImageThumbNormalXPath, 1024, true)

                        , ColumnInfo.CreateInt(Columns.QRCodeEncoding, false)
                        , ColumnInfo.CreateInt(Columns.QRCodeScale, false)
                        , ColumnInfo.CreateInt(Columns.QRCodeVersion, false)
                        , ColumnInfo.CreateInt(Columns.QRCodeCorrection, false)
                        , ColumnInfo.CreateString(Columns.QRCodeCharacterSet, 1024, true)
                        , ColumnInfo.CreateString(Columns.QRCodeBackColor, 1024, true)
                        , ColumnInfo.CreateString(Columns.QRCodeForeColor, 1024, true)
                        , ColumnInfo.CreateString(Columns.QRCodeImageFormat, 1024, true)

                        , ColumnInfo.CreateString(Columns.MergeDefinitionName, 1024, true)
                        , ColumnInfo.CreateString(Columns.MergeDefinitionKeyFields, 1024, true)
                        , ColumnInfo.CreateString(Columns.MergeDefinitionColExclude, 1024, true)
                        , ColumnInfo.CreateString(Columns.MergeDefinitionRowExclude, 1024, true)
                    };
                    return _columns;
                }
            }

            public static RowInfo PopulateRow(long? id
                , string name
                , string normalUrl
                , string mobileUrl
                , string version
                , DateTime releaseDate
                , string pageListUrl
                , string pageListUrlRedirects
                , int pageListMaxCount
                , string pageListPageNumberXPath
                , string pageListPageCountXPath
                , string pageListItemCountXPath
                , string pageListRagioneSocialeXPath
                , string pageListListXPath
                , bool pageListFeaturesEnabled
                , string pageListFeaturesXPath
                , int pageListMaxImageCount
                , string pageListImageFormat
                , string pageListImageXPath
                , string pageListImageThumbListXPath
                , string pageListImageThumbActiveXPath
                , string pageListImageThumbNormalXPath
                , int qrCodeEncoding
                , int qrCodeScale
                , int qrCodeVersion
                , int qrCodeCorrection
                , string qrCodeCharacterSet
                , string qrCodeBackColor
                , string qrCodeForeColor
                , string qrCodeImageFormat
                , string mergeDefinitionName
                , string mergeDefinitionKeyFields
                , string mergeDefinitionColExclude
                , string mergeDefinitionRowExclude)
            {
                var result = new RowInfo();
                if (id == null) return result;
                foreach (var c in COLUMNS)
                {
                    switch (c.Name)
                    {
                        case Columns.ID:
                            if (id != null) result.Add(c, id);
                            break;
                        case Columns.Code:
                            result.Add(c, ColumnInfo.GetDefaultForGuid());
                            break;
                        case Columns.Name:
                            result.Add(c, name);
                            break;
                        case Columns.NormalUrl:
                            result.Add(c, normalUrl);
                            break;
                        case Columns.MobileUrl:
                            result.Add(c, mobileUrl);
                            break;
                        case Columns.Version:
                            result.Add(c, version);
                            break;
                        case Columns.ReleaseDate:
                            result.Add(c, releaseDate);
                            break;
                        case Columns.PageListUrl:
                            result.Add(c, pageListUrl);
                            break;
                        case Columns.PageListUrlRedirects:
                            result.Add(c, pageListUrlRedirects);
                            break;
                        case Columns.PageListMaxCount:
                            result.Add(c, pageListMaxCount);
                            break;
                        case Columns.PageListMaxImageCount:
                            result.Add(c, pageListMaxImageCount);
                            break;
                        case Columns.PageListImageFormat:
                            result.Add(c, pageListImageFormat);
                            break;
                        case Columns.PageListPageCountXPath:
                            result.Add(c, pageListPageCountXPath);
                            break;
                        case Columns.PageListItemCountXPath:
                            result.Add(c, pageListItemCountXPath);
                            break;
                        case Columns.PageListRagioneSocialeXPath:
                            result.Add(c, pageListRagioneSocialeXPath);
                            break;
                        case Columns.PageListListXPath:
                            result.Add(c, pageListListXPath);
                            break;
                        case Columns.PageListFeaturesEnabled:
                            result.Add(c, pageListFeaturesEnabled);
                            break;
                        case Columns.PageListFeaturesXPath:
                            result.Add(c, pageListFeaturesXPath);
                            break;
                        case Columns.PageListImageXPath:
                            result.Add(c, pageListImageXPath);
                            break;
                        case Columns.PageListImageThumbListXPath:
                            result.Add(c, pageListImageThumbListXPath);
                            break;
                        case Columns.PageListImageThumbActiveXPath:
                            result.Add(c, pageListImageThumbActiveXPath);
                            break;
                        case Columns.PageListImageThumbNormalXPath:
                            result.Add(c, pageListImageThumbNormalXPath);
                            break;

                        case Columns.QRCodeEncoding:
                            result.Add(c, qrCodeEncoding);
                            break;
                        case Columns.QRCodeScale:
                            result.Add(c, qrCodeScale);
                            break;
                        case Columns.QRCodeVersion:
                            result.Add(c, qrCodeVersion);
                            break;
                        case Columns.QRCodeCorrection:
                            result.Add(c, qrCodeCorrection);
                            break;
                        case Columns.QRCodeCharacterSet:
                            result.Add(c, qrCodeCharacterSet);
                            break;
                        case Columns.QRCodeBackColor:
                            result.Add(c, qrCodeBackColor);
                            break;
                        case Columns.QRCodeForeColor:
                            result.Add(c, qrCodeForeColor);
                            break;
                        case Columns.QRCodeImageFormat:
                            result.Add(c, qrCodeImageFormat);
                            break;

                        case Columns.MergeDefinitionName:
                            result.Add(c, mergeDefinitionName);
                            break;
                        case Columns.MergeDefinitionKeyFields:
                            result.Add(c, mergeDefinitionKeyFields);
                            break;
                        case Columns.MergeDefinitionColExclude:
                            result.Add(c, mergeDefinitionColExclude);
                            break;
                        case Columns.MergeDefinitionRowExclude:
                            result.Add(c, mergeDefinitionRowExclude);
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }
            private static TableInfo _population;
            public static TableInfo Populate()
            {
                if (_population == null)
                {
                    _population = new TableInfo();
                    int id = 0;
                    _population.Add(DefaultImmobiliare);
                    _population.Add(DefaultCasa);
                    //foreach (RowInfo webSite in User.Populate())
                    //{
                    //    _population.Add(PopulateRow(webSite.GetValueAsInt64(ColumnsBase.ID)
                    //        , webSite.GetValueAsString(WebSite.Columns.Description)
                    //        , webSite.GetValueAsString(WebSite.Columns.NormalUrl)
                    //        , webSite.GetValueAsString(WebSite.Columns.MobileUrl)
                    //        , webSite.GetValueAsString(WebSite.Columns.Version)
                    //        , webSite.GetValueAsDateTime(WebSite.Columns.ReleaseDate).Value
                    //        , webSite.GetValueAsString(WebSite.Columns.PageListUrl)
                    //        , webSite.GetValueAsString(WebSite.Columns.PageListUrlRedirects)
                    //        , webSite.GetValueAsInt32(WebSite.Columns.PageListMaxCount).Value
                    //        , webSite.GetValueAsString(WebSite.Columns.PageListPageNumberXPath)
                    //        , webSite.GetValueAsString(WebSite.Columns.PageListPageCountXPath)
                    //        , webSite.GetValueAsString(WebSite.Columns.PageListItemCountXPath)
                    //        , webSite.GetValueAsString(WebSite.Columns.PageListRagioneSocialeXPath)
                    //        , webSite.GetValueAsString(WebSite.Columns.PageListListXPath)
                    //        , webSite.GetValueAsBoolean(WebSite.Columns.PageListFeaturesEnabled).Value
                    //        , webSite.GetValueAsString(WebSite.Columns.PageListFeaturesXPath)
                    //        , webSite.GetValueAsInt32(WebSite.Columns.PageListMaxImageCount).Value
                    //        , webSite.GetValueAsString(WebSite.Columns.PageListImageFormat)
                    //        , webSite.GetValueAsString(WebSite.Columns.PageListImageXPath)
                    //        , webSite.GetValueAsString(WebSite.Columns.PageListImageThumbActiveXPath)
                    //        , webSite.GetValueAsString(WebSite.Columns.PageListImageThumbNormalXPath)
                    //        , webSite.GetValueAsInt32(WebSite.Columns.QRCodeEncoding).Value
                    //        , webSite.GetValueAsInt32(WebSite.Columns.QRCodeScale).Value
                    //        , webSite.GetValueAsInt32(WebSite.Columns.QRCodeVersion).Value
                    //        , webSite.GetValueAsInt32(WebSite.Columns.QRCodeCorrection).Value
                    //        , webSite.GetValueAsString(WebSite.Columns.QRCodeCharacterSet)
                    //        , webSite.GetValueAsString(WebSite.Columns.QRCodeBackColor)
                    //        , webSite.GetValueAsString(WebSite.Columns.QRCodeForeColor)
                    //        , webSite.GetValueAsString(WebSite.Columns.QRCodeImageFormat)
                    //        , webSite.GetValueAsString(WebSite.Columns.MergeDefinitionName)
                    //        , webSite.GetValueAsString(WebSite.Columns.MergeDefinitionKeyFields)
                    //        , webSite.GetValueAsString(WebSite.Columns.MergeDefinitionColExclude)
                    //        , webSite.GetValueAsString(WebSite.Columns.MergeDefinitionRowExclude)));
                    //    id = _population.LastID();
                    //}
                    id = _population.LastID();
                }
                return _population;
            }

            internal static RowInfo DefaultImmobiliare
            {
                get
                {
                    return PopulateRow(0
                        , "Immobiliare"
                        , @"http://www.immobiliare.it"
                        , @"http://m.immobiliare.it"
                        , @"2.0.1.7"
                        //, new DateTime(2017, 02, 01)
                        //, @"/agenzie_immobiliari/" + MPCommonRes.TextRes.WebSite.KTAGCODICEAGENZIA + @".html?pag=" + MPCommonRes.TextRes.WebSite.KTAGINDICELISTA + @""
                        , new DateTime(2018, 06, 02)
                        , @"/agenzie_immobiliari/" + MPCommonRes.TextRes.WebSite.KTAGNUMEROAGENZIA + @"/" + MPCommonRes.TextRes.WebSite.KTAGCODICEAGENZIA + @"/?pag=" + MPCommonRes.TextRes.WebSite.KTAGINDICELISTA + @""
                        , @"/terreni"
                        , 25
                        , @"//*[@id='pageCount']/strong[1]"
                        , @"//*[@id='pageCount']/strong[2]"
                        , @"//*[@id='pageCount']"
                        , @"//*[@class='body-content agency-page']//*[@class='agency-title text-uppercase']"
                        , @"//*[@id='listing-container']/li/div/div/div/p//a"
                        , true
                        , @"//*[@id='sticky-contact-bottom']//*[@class='row section-data']/dl"
                        , 5
                        , @""
                        , @"//*[@id='foto']//*[@class='labs-gallery__slide']/img"
                        , @""
                        , @""
                        , @""
                        , 0
                        , 1
                        , 0
                        , 3
                        , @"Default UTF-8"
                        , @"White"
                        , @"Black"
                        , @"Png"
                        , @"VdO2013Data.OfferteData, VdO2013Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                        , @"IVDefinitionTypeName=VdO2013Data.OfferteData, VdO2013Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null|MergeContext=Column|MergeKind=IsKey|ReaderName|Codice|"
                        , @"IVDefinitionTypeName=VdO2013Data.OfferteData, VdO2013Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null|MergeContext=Column|MergeKind=Exclude|Ordinal|"
                        , @"IVDefinitionTypeName=VdO2013Data.OfferteData, VdO2013Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null|MergeContext=Row|MergeKind=Exclude|Status=Locked|"
                        );
                }
            }
            internal static RowInfo DefaultCasa
            {
                get
                {
                    return PopulateRow(0
                        , "Casa"
                        , @"http://www.casa.it"
                        , @"http://m.casa.it"
                        , @"2.0.1.7"
                        , new DateTime(2017, 02, 01)
                        , @"/vendita-residenziale/da-" + MPCommonRes.TextRes.WebSite.KTAGCODICEAGENZIA + @"/lista-" + MPCommonRes.TextRes.WebSite.KTAGINDICELISTA
                            + @"|/vendita-commerciale/da-" + MPCommonRes.TextRes.WebSite.KTAGCODICEAGENZIA + @"/lista-" + MPCommonRes.TextRes.WebSite.KTAGINDICELISTA
                            + @"|/affitti-residenziale/da-" + MPCommonRes.TextRes.WebSite.KTAGCODICEAGENZIA + @"/lista-" + MPCommonRes.TextRes.WebSite.KTAGINDICELISTA
                            + @"|/affitti-commerciale/da-" + MPCommonRes.TextRes.WebSite.KTAGCODICEAGENZIA + @"/lista-" + MPCommonRes.TextRes.WebSite.KTAGINDICELISTA
                            + @"|/affitti-vacanze/da-" + MPCommonRes.TextRes.WebSite.KTAGCODICEAGENZIA + @"/lista-" + MPCommonRes.TextRes.WebSite.KTAGINDICELISTA + @""
                        , @""
                        , 25
                        , @"//*[@class='resultsInfo']/span[1]"
                        , @"//*[@class='resultsInfo']/h1[1]/span[1]"
                        , @"//*[@class='agency-body']/h2"
                        , @"//*[@class='body-content agency-page']//*[@class='agency-title text-uppercase']"
                        , @"//*[@class='listing-list']/li/a"
                        , true
                        , @"//*[@id='primaryContent']//*[@id='features']//*[@class='featureListWrapper']//*[@class='featureList']/ul/li"
                        , 5
                        , @"800x600"
                        , @""
                        , @"//div[@id='app']//div[@class='info-gallery']//div[@class='photogallery']"
                        , @"./img[1]"
                        , @""
                        , 0
                        , 1
                        , 0
                        , 3
                        , @"Default UTF-8"
                        , @"White"
                        , @"Black"
                        , @"Png"
                        , @"VdO2013Data.OfferteData, VdO2013Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                        , @"IVDefinitionTypeName=VdO2013Data.OfferteData, VdO2013Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null|MergeContext=Column|MergeKind=IsKey|ReaderName|Codice|"
                        , @"IVDefinitionTypeName=VdO2013Data.OfferteData, VdO2013Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null|MergeContext=Column|MergeKind=Exclude|Ordinal|"
                        , @"IVDefinitionTypeName=VdO2013Data.OfferteData, VdO2013Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null|MergeContext=Row|MergeKind=Exclude|Status=Locked|"
                        );
                }
            }
            public static RowInfo GetWebSite(TableInfo webSites, string name, string normalUrl = null)
            {
                var webSite = from RowInfo s in webSites
                              where s.GetValueIsEqualTo(Columns.Name, name)
                                 || (normalUrl != null ? s.GetValueIsEqualTo(Columns.NormalUrl, normalUrl) : true)
                              select s;

                return webSite.Count() > 0 ? webSite.First() : null;
            }
        }
        #endregion WebSite

        #region WebSiteScript
        public class WebSiteFeature
        {
            public const string Name = LPConsts.DataSet.WebSiteFeature.Name;

            public class Columns : ColumnsBase
            {
                public const string WebSiteID = LPConsts.DataSet.WebSiteFeature.Columns.WebSiteID;
                public const string Name = LPConsts.DataSet.WebSiteFeature.Columns.Name;
                public const string XPath = LPConsts.DataSet.WebSiteFeature.Columns.XPath;
                public const string Caption = LPConsts.DataSet.WebSiteFeature.Columns.Caption;
                public const string Version = LPConsts.DataSet.WebSiteFeature.Columns.Version;
                public const string ReleaseDate = LPConsts.DataSet.WebSiteFeature.Columns.ReleaseDate;
                public const string ForCustomerID = LPConsts.DataSet.WebSiteScript.Columns.ForCustomerID;
                public const string ForProductID = LPConsts.DataSet.WebSiteScript.Columns.ForProductID;

                private static string[] _names;
                public static string[] NAMES
                {
                    get
                    {
                        if (_names == null)
                            _names = new string[] { ID
                            , Code
                            , WebSiteID
                            , Name
                            , XPath
                            , Caption
                            , Version
                            , ReleaseDate
                            , ForCustomerID
                            , ForProductID
                        };
                        return _names;
                    }
                }
            }

            private static ColumnInfo[] _columns;
            public static ColumnInfo[] COLUMNS
            {
                get
                {
                    var cWebSiteID = ColumnInfo.CreateInt64(Columns.WebSiteID
                            , false
                            , string.Format(ColumnInfo.ForeignKeyReferenceFormat, WebSite.Name, WebSite.Columns.ID)
                            , WebSite.Columns.Name + LPConsts.ExtendedProperties.ForeignKeyReferenceDisplayMemberSeparator + WebSite.Columns.Version);
                    cWebSiteID.PrimaryKeyIndex = 0;
                    var cName = ColumnInfo.CreateString(Columns.Name, 255, false);
                    cWebSiteID.PrimaryKeyIndex = 1;
                    var cVersion = ColumnInfo.CreateString(Columns.Version, 50, false);
                    cVersion.DefaultKind = ColumnInfoDefaultKind.None;
                    cVersion.Default = LPConsts.DataSet.Suite.DefaultVolantino.version;
                    if (_columns == null) _columns = new ColumnInfo[] {
                        ColumnsBase.CreateID()
                        , ColumnsBase.CreateCode()
                        , cWebSiteID
                        , cName
                        , ColumnInfo.CreateString(Columns.XPath, 1024, true)
                        , ColumnInfo.CreateString(Columns.Caption, 50, false)
                        , cVersion
                        , ColumnInfo.CreateDateTime(Columns.ReleaseDate, false)
                        , ColumnInfo.CreateInt64(Columns.ForCustomerID
                            , true
                            , string.Format(ColumnInfo.ForeignKeyReferenceFormat, Customer.Name, Customer.Columns.ID)
                            , Customer.Columns.Description)
                        , ColumnInfo.CreateInt64(Columns.ForProductID
                            , true
                            , string.Format(ColumnInfo.ForeignKeyReferenceFormat, Product.Name, Product.Columns.ID)
                            , Product.Columns.Description
                                + LPConsts.ExtendedProperties.ForeignKeyReferenceDisplayMemberSeparator + Product.Columns.Version
                                + LPConsts.ExtendedProperties.ForeignKeyReferenceDisplayMemberSeparator + Product.Columns.Build)
                    };
                    return _columns;
                }
            }

            public static RowInfo PopulateRow(long? id
                , long webSiteID
                , string name
                , string xpath
                , string caption
                , string version
                , DateTime releaseDate
                , long? forCustomerID
                , long? forProductID)
            {
                var result = new RowInfo();
                if (id == null || name == null || xpath == null || caption == null || version == null) return result;
                foreach (var c in COLUMNS)
                {
                    switch (c.Name)
                    {
                        case Columns.ID:
                            result.Add(c, id);
                            break;
                        case Columns.Code:
                            result.Add(c, ColumnInfo.GetDefaultForGuid());
                            break;
                        case Columns.WebSiteID:
                            result.Add(c, name);
                            break;
                        case Columns.Name:
                            result.Add(c, name);
                            break;
                        case Columns.XPath:
                            result.Add(c, string.IsNullOrEmpty(xpath) ? "Default" : xpath);
                            break;
                        case Columns.Caption:
                            result.Add(c, string.IsNullOrEmpty(caption) ? "Default" : caption);
                            break;
                        case Columns.Version:
                            result.Add(c, version);
                            break;
                        case Columns.ReleaseDate:
                            result.Add(c, releaseDate);
                            break;
                        case Columns.ForCustomerID:
                            if (forCustomerID != null)
                                result.Add(c, forCustomerID);
                            break;
                        case Columns.ForProductID:
                            if (forProductID != null)
                                result.Add(c, forProductID);
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }
            private static TableInfo _population;
            public static TableInfo Populate()
            {
                if (_population == null)
                {
                    _population = new TableInfo();
                }
                return _population;
            }

            public static TableInfo GetCustomerFeatures(TableInfo scriptings, int webSiteID, int forCustomerID, int? forProductID = null)
            {
                var customerFeatures = from RowInfo s in scriptings
                                         where s.GetValueIsEqualTo(Columns.WebSiteID, webSiteID)
                                            && s.GetValueIsEqualTo(Columns.ForCustomerID, forCustomerID)
                                            && (forProductID != null ? s.GetValueIsEqualTo(Columns.ForProductID, forProductID) : true)
                                         select s;

                return customerFeatures.ToList<RowInfo>() as TableInfo;
            }
        }
        #endregion WebSiteScript

        #region WebSiteScript
        public class WebSiteScript
        {
            public const string Name = LPConsts.DataSet.WebSiteScript.Name;

            public class Columns : ColumnsBase
            {
                public const string WebSiteID = LPConsts.DataSet.WebSiteScript.Columns.WebSiteID;
                public const string Name = LPConsts.DataSet.WebSiteScript.Columns.Name;
                public const string Category = LPConsts.DataSet.WebSiteScript.Columns.Category;
                public const string Kind = LPConsts.DataSet.WebSiteScript.Columns.Kind;
                public const string Language = LPConsts.DataSet.WebSiteScript.Columns.Language;
                public const string GroupId = LPConsts.DataSet.WebSiteScript.Columns.GroupId;
                public const string GroupIndex = LPConsts.DataSet.WebSiteScript.Columns.GroupIndex;
                public const string GroupCondition = LPConsts.DataSet.WebSiteScript.Columns.GroupCondition;
                public const string Text = LPConsts.DataSet.WebSiteScript.Columns.Text;
                public const string Parameters = LPConsts.DataSet.WebSiteScript.Columns.Parameters;
                public const string Encrypted = LPConsts.DataSet.WebSiteScript.Columns.Encrypted;
                public const string ForCustomerID = LPConsts.DataSet.WebSiteScript.Columns.ForCustomerID;
                public const string ForProductID = LPConsts.DataSet.WebSiteScript.Columns.ForProductID;
                public const string ActiveFrom = LPConsts.DataSet.WebSiteScript.Columns.ActiveFrom;
                public const string ActiveUntil = LPConsts.DataSet.WebSiteScript.Columns.ActiveUntil;
                public const string Notes = LPConsts.DataSet.WebSiteScript.Columns.Notes;

                private static string[] _names;
                public static string[] NAMES
                {
                    get
                    {
                        if (_names == null)
                            _names = new string[] { ID
                            , Code
                            , WebSiteID
                            , Name
                            , Category
                            , Kind
                            , Language
                            , GroupId
                            , GroupIndex
                            , GroupCondition
                            , Text
                            , Parameters
                            , Encrypted
                            , ForCustomerID
                            , ForProductID
                            , ActiveFrom
                            , ActiveUntil
                            , Notes
                        };
                        return _names;
                    }
                }
            }

            private static ColumnInfo[] _columns;
            public static ColumnInfo[] COLUMNS
            {
                get
                {
                    var cWebSiteID = ColumnInfo.CreateInt64(Columns.WebSiteID
                            , false
                            , string.Format(ColumnInfo.ForeignKeyReferenceFormat, WebSite.Name, WebSite.Columns.ID)
                            , WebSite.Columns.Name + LPConsts.ExtendedProperties.ForeignKeyReferenceDisplayMemberSeparator + WebSite.Columns.Version);
                    cWebSiteID.PrimaryKeyIndex = 0;
                    var cName = ColumnInfo.CreateString(Columns.Name, 255, false);
                    cName.PrimaryKeyIndex = 1;

                    if (_columns == null) _columns = new ColumnInfo[] {
                        ColumnsBase.CreateID()
                        , ColumnsBase.CreateCode()
                        , cWebSiteID
                        , cName
                        , ColumnInfo.CreateString(Columns.Category, 50, false)
                        , ColumnInfo.CreateString(Columns.Kind, 50, false)
                        , ColumnInfo.CreateString(Columns.Language, 50, false)
                        , ColumnInfo.CreateString(Columns.GroupId, 50, true)
                        , ColumnInfo.CreateInt(Columns.GroupIndex, true)
                        , ColumnInfo.CreateString(Columns.GroupCondition, 50, true)
                        , ColumnInfo.CreateString(Columns.Text, -1, false, true)
                        , ColumnInfo.CreateString(Columns.Parameters, -1, false)
                        , ColumnInfo.CreateBoolean(Columns.Encrypted, false)
                        , ColumnInfo.CreateInt64(Columns.ForCustomerID
                            , true
                            , string.Format(ColumnInfo.ForeignKeyReferenceFormat, Customer.Name, Customer.Columns.ID)
                            , Customer.Columns.Description)
                        , ColumnInfo.CreateInt64(Columns.ForProductID
                            , true
                            , string.Format(ColumnInfo.ForeignKeyReferenceFormat, Product.Name, Product.Columns.ID)
                            , Product.Columns.Description
                                + LPConsts.ExtendedProperties.ForeignKeyReferenceDisplayMemberSeparator + Product.Columns.Version
                                + LPConsts.ExtendedProperties.ForeignKeyReferenceDisplayMemberSeparator + Product.Columns.Build)
                        , ColumnInfo.CreateDateTime(Columns.ActiveFrom, false)
                        , ColumnInfo.CreateDateTime(Columns.ActiveUntil, true)
                        , ColumnInfo.CreateString(Columns.Notes, 4096, true)
                    };
                    return _columns;
                }
            }

            public static RowInfo PopulateRow(long? id
                , long webSiteID
                , string name
                , string category
                , string kind
                , string language
                , string groupId
                , int groupIndex
                , string groupCondition
                , string text
                , string parameters
                , bool encrypted
                , long? forCustomerID
                , long? forProductID
                , DateTime? activeFrom
                , DateTime? activeUntil = null
                , string notes = null)
            {
                var result = new RowInfo();
                if (id == null || name == null || category == null || kind == null || language == null || text == null) return result;
                foreach (var c in COLUMNS)
                {
                    switch (c.Name)
                    {
                        case Columns.ID:
                            result.Add(c, id);
                            break;
                        case Columns.Code:
                            result.Add(c, ColumnInfo.GetDefaultForGuid());
                            break;
                        case Columns.WebSiteID:
                            result.Add(c, name);
                            break;
                        case Columns.Name:
                            result.Add(c, name);
                            break;
                        case Columns.Category:
                            result.Add(c, string.IsNullOrEmpty(category) ? "Default" : category);
                            break;
                        case Columns.Kind:
                            result.Add(c, string.IsNullOrEmpty(kind) ? "Default" : kind);
                            break;
                        case Columns.Language:
                            result.Add(c, language);
                            break;
                        case Columns.GroupId:
                            result.Add(c, groupId);
                            break;
                        case Columns.GroupIndex:
                            result.Add(c, groupIndex);
                            break;
                        case Columns.GroupCondition:
                            result.Add(c, groupCondition);
                            break;
                        case Columns.Text:
                            result.Add(c, text);
                            break;
                        case Columns.Parameters:
                            result.Add(c, parameters);
                            break;
                        case Columns.Encrypted:
                            result.Add(c, encrypted);
                            break;
                        case Columns.ForCustomerID:
                            if (forCustomerID != null)
                                result.Add(c, forCustomerID);
                            break;
                        case Columns.ForProductID:
                            if (forProductID != null)
                                result.Add(c, forProductID);
                            break;
                        case Columns.ActiveFrom:
                            result.Add(c, activeFrom);
                            break;
                        case Columns.ActiveUntil:
                            if (activeUntil != null)
                                result.Add(c, (DateTime)activeUntil);
                            break;
                        case Columns.Notes:
                            result.Add(c, notes);
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }
            private static TableInfo _population;
            public static TableInfo Populate()
            {
                if (_population == null)
                {
                    _population = new TableInfo();
                }
                return _population;
            }

            public static TableInfo GetCustomerScripts(TableInfo scriptings, int webSiteID, int forCustomerID, int? forProductID = null)
            {
                var customerScripts = from RowInfo s in scriptings
                                      where s.GetValueIsEqualTo(Columns.WebSiteID, webSiteID)
                                         && s.GetValueIsEqualTo(Columns.ForCustomerID, forCustomerID)
                                         && (forProductID != null ? s.GetValueIsEqualTo(Columns.ForProductID, forProductID) : true)
                                      select s;

                return customerScripts.ToList<RowInfo>() as TableInfo;
            }
        }
        #endregion WebSiteScript
    }
}
