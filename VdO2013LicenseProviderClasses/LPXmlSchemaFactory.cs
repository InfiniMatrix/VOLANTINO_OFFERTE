using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
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
    internal class LPXmlSchemaFactory
    {
        //public static void AttachXmlAttributes(XmlAttributeOverrides xao, Type t)
        //{
        //    List<Type> types = new List<Type>();
        //    AttachXmlAttributes(xao, types, t);
        //}

        //public static void AttachXmlAttributes(XmlAttributeOverrides xao, List<Type> all, Type t)
        //{
        //    if (all.Contains(t))
        //        return;
        //    else
        //        all.Add(t);

        //    XmlAttributes attrs = GetAttributeList(t.GetCustomAttributes(false));
        //    xao.Add(t, attrs);

        //    foreach (var prop in t.GetProperties())
        //    {
        //        XmlAttributes innerAttrs = GetAttributeList(prop.GetCustomAttributes(false));
        //        xao.Add(t, prop.Name, innerAttrs);
        //        AttachXmlAttributes(xao, all, prop.PropertyType);
        //    }
        //}

        //private static XmlAttributes GetAttributeList(Object[] attributes)
        //{
        //    XmlAttributes list = new XmlAttributes();
        //    foreach (var attribute in attributes)
        //    {
        //        Type type = attribute.GetType();
        //        if (type.Name == "XmlAttributeAttribute") list.XmlAttribute = (XmlAttributeAttribute)attribute;
        //        else
        //            if (type.Name == "XmlArrayAttribute") list.XmlArray = (XmlArrayAttribute)attribute;
        //            else
        //                if (type.Name == "XmlArrayItemAttribute") list.XmlArrayItems.Add((XmlArrayItemAttribute)attribute);

        //    }
        //    return list;
        //}

        //public static String GetSchema<T>() where T : class
        //{
        //    XmlAttributeOverrides xao = new XmlAttributeOverrides();
        //    AttachXmlAttributes(xao, typeof(T));

        //    XmlReflectionImporter importer = new XmlReflectionImporter(xao);
        //    XmlSchemas schemas = new XmlSchemas();
        //    XmlSchemaExporter exporter = new XmlSchemaExporter(schemas);
        //    XmlTypeMapping map = importer.ImportTypeMapping(typeof(T));
        //    exporter.ExportTypeMapping(map);

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        schemas[0].Write(ms);
        //        ms.Position = 0;
        //        return new StreamReader(ms).ReadToEnd();
        //    }
        //}

        //public static Boolean WriteSchema<T>(String fileName) where T : class
        //{
        //    String schema = GetSchema<T>();
        //    try
        //    {
        //        File.WriteAllText(fileName, schema);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    }
        //    return false;
        //}

        private static XmlTypeCode GetSchemaElementTypeCode(Type type)
        {
            TypeCode tc = Type.GetTypeCode(type);
            switch (tc)
            {
                case TypeCode.Boolean:
                    return XmlTypeCode.Boolean;
                case TypeCode.Byte:
                    return XmlTypeCode.Byte;
                case TypeCode.Char:
                    return XmlTypeCode.String;
                case TypeCode.DBNull:
                    return XmlTypeCode.String;
                case TypeCode.DateTime:
                    return XmlTypeCode.DateTime;
                case TypeCode.Decimal:
                    return XmlTypeCode.Decimal;
                case TypeCode.Double:
                    return XmlTypeCode.Double;
                case TypeCode.Empty:
                    return XmlTypeCode.String;
                case TypeCode.Int16:
                    return XmlTypeCode.Short;
                case TypeCode.Int32:
                    return XmlTypeCode.Int;
                case TypeCode.Int64:
                    return XmlTypeCode.Long;
                case TypeCode.Object:
                    return XmlTypeCode.String;
                case TypeCode.SByte:
                    return XmlTypeCode.Byte;
                case TypeCode.Single:
                    return XmlTypeCode.Short;
                case TypeCode.String:
                    return XmlTypeCode.String;
                case TypeCode.UInt16:
                    return XmlTypeCode.UnsignedShort;
                case TypeCode.UInt32:
                    return XmlTypeCode.UnsignedInt;
                case TypeCode.UInt64:
                    return XmlTypeCode.UnsignedLong;
                default:
                    break;
            }
            return XmlTypeCode.Base64Binary;
        }

        private static XmlSchemaElement CreateSchemaElement(String name, XmlQualifiedName typeName, Boolean isNillable = true, int minOccours = 0, int maxOccours = 1)
        {
            XmlSchemaElement result = new XmlSchemaElement()
            {
                Name = name,
                SchemaTypeName = typeName,
                MinOccurs = (!isNillable && minOccours == 0) ? 1 : minOccours,
                MaxOccurs = maxOccours,
                IsNillable = isNillable
            };
            return result;
        }

        private static XmlSchemaElement CreateSchemaElement(String name, XmlTypeCode typeCode, Boolean isNillable = true, int minOccours = 0, int maxOccours = 1)
        {
            XmlQualifiedName n = XmlSchemaType.GetBuiltInSimpleType(typeCode).QualifiedName;
            return CreateSchemaElement(name, n, isNillable, minOccours, maxOccours);
        }

        private static List<ValidationEventArgs> _CreateSchemaErrors = null;
        public static List<ValidationEventArgs> CreateSchemaErrors
        {
            get
            {
                if (_CreateSchemaErrors == null)
                {
                    _CreateSchemaErrors = new List<ValidationEventArgs>();
                }
                return _CreateSchemaErrors;
            }
        }

        private static IEnumerable<MemberInfo> GetMembersWithAttribute(Type type, Type attributeType, MemberTypes[] memberTypes = null, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {

            if (!attributeType.IsSubclassOf(typeof(Attribute))) throw FormatExcept.Throw("Parameter 'attributeType' is not an attribue.");

            if (memberTypes == null || memberTypes.Length == 0)
                memberTypes = new MemberTypes[] { MemberTypes.Field, MemberTypes.Property };

            var members =
                from mi in type.GetMembers(bindingFlags)
                where (memberTypes.Contains(mi.MemberType) && mi.GetCustomAttributes(true).All(a => attributeType.Equals(a.GetType())))
                select mi;

            return members;
        }

        private static IEnumerable<MemberInfo> GetNonIgnoreMembers(Type type, MemberTypes[] memberTypes, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            var members =
                from mi in type.GetMembers(bindingFlags)
                where (memberTypes.Contains(mi.MemberType) && mi.GetCustomAttributes(true).All(a => a as XmlIgnoreAttribute == null))
                select mi;

            return members;
        }

        private class MemberAndAttribute
        {
            private MemberInfo _member;
            private XmlElementAttribute _element;

            public MemberAndAttribute(MemberInfo member, XmlElementAttribute element)
            {
                this._member = member;
                this._element = element;
            }

            public MemberInfo Member { get { return _member; } }
            public Boolean IsPropertyInfo { get { return _member is PropertyInfo; } }
            public Boolean IsFieldInfo { get { return _member is FieldInfo; } }

            private Boolean InternalHasElement { get { return _element != null; } }

            public XmlElementAttribute Element { get { return _element; } }
            public String ElementName { get { return InternalHasElement ? _element.ElementName : String.Empty; } }
            public String ElementDataType { get { return InternalHasElement ? _element.DataType : String.Empty; } }
            public Type ElementType { get { return InternalHasElement ? _element.Type : null; } }
            public Boolean ElementNullable { get { return InternalHasElement ? _element.IsNullable : false; } }
            public XmlSchemaForm ElementForm { get { return InternalHasElement ? _element.Form : XmlSchemaForm.Unqualified; } }
        }

        private static IEnumerable<MemberAndAttribute> GetMembersAndElement(IEnumerable<MemberInfo> members, Boolean sortByOrder = true)
        {
            var memberWithAttributes =
                from MemberInfo member in members
                from XmlElementAttribute ma in member.GetCustomAttributes(typeof(XmlElementAttribute), true).DefaultIfEmpty()
                orderby (ma == null || !sortByOrder ? -1 : ma.Order)
                select new MemberAndAttribute(member, ma);

            return memberWithAttributes;
        }

        private static IEnumerable<MemberAndAttribute> GetMembersAndElement(Type type, Type attributeType, MemberTypes[] memberTypes = null, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance, Boolean sortByOrder = true)
        {
            return GetMembersAndElement(GetMembersWithAttribute(type, attributeType, memberTypes, bindingFlags), sortByOrder);
        }

        public static XmlSchemas CreateSchema(String targetNameSpace, Type classType, String xsdFileName, Type[] extraClasses, Boolean compileSchemas = true)
        {
            // To determine if the request comes from service request or comes by XSD.exe
            Boolean isXSDApp = HttpContext.Current == null;
            XmlSchemas result = new XmlSchemas();
            if (classType == null) return result;

            // Manages for importing external types
            List<String> extraNamespaces = new List<String>();
            if (extraClasses != null && extraClasses.Length > 0)
            {
                XmlReflectionImporter importer = new XmlReflectionImporter(targetNameSpace);
                XmlSchemaExporter exporter = new XmlSchemaExporter(result);

                foreach (Type t in extraClasses)
                {
                    XmlTypeMapping tMapping = importer.ImportTypeMapping(t);
                    exporter.ExportTypeMapping(tMapping);
                    extraNamespaces.Add(tMapping.Namespace);
                }
            }

            XmlSchema schema = new XmlSchema()
            {
                TargetNamespace = targetNameSpace
            };
            schema.Namespaces.Add("tns", targetNameSpace);
            schema.Namespaces.Add("xs", TextRes.LicenseProvider.GlobalXSNameSpace);
            //classSchema.Namespaces.Add("xsi", VdOLicenseProvider.GlobalXSINameSpace);

            schema.ElementFormDefault = XmlSchemaForm.Qualified;
            //classSchema.AttributeFormDefault = XmlSchemaForm.Unqualified;

            XmlSchemaImport import = new XmlSchemaImport()
            {
                Namespace = TextRes.LicenseProvider.GlobalXSNameSpace
            };
            schema.Includes.Add(import);

            // Defines first Element which is the Class itself
            XmlSchemaElement eClass = new XmlSchemaElement()
            {
                Name = classType.Name
            };
            //eClass.SchemaTypeName = new XmlQualifiedName(classType.Name, targetNameSpace);
            //eClass.IsNillable = true;
            schema.Items.Add(eClass);

            eClass.GetXmlAnnotation(classType.FullName);

            // Gets the corresponding ComplexType
            XmlSchemaComplexType xClass = new XmlSchemaComplexType();
            //xClass.Name = classType.Name;
            //schema.Items.Add(xClass);
            eClass.SchemaType = xClass;

            // Starts the sequence definition for the Class
            XmlSchemaSequence xClassSequence = new XmlSchemaSequence();
            xClass.Particle = xClassSequence;
            // ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- -----
            var MaE = GetMembersAndElement(classType, typeof(XmlElementAttribute));
            // Get all Props and Fields members
            // where XmlIgnore is not specified

            //var members =
            //    from mi in classType.GetMembers(bf)
            //    where (mi.MemberType == MemberTypes.Field || mi.MemberType == MemberTypes.Property)
            //        && mi.GetCustomAttributes(true).All(a => a as XmlIgnoreAttribute == null)
            //    select mi;

            // Get all the member and XmlAttributes

            //var memberWithAttributes =
            //    from MemberInfo member in members
            //    from XmlElementAttribute ma in member.GetCustomAttributes(typeof(XmlElementAttribute), true).DefaultIfEmpty()
            //    orderby (ma != null ? ma.Order : -1)
            //    select new
            //    {
            //        Member = member,
            //        MemberIsProp = (member is PropertyInfo ? true : false),
            //        Attribute = ma,
            //        AttributeName = ma != null ? ma.ElementName : String.Empty,
            //        AttributeDataType = ma != null ? ma.DataType : String.Empty,
            //        AttributeType = ma != null ? ma.Type : null,
            //        AttributeIsNullable = ma != null ? ma.IsNullable : false,
            //        AttributeForm = ma != null ? ma.Form : XmlSchemaForm.Qualified
            //    };

            foreach (var info in MaE)
            {
                String aName = String.IsNullOrEmpty(info.ElementName) ? info.Member.Name : info.ElementName;
                Type aType = info.ElementType ?? (info.IsPropertyInfo ? (info.Member as PropertyInfo).PropertyType : (info.Member as FieldInfo).FieldType);
                String aTypeName = info.ElementDataType;
                Boolean aNillable = info.Element != null ? info.ElementNullable : (!aType.IsValueType || Nullable.GetUnderlyingType(aType) != null);



                if (aType.IsEnum)
                    xClassSequence.Items.Add(aType.GetXmlEnum(aName, schema));
                else
                {
                    if (String.IsNullOrEmpty(aTypeName))
                        xClassSequence.Items.Add(CreateSchemaElement(aName, GetSchemaElementTypeCode(aType), aNillable));
                    else
                        xClassSequence.Items.Add(CreateSchemaElement(aName, new XmlQualifiedName(aTypeName, targetNameSpace), aNillable));
                }
            }
            // ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- -----
            // ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- -----

            if (!isXSDApp)
            {
                XmlWriterSettings ws = new XmlWriterSettings()
                {
                    Encoding = System.Text.Encoding.UTF8,
                    NewLineHandling = NewLineHandling.Entitize,
                    NewLineChars = "\r\n",
                    Indent = true,
                    IndentChars = "  ",
                    NamespaceHandling = NamespaceHandling.Default,
                    OmitXmlDeclaration = false
                };
                try
                {
                    using (var tw = XmlTextWriter.Create(xsdFileName, ws))
                    {
                        schema.Write(tw);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }

            result.Add(schema);

            if (compileSchemas && !isXSDApp)
            {
                CreateSchemaErrors.Clear();
                result.Compile(new ValidationEventHandler(ProvideSchemaValidationCallbackOne), true);
            }
            return result;
        }

        public static void ProvideSchemaValidationCallbackOne(Object sender, ValidationEventArgs args)
        {
            CreateSchemaErrors.Add(args);
        }

        public static XmlSchemaComplexType ProvideSchema(HttpContext current, XmlSchemaSet xs, Type classType, String targetNameSpace, Type[] extraClasses, out ValidationEventArgs[] validationErrors)
        {
            // To determine if the request comes from service request or comes by XSD.exe
            Boolean isXSDApp = HttpContext.Current == null;

            validationErrors = null;
            xs.ValidationEventHandler += ProvideSchemaValidationCallbackOne;

            String xsdFile = String.Empty;
            if (!isXSDApp)
            {
                xsdFile = LPCommon.GetRelativeUri(current, targetNameSpace + @"/" + classType.Name + ".xsd");
                xsdFile = HttpContext.Current.Server.MapPath(xsdFile);
                String errFile = Path.ChangeExtension(xsdFile, ".err");

                // Verifico se precedentemente è stato generato un errore
                // e, nel caso, forzo la riscrittura dello schema
                if (File.Exists(errFile)) File.Delete(xsdFile);
                if (File.Exists(errFile)) File.Delete(errFile);
            }

            if (!File.Exists(xsdFile) || isXSDApp)
            {
                XmlSchemas xss = LPXmlSchemaFactory.CreateSchema(targetNameSpace, classType, xsdFile, extraClasses);
                foreach (XmlSchema s in xss)
                    xs.Add(s);
            }
            else
            {
                XmlSerializer schemaSerializer = new XmlSerializer(typeof(XmlSchema), extraClasses);
                using (XmlTextReader tr = new XmlTextReader(xsdFile))
                {
                    XmlSchema xss = (XmlSchema)schemaSerializer.Deserialize(tr, null);
                    xs.Add(xss);
                }
            }

            XmlSchemaComplexType result = null;
            xs.XmlResolver = new XmlUrlResolver();
            if (xs.Count > 0)
            {
                if (!xs.IsCompiled) xs.Compile();
                if (xs.Count > 0)
                {
                    XmlQualifiedName cName = new XmlQualifiedName(classType.Name, targetNameSpace);
                    result = (XmlSchemaComplexType)xs.Schemas().Cast<XmlSchema>().ToList()[xs.Count - 1].SchemaTypes[cName];
                }
            }

            if (!isXSDApp)
            {
                validationErrors = CreateSchemaErrors.ToArray();
                if (validationErrors.Length > 0)
                {
                    var sb = new System.Text.StringBuilder();
                    foreach (var ve in validationErrors)
                    {
                        sb.AppendLine(String.Format("{0}: {1} (Class: {2}, Line: {3}, Position: {4})", ve.Severity, ve.Message, classType, ve.Exception.LineNumber, ve.Exception.LinePosition));
                    }
                    File.WriteAllText(Path.ChangeExtension(xsdFile, ".err"), sb.ToString());
                }
            }

            return result;
        }

        public static void WriteXml(Object instance, XmlWriter writer)
        {
            var members = GetMembersAndElement(instance.GetType(), typeof(XmlElementAttribute));

            foreach (var info in members)
            {
                Object value = instance.GetType().InvokeMember(info.Member.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty, null, instance, null);
                writer.WriteElementString(info.Member.Name, value != null ? value.ToXmlString() : String.Empty);
            }
        }
    }
}