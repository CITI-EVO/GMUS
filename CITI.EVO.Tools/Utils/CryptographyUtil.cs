using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CITI.EVO.Tools.Security.Cryptography;

namespace CITI.EVO.Tools.Utils
{
	public static class CryptographyUtil
	{
		public static String ComputeMD5(String text)
		{
			return ComputeMD5(text, Encoding.UTF8);
		}
		public static String ComputeMD5(String text, Encoding encoding)
		{
			var textBytes = encoding.GetBytes(text);
			return ComputeMD5(textBytes);
		}
		public static String ComputeMD5(byte[] bytes)
		{
			using (var hashAlgorithmImpl = new MD5Cng())
			{
				var hashBytes = hashAlgorithmImpl.ComputeHash(bytes);
				return String.Concat(hashBytes.Select(b => b.ToString("x2")));
			}
		}

		public static String ComputeSHA1(String text)
		{
			return ComputeSHA1(text, Encoding.UTF8);
		}
		public static String ComputeSHA1(String text, Encoding encoding)
		{
			var textBytes = encoding.GetBytes(text);
			return ComputeSHA1(textBytes);
		}
		public static String ComputeSHA1(byte[] bytes)
		{
			using (var hashAlgorithmImpl = new SHA1Cng())
			{
				var hashBytes = hashAlgorithmImpl.ComputeHash(bytes);
				return String.Concat(hashBytes.Select(b => b.ToString("x2")));
			}
		}

		public static String ComputeSHA256(String text)
		{
			return ComputeSHA256(text, Encoding.UTF8);
		}
		public static String ComputeSHA256(String text, Encoding encoding)
		{
			var textBytes = encoding.GetBytes(text);
			return ComputeSHA256(textBytes);
		}
		public static String ComputeSHA256(byte[] bytes)
		{
			using (var hashAlgorithmImpl = new SHA256Cng())
			{
				var hashBytes = hashAlgorithmImpl.ComputeHash(bytes);
				return String.Concat(hashBytes.Select(b => b.ToString("x2")));
			}
		}

		public static String ComputeSHA384(String text)
		{
			return ComputeSHA384(text, Encoding.UTF8);
		}
		public static String ComputeSHA384(String text, Encoding encoding)
		{
			var textBytes = encoding.GetBytes(text);
			return ComputeSHA384(textBytes);
		}
		public static String ComputeSHA384(byte[] bytes)
		{
			using (var hashAlgorithmImpl = new SHA384Cng())
			{
				var hashBytes = hashAlgorithmImpl.ComputeHash(bytes);
				return String.Concat(hashBytes.Select(b => b.ToString("x2")));
			}
		}

		public static String ComputeSHA512(String text)
		{
			return ComputeSHA512(text, Encoding.UTF8);
		}
		public static String ComputeSHA512(String text, Encoding encoding)
		{
			var textBytes = encoding.GetBytes(text);
			return ComputeSHA512(textBytes);
		}
		public static String ComputeSHA512(byte[] bytes)
		{
			using (var hashAlgorithmImpl = new SHA512Cng())
			{
				var hashBytes = hashAlgorithmImpl.ComputeHash(bytes);
				return String.Concat(hashBytes.Select(b => b.ToString("x2")));
			}
		}

		public static String ComputeCRC8(String text)
		{
			return ComputeCRC8(text, Encoding.UTF8);
		}
		public static String ComputeCRC8(String text, Encoding encoding)
		{
			var textBytes = encoding.GetBytes(text);
			return ComputeCRC8(textBytes);
		}
		public static String ComputeCRC8(byte[] bytes)
		{
			using (var hashAlgorithmImpl = new CRC8())
			{
				var hashBytes = hashAlgorithmImpl.ComputeHash(bytes);
				return String.Concat(hashBytes.Select(b => b.ToString("x2")));
			}
		}

		public static String ComputeCRC16(String text)
		{
			return ComputeCRC16(text, Encoding.UTF8);
		}
		public static String ComputeCRC16(String text, Encoding encoding)
		{
			var textBytes = encoding.GetBytes(text);
			return ComputeCRC16(textBytes);
		}
		public static String ComputeCRC16(byte[] bytes)
		{
			using (var hashAlgorithmImpl = new CRC16())
			{
				var hashBytes = hashAlgorithmImpl.ComputeHash(bytes);
				return String.Concat(hashBytes.Select(b => b.ToString("x2")));
			}
		}

		public static String ComputeCRC32(String text)
		{
			return ComputeCRC32(text, Encoding.UTF8);
		}
		public static String ComputeCRC32(String text, Encoding encoding)
		{
			var textBytes = encoding.GetBytes(text);
			return ComputeCRC32(textBytes);
		}
		public static String ComputeCRC32(byte[] bytes)
		{
			using (var hashAlgorithmImpl = new CRC32())
			{
				var hashBytes = hashAlgorithmImpl.ComputeHash(bytes);
				return String.Concat(hashBytes.Select(b => b.ToString("x2")));
			}
		}

		public static String ComputeCRC64(String text)
		{
			return ComputeCRC64(text, Encoding.UTF8);
		}
		public static String ComputeCRC64(String text, Encoding encoding)
		{
			var textBytes = encoding.GetBytes(text);
			return ComputeCRC64(textBytes);
		}
		public static String ComputeCRC64(byte[] bytes)
		{
			using (var hashAlgorithmImpl = new CRC64())
			{
				var hashBytes = hashAlgorithmImpl.ComputeHash(bytes);
				return String.Concat(hashBytes.Select(b => b.ToString("x2")));
			}
		}

		public static String ComputeELF32(String text)
		{
			return ComputeELF32(text, Encoding.UTF8);
		}
		public static String ComputeELF32(String text, Encoding encoding)
		{
			var textBytes = encoding.GetBytes(text);
			return ComputeELF32(textBytes);
		}
		public static String ComputeELF32(byte[] bytes)
		{
			using (var hashAlgorithmImpl = new ELF32())
			{
				var hashBytes = hashAlgorithmImpl.ComputeHash(bytes);
				return String.Concat(hashBytes.Select(b => b.ToString("x2")));
			}
		}

		public static Guid ComputeGuidMD5(params Object[] texts)
		{
			return ComputeGuidMD5(Encoding.UTF8, texts);
		}
		public static Guid ComputeGuidMD5(Encoding encoding, params Object[] texts)
		{
			var text = String.Concat(texts);
			var textBytes = encoding.GetBytes(text);
			return ComputeGuidMD5(textBytes);
		}
		public static Guid ComputeGuidMD5(byte[] bytes)
		{
			using (var hashAlgorithmImpl = new MD5Cng())
			{
				var hashBytes = hashAlgorithmImpl.ComputeHash(bytes);
				return new Guid(hashBytes);
			}
		}

		public static Guid ComputeGuidSHA1(params Object[] texts)
		{
			return ComputeGuidSHA1(Encoding.UTF8, texts);
		}
		public static Guid ComputeGuidSHA1(Encoding encoding, params Object[] texts)
		{
			var text = String.Concat(texts);
			var textBytes = encoding.GetBytes(text);
			return ComputeGuidSHA1(textBytes);
		}
		public static Guid ComputeGuidSHA1(byte[] bytes)
		{
			using (var hashAlgorithmImpl = new SHA1Cng())
			{
				var hashBytes = hashAlgorithmImpl.ComputeHash(bytes);
				return new Guid(hashBytes);
			}
		}
	}
}
