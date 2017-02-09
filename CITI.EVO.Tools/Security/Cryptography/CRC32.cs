using System;
using System.Security.Cryptography;

namespace CITI.EVO.Tools.Security.Cryptography
{
    public class CRC32 : HashAlgorithm
    {
        private const uint polynomial = 0xedb88320;
        private const uint seed = 0xffffffff;

        private readonly uint[] table = new uint[256];

        public CRC32()
        {
            HashCode = seed;
            HashValue = BitConverter.GetBytes(HashCode);

            for (uint i = 0; i < table.Length; ++i)
            {
                uint temp = i;
                for (int j = 8; j > 0; --j)
                {
                    if ((temp & 1) == 1)
                    {
                        temp = (temp >> 1) ^ polynomial;
                    }
                    else
                    {
                        temp >>= 1;
                    }
                }
                table[i] = temp;
            }
        }

        public uint HashCode { get; private set; }

        public override int HashSize { get { return 32; } }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            uint crc = seed;

            for (int i = ibStart; i < cbSize; ++i)
            {
                unchecked
                {
                    byte index = (byte)(((crc) & 0xff) ^ array[i]);
                    crc = (crc >> 8) ^ table[index];
                }
            }

            HashCode = ~crc;
        }

        protected override byte[] HashFinal()
        {
            return BitConverter.GetBytes(HashCode);
        }

        public override void Initialize()
        {
            HashCode = seed;
            HashValue = BitConverter.GetBytes(HashCode);
        }
    }
}
