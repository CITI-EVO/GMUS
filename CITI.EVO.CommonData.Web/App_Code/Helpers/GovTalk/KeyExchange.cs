using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Xml;
using SdaWSCryptoClient.Keys;

namespace CITI.EVO.CommonData.Web.Helpers.GovTalk
{
    public class KeyExchange
    {
        private ECKey ecKey;

        public long KeyId { get; set; }

        public ECPublicKey GeneratedPublicKey { get; private set; }

        public ECPublicKey ReceivedPublicKey { get; private set; }

        /// <summary>
        /// Generates new private-public keypair. Public key will be sent to provider during key exchange. Private key will be used to calculate symetric encryption key
        /// </summary>
        public void GenerateKeyPair()
        {
            ecKey = new ECKey();

            GeneratedPublicKey = new ECPublicKey
            {
                X = ecKey.X,
                Y = ecKey.Y,
                CurveOId = ecKey.CurveOId
            };
        }

        /// <summary>
        /// Calculates symetric encryption key with generate keypair's private key and public key received by key exchange. Key should be stored in order to use it for encryption and decryption until next key exchange (persumably once per day)
        /// </summary>
        /// <returns></returns>
        public byte[] CalculateSecureSymetricKey()
        {
            return ecKey.GenerateKeyAgreement(ReceivedPublicKey.X, ReceivedPublicKey.Y);
        }

        /// <summary>
        /// Builds key exchange request xml with generated keypair's public key and calls key exchange subcontract to receive providers public key
        /// </summary>
        /// <param name="subcontractId">Key exchange subcontractId given by service provider</param>
        /// <param name="signingCert">Signing certificate</param>
        public void PerformExchange(String subcontractId, X509Certificate2 signingCert)
        {
            if (GeneratedPublicKey == null)
                GenerateKeyPair();

            var publicKeyXml = GeneratedPublicKey.ToXml();

            var firstChild = (XmlElement)publicKeyXml.FirstChild;
            firstChild.SignXml(signingCert, Guid.NewGuid());

            var requestNode = GovTalkHelpers.ComposeRequestXmlWithParamObjects(subcontractId, publicKeyXml);

            var keyExchangeResponse = GovTalkCallApi.GetResponse(requestNode.OuterXml);

            var keyExchangeDoc = new XmlDocument();
            keyExchangeDoc.LoadXml(keyExchangeResponse);

            var fileName = HttpContext.Current.Server.MapPath($"~/XmlLogs/keyExch_{DateTime.Now:yyyy.MM.dd_hh.mm.ss}.xml");
            using (var file = File.CreateText(fileName))
                keyExchangeDoc.Save(file);

            var resultStatus = GovTalkHelpers.GetResponseStatus(keyExchangeDoc);
            if (resultStatus.Code != "14")
            {
                var message = $"KeyExchange error [{resultStatus.Code}] {resultStatus.Message}";
                throw new Exception(message);
            }

            ReceivedPublicKey = new ECPublicKey();
            ReceivedPublicKey.LoadXml(keyExchangeResponse);

            KeyId = ReceivedPublicKey.Id;
        }

        /// <summary>
        /// Get stored encryption key calculated after last key exchange. Key will be used for request part encryption and response decryption until next key exchange (persumably once per day)
        /// </summary>
        /// <returns></returns>
        public byte[] GetCurrentEncryptionKey()
        {
            KeyId = 0;
            return null;
        }
    }
}