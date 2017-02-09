using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security.Cryptography;

namespace CITI.EVO.Tools.Extensions
{
    public static class CryptoExtensions
    {
        public enum HashFormat
        {
            Binary,
            Base64,
            Bytes,
            Hex,
            Dec,
        }

        public static String ComputeCrc16(this String text)
        {
            return ComputeCrc16(text, HashFormat.Hex);
        }
        public static String ComputeCrc16(this String text, HashFormat hashFormat)
        {
            return ComputeCrc16(text, Encoding.UTF8, hashFormat);
        }
        public static String ComputeCrc16(this String text, Encoding encoding, HashFormat hashFormat)
        {
            var bytes = encoding.GetBytes(text);
            return bytes.ComputeCrc16(hashFormat);
        }

        public static String ComputeCrc16(this byte[] bytes)
        {
            return ComputeCrc16(bytes, HashFormat.Hex);
        }
        public static String ComputeCrc16(this byte[] bytes, HashFormat hashFormat)
        {
            using (var hashImpl = new CRC16())
            {
                var hashBytes = hashImpl.ComputeHash(bytes);
                return ConvertToString(hashBytes, hashFormat);
            }
        }


        public static String ComputeCrc16Ccitt(this String text)
        {
            return ComputeCrc16Ccitt(text, HashFormat.Hex);
        }
        public static String ComputeCrc16Ccitt(this String text, HashFormat hashFormat)
        {
            return ComputeCrc16Ccitt(text, Encoding.UTF8, hashFormat);
        }
        public static String ComputeCrc16Ccitt(this String text, Encoding encoding, HashFormat hashFormat)
        {
            var bytes = encoding.GetBytes(text);
            return bytes.ComputeCrc16Ccitt(CRC16CCITT.Seeds.Zeros, hashFormat);
        }

        public static String ComputeCrc16Ccitt(this byte[] bytes)
        {
            return ComputeCrc16Ccitt(bytes, CRC16CCITT.Seeds.Zeros, HashFormat.Hex);
        }
        public static String ComputeCrc16Ccitt(this byte[] bytes, CRC16CCITT.Seeds seed)
        {
            return ComputeCrc16Ccitt(bytes, seed, HashFormat.Hex);
        }
        public static String ComputeCrc16Ccitt(this byte[] bytes, CRC16CCITT.Seeds seed, HashFormat hashFormat)
        {
            using (var hashImpl = new CRC16CCITT(seed))
            {
                var hashBytes = hashImpl.ComputeHash(bytes);
                return ConvertToString(hashBytes, hashFormat);
            }
        }

        public static String ComputeCrc32(this String text)
        {
            return ComputeCrc32(text, HashFormat.Hex);
        }
        public static String ComputeCrc32(this String text, HashFormat hashFormat)
        {
            return ComputeCrc32(text, Encoding.UTF8, hashFormat);
        }
        public static String ComputeCrc32(this String text, Encoding encoding, HashFormat hashFormat)
        {
            var bytes = encoding.GetBytes(text);
            return bytes.ComputeCrc32(hashFormat);
        }

        public static String ComputeCrc32(this byte[] bytes)
        {
            return ComputeCrc32(bytes, HashFormat.Hex);
        }
        public static String ComputeCrc32(this byte[] bytes, HashFormat hashFormat)
        {
            using (var hashImpl = new CRC32())
            {
                var hashBytes = hashImpl.ComputeHash(bytes);
                return ConvertToString(hashBytes, hashFormat);
            }
        }

        public static String ComputeCrc64(this String text)
        {
            return ComputeCrc64(text, HashFormat.Hex);
        }
        public static String ComputeCrc64(this String text, HashFormat hashFormat)
        {
            return ComputeCrc64(text, Encoding.UTF8, hashFormat);
        }
        public static String ComputeCrc64(this String text, Encoding encoding, HashFormat hashFormat)
        {
            var bytes = encoding.GetBytes(text);
            return bytes.ComputeCrc64(hashFormat);
        }

        public static String ComputeCrc64(this byte[] bytes)
        {
            return ComputeCrc64(bytes, HashFormat.Hex);
        }
        public static String ComputeCrc64(this byte[] bytes, HashFormat hashFormat)
        {
            using (var hashImpl = new CRC64())
            {
                var hashBytes = hashImpl.ComputeHash(bytes);
                return ConvertToString(hashBytes, hashFormat);
            }
        }

        public static String ComputeCrc8(this String text)
        {
            return ComputeCrc8(text, HashFormat.Hex);
        }
        public static String ComputeCrc8(this String text, HashFormat hashFormat)
        {
            return ComputeCrc8(text, Encoding.UTF8, hashFormat);
        }
        public static String ComputeCrc8(this String text, Encoding encoding, HashFormat hashFormat)
        {
            var bytes = encoding.GetBytes(text);
            return bytes.ComputeCrc8(hashFormat);
        }

        public static String ComputeCrc8(this byte[] bytes)
        {
            return ComputeCrc8(bytes, HashFormat.Hex);
        }
        public static String ComputeCrc8(this byte[] bytes, HashFormat hashFormat)
        {
            using (var hashImpl = new CRC8())
            {
                var hashBytes = hashImpl.ComputeHash(bytes);
                return ConvertToString(hashBytes, hashFormat);
            }
        }

        public static String ComputeElf32(this String text)
        {
            return ComputeElf32(text, HashFormat.Hex);
        }
        public static String ComputeElf32(this String text, HashFormat hashFormat)
        {
            return ComputeElf32(text, Encoding.UTF8, hashFormat);
        }
        public static String ComputeElf32(this String text, Encoding encoding, HashFormat hashFormat)
        {
            var bytes = encoding.GetBytes(text);
            return bytes.ComputeElf32(hashFormat);
        }

        public static String ComputeElf32(this byte[] bytes)
        {
            return ComputeElf32(bytes, HashFormat.Hex);
        }
        public static String ComputeElf32(this byte[] bytes, HashFormat hashFormat)
        {
            using (var hashImpl = new ELF32())
            {
                var hashBytes = hashImpl.ComputeHash(bytes);
                return ConvertToString(hashBytes, hashFormat);
            }
        }

        public static String ComputeMd5(this String text)
        {
            return ComputeMd5(text, HashFormat.Hex);
        }
        public static String ComputeMd5(this String text, HashFormat hashFormat)
        {
            return ComputeMd5(text, Encoding.UTF8, hashFormat);
        }
        public static String ComputeMd5(this String text, Encoding encoding, HashFormat hashFormat)
        {
            var bytes = encoding.GetBytes(text);
            return bytes.ComputeMd5(hashFormat);
        }

        public static String ComputeMd5(this byte[] bytes)
        {
            return ComputeMd5(bytes, HashFormat.Hex);
        }
        public static String ComputeMd5(this byte[] bytes, HashFormat hashFormat)
        {
            using (var hashImpl = MD5.Create())
            {
                var hashBytes = hashImpl.ComputeHash(bytes);
                return ConvertToString(hashBytes, hashFormat);
            }
        }


        public static Guid ComputeMd5Guid(this String text)
        {
            return ComputeMd5Guid(text, Encoding.UTF8);
        }
        public static Guid ComputeMd5Guid(this String text, Encoding encoding)
        {
            var bytes = encoding.GetBytes(text);
            return ComputeMd5Guid(bytes);
        }

        public static Guid ComputeMd5Guid(this byte[] bytes)
        {
            using (var hashImpl = MD5.Create())
            {
                var hashBytes = hashImpl.ComputeHash(bytes);
                return new Guid(hashBytes);
            }
        }


        public static String ComputeSha1(this String text)
        {
            return ComputeSha1(text, HashFormat.Hex);
        }
        public static String ComputeSha1(this String text, HashFormat hashFormat)
        {
            return ComputeSha1(text, Encoding.UTF8, hashFormat);
        }
        public static String ComputeSha1(this String text, Encoding encoding, HashFormat hashFormat)
        {
            var bytes = encoding.GetBytes(text);
            return bytes.ComputeSha1(hashFormat);
        }

        public static String ComputeSha1(this byte[] bytes)
        {
            return ComputeSha1(bytes, HashFormat.Hex);
        }
        public static String ComputeSha1(this byte[] bytes, HashFormat hashFormat)
        {
            using (var hashImpl = SHA1.Create())
            {
                var hashBytes = hashImpl.ComputeHash(bytes);
                return ConvertToString(hashBytes, hashFormat);
            }
        }

        public static Guid ComputeSha1Guid(this String text)
        {
            return ComputeSha1Guid(text, Encoding.UTF8);
        }
        public static Guid ComputeSha1Guid(this String text, Encoding encoding)
        {
            var bytes = encoding.GetBytes(text);
            return bytes.ComputeSha1Guid();
        }

        public static Guid ComputeSha1Guid(this byte[] bytes)
        {
            using (var hashImpl = SHA1.Create())
            {
                var hashBytes = hashImpl.ComputeHash(bytes);
                return new Guid(hashBytes);
            }
        }


        public static String ComputeSha256(this String text)
        {
            return ComputeSha256(text, HashFormat.Hex);
        }
        public static String ComputeSha256(this String text, HashFormat hashFormat)
        {
            return ComputeSha256(text, Encoding.UTF8, hashFormat);
        }
        public static String ComputeSha256(this String text, Encoding encoding, HashFormat hashFormat)
        {
            var bytes = encoding.GetBytes(text);
            return bytes.ComputeSha256(hashFormat);
        }

        public static String ComputeSha256(this byte[] bytes)
        {
            return ComputeSha256(bytes, HashFormat.Hex);
        }
        public static String ComputeSha256(this byte[] bytes, HashFormat hashFormat)
        {
            using (var hashImpl = SHA256.Create())
            {
                var hashBytes = hashImpl.ComputeHash(bytes);
                return ConvertToString(hashBytes, hashFormat);
            }
        }

        public static String ComputeSha384(this String text)
        {
            return ComputeSha384(text, HashFormat.Hex);
        }
        public static String ComputeSha384(this String text, HashFormat hashFormat)
        {
            return ComputeSha384(text, Encoding.UTF8, hashFormat);
        }
        public static String ComputeSha384(this String text, Encoding encoding, HashFormat hashFormat)
        {
            var bytes = encoding.GetBytes(text);
            return bytes.ComputeSha384(hashFormat);
        }

        public static String ComputeSha384(this byte[] bytes)
        {
            return ComputeSha384(bytes, HashFormat.Hex);
        }
        public static String ComputeSha384(this byte[] bytes, HashFormat hashFormat)
        {
            using (var hashImpl = SHA384.Create())
            {
                var hashBytes = hashImpl.ComputeHash(bytes);
                return ConvertToString(hashBytes, hashFormat);
            }
        }

        public static String ComputeSha512(this String text)
        {
            return ComputeSha512(text, HashFormat.Hex);
        }
        public static String ComputeSha512(this String text, HashFormat hashFormat)
        {
            return ComputeSha512(text, Encoding.UTF8, hashFormat);
        }
        public static String ComputeSha512(this String text, Encoding encoding, HashFormat hashFormat)
        {
            var bytes = encoding.GetBytes(text);
            return bytes.ComputeSha512(hashFormat);
        }

        public static String ComputeSha512(this byte[] bytes)
        {
            return ComputeSha512(bytes, HashFormat.Hex);
        }
        public static String ComputeSha512(this byte[] bytes, HashFormat hashFormat)
        {
            using (var hashImpl = SHA512.Create())
            {
                var hashBytes = hashImpl.ComputeHash(bytes);
                return ConvertToString(hashBytes, hashFormat);
            }
        }

        private static String ConvertToString(byte[] bytes, HashFormat hashFormat)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            switch (hashFormat)
            {
                case HashFormat.Base64:
                    return Convert.ToBase64String(bytes);
                case HashFormat.Binary:
                    {
                        var sb = new StringBuilder();

                        foreach (var b in bytes)
                        {
                            var bin = Convert.ToString(b, 2).PadLeft(8, '0');
                            sb.Append(bin);
                        }

                        return sb.ToString();
                    }
                case HashFormat.Bytes:
                    {
                        return String.Join(",", bytes);
                    }
                case HashFormat.Dec:
                    {
                        var binaryNumber = ConvertToString(bytes, HashFormat.Binary);
                        return NumberBaseConverter.ChangeBase(binaryNumber, 2, 10);
                    }
                case HashFormat.Hex:
                    {
                        using (var writer = new StringWriter())
                        {
                            writer.Write("0x");

                            foreach (var b in bytes)
                            {
                                writer.Write("{0:x2}", b);
                            }

                            return writer.ToString();
                        }
                    }
            }

            return null;
        }
    }
}
