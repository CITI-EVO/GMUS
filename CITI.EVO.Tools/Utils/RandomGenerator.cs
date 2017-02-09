using System;
using System.Globalization;
using System.Linq;
using System.Text;
using CITI.EVO.Tools.Security.Cryptography;

namespace CITI.EVO.Tools.Utils
{
    public static class RandomGenerator
    {
        public static String GenerateInt32String()
        {
            return GenerateInt32().ToString(CultureInfo.InvariantCulture);
        }

        public static uint GenerateInt32()
        {
            using (var crc32 = new CRC32())
            {
                var uniqText = String.Concat(Guid.NewGuid(), Guid.NewGuid(), Environment.TickCount);
                var srcBytes = Encoding.UTF32.GetBytes(uniqText);
                var hashBytes = crc32.ComputeHash(srcBytes);

                return BitConverter.ToUInt32(hashBytes, 0);
            }
        }

        public static String GenerateInt64String()
        {
            return GenerateInt64().ToString(CultureInfo.InvariantCulture);
        }

        public static ulong GenerateInt64()
        {
            using (var crc64 = new CRC64())
            {
                var uniqText = String.Concat(Guid.NewGuid(), Guid.NewGuid(), Environment.TickCount);
                var srcBytes = Encoding.UTF32.GetBytes(uniqText);
                var hashBytes = crc64.ComputeHash(srcBytes);

                return BitConverter.ToUInt64(hashBytes, 0);
            }
        }

        public static String GenerateHex32()
        {
            using (var crc32 = new CRC32())
            {
                var uniqText = String.Concat(Guid.NewGuid(), Guid.NewGuid(), Environment.TickCount);
                var srcBytes = Encoding.UTF32.GetBytes(uniqText);
                var hashBytes = crc32.ComputeHash(srcBytes);

                return String.Join(String.Empty, hashBytes.Select(b => b.ToString("x2")));
            }
        }

        public static String GenerateHex64()
        {
            using (var crc64 = new CRC64())
            {
                var uniqText = String.Concat(Guid.NewGuid(), Guid.NewGuid(), Environment.TickCount);
                var srcBytes = Encoding.UTF32.GetBytes(uniqText);
                var hashBytes = crc64.ComputeHash(srcBytes);

                return String.Join(String.Empty, hashBytes.Select(b => b.ToString("x2")));
            }
        }
    }
}
