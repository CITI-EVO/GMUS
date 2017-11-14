using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace CITI.EVO.CommonData.Web.Helpers.GovTalk
{
    public class EncryptKey
    {
        private long _keyId;
        private byte[] _encKey;
        private DateTime? _encKeyDate;

        private X509Certificate2 _certificate;

        public EncryptKey(X509Certificate2 certificate)
        {
            _certificate = certificate;
        }

        public long KeyId
        {
            get { return _keyId; }
        }

        public byte[] EncKey
        {
            get { return _encKey; }
        }

        public DateTime? EncKeyDate
        {
            get { return _encKeyDate; }
        }

        public void UpdateIfNeeded()
        {
            var diff = DateTime.Now - _encKeyDate.GetValueOrDefault();
            if (diff.TotalHours >= 23.9)
            {
                var defaultTempPath = ConfigurationManager.AppSettings["GovTalk_DefaultTemplate"];
                var keyExchSubContractId = ConfigurationManager.AppSettings["GovTalkKeyExchSubcontractId"];

                _encKey = GetSymetricEncryptionKey(defaultTempPath, keyExchSubContractId, _certificate, out _keyId);
                _encKeyDate = DateTime.Now;
            }
        }

        private byte[] GetSymetricEncryptionKey(String fileName, String subContractId, X509Certificate2 signingCertificate, out long keyId)
        {
            var keyExchange = new KeyExchange();
            var symetricKey = keyExchange.GetCurrentEncryptionKey();

            if (symetricKey == null)
            {
                keyExchange.GenerateKeyPair();
                keyExchange.PerformExchange(fileName, subContractId, signingCertificate);

                symetricKey = keyExchange.CalculateSecureSymetricKey(); //should be stored somewhere to use for encryption and decryption until next key exchange (persumably once per day)
            }

            keyId = keyExchange.KeyId;

            return symetricKey;
        }
    }
}