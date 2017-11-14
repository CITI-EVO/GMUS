using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using SdaWSCryptoClient;

namespace CITI.EVO.CommonData.Web.Helpers.GovTalk
{
    public static class XmlCrypting
    {
        /// <summary>
        /// Encrypt xml using encryption key and pack content with encryption key id, oid and iv
        /// </summary>
        /// <param name="element">Source xml to be encrypted</param>
        /// <param name="key">Encryption key</param>
        /// <param name="keyId">KeyId to be packed in encrypted content</param>
        public static void EncryptXml(this XmlElement element, byte[] key, long keyId)
        {
            using (AesCryptoServiceProvider aescp = new AesCryptoServiceProvider { Mode = CipherMode.CBC, KeySize = 256 })
            {
                aescp.GenerateIV();

                var iv = aescp.IV;
                var oid = new Oid("aes256");
                var elementArray = Encoding.UTF8.GetBytes(element.InnerXml);

                var encryptor = aescp.CreateEncryptor(key, iv);
                var encryptedData = encryptor.TransformFinalBlock(elementArray, 0, elementArray.Length);

                encryptedData = SdaEncryptedDataBuilder.BuildEncryptedData(oid.Value, encryptedData, iv, keyId);

                element.InnerXml = Convert.ToBase64String(encryptedData);
            }
        }

        /// <summary>
        /// Decrypt xml using encryption key and pack content with encryption key id, oid and iv
        /// </summary>
        /// <param name="element">Source xml to be decrypted</param>
        /// <param name="key">Encryption key</param>
        public static void DecryptXml(this XmlElement element, byte[] key)
        {
            using (var aescp = new AesCryptoServiceProvider { Mode = CipherMode.CBC, KeySize = 256 })
            {
                var sData = element.InnerXml;
                var eData = Convert.FromBase64String(sData);

                var sDaed = new SdaEncryptedData(eData);

                var iv = sDaed.IV;

                var decryptor = aescp.CreateDecryptor(key, iv);

                eData = sDaed.EncryptedContent;

                var data = decryptor.TransformFinalBlock(eData, 0, eData.Length);

                element.InnerXml = Encoding.UTF8.GetString(data);
            }
        }
    }
}