using System;
using System.Globalization;

namespace CITI.EVO.Tools.Comparers
{
    public class StringLogicalComparer : ComparerBase<String>
    {
        public static readonly StringLogicalComparer Default;
        public static readonly StringLogicalComparer CaseInsensitive;
        public static readonly StringLogicalComparer DetectFloatNumber;
        public static readonly StringLogicalComparer CaseInsensitiveAndDetectFloatNumber;

        static StringLogicalComparer()
        {
            Default = new StringLogicalComparer();
            CaseInsensitive = new StringLogicalComparer(true, false);
            DetectFloatNumber = new StringLogicalComparer(false, true);
			CaseInsensitiveAndDetectFloatNumber = new StringLogicalComparer(true, true);
        }

        private struct PartEntry
        {
            public int length;
            public Object value;
        }

	    private readonly NumberFormatInfo _numberFormatInfo;
        private readonly StringComparer _comparer;
		public StringLogicalComparer()
            : this(false, false)
        {
        }

        public StringLogicalComparer(bool ignoreCase, bool floatNumbers)
        {
            IgnoreCase = ignoreCase;
            FloatNumbers = floatNumbers;

			_numberFormatInfo = NumberFormatInfo.InvariantInfo;

            _comparer = (IgnoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        }

        public bool IgnoreCase { get; private set; }
        public bool FloatNumbers { get; private set; }

        public override int Compare(Object x, Object y)
        {
            var xStr = Convert.ToString(x);
            var yStr = Convert.ToString(y);

            return Compare(xStr, yStr);
        }

        public override int Compare(String x, String y)
        {
            int xIndex = 0;
            int yIndex = 0;

            x = (x ?? String.Empty);
            y = (y ?? String.Empty);

            while (xIndex < x.Length || yIndex < y.Length)
            {
                var xEntry = GetNextPart(x, xIndex);
                xIndex += xEntry.length;

                var yEntry = GetNextPart(y, yIndex);
                yIndex += xEntry.length;

                if (xEntry.value is double && yEntry.value is double)
                {
                    var xNumber = (double)xEntry.value;
                    var yNumber = (double)yEntry.value;

                    var order = xNumber.CompareTo(yNumber);
                    if (order != 0)
                    {
                        return order;
                    }
                }
                else
                {
                    var xText = Convert.ToString(xEntry.value);
                    var yText = Convert.ToString(yEntry.value);

                    var order = _comparer.Compare(xText, yText);
                    if (order != 0)
                    {
                        return order;
                    }
                }
            }

            return 0;
        }

        private PartEntry GetNextPart(String text, int startIndex)
        {
            PartEntry entry;

            char @char = (startIndex < text.Length ? text[startIndex] : '\0');

            if (char.IsDigit(@char))
            {
                var digits = GetAllDigits(text, startIndex);

                entry.length = digits.Length;
                entry.value = Convert.ToDouble(digits, _numberFormatInfo);
            }
            else
            {
                var nonDigits = GetAllNonDigits(text, startIndex);

                entry.length = nonDigits.Length;
                entry.value = nonDigits;
            }

            return entry;
        }

        private String GetAllNonDigits(String text, int startIndex)
        {
            var result = String.Empty;
            for (var i = startIndex; i < text.Length; i++)
            {
                if (char.IsDigit(text[i]))
                {
                    break;
                }

                result += text[i];
            }

            return result;
        }

        private String GetAllDigits(String text, int startIndex)
        {
            var result = String.Empty;
            for (var i = startIndex; i < text.Length; i++)
            {
                if (!char.IsDigit(text[i]))
                {
                    var strChar = Convert.ToString(text[i]);
                    if (!FloatNumbers || strChar != _numberFormatInfo.NumberDecimalSeparator || result.Contains(strChar))
                    {
                        break;
                    }
                }

                result += text[i];
            }

            if (result.EndsWith(_numberFormatInfo.NumberDecimalSeparator))
            {
                result = result.Substring(0, result.Length - 1);
            }

            return result;
        }

        public override bool Equals(String x, String y)
        {
            return Compare(x, y) == 0;
        }

        public override int GetHashCode(String obj)
        {
            if (obj == null)
            {
                return 0;
            }

            return _comparer.GetHashCode(obj);
        }
    }
}