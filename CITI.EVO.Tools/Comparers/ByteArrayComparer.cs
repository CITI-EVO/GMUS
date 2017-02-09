using System.Runtime.InteropServices;

namespace CITI.EVO.Tools.Comparers
{
    public class ByteArrayComparer : ComparerBase<byte[]>
    {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int memcmp(byte[] xArray, byte[] yArray, long count);

        public override int Compare(byte[] x, byte[] y)
        {
            var xLen = GetLength(x);
            var yLen = GetLength(y);

            var order = xLen.CompareTo(yLen);
            if (order == 0 && xLen > 0 && yLen > 0)
            {
                order = memcmp(x, y, x.Length);
            }

            return order;
        }

        public override bool Equals(byte[] x, byte[] y)
        {
            return Compare(x, y) == 0;
        }

        public override int GetHashCode(byte[] obj)
        {
            int hash = obj.Length + 1;

            for (var i = 0; i < obj.Length; i++)
            {
                hash *= 257;
                hash ^= obj[i];
            }

            return hash;
        }

        private int GetLength(byte[] array)
        {
            if (array == null)
            {
                return -1;
            }

            return array.Length;
        }
    }
}
