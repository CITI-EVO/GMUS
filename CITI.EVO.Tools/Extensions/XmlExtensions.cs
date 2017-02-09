using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using CITI.EVO.Tools.Comparers;

namespace CITI.EVO.Tools.Extensions
{
    public static class XmlExtensions
    {
        private static readonly Regex xmlNsRegex = new Regex(@"(xmlns:?[^=]*=[""][^""]*[""])", RegexOptions.Compiled |
                                                                                               RegexOptions.IgnoreCase |
                                                                                               RegexOptions.Multiline);

        public static String ToJson(this XDocument xDoc)
        {
            return xDoc.ToJson(true);
        }
        public static String ToJson(this XDocument xDoc, bool showNodeName)
        {
            var sb = new StringBuilder();
            sb.Append("{ ");

            XmlToJsonNode(sb, xDoc.Root, showNodeName);

            sb.Append("}");

            return sb.ToString();
        }

        public static String ToJson(this XElement xElem)
        {
            return xElem.ToJson(true);
        }
        public static String ToJson(this XElement xElem, bool showNodeName)
        {
            var sb = new StringBuilder();
            sb.Append("{ ");

            XmlToJsonNode(sb, xElem, showNodeName);

            sb.Append("}");

            return sb.ToString();
        }

        public static String ToJson(this XmlDocument xmlDoc)
        {
            return xmlDoc.ToJson(true);
        }
        public static String ToJson(this XmlDocument xmlDoc, bool showNodeName)
        {
            var sb = new StringBuilder();
            sb.Append("{ ");

            XmlToJsonNode(sb, xmlDoc.DocumentElement, showNodeName);
            sb.Append("}");

            return sb.ToString();
        }

        private static void XmlToJsonNode(StringBuilder sb, XElement node, bool showNodeName)
        {
            if (showNodeName)
            {
                sb.Append("\"" + SafeJsonText(node.Name.LocalName) + "\": ");
            }

            sb.Append("{");
            // Build a sorted list of key-value pairs
            //  where   key is case-sensitive nodeName
            //          value is an ArrayList of string or XmlElement
            //  so that we know whether the nodeName is an array or not.
            var childNodeNames = new SortedList();

            //  Add in all node attributes
            foreach (var attr in node.Attributes())
            {
                SetChildNode(childNodeNames, attr.Name.LocalName, attr.Value);
            }

            //  Add in all nodes
            foreach (var childNode in node.Nodes())
            {
                if (childNode is XText)
                {
                    var xText = (XText)childNode;
                    SetChildNode(childNodeNames, "value", xText.Value);
                }
                else if (childNode is XElement)
                {
                    var xElement = (XElement)childNode;
                    SetChildNode(childNodeNames, xElement.Name.LocalName, childNode);
                }
            }

            // Now output all stored info
            foreach (String childName in childNodeNames.Keys)
            {
                var arrayList = (ArrayList)childNodeNames[childName];
                if (arrayList.Count == 0)
                {
                    sb.Append(" \"" + SafeJsonText(childName) + "\": \"\", ");

                }
                else if (arrayList.Count == 1)
                {
                    OutputXmlNode(childName, arrayList[0], sb, true);
                }
                else
                {
                    sb.Append(" \"" + SafeJsonText(childName) + "\": [ ");

                    foreach (var child in arrayList)
                    {
                        OutputXmlNode(childName, child, sb, false);
                    }

                    sb.Remove(sb.Length - 2, 2);
                    sb.Append(" ], ");
                }
            }

            sb.Remove(sb.Length - 2, 2);
            sb.Append(" }");
        }
        private static void XmlToJsonNode(StringBuilder sb, XmlNode node, bool showNodeName)
        {
            if (showNodeName)
            {
                sb.Append("\"" + SafeJsonText(node.Name) + "\": ");
            }

            sb.Append("{");
            // Build a sorted list of key-value pairs
            //  where   key is case-sensitive nodeName
            //          value is an ArrayList of string or XmlElement
            //  so that we know whether the nodeName is an array or not.
            var childNodeNames = new SortedList();

            //  Add in all node attributes
            if (node.Attributes != null)
            {
                foreach (XmlAttribute attr in node.Attributes)
                {
                    SetChildNode(childNodeNames, attr.Name, attr.InnerText);
                }
            }

            //  Add in all nodes
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode is XmlText)
                {
                    SetChildNode(childNodeNames, "value", childNode.InnerText);
                }
                else if (childNode is XmlElement)
                {
                    SetChildNode(childNodeNames, childNode.Name, childNode);
                }
            }

            // Now output all stored info
            foreach (String childName in childNodeNames.Keys)
            {
                var arrayList = (ArrayList)childNodeNames[childName];
                if (arrayList.Count == 0)
                {
                    sb.Append(" \"" + SafeJsonText(childName) + "\": \"\", ");

                }
                else if (arrayList.Count == 1)
                {
                    OutputXmlNode(childName, arrayList[0], sb, true);
                }
                else
                {
                    sb.Append(" \"" + SafeJsonText(childName) + "\": [ ");

                    foreach (var child in arrayList)
                    {
                        OutputXmlNode(childName, child, sb, false);
                    }

                    sb.Remove(sb.Length - 2, 2);
                    sb.Append(" ], ");
                }
            }

            sb.Remove(sb.Length - 2, 2);
            sb.Append(" }");
        }

        private static String SafeJsonText(String text)
        {
            var sb = new StringBuilder(text.Length);

            foreach (char @char in text)
            {
                if (char.IsControl(@char) || @char == '\'')
                {
                    var charCode = (int)@char;
                    sb.Append(@"\u" + charCode.ToString("x4"));
                    continue;
                }

                if (@char == '\"' || @char == '\\' || @char == '/')
                {
                    sb.Append('\\');
                }

                sb.Append(@char);
            }

            return sb.ToString();
        }

        private static void SetChildNode(IDictionary childNodeNames, String nodeName, Object nodeValue)
        {
            if (nodeValue == null)
            {
                return;
            }

            // Pre-process contraction of XmlElement-s
            if (nodeValue is XmlElement)
            {
                // Convert  <aa></aa> into "aa":null
                //          <aa>xx</aa> into "aa":"xx"
                var childNode = (XmlNode)nodeValue;
                if (childNode.Attributes != null && childNode.Attributes.Count == 0)
                {
                    var children = childNode.ChildNodes;
                    if (children.Count == 0)
                    {
                        nodeValue = null;
                    }
                    else if (children.Count == 1 && (children[0] is XmlText))
                    {
                        nodeValue = children[0].InnerText;
                    }
                }
            }
            else if (nodeValue is XElement)
            {
                // Convert  <aa></aa> into "aa":null
                //          <aa>xx</aa> into "aa":"xx"
                var childNode = (XElement)nodeValue;
                if (!childNode.Attributes().Any())
                {
                    var children = childNode.Nodes().ToList();
                    if (children.Count == 0)
                    {
                        nodeValue = null;
                    }
                    else if (children.Count == 1 && (children[0] is XText))
                    {
                        nodeValue = ((XText)children[0]).Value;
                    }
                }
            }

            // Add nodeValue to ArrayList associated with each nodeName
            // If nodeName doesn't exist then add it
            var arrayList = childNodeNames[nodeName] as ArrayList;
            if (arrayList == null)
            {
                arrayList = new ArrayList();
                childNodeNames[nodeName] = arrayList;
            }

            if (nodeValue != null)
            {
                arrayList.Add(nodeValue);
            }
        }
        private static void OutputXmlNode(String childName, Object childValue, StringBuilder sb, bool showNodeName)
        {
            if (childValue == null)
            {
                if (showNodeName)
                {
                    sb.Append("\"" + SafeJsonText(childName) + "\": ");
                }

                sb.Append("null");
            }
            else if (childValue is String)
            {
                if (showNodeName)
                {
                    sb.Append("\"" + SafeJsonText(childName) + "\": ");
                }

                var strChildValue = (String)childValue;
                strChildValue = strChildValue.Trim();

                sb.Append("\"" + SafeJsonText(strChildValue) + "\"");
            }
            else if (childValue is XElement)
            {
                XmlToJsonNode(sb, (XElement)childValue, showNodeName);
            }
            else
            {
                XmlToJsonNode(sb, (XmlElement)childValue, showNodeName);
            }

            sb.Append(", ");
        }

        public static bool DeepEqualsEx(this XElement xElement, XElement other)
        {
            var comparer = new XElementComparer();
            return comparer.Equals(xElement, other);
        }

        public static IEnumerable<XElement> FindAll(this XElement xElement, String localName)
        {
            var xElems = from xElem in xElement.Descendants()
                         let xNs = xElem.Name.Namespace
                         let ns = xNs.NamespaceName
                         let xn = XName.Get(localName, ns)
                         where xElem.Name == xn
                         select xElem;

            return xElems;
        }

        public static XElement Find(this XElement xElement, String localName)
        {
            var resXElem = (from xElem in xElement.Descendants()
                            let xNs = xElem.Name.Namespace
                            let ns = xNs.NamespaceName
                            let xn = XName.Get(localName, ns)
                            where xElem.Name == xn
                            select xElem).FirstOrDefault();

            return resXElem;
        }

        public static XmlDocument ToXmlDocument(this XElement xElement)
        {
            var xmlDocument = new XmlDocument();

            using (var xmlReader = xElement.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }

            return xmlDocument;
        }

        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();

            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }

            return xmlDocument;
        }

        public static XElement ToXElement(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XElement.Load(nodeReader);
            }
        }

        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        public static String ToXmlText(this XElement xElement)
        {
            using (var writer = new StringWriter())
            {
                xElement.Save(writer);

                return writer.ToString();
            }
        }

        public static String InnerText(this XElement xElement)
        {
            using (var writer = new StringWriter())
            {
                foreach (var xNode in xElement.Nodes())
                {
                    if (xNode.NodeType == XmlNodeType.Text)
                    {
                        writer.Write(xNode);
                    }
                }

                return writer.ToString();
            }
        }

        public static String InnerXml(this XElement xElement)
        {
            using (var reader = xElement.CreateReader())
            {
                reader.MoveToContent();
                return reader.ReadInnerXml();
            }
        }

        public static String FullPath(this XElement xElement)
        {
            return FullPath(xElement, "\\");
        }

        public static String FullPath(this XElement xElement, bool ignoreNs)
        {
            return FullPath(xElement, "\\", ignoreNs);
        }

        public static String FullPath(this XElement xElement, String separator)
        {
            return FullPath(xElement, separator, false);
        }

        public static String FullPath(this XElement xElement, String separator, bool ignoreNs)
        {
            var list = new LinkedList<String>();

            var currXElem = xElement;
            while (currXElem != null)
            {
                var name = Convert.ToString(currXElem.Name);
                if (ignoreNs)
                {
                    name = currXElem.Name.LocalName;
                }

                list.AddFirst(name);
                currXElem = currXElem.Parent;
            }

            return String.Join(separator, list);
        }

        public static IList<String> Validate(this XDocument xDocument, String schemaFilePath)
        {
            var schemas = new XmlSchemaSet();
            schemas.Add("", XmlReader.Create(schemaFilePath));

            var errors = new List<String>();

            xDocument.Validate(schemas, (o, e) => errors.Add(e.Message));

            return errors;
        }

        public static IList<String> Validate(this XElement xElement, String schemaFilePath)
        {
            return Validate(new XDocument(xElement), schemaFilePath);
        }

        public static IList<String> Validate(this XmlDocument xmlDocument, String schemaFilePath)
        {
            xmlDocument.Schemas.Add("", schemaFilePath);

            var errors = new List<String>();

            xmlDocument.Validate((o, e) => errors.Add(e.Message));

            return errors;
        }

        public static XDocument RemoveAllNamespaces(this XDocument xDocument)
        {
            var xElement = RemoveAllNamespaces(xDocument.Root);
            return new XDocument(xElement);
        }

        public static XElement RemoveAllNamespaces(this XElement xElement)
        {
            if (!xElement.HasElements)
            {
                var newXElement = new XElement(xElement.Name.LocalName);
                newXElement.Value = xElement.Value;

                foreach (var attribute in xElement.Attributes())
                {
                    newXElement.Add(attribute);
                }

                return newXElement;
            }

            var children = xElement.Elements();
            children = children.Select(RemoveAllNamespaces);

            var result = new XElement(xElement.Name.LocalName, children);
            return result;
        }

        public static XmlDocument RemoveAllNamespaces(this XmlDocument xmlDocument)
        {
            var xmlText = RemoveAllNamespaces(xmlDocument.OuterXml);

            var newXmlDocument = new XmlDocument();
            newXmlDocument.LoadXml(xmlText);

            return newXmlDocument;
        }

        private static String RemoveAllNamespaces(String xmlText)
        {
            xmlText = xmlNsRegex.Replace(xmlText, String.Empty);
            return xmlText;
        }

        public static byte[] ToByteArray(this XElement xElement)
        {
            return xElement.ToByteArray(Encoding.UTF8);
        }

        public static byte[] ToByteArray(this XElement xElement, Encoding encoding)
        {
            if (xElement == null)
            {
                return null;
            }

            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream, encoding);

                xElement.Save(writer);

                writer.Flush();

                return stream.ToArray();
            }
        }

        public static XmlNode GetSubnode(this XmlNode xmlNode, string name, bool deep = true)
        {
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (childNode.Name == name)
                    return childNode;

                if (deep)
                {
                    var node = childNode.GetSubnode(name, deep);
                    if (node != null && node.Name == name)
                        return node;
                }
            }
            return null;
        }
    }
}


