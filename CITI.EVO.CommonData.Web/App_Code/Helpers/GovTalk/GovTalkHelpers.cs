using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using CITI.EVO.CommonData.Svc.Contracts.GovTalk.Response;

namespace CITI.EVO.CommonData.Web.Helpers.GovTalk
{
    public static class GovTalkHelpers
    {
        public static Guid GenerateSignId()
        {
            return Guid.NewGuid();
        }

        public static String GetXmlFile(String fileName)
        {
            var filePath = HttpContext.Current.Server.MapPath(fileName);

            var senderID = ConfigurationManager.AppSettings["GovTalkSenderID"];
            var authVal = ConfigurationManager.AppSettings["GovTalkAuth"];
            var subcontractId = ConfigurationManager.AppSettings["GovTalkMainSubcontractId"];

            var xmlText = File.ReadAllText(filePath);

            xmlText = xmlText.Replace("$!{SenderID}", senderID);
            xmlText = xmlText.Replace("$!{AuthVal}", authVal);
            xmlText = xmlText.Replace("$!{SubcontractId}", subcontractId);

            return xmlText;
        }

        /// <summary>
        /// Finds node in xml recursively by name
        /// </summary>
        /// <param name="xmlNode">Source xml node</param>
        /// <param name="name">Search node name</param>
        /// <returns></returns>
        public static XmlNode GetSubnode(this XmlNode xmlNode, String name)
        {
            return GetSubnode(xmlNode, name, true);
        }

        /// <summary>
        /// Finds node in xml recursively by name
        /// </summary>
        /// <param name="xmlNode">Source xml node</param>
        /// <param name="name">Search node name</param>
        /// <param name="deep">If true performs recursive search</param>
        /// <returns></returns>
        public static XmlNode GetSubnode(this XmlNode xmlNode, String name, bool deep)
        {
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (childNode.Name == name)
                    return childNode;

                if (deep)
                {
                    var node = GetSubnode(childNode, name, deep);
                    if (node != null && node.Name == name)
                        return node;
                }
            }

            return null;
        }

        public static IEnumerable<XmlNode> NodeTraversal(this XmlNode xmlNode)
        {
            return NodeTraversal(xmlNode, true);
        }
        public static IEnumerable<XmlNode> NodeTraversal(this XmlNode xmlNode, bool deep)
        {
            var stack = new Stack<XmlNode>();
            stack.Push(xmlNode);

            foreach (XmlNode xmlChild in xmlNode.ChildNodes)
                stack.Push(xmlChild);

            while (stack.Count > 0)
            {
                var node = stack.Pop();
                yield return node;

                if (ReferenceEquals(node, xmlNode))
                    continue;

                if (deep)
                {
                    foreach (XmlNode xmlChild in xmlNode.ChildNodes)
                        stack.Push(xmlChild);
                }
            }
        }

        /// <summary>
        /// Returns whether xml is in or not in GovTalk format
        /// </summary>
        /// <param name="xmlMessage">Source xml string</param>
        /// <returns></returns>
        public static bool IsGovTalkMessage(this String xmlMessage)
        {
            return xmlMessage.StartsWith(@"<GovTalkMessage xmlns=""http://www.govtalk.gov.uk/CM/envelope"">");
        }

        /// <summary>
        /// When async transaction has been called through DEA, call result should be made by speial GovTalk message with Qualifier="poll". This type of request is being posted on different url
        /// </summary>
        /// <param name="xmlMessage">Source xml string</param>
        /// <returns></returns>
        public static bool IsPollMessage(this String xmlMessage)
        {
            var xdoc = new XmlDocument();
            xdoc.LoadXml(xmlMessage);

            var qualifierNode = GetSubnode((XmlNode) xdoc, (string) "Qualifier", (bool) true);
            return (qualifierNode != null && qualifierNode.InnerText.ToLower() == "poll");
        }

        /// <summary>
        /// Put request parameter object xml in api acceptible xml format using template
        /// </summary>
        /// <param name="xmlText"></param>
        /// <param name="subContractId">Value of SubcontractId of which request will be composed</param>
        /// <param name="paramObjectsDoc">XmlDocument which contains parameter objects' nodes</param>
        /// <returns></returns>
        public static XmlElement ComposeRequestXmlWithParamObjects(String xmlText, String subContractId, XmlDocument paramObjectsDoc)
        {
            var document = new XmlDocument();
            document.LoadXml(xmlText);

            var requestNode = (XmlElement)document.GetSubnode("Request");

            var subContractNode = (XmlElement)requestNode.GetSubnode("SubcontractId");
            subContractNode.InnerText = subContractId;

            var parametersNode = (XmlElement)requestNode.GetSubnode("Parameters");
            foreach (XmlNode paramObject in paramObjectsDoc.ChildNodes)
            {
                var pObject = document.ImportNode(paramObject, true);
                parametersNode.AppendChild(pObject);
            }

            return (XmlElement)document.FirstChild;
        }

        /// <summary>
        /// Fetches installed certificate from certificate store (Trusted People) by Thumbnail
        /// </summary>
        /// <returns></returns>
        public static X509Certificate2 GetSigningCertificate(String key)
        {
            var certificate = GetSigningCertificate(key, StoreLocation.CurrentUser);
            if (certificate == null)
                certificate = GetSigningCertificate(key, StoreLocation.LocalMachine);

            return certificate;
        }

        /// <summary>
        /// Fetches installed certificate from certificate store (Trusted People) by Thumbnail
        /// </summary>
        /// <returns></returns>
        public static X509Certificate2 GetSigningCertificate(String key, StoreLocation store)
        {
            var cert = GetSigningCertificate(key, StoreName.My, store);
            if (cert != null)
                return cert;

            cert = GetSigningCertificate(key, StoreName.Root, store);
            if (cert != null)
                return cert;

            cert = GetSigningCertificate(key, StoreName.TrustedPeople, store);
            if (cert != null)
                return cert;

            cert = GetSigningCertificate(key, StoreName.TrustedPublisher, store);
            if (cert != null)
                return cert;

            return null;
        }

        public static X509Certificate2 GetSigningCertificate(String key, StoreName name, StoreLocation store)
        {
            key = CleanThumbprint(key);

            var comparer = StringComparer.OrdinalIgnoreCase;

            var fileQuery = (from n in GetX509Certificate2Collection(name, store)
                             where comparer.Equals(key, n.SubjectName.Name) ||
                                   comparer.Equals(key, n.SerialNumber) ||
                                   comparer.Equals(key, n.FriendlyName) ||
                                   comparer.Equals(key, n.Thumbprint)
                             select n);

            var certificate = fileQuery.FirstOrDefault();
            return certificate;
        }

        private static IEnumerable<X509Certificate2> GetX509Certificate2Collection(StoreName name, StoreLocation store)
        {
            var certFile = ConfigurationManager.AppSettings["GovTalkCertFile"];
            var certPath = HttpContext.Current.Server.MapPath(certFile);

            var certPassword = ConfigurationManager.AppSettings["GovTalkCertPassword"];

            if (File.Exists(certPath))
            {
                var collection = new X509Certificate2Collection();
                collection.Import(certPath, certPassword, X509KeyStorageFlags.MachineKeySet);

                foreach (X509Certificate2 item in collection)
                    yield return item;
            }

            var xstore = new X509Store(name, store);
            xstore.Open(OpenFlags.ReadOnly);

            foreach (X509Certificate2 item in xstore.Certificates)
                yield return item;

            xstore.Close();
        }

        /// <summary>
        /// Deserializes response's 'ResultStatus' node into ResultStatus object
        /// </summary>
        /// <param name="responseDoc">Response xml string returned by service</param>
        /// <returns></returns>
        public static ResultStatus GetResponseStatus(XmlDocument responseDoc)
        {
            var statusNode = responseDoc.GetSubnode("ResultStatus");
            var serializer = new XmlSerializer(typeof(ResultStatus), statusNode.NamespaceURI);

            var reader = XmlReader.Create(new StringReader(statusNode.OuterXml));
            var status = (ResultStatus)serializer.Deserialize(reader);

            return status;
        }

        public static String CleanThumbprint(String thumbprint)
        {
            //replace spaces, non word chars and convert to uppercase
            return Regex.Replace(thumbprint, @"\s|\W", "").ToUpper();
        }
    }
}