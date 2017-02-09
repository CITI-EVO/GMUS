using System;

namespace CITI.EVO.Tools.Helpers
{
    public class HashHelper
    {
        public static int GuidHash(Guid value)
        {
            var bytes = value.ToByteArray();

            var a = ((int)bytes[3] << 24) | ((int)bytes[2] << 16) | ((int)bytes[1] << 8) | bytes[0];
            var b = (short)((bytes[5] << 8) | bytes[4]);
            var c = (short)((bytes[7] << 8) | bytes[6]);
            var d = bytes[8];
            var e = bytes[9];
            var f = bytes[10];
            var g = bytes[11];
            var h = bytes[12];
            var i = bytes[13];
            var j = bytes[14];
            var k = bytes[15];

            var hash = a ^ ((b << 16) | (ushort)c) ^ ((f << 24) | k);
            return hash;
        }
        /* End Of Guid Hash Function */

        public static long RSHash(byte[] bytes)
        {
            int b = 378551;
            int a = 63689;
            long hash = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                hash = hash * a + bytes[i];
                a = a * b;
            }

            return hash;
        }
        /* End Of RS Hash Function */

        public long JSHash(byte[] bytes)
        {
            long hash = 1315423911;

            for (int i = 0; i < bytes.Length; i++)
            {
                hash ^= ((hash << 5) + bytes[i] + (hash >> 2));
            }

            return hash;
        }
        /* End Of JS Hash Function */

        public long PJWHash(byte[] bytes)
        {
            const int bitsInUnsignedInt = 4 * 8;
            const int threeQuarters = (bitsInUnsignedInt * 3) / 4;
            const int oneEighth = bitsInUnsignedInt / 8;

            const long highBits = (long)(0xFFFFFFFF) << bitsInUnsignedInt - oneEighth;

            long hash = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                hash = (hash << oneEighth) + bytes[i];

                long test;
                if ((test = hash & highBits) != 0)
                {
                    hash = ((hash ^ (test >> threeQuarters)) & (~highBits));
                }
            }

            return hash;
        }
        /* End Of  P. J. Weinberger Hash Function */

        public long ELFHash(byte[] bytes)
        {
            long hash = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                hash = (hash << 4) + bytes[i];

                long x;
                if ((x = hash & 0xF0000000L) != 0)
                {
                    hash ^= (x >> 24);
                }
                hash &= ~x;
            }

            return hash;
        }
        /* End Of ELF Hash Function */

        public long BKDRHash(byte[] bytes)
        {
            const long seed = 131; // 31 131 1313 13131 131313 etc..
            long hash = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                hash = (hash * seed) + bytes[i];
            }

            return hash;
        }
        /* End Of BKDR Hash Function */

        public long SDBMHash(byte[] bytes)
        {
            long hash = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                hash = bytes[i] + (hash << 6) + (hash << 16) - hash;
            }

            return hash;
        }
        /* End Of SDBM Hash Function */

        public long DJBHash(byte[] bytes)
        {
            long hash = 5381;

            for (int i = 0; i < bytes.Length; i++)
            {
                hash = ((hash << 5) + hash) + bytes[i];
            }

            return hash;
        }
        /* End Of DJB Hash Function */

        public long DEKHash(byte[] bytes)
        {
            long hash = bytes.Length;

            for (int i = 0; i < bytes.Length; i++)
            {
                hash = ((hash << 5) ^ (hash >> 27)) ^ bytes[i];
            }

            return hash;
        }
        /* End Of DEK Hash Function */

        public long BPHash(byte[] bytes)
        {
            long hash = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                hash = hash << 7 ^ bytes[i];
            }

            return hash;
        }
        /* End Of BP Hash Function */

        public long FNVHash(byte[] bytes)
        {
            long fnv_prime = 0x811C9DC5;
            long hash = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                hash *= fnv_prime;
                hash ^= bytes[i];
            }

            return hash;
        }
        /* End Of FNV Hash Function */

        public long APHash(byte[] bytes)
        {
            long hash = 0xAAAAAAAA;

            for (int i = 0; i < bytes.Length; i++)
            {
                if ((i & 1) == 0)
                {
                    hash ^= ((hash << 7) ^ bytes[i] * (hash >> 3));
                }
                else
                {
                    hash ^= (~((hash << 11) + bytes[i] ^ (hash >> 5)));
                }
            }

            return hash;
        }
        /* End Of AP Hash Function */
    }
}
