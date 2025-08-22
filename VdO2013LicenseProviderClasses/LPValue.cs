using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.IO;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.Serialization.Advanced;

using MPExtensionMethods;

namespace VdO2013LicenseProviderClasses
{
    [Serializable]
    public class LPValue
    {
#if LICENSEPROVIDER_VER2
        #region VER2
        #region Private Fields
        private Object _inner;
        private Object _value;
        #endregion Private Fields

        #region ctor
        /// <summary>
        /// Costruttore senza parametri necessario per la serializzazione.
        /// </summary>
        public LPValue()
        {
            _inner = null;
            _value = null;
        }
        /// <summary>
        /// Costrutture che inizializza la proprietà Value.
        /// </summary>
        public LPValue(Object value) : this() { Value = value; }
        #endregion ctor

        #region Value
        #region Value Get & Set
        /// <summary>
        /// Metodo interno per accedere direttamente al valore dell'oggetto Value.
        /// </summary>
        private Object GetValue() { return _value; }
        /// <summary>
        /// Metodo interno per impostare il valore interno dell'oggetto Value.
        /// </summary>
        private void SetValue(Object value)
        {
            _inner = value;
            String xmlText = null;
            if (value is Guid)
            {
                _value = ((Guid)value).ToString();
            }
            else
                if (LPMessageHelper.ValueTryReadXml(value, out xmlText))
                {
                    _value = xmlText;
                }
                else
                    _value = value;
        }
        #endregion Value Get & Set

        /// <summary>
        /// Dato che non tutti gli oggetti possono essere serializzati da un web service
        /// Value viene sempre convertito in un tipo *restituibile* mentre l'istanza vera
        /// e propria dell'oggetto viene conservata in InnerValue (non pubblicato)
        /// </summary>
        [XmlElement(Order = 0)]
        public Object Value { get { return GetValue(); } set { SetValue(value); } }

        /// <summary>
        /// Restituisce la classe dell'oggetto Value interno.
        /// </summary>
        [XmlIgnore]
        public Type ValueType { get { return Exists ? Value.GetType() : null; } set { } }

        /// <summary>
        /// Proprietà di ispezione per valutare se il valore esiste.
        /// </summary>
        [XmlElement(Order = 1, IsNullable = false)]
        public Boolean Exists { get { return !IsNull && !IsDBNull; } set { } }

        /// <summary>
        /// Restituisce il nome della classe dell'oggetto Value interno.
        /// </summary>
        [XmlElement(Order = 2, IsNullable = false)]
        public String TypeName { get { return Exists ? LPMessageHelper.GetTypeName(Value.GetType(), EmptyTypeName) : EmptyTypeName; } set { } }

        /// <summary>
        /// Restituisce il codice di tipo della classe dell'oggetto Value interno.
        /// </summary>
        [XmlElement(Order = 3, IsNullable = false)]
        public TypeCode TypeCode { get { return Exists ? LPMessageHelper.GetTypeCode(Value.GetType(), EmptyTypeCode) : EmptyTypeCode; } set { } }
        #endregion Value

        #region Inner
        /// <summary>
        /// Valore interno.
        /// </summary>
        [XmlIgnore]
        public Object Inner { get { return _inner; } }

        /// <summary>
        /// Tipo del valore interno.
        /// </summary>
        [XmlIgnore]
        public Type InnerType { get { return InnerExists ? Inner.GetType() : null; } set { } }

        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è stato inizializzato.
        /// </summary>
        [XmlElement(Order = 4, IsNullable = false)]
        public Boolean InnerExists { get { return _inner != null; } }

        /// <summary>
        /// Nome del tipo del valore interno.
        /// </summary>
        [XmlElement(Order = 5, IsNullable = false)]
        public String InnerTypeName { get { return InnerExists ? LPMessageHelper.GetTypeName(Inner.GetType(), EmptyTypeName) : EmptyTypeName; } set { } }

        /// <summary>
        /// Restituisce il codice di tipo della classe dell'oggetto Value interno.
        /// </summary>
        [XmlElement(Order = 6, IsNullable = false)]
        public TypeCode InnerTypeCode { get { return InnerExists ? LPMessageHelper.GetTypeCode(Inner.GetType(), EmptyTypeCode) : EmptyTypeCode; } set { } }
        #endregion Inner

        #region Is Properties
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un null.
        /// </summary>
        [XmlElement(Order = 7, IsNullable = false)]
        public Boolean IsNull { get { return Value == null; } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un DBNull.
        /// </summary>
        [XmlElement(Order = 8, IsNullable = false)]
        public Boolean IsDBNull { get { return DBNull.Value.Equals(Value); } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore non è valorizzato.
        /// </summary>
        [XmlElement(Order = 9, IsNullable = false)]
        public Boolean IsEmpty
        {
            get
            {
                if (IsNull || IsDBNull) return false;
                return (IsInt16 && ToInt16().Equals(EmptyInt16))
                    || (IsInt32 && ToInt32().Equals(EmptyInt32))
                    || (IsInt64 && ToInt64().Equals(EmptyInt64))
                    || (IsDateTime && ToDateTime().Equals(EmptyDateTime))
                    || (IsBoolean && ToBoolean().Equals(EmptyBoolean))
                    || (IsDataSet && ToDataSet().Equals(EmptyDataSet))
                    || (IsDataTable && ToDataTable().Equals(EmptyDataTable))
                    || (IsDataView && ToDataView().Equals(EmptyDataView))
                    || (IsXmlData && ToXmlData().Equals(EmptyXmlData))
                    || (IsText && ToText().Equals(EmptyString))
                    ;
            }
            set { }
        }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un String.
        /// </summary>
        [XmlElement(Order = 10, IsNullable = false)]
        public Boolean IsText { get { return ValueType == typeof(String); } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un Int16.
        /// </summary>
        [XmlElement(Order = 11, IsNullable = false)]
        public Boolean IsInt16 { get { return ValueType == typeof(Int16); } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un Int32.
        /// </summary>
        [XmlElement(Order = 12, IsNullable = false)]
        public Boolean IsInt32 { get { return ValueType == typeof(Int32); } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un Int64.
        /// </summary>
        [XmlElement(Order = 13, IsNullable = false)]
        public Boolean IsInt64 { get { return ValueType == typeof(Int64); } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un DateTime.
        /// </summary>
        [XmlElement(Order = 14, IsNullable = false)]
        public Boolean IsDateTime { get { return ValueType == typeof(DateTime); } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un LPBoolean.
        /// </summary>
        [XmlElement(Order = 15, IsNullable = false)]
        public Boolean IsBoolean { get { return ValueType == typeof(LPBoolean); } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un DataSet.
        /// </summary>
        [XmlElement(Order = 16, IsNullable = false)]
        public Boolean IsDataSet { get { return Inner is DataSet; } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un DataTable.
        /// </summary>
        [XmlElement(Order = 17, IsNullable = false)]
        public Boolean IsDataTable { get { return Inner is DataTable; } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un DataView.
        /// </summary>
        [XmlElement(Order = 18, IsNullable = false)]
        public Boolean IsDataView { get { return Inner is DataView; } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un Xml.
        /// </summary>
        [XmlElement(Order = 19, IsNullable = false)]
        public Boolean IsXmlData { get { return Inner != null && LPMessageHelper.ValueIsXmlData(Inner.GetType()); } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un Guid.
        /// </summary>
        [XmlElement(Order = 20, IsNullable = false)]
        public Boolean IsGuid { get { return ValueType == typeof(Guid); } set { } }
        #endregion Is Properties

        #region Public Fields
        /// <summary>
        /// Valore di un oggetto vuoto.
        /// </summary>
        [XmlElement(Order = 21, IsNullable = true)]
        public String EmptyTypeName = DefaultEmptyTypeName;
        /// <summary>
        /// Valore di un oggetto vuoto.
        /// </summary>
        [XmlElement(Order = 22, IsNullable = false)]
        public TypeCode EmptyTypeCode = DefaultEmptyTypeCode;

        /// <summary>
        /// Valore di un String vuoto.
        /// </summary>
        [XmlElement(Order = 23, IsNullable = true)]
        public String EmptyString = DefaultEmptyString;

        /// <summary>
        /// Valore di un Int16 vuoto.
        /// </summary>
        [XmlElement(Order = 24, IsNullable = false)]
        public Int16 EmptyInt16 = DefaultEmptyInt16;

        /// <summary>
        /// Valore di un Int32 vuoto.
        /// </summary>
        [XmlElement(Order = 25, IsNullable = false)]
        public Int32 EmptyInt32 = DefaultEmptyInt32;

        /// <summary>
        /// Valore di un Int64 vuoto.
        /// </summary>
        [XmlElement(Order = 26, IsNullable = false)]
        public Int64 EmptyInt64 = DefaultEmptyInt64;

        /// <summary>
        /// Valore di un DateTime vuoto.
        /// </summary>
        [XmlElement(Order = 27, IsNullable = false)]
        public DateTime EmptyDateTime = DefaultEmptyDateTime;

        /// <summary>
        /// Valore di un Boolean vuoto.
        /// </summary>
        [XmlElement(Order = 28, IsNullable = false)]
        public LPBoolean EmptyBoolean = DefaultEmptyBoolean;

        /// <summary>
        /// Valore di un Xml vuoto.
        /// </summary>
        [XmlElement(Order = 29, IsNullable = true)]
        public String EmptyXmlData = DefaultEmptyXmlData;

        /// <summary>
        /// Valore di un DataSet vuoto.
        /// </summary>
        [XmlElement(Order = 30, IsNullable = true)]
        public static readonly DataSet EmptyDataSet = DefaultEmptyDataSet;

        /// <summary>
        /// Valore di un DataTable vuoto.
        /// </summary>
        [XmlElement(Order = 31, IsNullable = true)]
        public static readonly DataTable EmptyDataTable = DefaultEmptyDataTable;

        //[XmlElement(Order = 32, IsNullable = true)]
        /// <summary>
        /// Valore di un DataView vuoto.
        /// </summary>
        public static readonly DataView EmptyDataView = DefaultEmptyDataView;

        /// <summary>
        /// Valore di un Guid vuoto.
        /// </summary>
        [XmlElement(Order = 32, IsNullable = false)]
        public static readonly Guid EmptyGuid = DefaultEmptyGuid;
        #endregion Public Fields

        #region Public Static ReadOnly
        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 33, IsNullable = true)]
        public static readonly String DefaultEmptyTypeName = @"null";
        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 34, IsNullable = true)]
        public static readonly TypeCode DefaultEmptyTypeCode = TypeCode.Empty;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 35, IsNullable = true)]
        public static readonly String DefaultEmptyString = @"null";

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 36, IsNullable = false)]
        public static readonly Int16 DefaultEmptyInt16 = Int16.MinValue;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 37, IsNullable = false)]
        public static readonly Int32 DefaultEmptyInt32 = Int32.MinValue;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 38, IsNullable = false)]
        public static readonly Int64 DefaultEmptyInt64 = Int64.MinValue;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 39, IsNullable = false)]
        public static readonly DateTime DefaultEmptyDateTime = DateTime.MinValue;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 40, IsNullable = false)]
        public static readonly LPBoolean DefaultEmptyBoolean = LPBoolean.Unassigned;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 41, IsNullable = true)]
        public static readonly String DefaultEmptyXmlData = @"<!-- empty -->";

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 42, IsNullable = true)]
        public static readonly DataSet DefaultEmptyDataSet = null;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 43, IsNullable = true)]
        public static readonly DataTable DefaultEmptyDataTable = null;

        //[XmlElement(Order = 44, IsNullable = true)]
        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        public static readonly DataView DefaultEmptyDataView = null;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 44, IsNullable = false)]
        public static readonly Guid DefaultEmptyGuid = Guid.Empty;
        #endregion Public Static ReadOnly

        #region As Properties
        /// <summary>
        /// Restituisce InnerValue come un String se IsString è vera.
        /// </summary>
        [XmlElement(Order = 45, IsNullable = true)]
        public String AsText { get { return ToText(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un Int16 se IsInt16 è vera.
        /// </summary>
        [XmlElement(Order = 46, IsNullable = true)]
        public Int16? AsInt16 { get { return ToInt16(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un Int32 se IsInt32 è vera.
        /// </summary>
        [XmlElement(Order = 47, IsNullable = true)]
        public Int32? AsInt32 { get { return ToInt32(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un Int64 se IsInt64 è vera.
        /// </summary>
        [XmlElement(Order = 48, IsNullable = true)]
        public Int64? AsInt64 { get { return ToInt64(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un DateTime se IsDateTime è vera.
        /// </summary>
        [XmlElement(Order = 49, IsNullable = true)]
        public DateTime? AsDateTime { get { return ToDateTime(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un Boolean? se IsBoolean è vera.
        /// </summary>
        [XmlElement(Order = 50, IsNullable = true)]
        public LPBoolean? AsBoolean { get { return ToBoolean(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un DataSet se IsDataSet è vera.
        /// </summary>
        [XmlElement(Order = 51, IsNullable = true)]
        public DataSet AsDataSet { get { return ToDataSet(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un DataTable se IsDataTable è vera.
        /// </summary>
        [XmlElement(Order = 52, IsNullable = true)]
        public DataTable AsDataTable { get { return ToDataTable(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un DataView se IsDataView è vera.
        /// </summary>
        [XmlIgnore]
        public DataView AsDataView { get { return ToDataView(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un Guid se IsGuid è vera.
        /// </summary>
        [XmlElement(Order = 53, IsNullable = false)]
        public Guid AsGuid { get { return ToGuid(); } set { } }
        #endregion

        #region To Methods
        /// <summary>
        /// Restituisce InnerValue come String.
        /// </summary>
        public String ToText() { return IsText ? (String)Value : EmptyString; }
        /// <summary>
        /// Restituisce InnerValue come Int16.
        /// </summary>
        public Int16 ToInt16() { return IsInt16 ? Convert.ToInt16(Value) : EmptyInt16; }
        /// <summary>
        /// Restituisce InnerValue come Int32.
        /// </summary>
        public Int32 ToInt32() { return IsInt32 ? Convert.ToInt32(Value) : EmptyInt32; }
        /// <summary>
        /// Restituisce InnerValue come Int64.
        /// </summary>
        public Int64 ToInt64() { return IsInt64 ? Convert.ToInt64(Value) : EmptyInt64; }
        /// <summary>
        /// Restituisce InnerValue come DateTime.
        /// </summary>
        public DateTime ToDateTime() { return IsDateTime ? Convert.ToDateTime(Value) : EmptyDateTime; }
        /// <summary>
        /// Restituisce InnerValue come LPBoolean.
        /// </summary>
        public LPBoolean ToBoolean() { return IsBoolean ? (LPBoolean)Value : EmptyBoolean; }
        /// <summary>
        /// Restituisce InnerValue come Xml.
        /// </summary>
        public String ToXmlData() { return IsXmlData ? (String)Value : EmptyXmlData; }
        /// <summary>
        /// Restituisce InnerValue come DataSet.
        /// </summary>
        public DataSet ToDataSet() { return IsDataSet ? _inner as DataSet : EmptyDataSet; }
        /// <summary>
        /// Restituisce InnerValue come DataTable.
        /// </summary>
        public DataTable ToDataTable() { return IsDataTable ? _inner as DataTable : EmptyDataTable; }
        /// <summary>
        /// Restituisce InnerValue come DataView.
        /// </summary>
        public DataView ToDataView() { return IsDataView ? _inner as DataView : EmptyDataView; }
        /// <summary>
        /// Restituisce InnerValue come Guid.
        /// </summary>
        public Guid ToGuid() { return IsGuid ? (Guid)_inner : EmptyGuid; }
        #endregion To Methods

        #region Equals Methods
        /// <summary>
        /// Metodo di ispezione per valutare se InnerValue è uguale a un String.
        /// </summary>
        /// <param name="value"></param>
        public Boolean Equals(String value) { return IsText && ToText().Equals(value); }
        /// <summary>
        /// Metodo di ispezione per valutare se InnerValue è uguale a un Int16.
        /// </summary>
        /// <param name="value"></param>
        public Boolean Equals(Int16 value) { return IsInt16 && ToInt16().Equals(value); }
        /// <summary>
        /// Metodo di ispezione per valutare se InnerValue è uguale a un Int32.
        /// </summary>
        /// <param name="value"></param>
        public Boolean Equals(Int32 value) { return IsInt32 && ToInt32().Equals(value); }
        /// <summary>
        /// Metodo di ispezione per valutare se InnerValue è uguale a un Int64.
        /// </summary>
        /// <param name="value"></param>
        public Boolean Equals(Int64 value) { return IsInt64 && ToInt64().Equals(value); }
        /// <summary>
        /// Metodo di ispezione per valutare se InnerValue è uguale a un DateTime.
        /// </summary>
        /// <param name="value"></param>
        public Boolean Equals(DateTime value) { return IsDateTime && ToDateTime().Equals(value); }
        /// <summary>
        /// Metodo di ispezione per valutare se InnerValue è uguale a un LPBoolean.
        /// </summary>
        /// <param name="value"></param>
        public Boolean Equals(LPBoolean value) { return IsBoolean && ToBoolean().Equals(value); }
        /// <summary>
        /// Metodo di ispezione per valutare se InnerValue è uguale a un Guid.
        /// </summary>
        public Boolean Equals(Guid value) { return IsGuid && ToGuid().Equals(value); }
        #endregion Equals Methods
        #endregion VER2
#elif LICENSEPROVIDER_VER3
        #region VER3
        #region Private Fields
        private Object _raw;
        private Object _value;

        private bool _valueChanged;
        private bool _rawChanged;

        #endregion Private Fields

        #region ctor
        /// <summary>
        /// Costruttore senza parametri necessario per la serializzazione.
        /// </summary>
        public LPValue()
        {
            _raw = null;
            _value = null;
        }
        /// <summary>
        /// Costrutture che inizializza la proprietà Value.
        /// </summary>
        public LPValue(Object value) : this() { Value = value; }
        #endregion ctor

        #region Value

        #region Value Get & Set
        /// <summary>
        /// Metodo interno per accedere direttamente al valore dell'oggetto Value.
        /// </summary>
        private Object InternalGetValue() { return _value; }
        /// <summary>
        /// Metodo interno per impostare il valore interno dell'oggetto Value.
        /// </summary>
        private void InternalSetValue(Object value)
        {
            _raw = value;
            _rawChanged = true;

            if (value == null) return;
            String xmlText = null;
            if (value is Guid)
            {
                _value = ((Guid)value).ToString();
            }
            else if (LPMessageHelper.ValueTryReadXml(value, out xmlText))
            {
                _value = xmlText;
            }
            else
            {
                _value = value;
            }
            _valueChanged = true;
        }
        #endregion Value Get & Set

        /// <summary>
        /// Dato che non tutti gli oggetti possono essere serializzati da un web service
        /// Value viene sempre convertito in un tipo *restituibile* mentre l'istanza vera
        /// e propria dell'oggetto viene conservata in InnerValue (non pubblicato)
        /// </summary>
        [XmlElement(Order = 0)]
        public Object Value { get { return InternalGetValue(); } set { InternalSetValue(value); } }

        /// <summary>
        /// Proprietà di ispezione per valutare se il valore esiste.
        /// </summary>
        [XmlElement(Order = 1, IsNullable = false)]
        public Boolean HasValue { get { return !IsNull && !IsDBNull; } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è stato cambiato.
        /// Se il valore viene impostato a null, Changed ritornerà sempre False.
        /// Vedi anche <see cref="RawChanged"/> verifica se è stato mai eseguito il set della proprietà Value.
        /// </summary>
        /// <remarks>Se il valore viene impostato a null, Changed ritornerà sempre False.</remarks>
        [XmlElement(Order = 2, IsNullable = false)]
        public Boolean Changed { get { return _valueChanged; } set { } }

        /// <summary>
        /// Restituisce la classe dell'oggetto Value interno.
        /// </summary>
        [XmlIgnore]
        public Type Type { get { return HasValue ? Value.GetType() : null; } set { } }
        /// <summary>
        /// Restituisce il nome della classe dell'oggetto Value interno.
        /// </summary>
        [XmlElement(Order = 3, IsNullable = false)]
        public String TypeName { get { return HasValue ? LPMessageHelper.GetTypeName(Value.GetType(), EmptyTypeName) : EmptyTypeName; } set { } }
        /// <summary>
        /// Restituisce il codice di tipo della classe dell'oggetto Value interno.
        /// </summary>
        [XmlElement(Order = 4, IsNullable = false)]
        public TypeCode TypeCode { get { return HasValue ? LPMessageHelper.GetTypeCode(Value.GetType(), EmptyTypeCode) : EmptyTypeCode; } set { } }
        #endregion Value

        #region Raw
        /// <summary>
        /// Valore interno.
        /// </summary>
        [XmlIgnore]
        public Object Raw { get { return _raw; } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è stato inizializzato.
        /// </summary>
        [XmlElement(Order = 5, IsNullable = false)]
        public Boolean HasRawValue { get { return _raw != null; } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è stato cambiato.
        /// Se il valore viene impostato a null, Changed ritornerà sempre False.
        /// Vedi anche <see cref=""/>
        /// </summary>
        /// <remarks>Se il valore viene impostato a null, Changed ritornerà sempre False.</remarks>
        [XmlElement(Order = 6, IsNullable = false)]
        public Boolean RawChanged { get { return _rawChanged; } set { } }

        /// <summary>
        /// Tipo del valore interno.
        /// </summary>
        [XmlIgnore]
        public Type RawType { get { return HasRawValue ? Raw.GetType() : null; } set { } }
        /// <summary>
        /// Nome del tipo del valore interno.
        /// </summary>
        [XmlElement(Order = 7, IsNullable = false)]
        public String RawTypeName { get { return HasRawValue ? LPMessageHelper.GetTypeName(Raw.GetType(), EmptyTypeName) : EmptyTypeName; } set { } }

        /// <summary>
        /// Restituisce il codice di tipo della classe dell'oggetto Value interno.
        /// </summary>
        [XmlElement(Order = 8, IsNullable = false)]
        public TypeCode RawTypeCode { get { return HasRawValue ? LPMessageHelper.GetTypeCode(Raw.GetType(), EmptyTypeCode) : EmptyTypeCode; } set { } }
        #endregion Raw

        #region Is Properties
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un null.
        /// </summary>
        [XmlElement(Order = 9, IsNullable = false)]
        public Boolean IsNull { get { return Value == null; } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un DBNull.
        /// </summary>
        [XmlElement(Order = 10, IsNullable = false)]
        public Boolean IsDBNull { get { return DBNull.Value.Equals(Value); } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore non è valorizzato.
        /// </summary>
        [XmlElement(Order = 11, IsNullable = false)]
        public Boolean IsEmpty
        {
            get
            {
                if (IsNull || IsDBNull) return false;
                return (IsInt16 && ToInt16().Equals(EmptyInt16))
                    || (IsInt32 && ToInt32().Equals(EmptyInt32))
                    || (IsInt64 && ToInt64().Equals(EmptyInt64))
                    || (IsDateTime && ToDateTime().Equals(EmptyDateTime))
                    || (IsBoolean && ToBoolean().Equals(EmptyBoolean))
                    || (IsDataSet && ToDataSet().Equals(EmptyDataSet))
                    || (IsDataTable && ToDataTable().Equals(EmptyDataTable))
                    || (IsDataView && ToDataView().Equals(EmptyDataView))
                    || (IsXmlData && ToXmlData().Equals(EmptyXmlData))
                    || (IsText && ToText().Equals(EmptyString))
                    ;
            }
            set { }
        }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un String.
        /// </summary>
        [XmlElement(Order = 12, IsNullable = false)]
        public Boolean IsText { get { return Type == typeof(String); } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un Int16.
        /// </summary>
        [XmlElement(Order = 13, IsNullable = false)]
        public Boolean IsInt16 { get { return Type == typeof(Int16); } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un Int32.
        /// </summary>
        [XmlElement(Order = 14, IsNullable = false)]
        public Boolean IsInt32 { get { return Type == typeof(Int32); } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un Int64.
        /// </summary>
        [XmlElement(Order = 15, IsNullable = false)]
        public Boolean IsInt64 { get { return Type == typeof(Int64); } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un DateTime.
        /// </summary>
        [XmlElement(Order = 16, IsNullable = false)]
        public Boolean IsDateTime { get { return Type == typeof(DateTime); } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un LPBoolean.
        /// </summary>
        [XmlElement(Order = 17, IsNullable = false)]
        public Boolean IsBoolean { get { return Type == typeof(LPBoolean); } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un DataSet.
        /// </summary>
        [XmlElement(Order = 18, IsNullable = false)]
        public Boolean IsDataSet { get { return Raw is DataSet; } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un DataTable.
        /// </summary>
        [XmlElement(Order = 19, IsNullable = false)]
        public Boolean IsDataTable { get { return Raw is DataTable; } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un DataView.
        /// </summary>
        [XmlElement(Order = 20, IsNullable = false)]
        public Boolean IsDataView { get { return Raw is DataView; } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un Xml.
        /// </summary>
        [XmlElement(Order = 21, IsNullable = false)]
        public Boolean IsXmlData { get { return Raw != null && LPMessageHelper.ValueIsXmlData(Raw.GetType()); } set { } }
        /// <summary>
        /// Proprietà di ispezione per valutare se il valore è un Guid.
        /// </summary>
        [XmlElement(Order = 22, IsNullable = false)]
        public Boolean IsGuid { get { return Type == typeof(Guid); } set { } }
        #endregion Is Properties

        #region Public Fields
        /// <summary>
        /// Valore di un oggetto vuoto.
        /// </summary>
        [XmlElement(Order = 23, IsNullable = true)]
        public String EmptyTypeName = DefaultEmptyTypeName;
        /// <summary>
        /// Valore di un oggetto vuoto.
        /// </summary>
        [XmlElement(Order = 24, IsNullable = false)]
        public TypeCode EmptyTypeCode = DefaultEmptyTypeCode;

        /// <summary>
        /// Valore di un String vuoto.
        /// </summary>
        [XmlElement(Order = 25, IsNullable = true)]
        public String EmptyString = DefaultEmptyString;

        /// <summary>
        /// Valore di un Int16 vuoto.
        /// </summary>
        [XmlElement(Order = 26, IsNullable = false)]
        public Int16 EmptyInt16 = DefaultEmptyInt16;

        /// <summary>
        /// Valore di un Int32 vuoto.
        /// </summary>
        [XmlElement(Order = 27, IsNullable = false)]
        public Int32 EmptyInt32 = DefaultEmptyInt32;

        /// <summary>
        /// Valore di un Int64 vuoto.
        /// </summary>
        [XmlElement(Order = 28, IsNullable = false)]
        public Int64 EmptyInt64 = DefaultEmptyInt64;

        /// <summary>
        /// Valore di un DateTime vuoto.
        /// </summary>
        [XmlElement(Order = 29, IsNullable = false)]
        public DateTime EmptyDateTime = DefaultEmptyDateTime;

        /// <summary>
        /// Valore di un Boolean vuoto.
        /// </summary>
        [XmlElement(Order = 30, IsNullable = false)]
        public LPBoolean EmptyBoolean = DefaultEmptyBoolean;

        /// <summary>
        /// Valore di un Xml vuoto.
        /// </summary>
        [XmlElement(Order = 31, IsNullable = true)]
        public String EmptyXmlData = DefaultEmptyXmlData;

        /// <summary>
        /// Valore di un DataSet vuoto.
        /// </summary>
        [XmlElement(Order = 32, IsNullable = true)]
        public static readonly DataSet EmptyDataSet = DefaultEmptyDataSet;

        /// <summary>
        /// Valore di un DataTable vuoto.
        /// </summary>
        [XmlElement(Order = 33, IsNullable = true)]
        public static readonly DataTable EmptyDataTable = DefaultEmptyDataTable;

        /// <summary>
        /// Valore di un DataView vuoto.
        /// </summary>
        [XmlIgnore]
        public static readonly DataView EmptyDataView = DefaultEmptyDataView;

        /// <summary>
        /// Valore di un Guid vuoto.
        /// </summary>
        [XmlElement(Order = 34, IsNullable = false)]
        public static readonly Guid EmptyGuid = DefaultEmptyGuid;
        #endregion Public Fields

        #region Public Static ReadOnly
        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 35, IsNullable = true)]
        public static readonly String DefaultEmptyTypeName = @"null";
        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 36, IsNullable = true)]
        public static readonly TypeCode DefaultEmptyTypeCode = TypeCode.Empty;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 37, IsNullable = true)]
        public static readonly String DefaultEmptyString = @"null";

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 38, IsNullable = false)]
        public static readonly Int16 DefaultEmptyInt16 = Int16.MinValue;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 39, IsNullable = false)]
        public static readonly Int32 DefaultEmptyInt32 = Int32.MinValue;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 40, IsNullable = false)]
        public static readonly Int64 DefaultEmptyInt64 = Int64.MinValue;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 41, IsNullable = false)]
        public static readonly DateTime DefaultEmptyDateTime = DateTime.MinValue;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 42, IsNullable = false)]
        public static readonly LPBoolean DefaultEmptyBoolean = LPBoolean.Unassigned;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 43, IsNullable = true)]
        public static readonly String DefaultEmptyXmlData = @"<!-- empty -->";

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 44, IsNullable = true)]
        public static readonly DataSet DefaultEmptyDataSet = null;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 45, IsNullable = true)]
        public static readonly DataTable DefaultEmptyDataTable = null;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlIgnore]
        public static readonly DataView DefaultEmptyDataView = null;

        /// <summary>
        /// Valore di default di un _value non inizializzato.
        /// </summary>
        [XmlElement(Order = 46, IsNullable = false)]
        public static readonly Guid DefaultEmptyGuid = Guid.Empty;
        #endregion Public Static ReadOnly

        #region As Properties
        /// <summary>
        /// Restituisce InnerValue come un String se IsString è vera.
        /// </summary>
        [XmlElement(Order = 47, IsNullable = true)]
        public String AsText { get { return ToText(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un Int16 se IsInt16 è vera.
        /// </summary>
        [XmlElement(Order = 48, IsNullable = true)]
        public Int16? AsInt16 { get { return ToInt16(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un Int32 se IsInt32 è vera.
        /// </summary>
        [XmlElement(Order = 49, IsNullable = true)]
        public Int32? AsInt32 { get { return ToInt32(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un Int64 se IsInt64 è vera.
        /// </summary>
        [XmlElement(Order = 50, IsNullable = true)]
        public Int64? AsInt64 { get { return ToInt64(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un DateTime se IsDateTime è vera.
        /// </summary>
        [XmlElement(Order = 51, IsNullable = true)]
        public DateTime? AsDateTime { get { return ToDateTime(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un Boolean? se IsBoolean è vera.
        /// </summary>
        [XmlElement(Order = 52, IsNullable = true)]
        public LPBoolean? AsBoolean { get { return ToBoolean(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un DataSet se IsDataSet è vera.
        /// </summary>
        [XmlElement(Order = 53, IsNullable = true)]
        public DataSet AsDataSet { get { return ToDataSet(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un DataTable se IsDataTable è vera.
        /// </summary>
        [XmlElement(Order = 54, IsNullable = true)]
        public DataTable AsDataTable { get { return ToDataTable(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un DataView se IsDataView è vera.
        /// </summary>
        [XmlIgnore]
        public DataView AsDataView { get { return ToDataView(); } set { } }
        /// <summary>
        /// Restituisce InnerValue come un Guid se IsGuid è vera.
        /// </summary>
        [XmlElement(Order = 55, IsNullable = false)]
        public Guid AsGuid { get { return ToGuid(); } set { } }
        #endregion

        #region To Methods
        /// <summary>
        /// Restituisce InnerValue come String.
        /// </summary>
        public String ToText() { return IsText ? (String)Value : EmptyString; }
        /// <summary>
        /// Restituisce InnerValue come Int16.
        /// </summary>
        public Int16 ToInt16() { return IsInt16 ? Convert.ToInt16(Value) : EmptyInt16; }
        /// <summary>
        /// Restituisce InnerValue come Int32.
        /// </summary>
        public Int32 ToInt32() { return IsInt32 ? Convert.ToInt32(Value) : EmptyInt32; }
        /// <summary>
        /// Restituisce InnerValue come Int64.
        /// </summary>
        public Int64 ToInt64() { return IsInt64 ? Convert.ToInt64(Value) : EmptyInt64; }
        /// <summary>
        /// Restituisce InnerValue come DateTime.
        /// </summary>
        public DateTime ToDateTime() { return IsDateTime ? Convert.ToDateTime(Value) : EmptyDateTime; }
        /// <summary>
        /// Restituisce InnerValue come LPBoolean.
        /// </summary>
        public LPBoolean ToBoolean() { return IsBoolean ? (LPBoolean)Value : EmptyBoolean; }
        /// <summary>
        /// Restituisce InnerValue come Xml.
        /// </summary>
        public String ToXmlData() { return IsXmlData ? (String)Value : EmptyXmlData; }
        /// <summary>
        /// Restituisce InnerValue come DataSet.
        /// </summary>
        public DataSet ToDataSet() { return IsDataSet ? _raw as DataSet : EmptyDataSet; }
        /// <summary>
        /// Restituisce InnerValue come DataTable.
        /// </summary>
        public DataTable ToDataTable() { return IsDataTable ? _raw as DataTable : EmptyDataTable; }
        /// <summary>
        /// Restituisce InnerValue come DataView.
        /// </summary>
        public DataView ToDataView() { return IsDataView ? _raw as DataView : EmptyDataView; }
        /// <summary>
        /// Restituisce InnerValue come Guid.
        /// </summary>
        public Guid ToGuid() { return IsGuid ? (Guid)_raw : EmptyGuid; }
        #endregion To Methods

        #region Equals Methods
        /// <summary>
        /// Metodo di ispezione per valutare se InnerValue è uguale a un String.
        /// </summary>
        /// <param name="value"></param>
        public Boolean Equals(String value) { return IsText && ToText().Equals(value); }
        /// <summary>
        /// Metodo di ispezione per valutare se InnerValue è uguale a un Int16.
        /// </summary>
        /// <param name="value"></param>
        public Boolean Equals(Int16 value) { return IsInt16 && ToInt16().Equals(value); }
        /// <summary>
        /// Metodo di ispezione per valutare se InnerValue è uguale a un Int32.
        /// </summary>
        /// <param name="value"></param>
        public Boolean Equals(Int32 value) { return IsInt32 && ToInt32().Equals(value); }
        /// <summary>
        /// Metodo di ispezione per valutare se InnerValue è uguale a un Int64.
        /// </summary>
        /// <param name="value"></param>
        public Boolean Equals(Int64 value) { return IsInt64 && ToInt64().Equals(value); }
        /// <summary>
        /// Metodo di ispezione per valutare se InnerValue è uguale a un DateTime.
        /// </summary>
        /// <param name="value"></param>
        public Boolean Equals(DateTime value) { return IsDateTime && ToDateTime().Equals(value); }
        /// <summary>
        /// Metodo di ispezione per valutare se InnerValue è uguale a un LPBoolean.
        /// </summary>
        /// <param name="value"></param>
        public Boolean Equals(LPBoolean value) { return IsBoolean && ToBoolean().Equals(value); }
        /// <summary>
        /// Metodo di ispezione per valutare se InnerValue è uguale a un Guid.
        /// </summary>
        public Boolean Equals(Guid value) { return IsGuid && ToGuid().Equals(value); }
        #endregion Equals Methods

        public void CopyTo(LPValue value)
        {
            value._raw = this._raw;
            value._value = this._value;
        }
        #endregion VER3
#endif
    }
}
