using System;
using System.Security.Cryptography;

namespace CITI.EVO.Tools.Security.Cryptography
{
    public class CRC16 : HashAlgorithm
    {
        private const ushort polynomial = 0xA001;
        private const ushort seed = 0xffff;

        private readonly ushort[] table = new ushort[256];

        public CRC16()
        {
            HashCode = seed;
            HashValue = BitConverter.GetBytes(HashCode);

            for (ushort i = 0; i < table.Length; ++i)
            {
                ushort value = 0;
                ushort temp = i;

                for (byte j = 0; j < 8; ++j)
                {
                    if (((value ^ temp) & 0x0001) != 0)
                    {
                        value = (ushort)((value >> 1) ^ polynomial);
                    }
                    else
                    {
                        value >>= 1;
                    }

                    temp >>= 1;
                }

                table[i] = value;
            }
        }

        public ushort HashCode { get; private set; }

        public override int HashSize { get { return 16; } }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            ushort crc = seed;

            for (int i = ibStart; i < cbSize; ++i)
            {
                unchecked
                {
                    byte index = (byte)(crc ^ array[i]);
                    crc = (ushort)((crc >> 8) ^ table[index]);
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
