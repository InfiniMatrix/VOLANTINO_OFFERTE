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
    public class LPError
    {
        #region Private Fields
        private LPMessage _result;
        private Exception _error;
        #endregion Private Fields

        #region ctor
        /// <summary>
        /// Costruttore senza parametri necessario per la serializzazione.
        /// </summary>
        protected LPError() { }
        public LPError(LPMessage result) : this() { _result = result; }
        public LPError(LPMessage result, Exception error) : this(result) { Error = error; }
        public LPError(LPMessage result, String errorMessage) : this(result, new Exception(errorMessage)) { }
        /// <summary>
        /// Costruttore con parametri.
        /// </summary>
        public LPError(LPMessage result, String errorMessage, Exception innerException) : this(result, new Exception(errorMessage, innerException)) { }
        #endregion ctor

        /// <summary>
        /// Indica se esiste un errore.
        /// </summary>
        [XmlElement(Order = 0, IsNullable = false)]
#if LICENSEPROVIDER_VER2
        public Boolean Exists { get { return Error != null; } set { } }
#elif LICENSEPROVIDER_VER3
        public Boolean Assigned { get { return Error != null; } set { } }
#endif
        #region Error Get & Set
        private Exception InternalGetError()
        {
            return _error;
        }
        /// <summary>
        /// Copia le proprietà dell'oggetto Exception nella classe LPError.
        /// </summary>
        private void InternalSetError(Exception value)
        {
            this._error = value;
            if (_result != null && _result.Result != null) _result.Result.Value = LPBoolean.False;
        }
        #endregion Error Get & Set

        /// <summary>
        /// Crea un oggetto LPError a partire da una eccezione.
        /// </summary>
        public static LPError FromString(LPMessage result, String message, Exception innerException = null) { return new LPError(result, message, innerException); }

        public void FromString(String message, Exception innerException = null) { Error = new Exception(message, innerException); }

        /// <summary>
        /// La class Exception non può essere serializzata quindi le sue proprietà vengono estratte e rappresentate nella classe LPError
        /// </summary>
        [XmlIgnore]
        public Exception Error { get { return InternalGetError(); } set { InternalSetError(value); } }

        /// <summary>
        /// Se esiste un un eccezione ne riporta la classe.
        /// </summary>
        [XmlIgnore]
        public Type Type { get { return Assigned ? _error.GetType() : null; } set { } }

        /// <summary>
        /// Se esiste un'eccezione ne riporta il nome della classe.
        /// </summary>
        [XmlElement(Order = 1, IsNullable = false)]
        public String TypeName { get { return Assigned ? LPMessageHelper.GetTypeName(Type, EmptyString) : EmptyString; } set { } }

        /// <summary>
        /// Messaggio dell'eccezione.
        /// </summary>
        [XmlElement(Order = 2, IsNullable = false)]
        public String Message { get { return Assigned ? Error.Message : EmptyString; } set { } }

        /// <summary>
        /// Proprietà Text dell'eccezione.
        /// </summary>
        [XmlElement(Order = 3, IsNullable = false)]
        public String Text { get { return Assigned ? Error.ToString() : EmptyString; } set { } }

        /// <summary>
        /// Proprietà Source dell'eccezione.
        /// </summary>
        [XmlElement(Order = 4, IsNullable = false)]
        public String Source { get { return Assigned && Error.Source != null ? Error.Source : EmptyString; } set { } }

        /// <summary>
        /// Proprietà Stack dell'eccezione.
        /// </summary>
        [XmlElement(Order = 5, IsNullable = false)]
        public String Stack { get { return Assigned && Error.StackTrace != null ? Error.StackTrace : EmptyString; } set { } }

        [XmlElement(Order = 6, IsNullable = true)]
        public String EmptyString = @"null";

#if LICENSEPROVIDER_VER3
        public void CopyTo(LPError error)
        {
            if (Assigned)
            {
                error._result = this._result;
                error._error = this._error;
            }
        }
#endif
    }
}
