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
    //[Serializable]
    //[XmlSchemaProvider(LPCommon.Consts.XmlProvideSchemaMethodName)]
    //[XmlRoot(ElementName = ClassTypeName, IsNullable = false, Namespace = ClassNameSpace)]
    public class LPMagicCode //: ISerializable
    //, IXmlSerializable
    {
        //public const String ClassTypeName = LPConsts.LPMagicCodeClassTypeName;
        //public const String ClassNameSpace = LPConsts.GlobalNameSpace + "/" + LPConsts.SchemaFolderName;
        //public const String ClassSchemaFile = ClassNameSpace + @"/" + ClassTypeName + @".xsd";
        //public static readonly Type ClassType = typeof(LPMagicCode);
        //public static readonly Type[] ExtraClasses = new Type[] { };

        private String _id;
        private DateTime _timeStamp;
        private String _code;

        /// <summary>
        /// Progressivo della creazione dell'oggetto.
        /// </summary>
        [XmlElement(Order = 0)]
        public String ID { get { return _id; } set { } }
        /// <summary>
        /// TimeStamp della creazione dell'oggetto.
        /// </summary>
        [XmlElement(Order = 1)]
        public DateTime TimeStamp { get { return _timeStamp; } set { } }
        /// <summary>
        /// Versione della classe LPMagicCode.
        /// </summary>
        [XmlElement(Order = 2)]
        public String Version { get { return TextRes.LicenseProvider.GlobalVersion; } set { } }
        /// <summary>
        /// Codice generato.
        /// </summary>
        [XmlElement(Order = 3)]
        public String Code { get { return _code; } set { } }

        /// <summary>
        /// Costruttore senza parametri necessario per la serializzazione.
        /// </summary>
        public LPMagicCode()
        {
            this._id = LPCommon.GetNewID(LPIdentityKind.MagicCode);
            this._timeStamp = DateTime.UtcNow;
            this._code = null;
        }

        /// <summary>
        /// Costruttore che inizializza la proprietà Code.
        /// </summary>
        public LPMagicCode(String code)
            : this()
        {
            this._code = code;
        }

        public override String ToString()
        {
            return _code;
        }

        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    info.AddValue("ID", ID);
        //    info.AddValue("TimeStamp", TimeStamp.ToString("o"));
        //    info.AddValue("Code", Code);
        //}

        //public static XmlSchemaComplexType ProvideSchema(XmlSchemaSet xs)
        //{
        //    ValidationEventArgs[] validationErrors = null;
        //    return LPXmlSchemaFactory.ProvideSchema(HttpContext.Current, xs, ClassType, ClassNameSpace, ExtraClasses, out validationErrors);
        //}

        //public System.Xml.Schema.XmlSchema GetSchema()
        //{
        //    return (null);
        //}

        //public void ReadXml(System.Xml.XmlReader reader)
        //{
        //    //throw new NotImplementedException();
        //}

        //public void WriteXml(System.Xml.XmlWriter writer)
        //{
        //    LPXmlSchemaFactory.WriteXml(this, writer);
        //    //writer.WriteElementString("id", ID);//<--- Dovrebbe dare errore se validata contro XSD
        //    //writer.WriteElementString("Version", Version);
        //    //writer.WriteElementString("TimeStamp", TimeStamp.ToXmlString());
        //    //writer.WriteElementString("Code", Code);
        //}
    }
}