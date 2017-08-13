using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using CITI.EVO.CommonData.Svc.Contracts;
using CITI.EVO.Tools.Utils;

namespace CITI.EVO.CommonData.Web.Helpers.GovTalk
{
    public static class PersonApi
    {
        public static PersonInfoContract GetPerson(String privateNumber, String birthYear)
        {
            var certificateKey = ConfigurationManager.AppSettings["GovTalkCertKey"];
            var mainSubcontractId = ConfigurationManager.AppSettings["GovTalkMainSubcontractId"];
            var keyExchSubcontractId = ConfigurationManager.AppSettings["GovTalkKeyExchSubcontractId"];

            var path = HttpContext.Current.Server.MapPath("~/Templates/GovTalk/SRNSFPersonInfo.xml");

            var requestDoc = new XmlDocument();
            requestDoc.Load(path);

            var signingCertificate = GovTalkHelpers.GetSigningCertificate(certificateKey);
            if (signingCertificate == null)
                throw new Exception("Could not find signing certificate");

            var requestElement = (XmlElement)requestDoc.GetSubnode("Request");
            var parametersElement = (XmlElement)requestElement.GetSubnode("Parameters");

            var subcontractElement = (XmlElement)requestElement.GetSubnode("SubcontractId");
            subcontractElement.InnerText = mainSubcontractId;

            var personElement = requestDoc.CreateElement("Person");

            var privateNumberElement = requestDoc.CreateElement("PrivateNumber");
            privateNumberElement.InnerText = privateNumber;

            var birthYearElement = requestDoc.CreateElement("BirthYear");
            birthYearElement.InnerText = birthYear;

            personElement.AppendChild(privateNumberElement);
            personElement.AppendChild(birthYearElement);

            parametersElement.AppendChild(personElement);

            requestElement.SignXml(signingCertificate, Guid.NewGuid());

            long keyId;
            var key = GetSymetricEncryptionKey(keyExchSubcontractId, signingCertificate, out keyId);

            requestElement.EncryptXml(key, keyId);

            var response = GovTalkCallApi.GetResponse(requestDoc.OuterXml);

            //Initialize response xml document by response string returned by service api
            var responseDoc = new XmlDocument();
            responseDoc.LoadXml(response);

            //Deserialize 'ResultStatus' node of response xml into corresponding object
            var status = GovTalkHelpers.GetResponseStatus(responseDoc);
            if (status.Code != "14") //14 is OK
                throw new Exception($"{status.Code} - {status.Message}");

            var encryptedNode = (XmlElement)responseDoc.GetSubnode("Person");
            if (encryptedNode != null)
                encryptedNode.DecryptXml(key);

            if (!((XmlElement)responseDoc.GetSubnode("Response")).ValidateSignature())
                throw new Exception("Signature is not valid");

            var responseXElem = XElement.Parse(response);

            var personXElem = responseXElem.Element("Person");
            if (personXElem == null)
                return null;

            var contract = new PersonInfoContract
            {
                ID = (String) personXElem.Element("ID"),
                PersonalID = (String) personXElem.Element("PrivateNumber"),
                LastName = (String) personXElem.Element("LastName"),
                FirstName = (String) personXElem.Element("FirstName"),
                BirthDate = DataConverter.ToNullableDateTime((String) personXElem.Element("BirthDate")),
                PersonStatus = (String) personXElem.Element("PersonStatus"),
                PersonStatusId = (String) personXElem.Element("PersonStatusId"),
                CitizenshipCountry = (String) personXElem.Element("CitizenshipCountry"),
                CitizenshipCountryID = (String) personXElem.Element("CitizenshipCountryID")
            };

            return contract;
        }

        /// <summary>
        /// Gets current agreed symetric key if present, otherwise generates new keypair with which performs agreement and calculates new symetric key
        /// </summary>
        /// <param name="subcontractId"></param>
        /// <param name="signingCertificate">Certificate for signing on 'ECKeyValue' node in key exchange request xml</param>
        /// <param name="keyId">Service provider-side Id of symetric key</param>
        /// <returns></returns>
        private static byte[] GetSymetricEncryptionKey(String subcontractId, X509Certificate2 signingCertificate, out long keyId)
        {
            var keyExchange = new KeyExchange();
            var symetricKey = keyExchange.GetCurrentEncryptionKey();

            if (symetricKey == null)
            {
                keyExchange.GenerateKeyPair();
                keyExchange.PerformExchange(subcontractId, signingCertificate);

                symetricKey = keyExchange.CalculateSecureSymetricKey(); //should be stored somewhere to use for encryption and decryption until next key exchange (persumably once per day)
            }

            keyId = keyExchange.KeyId;

            return symetricKey;
        }
    }
}