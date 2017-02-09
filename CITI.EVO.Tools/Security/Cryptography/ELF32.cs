using System;
using System.Security.Cryptography;

namespace CITI.EVO.Tools.Security.Cryptography
{
    public class ELF32 : HashAlgorithm
    {
        private const uint polynomial = 0xf0000000;
        private const uint seed = 0;

        public ELF32()
        {
            HashCode = seed;
        }

        public uint HashCode { get; private set; }

        public override int HashSize { get { return 32; } }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            uint elf = seed;

            for (int i = ibStart; i < cbSize; i++)
            {
                unchecked
                {
                    elf = (elf << 4) + array[i];
                    uint work = (elf & polynomial);

                    if (work != 0)
                    {
                        elf ^= (work >> 24);
                    }

                    elf &= ~work;
                }
            }

            HashCode = elf;
        }

        protected override byte[] HashFinal()
        {
            return BitConverter.GetBytes(HashCode);
        }

        public override void Initialize()
        {
            HashCode = seed;
        }
    }
}