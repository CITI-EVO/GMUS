using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace CITI.EVO.CommonData.Web.Helpers.GovTalk
{
    public static class GovTalkCallApi
    {
        private static EncryptKey _encryptKey;
        private static X509Certificate2 _signingCertificate;

        public static XmlElement LoadXml(String method, bool encrypt, IDictionary<String, Object> @params)
        {
            return LoadXml(method, encrypt, @params, null);
        }
        public static XmlElement LoadXml(String method, bool encrypt, IDictionary<String, Object> @params, String resultContract)
        {
            var methodKey = $"GovTalk_{method}";

            var methodFilePath = ConfigurationManager.AppSettings[methodKey];

            var xmlText = GovTalkHelpers.GetXmlFile(methodFilePath);

            foreach (var pair in @params)
            {
                var key = String.Concat("$!{", pair.Key, "}");
                var value = Convert.ToString(pair.Value);

                if (pair.Value is DateTime)
                    value = XmlConvert.ToString((DateTime)pair.Value, XmlDateTimeSerializationMode.Local);

                xmlText = xmlText.Replace(key, value);
            }

            var requestDoc = new XmlDocument();
            requestDoc.LoadXml(xmlText);

            var signId = GovTalkHelpers.GenerateSignId();
            var certificate = GetCertificate();

            var requestElement = (XmlElement)requestDoc.GetSubnode("Request");
            requestElement.SignXml(certificate, signId);

            var paramsElement = (XmlElement)requestElement.GetSubnode("Parameters");

            var contractName = resultContract;
            if (String.IsNullOrWhiteSpace(contractName))
                contractName = paramsElement.FirstChild.Name;

            if (encrypt)
            {
                var encKey = GetEncryptKey();
                paramsElement.EncryptXml(encKey.EncKey, encKey.KeyId);
            }

            var response = GetResponse(requestDoc.OuterXml);

            //Initialize response xml document by response string returned by service api
            var responseDoc = new XmlDocument();
            responseDoc.LoadXml(response);

            //Deserialize 'ResultStatus' node of response xml into corresponding object
            var status = GovTalkHelpers.GetResponseStatus(responseDoc);
            if (status.Code != "14") //14 is OK
            {
                if (status.Code == "31")//TODO: 31 not found records, temporary
                    return null;

                throw new Exception($"{status.Code} - {status.Message}");
            }

            var resultNode = (XmlElement)responseDoc.GetSubnode(contractName);
            if (resultNode != null && encrypt)
            {
                var encKey = GetEncryptKey();
                resultNode.DecryptXml(encKey.EncKey);
            }

            var responseNode = (XmlElement)responseDoc.GetSubnode("Response");
            if (!responseNode.ValidateSignature())
                throw new Exception("Signature is not valid");

            if (resultNode != null)
                return resultNode;

            return (XmlElement)responseDoc.FirstChild;
        }

        /// <summary>
        /// Calls service api using request xml string
        /// </summary>
        /// <param name="requestXml">request xml string</param>
        /// <returns></returns>
        public static String GetResponse(String requestXml)
        {
            var apiUrl = ConfigurationManager.AppSettings["GovTalkApiUrl"];

            if (requestXml.IsGovTalkMessage())
            {
                if (requestXml.IsPollMessage())
                    apiUrl += "poll";
                else
                    apiUrl += "submission";
            }

            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/xml";
                client.Encoding = Encoding.UTF8;

                return client.UploadString(apiUrl, requestXml);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static EncryptKey GetEncryptKey()
        {
            if (_encryptKey == null)
            {
                var certificate = GetCertificate();
                _encryptKey = new EncryptKey(certificate);
            }

            _encryptKey.UpdateIfNeeded();

            return _encryptKey;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static X509Certificate2 GetCertificate()
        {
            if (_signingCertificate == null)
            {
                var certificateKey = ConfigurationManager.AppSettings["GovTalkCertKey"];

                _signingCertificate = GovTalkHelpers.GetSigningCertificate(certificateKey);
                if (_signingCertificate == null)
                    throw new Exception("Could not find signing certificate");
            }

            return _signingCertificate;
        }
    }
}