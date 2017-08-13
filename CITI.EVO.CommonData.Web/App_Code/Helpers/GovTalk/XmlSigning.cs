using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using SdaWSCryptoClient;

namespace CITI.EVO.CommonData.Web.Helpers.GovTalk
{
    public static class XmlSigning
    {
        public static void SignXml(this XmlElement unsignedXml, X509Certificate2 certificate, Guid signObjectUid)
        {
            var idAttribute = unsignedXml.Attributes["Id"];
            if (idAttribute == null)
                idAttribute = unsignedXml.OwnerDocument.CreateAttribute("Id");

            unsignedXml.Attributes.Append(idAttribute).Value = signObjectUid.ToString();

            var signature = XMLSigner.GenerateXMLSignature(unsignedXml, unsignedXml.Attributes["Id"].Value, certificate);
            unsignedXml.AppendChild(signature);
        }

        public static bool ValidateSignature(this XmlElement signedXml)
        {
            var sig = (XmlElement)signedXml.GetSubnode("Signature");
            if (sig == null || sig.ParentNode != signedXml)
                throw new Exception("Signature element not found");

            signedXml.RemoveChild(sig);

            var result = XMLSigner.VerifyXMLSignature(signedXml, sig);
            signedXml.AppendChild(sig);

            return result;
        }
    }
}