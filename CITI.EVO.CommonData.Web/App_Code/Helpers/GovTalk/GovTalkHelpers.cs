using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Web.Helpers.GovTalk
{
    public static class GovTalkHelpers
    {
        /// <summary>
        /// Finds node in xml recursively by name
        /// </summary>
        /// <param name="xmlNode">Source xml node</param>
        /// <param name="name">Search node name</param>
        /// <param name="deep">If true performs recursive search</param>
        /// <returns></returns>
        public static XmlNode GetSubnode(this XmlNode xmlNode, String name, bool deep = true)
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

        public static IEnumerable<XmlNode> NodeTraversal(this XmlNode xmlNode, String name, bool deep = true)
        {
            var stack = new Stack<XmlNode>();
            stack.Push(xmlNode);

            while (stack.Count > 0)
            {
                var node = stack.Pop();
                yield return node;

                foreach (XmlNode child in xmlNode.ChildNodes)
                    stack.Push(child);
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

            var qualifierNode = xdoc.GetSubnode("Qualifier", true);
            return (qualifierNode != null && qualifierNode.InnerText.ToLower() == "poll");
        }

        /// <summary>
        /// Put request parameter object xml in api acceptible xml format using template
        /// </summary>
        /// <param name="subcontractId">Value of SubcontractId of which request will be composed</param>
        /// <param name="paramObjectsDocument">XmlDocument which contains parameter objects' nodes</param>
        /// <returns></returns>
        public static XmlElement ComposeRequestXmlWithParamObjects(String subcontractId, XmlDocument paramObjectsDocument)
        {
            var path = HttpContext.Current.Server.MapPath("~/Templates/GovTalk/SRNSFPersonInfo.xml");

            var document = new XmlDocument();
            document.Load(path);

            var requestNode = (XmlElement)document.GetSubnode("Request");

            var subcontractNode = (XmlElement)requestNode.GetSubnode("SubcontractId");
            subcontractNode.InnerText = subcontractId;

            var paramsNode = (XmlElement)requestNode.GetSubnode("Parameters");
            foreach (XmlNode paramObject in paramObjectsDocument.ChildNodes)
            {
                var pObject = document.ImportNode(paramObject, true);
                paramsNode.AppendChild(pObject);
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

            var xstore = new X509Store(name, store);
            xstore.Open(OpenFlags.ReadOnly);

            var query = (from n in xstore.Certificates.Cast<X509Certificate2>()
                         where comparer.Equals(key, n.SubjectName.Name) ||
                               comparer.Equals(key, n.SerialNumber) ||
                               comparer.Equals(key, n.FriendlyName) ||
                               comparer.Equals(key, n.Thumbprint)
                         select n);

            var certificate = query.FirstOrDefault();
            xstore.Close();

            return certificate;
        }

        /// <summary>
        /// Deserializes response's 'ResultStatus' node into ResultStatus object
        /// </summary>
        /// <param name="response">Response xml string returned by service</param>
        /// <param name="responseDoc"></param>
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