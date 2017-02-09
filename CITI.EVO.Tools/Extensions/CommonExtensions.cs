using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using CITI.EVO.Tools.Collections;

namespace CITI.EVO.Tools.Extensions
{
    public static class CommonExtensions
    {
        private const int DefaultCapacity = 128;

		public static IList<TItem> Syncronized<TItem>(this IList<TItem> source)
		{
			return new SynchronizedList<TItem>(source);
		}
		public static IDictionary<TKey, TValue> Syncronized<TKey, TValue>(this IDictionary<TKey, TValue> source)
		{
			return new SynchronizedDictionary<TKey, TValue>(source);
		}

		public static void AddIfNotPresent<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (!source.ContainsKey(key))
            {
                source.Add(key, value);
            }
        }
        public static void AddIfNotNull<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (ReferenceEquals(value, null))
            {
                return;
            }

            source.Add(key, value);
        }
        public static void AddIfNotNullAndNotPresent<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (ReferenceEquals(value, null) || source.ContainsKey(key))
            {
                return;
            }

            source.Add(key, value);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            return GetValueOrDefault(source, key, default(TValue));
        }
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue)
        {
            TValue value;

            if (source != null && source.TryGetValue(key, out value))
            {
                return value;
            }

            return defaultValue;
        }

        public static IEnumerable<TValue> GetValueOrDefault<TKey, TValue>(this ILookup<TKey, TValue> source, TKey key)
        {
            if (source.Contains(key))
            {
                return source[key];
            }

            return null;
        }
        public static IEnumerable<TValue> GetValueOrDefault<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> source, TKey key)
        {
            var value = source.FirstOrDefault(n => Equals(n.Key, key));
            return value;
        }

        public static bool IsNullOrEmpty<T>(this IList<T> source, bool checkValues = false)
        {
            if (source == null || source.Count == 0)
                return true;


            var result = false;

            if (checkValues)
            {
                result = source.All(p => p == null);
            }

            return result;

        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source, bool checkValues = false)
        {
            if (source == null)
                return true;

            bool result;

            if (checkValues)
            {
                result = source.All(p => p == null);
            }
            else
            {
                result = !source.Any();
            }

            return result;
        }

        public static TResult GetScalarValue<TResult>(this IEnumerable collection)
        {
            Object item = null;
            foreach (var entry in collection)
            {
                item = entry;
                break;
            }

            if (item == null)
                return default(TResult);

            var itemType = item.GetType();
            var firstProperty = (from n in itemType.GetProperties()
                                 where n.PropertyType == typeof(TResult)
                                 select n).FirstOrDefault();

            if (firstProperty == null)
                throw new Exception();

            var value = firstProperty.GetValue(item, null);
            return (TResult)value;
        }

        public static SortedDictionary<TKey, TItem> ToSortedDictionary<TItem, TKey>(this IEnumerable<TItem> source, Func<TItem, TKey> keySelector)
        {
            return ToSortedDictionary(source, keySelector, Comparer<TKey>.Default);
        }
        public static SortedDictionary<TKey, TItem> ToSortedDictionary<TItem, TKey>(this IEnumerable<TItem> source, Func<TItem, TKey> keySelector, IComparer<TKey> comparer)
        {
            return ToSortedDictionary(source, keySelector, n => n, comparer);
        }

        public static SortedDictionary<TKey, TValue> ToSortedDictionary<TItem, TKey, TValue>(this IEnumerable<TItem> source, Func<TItem, TKey> keySelector, Func<TItem, TValue> valueSelector)
        {
            return ToSortedDictionary(source, keySelector, valueSelector, Comparer<TKey>.Default);
        }
        public static SortedDictionary<TKey, TValue> ToSortedDictionary<TItem, TKey, TValue>(this IEnumerable<TItem> source, Func<TItem, TKey> keySelector, Func<TItem, TValue> valueSelector, IComparer<TKey> comparer)
        {
            var dest = new SortedDictionary<TKey, TValue>(comparer);

            foreach (var item in source)
            {
                var key = keySelector(item);
                var value = valueSelector(item);

                dest.Add(key, value);
            }

            return dest;
        }

        public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            return source.ToSortedDictionary(Comparer<TKey>.Default);
        }
        public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, IComparer<TKey> comparer)
        {
            var dest = new SortedDictionary<TKey, TValue>(comparer);

            foreach (var item in source)
            {
                dest.Add(item.Key, item.Value);
            }

            return dest;
        }

        public static SortedDictionary<TKey, IList<TValue>> ToSortedDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> source)
        {
            return source.ToSortedDictionary(Comparer<TKey>.Default);
        }
        public static SortedDictionary<TKey, IList<TValue>> ToSortedDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> source, IComparer<TKey> comparer)
        {
            var dest = new SortedDictionary<TKey, IList<TValue>>(comparer);

            foreach (var item in source)
            {
                dest.Add(item.Key, item.ToList());
            }

            return dest;
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            return source.ToDictionary(EqualityComparer<TKey>.Default);
        }
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, IEqualityComparer<TKey> comparer)
        {
            var dest = new Dictionary<TKey, TValue>(DefaultCapacity, comparer);

            foreach (var item in source)
            {
                dest.Add(item.Key, item.Value);
            }

            return dest;
        }

        public static Dictionary<TKey, IList<TValue>> ToDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> source)
        {
            return source.ToDictionary(EqualityComparer<TKey>.Default);
        }
        public static Dictionary<TKey, IList<TValue>> ToDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> source, IEqualityComparer<TKey> comparer)
        {
            var dest = new Dictionary<TKey, IList<TValue>>(DefaultCapacity, comparer);

            foreach (var item in source)
            {
                dest.Add(item.Key, item.ToList());
            }

            return dest;
        }

        public static SortedDictionary<TKey, IList<TItem>> ToSortedLookup<TItem, TKey>(this IEnumerable<TItem> source, Func<TItem, TKey> keySelector)
        {
            return ToSortedLookup(source, keySelector, Comparer<TKey>.Default);
        }
        public static SortedDictionary<TKey, IList<TItem>> ToSortedLookup<TItem, TKey>(this IEnumerable<TItem> source, Func<TItem, TKey> keySelector, IComparer<TKey> comparer)
        {
            return ToSortedLookup(source, keySelector, n => n, comparer);
        }

        public static SortedDictionary<TKey, IList<TValue>> ToSortedLookup<TItem, TKey, TValue>(this IEnumerable<TItem> source, Func<TItem, TKey> keySelector, Func<TItem, TValue> valueSelector)
        {
            return ToSortedLookup(source, keySelector, valueSelector, Comparer<TKey>.Default);
        }
        public static SortedDictionary<TKey, IList<TValue>> ToSortedLookup<TItem, TKey, TValue>(this IEnumerable<TItem> source, Func<TItem, TKey> keySelector, Func<TItem, TValue> valueSelector, IComparer<TKey> comparer)
        {
            var dest = new SortedDictionary<TKey, IList<TValue>>(comparer);

            foreach (var item in source)
            {
                var key = keySelector(item);

                IList<TValue> list;
                if (!dest.TryGetValue(key, out list))
                {
                    list = new List<TValue>();
                    dest.Add(key, list);
                }

                var value = valueSelector(item);

                list.Add(value);
            }

            return dest;
        }

        public static SortedList<TKey, TItem> ToSortedList<TItem, TKey>(this IEnumerable<TItem> source, Func<TItem, TKey> keySelector)
        {
            return ToSortedList(source, keySelector, Comparer<TKey>.Default);
        }
        public static SortedList<TKey, TItem> ToSortedList<TItem, TKey>(this IEnumerable<TItem> source, Func<TItem, TKey> keySelector, IComparer<TKey> comparer)
        {
            return ToSortedList(source, keySelector, n => n, comparer);
        }

        public static SortedList<TKey, TValue> ToSortedList<TItem, TKey, TValue>(this IEnumerable<TItem> source, Func<TItem, TKey> keySelector, Func<TItem, TValue> valueSelector)
        {
            return ToSortedList(source, keySelector, valueSelector, Comparer<TKey>.Default);
        }
        public static SortedList<TKey, TValue> ToSortedList<TItem, TKey, TValue>(this IEnumerable<TItem> source, Func<TItem, TKey> keySelector, Func<TItem, TValue> valueSelector, IComparer<TKey> comparer)
        {
            var dest = new SortedList<TKey, TValue>(DefaultCapacity, comparer);

            foreach (var item in source)
            {
                var key = keySelector(item);
                var value = valueSelector(item);

                dest.Add(key, value);
            }

            return dest;
        }

        public static SortedSet<TItem> ToSortedSet<TItem>(this IEnumerable<TItem> source)
        {
            return ToSortedSet(source, Comparer<TItem>.Default);
        }
        public static SortedSet<TItem> ToSortedSet<TItem>(this IEnumerable<TItem> source, IComparer<TItem> comparer)
        {
            var dest = new SortedSet<TItem>(comparer);

            foreach (var item in source)
            {
                dest.Add(item);
            }

            return dest;
        }

        public static HashSet<TItem> ToHashSet<TItem>(this IEnumerable<TItem> source)
        {
            return ToHashSet(source, EqualityComparer<TItem>.Default);
        }
        public static HashSet<TItem> ToHashSet<TItem>(this IEnumerable<TItem> source, IEqualityComparer<TItem> equalityComparer)
        {
            var dest = new HashSet<TItem>(equalityComparer);

            foreach (var item in source)
            {
                dest.Add(item);
            }

            return dest;
        }

        public static IEnumerable<TItem> Distinct<TItem, TValue>(this IEnumerable<TItem> source, Func<TItem, TValue> valueSelector)
        {
            return source.Distinct(valueSelector, EqualityComparer<TValue>.Default);
        }
        public static IEnumerable<TItem> Distinct<TItem, TValue>(this IEnumerable<TItem> source, Func<TItem, TValue> valueSelector, IEqualityComparer<TValue> equalityComparer)
        {
            var dict = new Dictionary<TValue, TItem>(DefaultCapacity, equalityComparer);

            foreach (var item in source)
            {
                var value = valueSelector(item);

                if (!dict.ContainsKey(value))
                {
                    dict.Add(value, item);
                }
            }

            return new List<TItem>(dict.Values);
        }

        public static bool GetBitAt(this sbyte value, int bitIndex)
        {
            var pos = (sizeof(sbyte) * 8) - bitIndex;
            var mask = (sbyte)(1 << pos - 1);

            var bit = (value & mask) != 0;
            return bit;
        }

        public static bool GetBitAt(this byte value, int bitIndex)
        {
            var pos = (sizeof(byte) * 8) - bitIndex;
            var mask = (byte)(1 << pos - 1);

            var bit = (value & mask) != 0;
            return bit;
        }

        public static bool GetBitAt(this short value, int bitIndex)
        {
            var pos = (sizeof(short) * 8) - bitIndex;
            var mask = (short)(1 << pos - 1);

            var bit = (value & mask) != 0;
            return bit;
        }

        public static bool GetBitAt(this ushort value, int bitIndex)
        {
            var pos = (sizeof(ushort) * 8) - bitIndex;
            var mask = (ushort)(1 << pos - 1);

            var bit = (value & mask) != 0;
            return bit;
        }

        public static bool GetBitAt(this int value, int bitIndex)
        {
            var pos = (sizeof(int) * 8) - bitIndex;
            var mask = (int)(1 << pos - 1);

            var bit = (value & mask) != 0;
            return bit;
        }

        public static bool GetBitAt(this uint value, int bitIndex)
        {
            var pos = (sizeof(uint) * 8) - bitIndex;
            var mask = (uint)(1 << pos - 1);

            var bit = (value & mask) != 0;
            return bit;
        }

        public static bool GetBitAt(this long value, int bitIndex)
        {
            var pos = (sizeof(long) * 8) - bitIndex;
            var mask = (long)(1 << pos - 1);

            var bit = (value & mask) != 0;
            return bit;
        }

        public static bool GetBitAt(this ulong value, int bitIndex)
        {
            var pos = (sizeof(ulong) * 8) - bitIndex;
            var mask = (ulong)(1 << pos - 1);

            var bit = (value & mask) != 0;
            return bit;
        }

        public static bool GetBitAt(this double value, int bitIndex)
        {
            var @long = BitConverter.DoubleToInt64Bits(value);
            return GetBitAt(@long, bitIndex);
        }


        public static byte[] ReadToEnd(this BinaryReader reader)
        {
            using (var destStream = new MemoryStream())
            {
                int readed;
                var buffer = new byte[8192];

                while ((readed = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    destStream.Write(buffer, 0, readed);
                }

                return destStream.ToArray();
            }
        }

        public static byte[] ReadToEnd(this Stream stream)
        {
            using (var destStream = new MemoryStream())
            {
                int readed;
                var buffer = new byte[8192];

                while ((readed = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    destStream.Write(buffer, 0, readed);
                }

                return destStream.ToArray();
            }
        }

        public static String Between(this String value, String a, String b)
        {
            int posA = value.IndexOf(a, StringComparison.Ordinal);
            if (posA == -1)
            {
                return String.Empty;
            }

            int posB = value.LastIndexOf(b, StringComparison.Ordinal);
            if (posB == -1)
            {
                return String.Empty;
            }

            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB)
            {
                return String.Empty;
            }

            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }

        public static String Before(this String value, String a)
        {
            int posA = value.IndexOf(a, StringComparison.Ordinal);
            if (posA == -1)
            {
                return String.Empty;
            }

            return value.Substring(0, posA);
        }

        public static String After(this String value, String a)
        {
            int posA = value.LastIndexOf(a, StringComparison.Ordinal);
            if (posA == -1)
            {
                return String.Empty;
            }

            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= value.Length)
            {
                return String.Empty;
            }

            return value.Substring(adjustedPosA);
        }

        public static String TrimLen(this String value, int length)
        {
            if (String.IsNullOrEmpty(value) || value.Length <= length)
            {
                return value;
            }

            return value.Substring(0, length);
        }

        public static String UTrim(this String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Trim();
        }

        public static String UTrimStart(this String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.TrimStart();
        }

        public static String UTrimEnd(this String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.TrimEnd();
        }

        public static bool UEquals(this String value, String other)
        {
            return String.Equals(value.UTrim(), other.UTrim(), StringComparison.OrdinalIgnoreCase);
        }

        public static BigInteger Sqrt(this BigInteger n)
        {
            if (n == 0)
            {
                return 0;
            }

            if (n > 0)
            {
                var binLen = (int)Math.Ceiling(BigInteger.Log(n, 2));
                var root = BigInteger.One << (binLen / 2);

                while (!IsSqrt(n, root))
                {
                    root += n / root;
                    root /= 2;
                }

                return root;
            }

            throw new ArithmeticException("NaN");
        }

        public static BigInteger Pow(this BigInteger n, BigInteger k)
        {
            var b = BigInteger.One;

            while (k > BigInteger.Zero)
            {
                if (k % 2 == 0)
                {
                    k /= 2;
                    n *= n; // [ a = (a*a)%n; ]
                }
                else
                {
                    k--;
                    b *= n; // [ b = (b*a)%n; ]
                }
            }

            return b;
        }

        public static BigInteger PowMod(this BigInteger n, BigInteger k, BigInteger m)
        {
            var b = BigInteger.One;

            while (k > BigInteger.Zero)
            {
                if (k % 2 == 0)
                {
                    k /= 2;
                    n = (n * n) % n;
                }
                else
                {
                    k--;
                    b = (b * n) % n;
                }
            }

            return b;
        }

        private static bool IsSqrt(BigInteger n, BigInteger root)
        {
            var lowerBound = root * root;
            var upperBound = (root + 1) * (root + 1);

            return (n >= lowerBound && n < upperBound);
        }

        public static bool IsNullOrEmpty(this Guid? value)
        {
            return (value == null || value == Guid.Empty);
        }
    }
}
