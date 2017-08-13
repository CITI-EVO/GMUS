﻿using System;
using System.Globalization;

namespace CITI.EVO.Tools.Comparers
{
    public class StringLogicalComparer : ComparerBase<String>
    {
        public static readonly StringLogicalComparer Ordinal;
        public static readonly StringLogicalComparer OrdinalIgnoreCase;
        public static readonly StringLogicalComparer FloatingNumberSensitive;
        public static readonly StringLogicalComparer FloatingNumberIgnoreCase;

        static StringLogicalComparer()
        {
            Ordinal = new StringLogicalComparer();
            OrdinalIgnoreCase = new StringLogicalComparer(true, false);
            FloatingNumberSensitive = new StringLogicalComparer(false, true);
            FloatingNumberIgnoreCase = new StringLogicalComparer(true, true);
        }

        private readonly bool _ignoreCase;
        private readonly bool _floatNumbers;

        private readonly int _decimalSeparatorLen;
        private readonly String _decimalSeparator;

        private readonly StringComparer _comparer;
        private readonly NumberFormatInfo _numberFormatInfo;

        public StringLogicalComparer()
            : this(false, false)
        {
        }
        public StringLogicalComparer(bool ignoreCase, bool floatNumbers)
        {
            _ignoreCase = ignoreCase;
            _floatNumbers = floatNumbers;

            _numberFormatInfo = NumberFormatInfo.InvariantInfo;
            _decimalSeparator = _numberFormatInfo.NumberDecimalSeparator;
            _decimalSeparatorLen = _decimalSeparator.Length;

            _comparer = (_ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        }

        public bool IgnoreCase
        {
            get { return _ignoreCase; }
        }

        public bool FloatNumbers
        {
            get { return _floatNumbers; }
        }

        public override int Compare(Object x, Object y)
        {
            var xStr = Convert.ToString(x);
            var yStr = Convert.ToString(y);

            return Compare(xStr, yStr);
        }

        public override int Compare(String x, String y)
        {
            x = (x ?? String.Empty);
            y = (y ?? String.Empty);

            var xIndex = 0;
            var yIndex = 0;

            while (xIndex < x.Length || yIndex < y.Length)
            {
                var xSegment = NextSegment(x, xIndex);
                var ySegment = NextSegment(y, yIndex);

                var order = CompareSegments(x, xSegment, y, ySegment);
                if (order != 0)
                    return order;

                xIndex = xSegment.index + xSegment.length;
                yIndex = ySegment.index + ySegment.length;
            }

            return 0;
        }

        private Segment NextSegment(String text, int start)
        {
            if (text.Length == 0 || text.Length <= start)
                return new Segment();

            var index = start;
            var separator = false;

            var segment = new Segment
            {
                index = index,
                length = 0,
                digits = char.IsDigit(text[index])
            };

            while (index < text.Length)
            {
                var @char = text[index++];

                if (char.IsDigit(@char))
                {
                    if (!segment.digits)
                        break;
                }
                else
                {
                    if (segment.digits)
                    {
                        if (_floatNumbers && !separator && IsDecimalSeparator(text, index))
                            separator = true;
                        else
                            break;
                    }
                }

                segment.length++;
            }

            return segment;
        }

        private int CompareSegments(String x, Segment xSegment, String y, Segment ySegment)
        {
            var xs = x.Substring(xSegment.index, xSegment.length);
            var ys = y.Substring(ySegment.index, ySegment.length);

            if (xSegment.digits && ySegment.digits)
            {
                double xd, yd;
                if (double.TryParse(xs, NumberStyles.Any, _numberFormatInfo, out xd) &&
                    double.TryParse(ys, NumberStyles.Any, _numberFormatInfo, out yd))
                {
                    return xd.CompareTo(yd);
                }
            }

            return _comparer.Compare(xs, ys);
        }

        private bool IsDecimalSeparator(String text, int index)
        {
            if (_decimalSeparatorLen == 1)
                return text[index] == _decimalSeparator[0];

            var order = String.Compare(text, index, _decimalSeparator, 0, _decimalSeparatorLen);
            return (order == 0);
        }

        public override bool Equals(String x, String y)
        {
            return _comparer.Equals(x, y);
        }

        public override int GetHashCode(String obj)
        {
            if (obj == null)
                return 0;

            return _comparer.GetHashCode(obj);
        }

        private struct Segment
        {
            public int index;
            public int length;
            public bool digits;
        }
    }
}