using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CITI.EVO.Tools.Utils
{
	public static class StringCipherUtil
	{
		private static readonly byte[] _u8Salt;
		private static readonly Encoding _encoding;

		static StringCipherUtil()
		{
			_u8Salt = new byte[] { 0x26, 0x19, 0x81, 0x4E, 0xA0, 0x6D, 0x95, 0x34, 0x26, 0x75, 0x64, 0x05, 0xF6 };
			_encoding = Encoding.UTF8;
		}

		public static String Encrypt(String plainText, String password)
		{
			using (var pdb = new Rfc2898DeriveBytes(password, _u8Salt))
			{
				using (var rijndael = new RijndaelManaged { Padding = PaddingMode.PKCS7, Key = pdb.GetBytes(32), IV = pdb.GetBytes(16) })
				{
					using (var memoryStream = new MemoryStream())
					{
						using (var cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write))
						{
							var data = _encoding.GetBytes(plainText);

							cryptoStream.Write(data, 0, data.Length);
							cryptoStream.FlushFinalBlock();

							var base64 = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
							return base64;
						}
					}
				}
			}
		}

		public static String Decrypt(String cipherText, String password)
		{
			using (var pdb = new Rfc2898DeriveBytes(password, _u8Salt))
			{
				using (var rijndael = new RijndaelManaged { Padding = PaddingMode.PKCS7, Key = pdb.GetBytes(32), IV = pdb.GetBytes(16) })
				{
					using (var memoryStream = new MemoryStream())
					{
						using (var cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
						{
							var data = Convert.FromBase64String(cipherText);

							cryptoStream.Write(data, 0, data.Length);
							cryptoStream.FlushFinalBlock();

							var plainText = _encoding.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
							return plainText;
						}
					}
				}
			}
		}
	}
}