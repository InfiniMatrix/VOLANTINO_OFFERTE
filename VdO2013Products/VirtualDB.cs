using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VdO2013Products
{
    public enum ColumnInfoDefaultKind
    {
        None = 0,
        Today = 1,
        Now = 2,
        Guid = 4,
        //Delegate = 8
    }

    public class ColumnsBase
    {
        public const String ID = "ID";
        public const String Code = "Code";

        public static ColumnInfo CreateID() { return ColumnInfo.CreateAutoIncrement(ID); }
        public static ColumnInfo CreateCode() { return ColumnInfo.CreateGuid(Code, false); }
    }

    public struct ColumnInfo
    {
        public const String ForeignKeyReferenceFormatSeparator = "!";
        public const String ForeignKeyReferenceFormat = "{0}" + ForeignKeyReferenceFormatSeparator + "{1}";

        public String Name;
        public String Caption;
        public Type DataType;
        public Boolean AllowDBull;
        public Int32 MaxLenght;
        public Boolean AutoIncrement;
        public Int32 AutoIncrementSeed;
        public Int32 AutoIncrementStep;
        public Int16 PrimaryKeyIndex;
        public Boolean Unique;
        public Object Default;
        public Boolean ReadOnly;
        public ColumnInfoDefaultKind DefaultKind;
        public String Expression;
        public String ForeignKeyReference;
        public String ForeignKeyReferenceDisplayMember;

        public ColumnInfo(String name
            , Type dataType
            , Boolean allowDBNull = true
            , Int32 maxLength = -1
            , Boolean autoIncrement = false
            , ColumnInfoDefaultKind kind = ColumnInfoDefaultKind.None
            , String foreignKeyReference = null
            , String foreignKeyReferenceDisplayMember = null)
        {
            this.Name = name;
            this.Caption = Name;
            this.DataType = dataType;
            this.AllowDBull = allowDBNull;
            this.MaxLenght = maxLength;
            this.AutoIncrement = autoIncrement;
            if (this.AutoIncrement)
            {
                this.AutoIncrementSeed = 0;
                this.AutoIncrementStep = 1;
            }
            else
            {
                this.AutoIncrementSeed = -1;
                this.AutoIncrementStep = -1;
            }
            this.PrimaryKeyIndex = -1;
            this.Unique = false;
            this.Default = null;
            this.ReadOnly = false;
            this.DefaultKind = kind;
            this.Expression = String.Empty;
            this.ForeignKeyReference = foreignKeyReference;
            this.ForeignKeyReferenceDisplayMember = foreignKeyReferenceDisplayMember;
        }

        public static ColumnInfo CreateAutoIncrement(String name)
        {
            return new ColumnInfo(name, typeof(Int64), false, -1, true) { Unique = true };
        }
        public static ColumnInfo CreateInt(String name, Boolean allowDBNull = true, String foreignKeyReference = null, String foreignKeyReferenceDisplayMember = null)
        {
            return new ColumnInfo(name, typeof(Int32), allowDBNull) { ForeignKeyReference = foreignKeyReference, ForeignKeyReferenceDisplayMember = foreignKeyReferenceDisplayMember };
        }
        public static ColumnInfo CreateBoolean(String name, Boolean allowDBNull = true)
        {
            return new ColumnInfo(name, typeof(Boolean), allowDBNull);
        }
        public static ColumnInfo CreateInt64(String name, Boolean allowDBNull = true, String foreignKeyReference = null, String foreignKeyReferenceDisplayMember = null)
        {
            return new ColumnInfo(name, typeof(Int64), allowDBNull) { ForeignKeyReference = foreignKeyReference, ForeignKeyReferenceDisplayMember = foreignKeyReferenceDisplayMember };
        }
        public static ColumnInfo CreateString(String name, Int32 maxLength = -1, Boolean allowDBNull = true, Boolean unique = false, String foreignKeyReference = null, String foreignKeyReferenceDisplayMember = null)
        {
            return new ColumnInfo(name, typeof(String), allowDBNull, maxLength) { Unique = unique, ForeignKeyReference = foreignKeyReference, ForeignKeyReferenceDisplayMember = foreignKeyReferenceDisplayMember };
        }
        public static ColumnInfo CreateDateTime(String name, Boolean allowDBNull = true, String foreignKeyReference = null, String foreignKeyReferenceDisplayMember = null)
        {
            return new ColumnInfo(name, typeof(DateTime), allowDBNull) { ForeignKeyReference = foreignKeyReference, ForeignKeyReferenceDisplayMember = foreignKeyReferenceDisplayMember };
        }
        public static ColumnInfo CreateGuid(String name, Boolean allowDBNull = true, String foreignKeyReference = null, String foreignKeyReferenceDisplayMember = null)
        {
            return new ColumnInfo(name, typeof(Guid), allowDBNull, -1, false, ColumnInfoDefaultKind.Guid) { Unique = true, ForeignKeyReference = foreignKeyReference, ForeignKeyReferenceDisplayMember = foreignKeyReferenceDisplayMember };
        }
        public static ColumnInfo CreateToday(String name, Boolean allowDBNull = true)
        {
            return new ColumnInfo(name, typeof(DateTime), allowDBNull, -1, false, ColumnInfoDefaultKind.Today);
        }
        public static ColumnInfo CreateTimeStamp(String name, Boolean allowDBNull = true)
        {
            return new ColumnInfo(name, typeof(DateTime), allowDBNull, -1, false, ColumnInfoDefaultKind.Now);
        }
        public static Object GetDefaultValue(ColumnInfoDefaultKind kind)
        {
            switch (kind)
            {
                case ColumnInfoDefaultKind.None:
                    return DBNull.Value;
                case ColumnInfoDefaultKind.Today:
                    return DateTime.Today;
                case ColumnInfoDefaultKind.Now:
                    return DateTime.Now;
                case ColumnInfoDefaultKind.Guid:
                    return Guid.NewGuid();
                default:
                    break;
            }
            return null;
        }

        public static DateTime GetDefaultForDateTime()
        {
            return (DateTime)GetDefaultValue(ColumnInfoDefaultKind.Now);
        }
        public static DateTime GetDefaultForDate()
        {
            return (DateTime)GetDefaultValue(ColumnInfoDefaultKind.Today);
        }
        public static Guid GetDefaultForGuid()
        {
            return (Guid)GetDefaultValue(ColumnInfoDefaultKind.Guid);
        }
    }

    public class RowInfo : Dictionary<ColumnInfo, Object>
    {
        public Object GetValue(ColumnInfo column)
        {
            return this[column] ?? DBNull.Value;
        }
        public Object GetValue(String column)
        {
            var keys = from k in this.Keys
                       where k.Name.Equals(column)
                       select k;

            if (keys.Count() > 0)
            {
                Object v = this[keys.First()];
                return v ?? DBNull.Value;
            }
            else
                return DBNull.Value;
        }

        public Int16? GetValueAsInt16(ColumnInfo column)
        {
            Object v = GetValue(column);
            Int16 result = -1;
            if (!DBNull.Value.Equals(v)) if (!Int16.TryParse(v.ToString(), out result)) return null;
            return result;
        }

        public Int16? GetValueAsInt16(String column)
        {
            Object v = GetValue(column);
            Int16 result = -1;
            if (!DBNull.Value.Equals(v)) if (!Int16.TryParse(v.ToString(), out result)) return null;
            return result;
        }

        public Int32? GetValueAsInt32(ColumnInfo column)
        {
            Object v = GetValue(column);
            Int32 result = -1;
            if (!DBNull.Value.Equals(v)) if(!Int32.TryParse(v.ToString(), out result)) return null;
            return result;
        }
        public Int32? GetValueAsInt32(String column)
        {
            Object v = GetValue(column);
            Int32 result = -1;
            if (!DBNull.Value.Equals(v)) if (!Int32.TryParse(v.ToString(), out result)) return null;
            return result;
        }

        public Int64? GetValueAsInt64(ColumnInfo column)
        {
            Object v = GetValue(column);
            Int64 result = -1;
            if (!DBNull.Value.Equals(v)) if(!Int64.TryParse(v.ToString(), out result)) return null;
            return result;
        }
        public Int64? GetValueAsInt64(String column)
        {
            Object v = GetValue(column);
            Int64 result = -1;
            if (!DBNull.Value.Equals(v)) if (!Int64.TryParse(v.ToString(), out result)) return null;
            return result;
        }

        public String GetValueAsString(ColumnInfo column)
        {
            Object v = GetValue(column);
            String result = String.Empty;
            if (!DBNull.Value.Equals(v)) result = v.ToString();
            return result;
        }
        public String GetValueAsString(String column)
        {
            Object v = GetValue(column);
            String result = String.Empty;
            if (!DBNull.Value.Equals(v)) result = v.ToString();
            return result;
        }

        public DateTime? GetValueAsDateTime(ColumnInfo column)
        {
            Object v = GetValue(column);
            DateTime result = DateTime.MinValue;
            if (!DBNull.Value.Equals(v)) if (!DateTime.TryParse(v.ToString(), out result)) return null;
            return result;
        }
        public DateTime? GetValueAsDateTime(String column)
        {
            Object v = GetValue(column);
            DateTime result = DateTime.MinValue;
            if (!DBNull.Value.Equals(v)) if (!DateTime.TryParse(v.ToString(), out result)) return null;
            return result;
        }

        public Boolean? GetValueAsBoolean(ColumnInfo column)
        {
            Object v = GetValue(column);
            Boolean result = false;
            if (!DBNull.Value.Equals(v)) if(!Boolean.TryParse(v.ToString(), out result)) return null;
            return result;
        }
        public Boolean? GetValueAsBoolean(String column)
        {
            Object v = GetValue(column);
            Boolean result = false;
            if (!DBNull.Value.Equals(v)) if (!Boolean.TryParse(v.ToString(), out result)) return null;
            return result;
        }

        public ColumnInfo this[String column]
        {
            get
            {
                var keys = from k in this.Keys
                           where k.Name.Equals(column)
                           select k;

                if (keys.Count() > 0)
                    return keys.First();
                else
                    return default(ColumnInfo);
            }
        }

        public Object[] GetValues()
        {
            return this.Values.ToArray<Object>();
        }

        public Boolean GetValueIsEqualTo(ColumnInfo column, Object value)
        {
            if (String.IsNullOrEmpty(column.Name)) return false;

            Object v = GetValue(column);

            if (v == null && value == null) return true;
            if (DBNull.Value.Equals(v) && DBNull.Value.Equals(value)) return true;
            return String.Format("{0}", v).Equals(String.Format("{0}", value));
        }

        public Boolean GetValueIsEqualTo(String column, Object value)
        {
            Object v = GetValue(column);

            if (v == null && value == null) return true;
            if (DBNull.Value.Equals(v) && DBNull.Value.Equals(value)) return true;
            return String.Format("{0}", v).Equals(String.Format("{0}", value));
        }
    }

    public class TableInfo : List<RowInfo>
    {
        new public void Add(RowInfo row)
        {
            base.Add(row);
            foreach (var c in row.ToList())
            {
                switch (c.Key.DefaultKind)
                {
                    case ColumnInfoDefaultKind.None:
                        break;
                    case ColumnInfoDefaultKind.Today:
                        row[c.Key] = ColumnInfo.GetDefaultForDate();
                        break;
                    case ColumnInfoDefaultKind.Now:
                        row[c.Key] = ColumnInfo.GetDefaultForDateTime();
                        break;
                    case ColumnInfoDefaultKind.Guid:
                        row[c.Key] = ColumnInfo.GetDefaultForGuid();
                        break;
                    default:
                        break;
                }
            }
        }

        public int LastID()
        {
            int result = 0;
            if (Count > 0)
                int.TryParse(this[Count - 1][ColumnsBase.ID].ToString(), out result);
            return result;
        }
    }
}
