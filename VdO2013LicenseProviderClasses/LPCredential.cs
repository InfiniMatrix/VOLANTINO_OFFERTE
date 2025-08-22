using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    public class LPCredential
    {
        #region Private Fields
        private String _id;
        private DateTime _timeStamp;
        private String _login;
        private String _password;
        private String _context;
        private String _magicCode;
        #endregion Private Fields

        #region ctor
        /// <summary>
        /// Costrutture senza parametri necessario per la serializzazione.
        /// </summary>
        protected LPCredential()
        {
            this._id = LPCommon.GetNewID(LPIdentityKind.MagicCode);
            this._timeStamp = DateTime.UtcNow;
            this._login = null;
            this._password = null;
            this._context = null;
            this._magicCode = null;
        }
        /// <summary>
        /// Crea un oggetto LPCredential e lo inizializza con Login e Password.
        /// </summary>
        public LPCredential(String login, String password) : this()
        {
            this._login = login;
            this._password = password;
        }
        /// <summary>
        /// Crea un oggetto LPCredential e lo inizializza con Login, Password e Context.
        /// </summary>
        public LPCredential(String login, String password, String context) : this(login, password)
        {
            this._context = context;
        }
        #endregion ctor

        private String NewMagicCode(params String[] args)
        {
            if (args == null || args.Length == 0)
            {
                return "Insufficient parameters";
            }

            if (args.Length == 1)
            {
                if (args[0].Length <= 10)
                {
                    return "Only one parameter given: length must be 10 characters(at least)";
                }
            }

            String argsString = String.Join("|", args);
            if (argsString.Length < 10)
            {
                return String.Format("{0} parameters given: total length must be 10 characters(at least)", args.Length);
            }

            int argCode = 29011976;
            for (int i = 0; i < argsString.Length; i++)
            {
                if (i % 2 == 0)
                {
                    argCode -= (int)argsString[i];
                }
                else
                {
                    argCode -= ((int)argsString[i] * 2);
                }
            }

            var r = new System.Random(argCode).Next();
            return r.ToString();
        }

        private Boolean TryNewMagicCode()
        {
            if (String.IsNullOrEmpty(_login) || String.IsNullOrEmpty(_password) || String.IsNullOrEmpty(_context)) return false;
            try
            {
                _magicCode = NewMagicCode(_login, _password, _context);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToReccurentString());
            }
            return false;
        }

        /// <summary>
        /// Progressivo dell'oggetto creato.
        /// </summary>
        [XmlElement(Order = 0)]
        public String ID { get { return _id; } set { } }
        /// <summary>
        /// TimeStamp della creazione dell'oggetto.
        /// </summary>
        [XmlElement(Order = 1)]
        public DateTime TimeStamp { get { return _timeStamp; } set { } }
        /// <summary>
        /// Versione della classe LPCredendial.
        /// </summary>
        [XmlElement(Order = 2)]
        public String Version { get { return TextRes.LicenseProvider.GlobalVersion; } set { } }
        /// <summary>
        /// Proprietà Login.
        /// </summary>
        [XmlElement(Order = 3)]
        public String Login { get { return _login; } set { _login = value; TryNewMagicCode(); } }
        /// <summary>
        /// Proprietà password (Attenzione: il valore non viene criptato).
        /// </summary>
        [XmlElement(Order = 4)]
        public String Password { get { return _password; } set { _password = value; TryNewMagicCode(); } }
        /// <summary>
        /// Contesto di sicurezza della credenziale.
        /// </summary>
        [XmlElement(Order = 5)]
        public String Context { get { return _context; } set { _context = value; TryNewMagicCode(); } }
        /// <summary>
        /// Vedere la classe LPMagicCode.
        /// </summary>
        [XmlElement(Order = 6)]
        public String MagicCode { get { return _magicCode; } set { } }

#if LICENSEPROVIDER_VER3
        public override string ToString()
        {
            return string.Format("{0}\\{1}[Magic={2};Version={3}]", Context, Login, MagicCode, Version);
        }
#endif
    }
}