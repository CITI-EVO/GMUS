using System.Security.Cryptography;

namespace CITI.EVO.Tools.Security.Cryptography
{
    public class CRC8 : HashAlgorithm
    {
        private const byte polynomial = 0xd5;
        private const byte seed = 0xff;

        private readonly byte[] table = new byte[256];

        public CRC8()
        {
            HashCode = seed;
            HashValue = new[] { HashCode };

            for (int i = 0; i < 256; ++i)
            {
                int temp = i;

                for (int j = 0; j < 8; ++j)
                {
                    if ((temp & 0x80) != 0)
                    {
                        temp = (temp << 1) ^ polynomial;
                    }
                    else
                    {
                        temp <<= 1;
                    }
                }

                table[i] = (byte)temp;
            }
        }

        public byte HashCode { get; private set; }

        public override int HashSize { get { return 8; } }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            byte crc = seed;

            for (int i = ibStart; i < cbSize; ++i)
            {
                byte b = array[i];
                crc = table[crc ^ b];
            }

            HashCode = crc;
        }

        protected override byte[] HashFinal()
        {
            return new[] { HashCode };
        }

        public override void Initialize()
        {
            HashCode = seed;
            HashValue = new[] { HashCode };
        }
    }
}
