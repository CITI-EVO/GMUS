using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.ExpressionEngine.Common;
using CITI.EVO.Tools.Utils;

namespace CITI.EVO.Tools.ExpressionEngine
{
    internal static class ExpressionHelper
    {
        private static readonly StringLogicalComparer defaultComparer = StringLogicalComparer.FloatingNumberSensitive;
        private static readonly IComparer<Object> comparisonComparer = new ComparisonComparer<Object>(Compare);
        private static readonly StringComparer ordinalComparer = StringComparer.Ordinal;

        public static IComparer<Object> Comparer
        {
            get { return comparisonComparer; }
        }

        public static int Compare(Object x, Object y)
        {
            var xs = Convert.ToString(x);
            var ys = Convert.ToString(y);

            if (String.IsNullOrEmpty(xs) && String.IsNullOrEmpty(ys))
                return 0;

            var xDate = DataConverter.ToNullableDateTime(x);
            var yDate = DataConverter.ToNullableDateTime(y);

            if (xDate != null && yDate != null)
                return xDate.Value.CompareTo(yDate.Value);

            if (xDate != null && yDate == null && String.IsNullOrEmpty(ys))
                return 1;

            if (yDate != null && xDate == null && String.IsNullOrEmpty(xs))
                return -1;

            var xNum = DataConverter.ToNullableDouble(x);
            var yNum = DataConverter.ToNullableDouble(y);

            if (xNum != null && yNum != null)
                return xNum.Value.CompareTo(yNum.Value);

            if (xNum != null && yNum == null && String.IsNullOrEmpty(ys))
                return 1;

            if (yNum != null && xNum == null && String.IsNullOrEmpty(xs))
                return -1;

            var order = ordinalComparer.Compare(xs, ys);
            if (order != 0)
                order = defaultComparer.Compare(xs, ys);

            return order;
        }

        public static Object GetAny(Object value)
        {
            if (IsNumber(value))
                return GetNumber(value);

            if (IsString(value))
                return GetString(value);

            if (IsDateTime(value))
                return GetDateTime(value);

            return value;
        }

        public static double GetNumber(Object value)
        {
            if (value is double)
                return (double)value;

            var number = DataConverter.ToNullableDouble(value);
            return number.GetValueOrDefault();
        }

        public static String GetString(Object value)
        {
            var strValue = Convert.ToString(value);

            if (IsString(value))
                return strValue.Substring(1, strValue.Length - 2);

            return strValue;
        }

        public static String GetQuota(Object value)
        {
            var strValue = Convert.ToString(value);

            if (IsString(value))
                return strValue.Substring(0, 1);

            throw new Exception();
        }

        public static DateTime GetDateTime(Object value)
        {
            if (value is DateTime)
                return (DateTime)value;

            var strValue = Convert.ToString(value);

            if (IsDateTime(value))
            {
                strValue = strValue.Substring(1, strValue.Length - 2);

                var dateTime = DataConverter.ToNullableDateTime(strValue);
                if (dateTime == null)
                    throw new Exception($"'{value}' is not correct DateTime");

                return dateTime.Value;
            }

            var result = DataConverter.ToNullableDateTime(strValue);
            if (result.HasValue)
                return result.Value;

            throw new Exception($"'{value}' is not correct DateTime");
        }

        public static bool IsNumber(Object value)
        {
            if (value is double)
                return true;

            var number = DataConverter.ToNullableDouble(value);
            return (number != null);
        }

        public static bool IsString(Object value)
        {
            if (IsNumber(value))
                return false;

            var strValue = Convert.ToString(value);
            if (strValue.StartsWith("'"))
            {
                var nextIndex = strValue.IndexOf('\'', 1);
                if (nextIndex == strValue.Length - 1)
                    return true;
            }
            else if (strValue.StartsWith("\""))
            {
                var nextIndex = strValue.IndexOf('\"', 1);
                if (nextIndex == strValue.Length - 1)
                    return true;
            }

            return false;
        }

        public static bool IsDateTime(Object value)
        {
            if (value is DateTime)
                return true;

            if (IsNumber(value))
                return false;

            var strValue = Convert.ToString(value);
            if (strValue.StartsWith("["))
            {
                var nextIndex = strValue.IndexOf(']', 1);
                if (nextIndex == strValue.Length - 1)
                    return true;
            }

            return false;
        }

        public static bool IsInteger(Object value)
        {
            if (IsNumber(value))
            {
                var number = GetNumber(value);
                return Math.Abs(Math.Truncate(number) - number) < double.Epsilon;
            }

            return false;
        }

        public static bool IsEmptyOrSpace(Object value)
        {
            var strValue = Convert.ToString(value);
            return String.IsNullOrEmpty(strValue) || String.IsNullOrEmpty(strValue.Trim());
        }

        public static bool IsListOrDictionary(Object obj)
        {
            if (obj == null)
                return false;

            var type = obj.GetType();

            return IsListOrDictionary(type);
        }
        public static bool IsListOrDictionary(Type type)
        {
            var interfaces = type.GetInterfaces();

            if (interfaces.Any(x => x.Name == "IDictionary" || x.Name == "IDictionary`2" || x.Name == "IList" || x.Name == "IList`1"))
                return true;

            return false;
        }

        public static bool IsDateTimeNode(ExpressionNode node, Object value)
        {
            if ((node.ValueType == ValueTypes.DateTime) ||
                (node.ValueType == ValueTypes.Variable && value is DateTime) ||
                (node.ActionType == ActionTypes.Function && value is DateTime))
                return true;

            return false;
        }

        public static int Compare(ExpressionNode leftNode, Object leftValue, ExpressionNode rightNode, Object rightValue)
        {
            if (IsDateTimeNode(leftNode, leftValue) || IsDateTimeNode(rightNode, rightValue))
            {
                var leftDate = GetDateTime(leftValue);
                var rightDate = GetDateTime(rightValue);

                return leftDate.CompareTo(rightDate);
            }

            return Compare(leftValue, rightValue);
        }
    }
}