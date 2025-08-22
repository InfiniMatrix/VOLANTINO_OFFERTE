using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    public class LPMessage
    {
        #region Private Fields
        private String _id;
        private DateTime _timeStamp;
        private LPValue _result;
        private LPError _error;
        #endregion Private Fields

        #region ctor
        /// <summary>
        /// Costruttore senza parametri per la serializzazione.
        /// </summary>
        public LPMessage()
        {
            this._id = LPCommon.GetNewID(LPIdentityKind.MagicCode);
            this._timeStamp = DateTime.UtcNow;
            this.Operation = null;

            this.KeyPairMagicCode = null;

            this._result = new LPValue();
            this._error = new LPError(this);
        }
        /// <summary>
        /// Costruttore che inizializza Operation.
        /// </summary>
        public LPMessage(String operation) : this() { this.Operation = operation; }
        /// <summary>
        /// Costruttore che inizializza Operation e LPValue.
        /// </summary>
        public LPMessage(String operation, Object value) : this(operation) { this.Result.Value = value; }
        /// <summary>
        /// Costruttore che inizializza Operation, LPError e LPValue.
        /// </summary>
        public LPMessage(String operation, Exception error, Object value) : this(operation, value) { this.Error.Error = error; }
        /// <summary>
        /// Costruttore che inizializza Operation, LPError e LPValue.
        /// </summary>
        public LPMessage(String operation, String errorMessage, Object value) : this(operation, value) { this.Error.FromString(errorMessage); }
        #endregion ctor

        #region Public Fields
        /// <summary>
        /// Progressivo della creazione dell'oggetto.
        /// </summary>
        [XmlElement(Order = 0, IsNullable = false)]
        public String ID { get { return _id; } set { } }

        /// <summary>
        /// TimeStamp di creazione dell'oggetto LPMessage.
        /// </summary>
        [XmlElement(Order = 1, IsNullable = false)]
        public DateTime TimeStamp { get { return _timeStamp; } set { } }

        /// <summary>
        /// Versione della classe LPMessage.
        /// </summary>
        [XmlElement(Order = 2, IsNullable = false)]
        public String Version { get { return TextRes.LicenseProvider.GlobalVersion; } set { } }

        [XmlElement(Order = 3, IsNullable = false)]
        public String Operation;

        /// <summary>
        /// Espone il valore di LPValue.
        /// </summary>
        [XmlElement(Order = 4, IsNullable = false)]
        public LPValue Result { get { return _result; } set { } }

        /// <summary>
        /// Espone il valore di LPError.
        /// </summary>
        [XmlElement(Order = 5, IsNullable = false)]
        public LPError Error { get { return _error; } set { } }

        [XmlElement(Order = 6, IsNullable = true)]
        public String EmptyString = @"null";

        [XmlElement(Order = 7, IsNullable = false)]
        public String LogMessage;
        #endregion Public Fields

        /// <summary>
        /// Restituisce il contenuto di della classe in un oggetto DataTable.
        /// </summary>
        [XmlInclude(typeof(DataTable))]
        public DataTable AsDataTable()
        {
            DataTable result = new DataTable(String.IsNullOrEmpty(this.Operation) ? String.Format("Error{0}", this.ID) : this.Operation);

            using (DataColumn c = result.Columns.Add())
            {
                c.ColumnName = "ID";
                c.DataType = typeof(String);
                c.ReadOnly = true;
                c.AllowDBNull = false;
            }
            using (DataColumn c = result.Columns.Add())
            {
                c.ColumnName = "TimeStamp";
                c.DataType = typeof(DateTime);
                c.ReadOnly = true;
                c.AllowDBNull = false;
            }
            using (DataColumn c = result.Columns.Add())
            {
                c.ColumnName = "HasValue";
                c.DataType = typeof(Boolean);
                c.ReadOnly = true;
                c.AllowDBNull = true;
            }
            using (DataColumn c = result.Columns.Add())
            {
                c.ColumnName = "Value";
                c.DataType = typeof(String);
                c.ReadOnly = true;
                c.AllowDBNull = true;
            }
            using (DataColumn c = result.Columns.Add())
            {
                c.ColumnName = "Type";
                c.DataType = typeof(String);
                c.ReadOnly = true;
                c.AllowDBNull = true;
            }
            using (DataColumn c = result.Columns.Add())
            {
                c.ColumnName = "HasError";
                c.DataType = typeof(Boolean);
                c.ReadOnly = true;
                c.AllowDBNull = true;
            }
            using (DataColumn c = result.Columns.Add())
            {
                c.ColumnName = "ExceptionMessage";
                c.DataType = typeof(String);
                c.ReadOnly = true;
                c.AllowDBNull = true;
            }
            using (DataColumn c = result.Columns.Add())
            {
                c.ColumnName = "ExceptionType";
                c.DataType = typeof(String);
                c.ReadOnly = true;
                c.AllowDBNull = true;
            }
            using (DataColumn c = result.Columns.Add())
            {
                c.ColumnName = "ExceptionToString";
                c.DataType = typeof(String);
                c.ReadOnly = true;
                c.AllowDBNull = true;
            }

            DataRow r = result.NewRow();
            try
            {
                r["ID"] = this.ID;
                r["TimeStamp"] = this.TimeStamp;
                r["HasValue"] = this.Result.HasValue;
                if (this.Result.HasValue)
                {
                    r["Value"] = this.Result.ToText();
                    r["Type"] = this.Result.TypeName;
                }
                r["HasError"] = this.Error.Assigned;
                if (this.Error.Assigned)
                {
                    r["ExceptionMessage"] = this.Error.Message;
                    r["ExceptionType"] = this.Error.TypeName;
                    r["ExceptionToString"] = this.Error.Text;
                }
                result.Rows.Add(r);
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// Restituisce LPMessage come String.
        /// </summary>
        public override String ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendFormat("Id:{0}", this.ID);
            sb.AppendFormat("; Version:{0}", this.Version);
            sb.AppendFormat("; Operation:{0}", !String.IsNullOrEmpty(this.Operation) ? this.Operation : this.EmptyString);
            sb.AppendFormat("; TimeStamp:{0}", this.TimeStamp.ToString("yyyy-MM-dd HH.mm.ss"));
            sb.AppendFormat("; HasResult:{0}", this.Result.HasValue);
            sb.AppendFormat("; ResultType:{0}", this.Result.TypeName);
            sb.AppendFormat("; HasError:{0}", this.Error.Assigned);
            sb.AppendFormat("; ErrorType:{0}", this.Error.Assigned ? this.Error.TypeName : this.Error.EmptyString);
            sb.AppendFormat("; HasInnerResult:{0}", this.Result.HasRawValue);
            sb.AppendFormat("; InnerResultType:{0}", this.Result.RawTypeName);
            return sb.ToString();
        }

        [XmlIgnore]
        public String KeyPairMagicCode;

        public static LPMessage CreateEmpty(String message) { return new LPMessage(message); }
        public static LPMessage CreateError(String message) { return new LPMessage(message, new Exception(message)); }
        public static LPMessage CreateError(Exception error) { return new LPMessage(error.Message, error); }

#if DEBUG
        public String debugToString()
        {
            return String.Format("operation={0};\treturnValue={1};\texception={2}", Operation, ToString(), this.Error.Assigned ? this.Error.Error.ToString() : "null");
        }
#endif

        public void CopyTo(LPMessage message)
        {
            this.Result.CopyTo(message.Result);
            this.Error.CopyTo(message.Error);
        }
    }
}