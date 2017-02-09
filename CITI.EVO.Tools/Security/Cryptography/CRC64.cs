using System;
using System.Security.Cryptography;

namespace CITI.EVO.Tools.Security.Cryptography
{
    public class CRC64 : HashAlgorithm
    {
        private const ulong polynomial = 0xC96C5795D7870F42;
        private const ulong seed = 0xffffffffffffffff;

        private readonly ulong[] table = new ulong[256];

        public CRC64()
        {
            HashCode = seed;
            HashValue = BitConverter.GetBytes(HashCode);

            for (uint i = 0; i < 256; ++i)
            {
                ulong crc = i;

                for (uint j = 0; j < 8; ++j)
                {
                    if ((crc & 1) == 1)
                    {
                        crc = (crc >> 1) ^ polynomial;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }

                table[i] = crc;
            }
        }

        public ulong HashCode { get; private set; }

        public override int HashSize { get { return 64; } }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            ulong crc = seed;

            for (int i = ibStart; i < cbSize; ++i)
            {
                unchecked
                {
                    byte index = (byte)(((crc >> 56) ^ array[i]) & 0xff);
                    crc = table[index] ^ (crc << 8);
                }
            }

            HashCode = crc;
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
