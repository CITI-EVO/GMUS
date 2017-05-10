using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CITI.EVO.Tools.ExpressionEngine
{
    internal static class FunctionEvaluator
    {
        private static readonly Regex dayRx = new Regex(@"^(0[1-9]|[12][0-9]|3[01])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex yearRx = new Regex(@"^(19|20)\d\d$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex monthRx = new Regex(@"^(0[1-9]|1[012])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static Object Eval(ExpressionNode node, Func<String, Object> varResolver)
        {
            var args = new List<Object>();

            foreach (var paramNode in node.Params)
            {
                if (paramNode != null)
                {
                    var value = (Object)null;

                    if (args.Count == 0)
                        value = ExpressionEvaluator.Eval(paramNode, varResolver);
                    else
                    {
                        switch (node.Action.ToLower())
                        {
                            case "if":
                            case "any":
                            case "all":
                            case "switch":
                                value = paramNode;
                                break;
                            default:
                                value = ExpressionEvaluator.Eval(paramNode, varResolver);
                                break;
                        }
                    }

                    args.Add(value);
                }
            }

            switch (node.Action.ToLower())
            {
                case "sqrt":
                    return Math.Sqrt(ExpressionHelper.GetNumber(args[0]));
                case "sin":
                    return Math.Sin(ExpressionHelper.GetNumber(args[0]));
                case "cos":
                    return Math.Cos(ExpressionHelper.GetNumber(args[0]));
                case "tan":
                    return Math.Tan(ExpressionHelper.GetNumber(args[0]));
                case "asin":
                    return Math.Asin(ExpressionHelper.GetNumber(args[0]));
                case "acos":
                    return Math.Acos(ExpressionHelper.GetNumber(args[0]));
                case "atan":
                    return Math.Atan(ExpressionHelper.GetNumber(args[0]));
                case "abs":
                    return Math.Abs(ExpressionHelper.GetNumber(args[0]));
                case "atan2":
                    return Math.Atan2(ExpressionHelper.GetNumber(args[0]), ExpressionHelper.GetNumber(args[1]));
                case "ceil":
                    return Math.Ceiling(ExpressionHelper.GetNumber(args[0]));
                case "cosh":
                    return Math.Cosh(ExpressionHelper.GetNumber(args[0]));
                case "exp":
                    return Math.Exp(ExpressionHelper.GetNumber(args[0]));
                case "floor":
                    return Math.Floor(ExpressionHelper.GetNumber(args[0]));
                case "log":
                    return Math.Log(ExpressionHelper.GetNumber(args[0]));
                case "log10":
                    return Math.Log10(ExpressionHelper.GetNumber(args[0]));
                case "round":
                    {
                        if (args.Count > 1)
                            return Math.Round(ExpressionHelper.GetNumber(args[0]), (int)ExpressionHelper.GetNumber(args[1]));

                        return Math.Round(ExpressionHelper.GetNumber(args[0]));
                    }
                case "sign":
                    return Math.Sign(ExpressionHelper.GetNumber(args[0]));
                case "sinh":
                    return Math.Sinh(ExpressionHelper.GetNumber(args[0]));
                case "tanh":
                    return Math.Tanh(ExpressionHelper.GetNumber(args[0]));
                case "trunc":
                    return Math.Truncate(ExpressionHelper.GetNumber(args[0]));
                case "pow":
                    return Math.Pow(ExpressionHelper.GetNumber(args[0]), ExpressionHelper.GetNumber(args[1]));
                case "or":
                    return Convert.ToInt64(args[0]) | Convert.ToInt64(args[1]);
                case "and":
                    return Convert.ToInt64(args[0]) & Convert.ToInt64(args[1]);
                case "xor":
                    return Convert.ToInt64(args[0]) ^ Convert.ToInt64(args[1]);
                case "mod":
                    return ExpressionHelper.GetNumber(args[0]) % ExpressionHelper.GetNumber(args[1]);
                case "len":
                    {
                        if (args[0] == null)
                            return 0;

                        var val = args[0];
                        if (val is ICollection)
                        {
                            var collection = (ICollection)val;
                            return collection.Count;
                        }

                        var type = val.GetType();
                        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>))
                        {
                            var countProp = type.GetProperty("Count");
                            return countProp.GetValue(val);
                        }

                        var strVal = Convert.ToString(val);
                        return strVal.Length;
                    }
                case "get":
                    {
                        var val = args[0];
                        if (val == null)
                            return null;

                        if (val is IList)
                        {
                            var list = (IList)val;
                            return list[0];
                        }

                        var type = val.GetType();
                        if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IList<>) || type.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
                        {
                            var itemProp = type.GetProperty("get_Item");
                            return itemProp.GetValue(val, new[] { args[1] });
                        }

                        var idx = (int)ExpressionHelper.GetNumber(args[1]);
                        var strVal = Convert.ToString(val);
                        return strVal[idx];
                    }
                case "substring":
                    {
                        var text = Convert.ToString(args[0]);
                        var index = (int)ExpressionHelper.GetNumber(args[1]);

                        var length = (args.Count > 2 ? (int)ExpressionHelper.GetNumber(args[2]) : text.Length);
                        length = Math.Min(length, text.Length - index);

                        return text.Substring(index, length);
                    }
                case "getdatetime":
                    {
                        var currDate = DateTime.Now;
                        if (args.Count == 0)
                            return currDate;

                        var year = (int)ExpressionHelper.GetNumber(args.Count > 0 ? args[0] : currDate.Year);
                        var month = (int)ExpressionHelper.GetNumber(args.Count > 1 ? args[1] : currDate.Month);
                        var day = (int)ExpressionHelper.GetNumber(args.Count > 2 ? args[2] : currDate.Day);

                        var hour = (int)ExpressionHelper.GetNumber(args.Count > 3 ? args[3] : currDate.Hour);
                        var minute = (int)ExpressionHelper.GetNumber(args.Count > 4 ? args[4] : currDate.Minute);
                        var second = (int)ExpressionHelper.GetNumber(args.Count > 5 ? args[5] : currDate.Second);

                        var milisecond = (int)ExpressionHelper.GetNumber(args.Count > 6 ? args[6] : currDate.Millisecond);

                        return new DateTime(year, month, day, hour, minute, second, milisecond);
                    }
                case "getdate":
                    {
                        var currDate = DateTime.Now.Date;
                        if (args.Count == 0)
                            return currDate;

                        var year = (int)ExpressionHelper.GetNumber(args.Count > 0 ? args[0] : currDate.Year);
                        var month = (int)ExpressionHelper.GetNumber(args.Count > 1 ? args[1] : currDate.Month);
                        var day = (int)ExpressionHelper.GetNumber(args.Count > 2 ? args[2] : currDate.Day);

                        return new DateTime(year, month, day);
                    }
                case "getlength":
                    return (args[0] != null ? args[0].ToString().Trim().Length : 0);
                case "isempty":
                    return ExpressionHelper.IsEmptyOrSpace(args[0]);
                case "isdate":
                    return ExpressionHelper.IsDateTime(args[0]);
                case "isday":
                    {
                        var input = Convert.ToString(args[0]);
                        return dayRx.IsMatch(input);
                    }
                case "ismonth":
                    {
                        var input = Convert.ToString(args[0]);
                        return monthRx.IsMatch(input);
                    }
                case "isyear":
                    {
                        var input = Convert.ToString(args[0]);
                        return yearRx.IsMatch(input);
                    }
                case "isnumber":
                    {
                        return ExpressionHelper.IsNumber(args[0]);
                    }
                case "isinteger":
                    {
                        return ExpressionHelper.IsInteger(args[0]);
                    }
                case "rgx":
                    {
                        var input = Convert.ToString(args[0]);
                        var pattern = Convert.ToString(args[1]);

                        return Regex.IsMatch(input, pattern);
                    }
                case "min":
                    {
                        if (!(args[0] is String) && args[0] is IEnumerable)
                        {
                            var val = 0D;

                            var list = (IEnumerable)args[0];
                            foreach (var item in list)
                            {
                                if (item == null)
                                    continue;

                                val = Math.Min(val, ExpressionHelper.GetNumber(item));
                            }

                            return val;
                        }

                        return Math.Min(ExpressionHelper.GetNumber(args[0]), ExpressionHelper.GetNumber(args[1]));
                    }
                case "max":
                    {
                        if (!(args[0] is String) && args[0] is IEnumerable)
                        {
                            var val = (double?)null;

                            var list = (IEnumerable)args[0];
                            foreach (var item in list)
                            {
                                if (item == null)
                                    continue;

                                if (val == null)
                                    val = ExpressionHelper.GetNumber(item);
                                else
                                    val = Math.Max(val.Value, ExpressionHelper.GetNumber(item));
                            }

                            return val;
                        }

                        return Math.Max(ExpressionHelper.GetNumber(args[0]), ExpressionHelper.GetNumber(args[1]));
                    }
                case "avg":
                    {
                        if (!(args[0] is String) && args[0] is IEnumerable)
                        {
                            var val = 0D;
                            var count = 0;

                            var list = (IEnumerable)args[0];
                            foreach (var item in list)
                            {
                                if (item == null)
                                    continue;

                                val += ExpressionHelper.GetNumber(item);
                                count++;
                            }

                            return val / count;
                        }

                        return (ExpressionHelper.GetNumber(args[0]) + ExpressionHelper.GetNumber(args[1])) / 2D;
                    }
                case "sum":
                    {
                        if (!(args[0] is String) && args[0] is IEnumerable)
                        {
                            var val = 0D;

                            var list = (IEnumerable)args[0];
                            foreach (var item in list)
                            {
                                if (item == null)
                                    continue;

                                val += ExpressionHelper.GetNumber(item);
                            }

                            return val;
                        }

                        return ExpressionHelper.GetNumber(args[0]) + ExpressionHelper.GetNumber(args[1]);
                    }
                case "all":
                    {
                        if (!(args[0] is String) && args[0] is IEnumerable)
                        {
                            var conditionNde = (ExpressionNode)args[1];

                            var list = (IEnumerable)args[0];
                            foreach (var item in list)
                            {
                                var res = ExpressionEvaluator.Eval(conditionNde, n => (n == "@" ? item : varResolver(n)));

                                if (!Convert.ToBoolean(res))
                                    return false;
                            }

                            return true;
                        }

                        return false;
                    }
                case "any":
                    {
                        if (!(args[0] is String) && args[0] is IEnumerable)
                        {
                            var conditionNde = (ExpressionNode)args[1];

                            var list = (IEnumerable)args[0];
                            foreach (var item in list)
                            {
                                var res = ExpressionEvaluator.Eval(conditionNde, n => (n == "@" ? item : varResolver(n)));

                                if (Convert.ToBoolean(res))
                                    return true;
                            }
                        }

                        return false;
                    }
                case "if":
                    {
                        var flag = Convert.ToBoolean(args[0]);

                        var trueNode = (ExpressionNode)args[1];
                        var falseNode = (ExpressionNode)args[2];

                        if (flag)
                            return ExpressionEvaluator.Eval(trueNode, varResolver);

                        return ExpressionEvaluator.Eval(falseNode, varResolver);
                    }
                case "switch":
                    {
                        if (args.Count % 2 > 0)
                            throw new ArgumentOutOfRangeException();

                        var switchValue = args[0];

                        for (int i = 1; i < args.Count; i += 2)
                        {
                            var caseValue = args[i];
                            var resultNode = (ExpressionNode)args[i + 1];

                            if (Equals(switchValue, caseValue))
                                return ExpressionEvaluator.Eval(resultNode, varResolver);
                        }

                        var defNode = (ExpressionNode)args[args.Count - 1];
                        return ExpressionEvaluator.Eval(defNode, varResolver);
                    }
                case "comp":
                    return Comparer.DefaultInvariant.Compare(args[0], args[1]);
                case "lower":
                    return Convert.ToString(args[0]).ToLower();
                case "upper":
                    return Convert.ToString(args[0]).ToUpper();
                case "contains":
                    {
                        var val = args[0];
                        if (val == null)
                            return null;

                        if (val is IList)
                        {
                            var list = (IList)val;
                            return list.Contains(args[1]);
                        }

                        var type = val.GetType();
                        if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IList<>) || type.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
                        {
                            var itemProp = type.GetProperty("get_Item");
                            return itemProp.GetValue(val, new[] { args[1] });
                        }

                        var strValue = Convert.ToString(args[0]);
                        return strValue.Contains(Convert.ToString(args[1]));
                    }
                case "startswith":
                    {
                        var strValue = Convert.ToString(args[0]);
                        return strValue.StartsWith(Convert.ToString(args[1]));
                    }
                case "endswith":
                    {
                        var strValue = Convert.ToString(args[0]);
                        return strValue.EndsWith(Convert.ToString(args[1]));
                    }
                case "trim":
                    {
                        var strValue = Convert.ToString(args[0]);
                        return strValue.Trim();
                    }
                case "ltrim":
                    {
                        var strValue = Convert.ToString(args[0]);
                        return strValue.TrimStart();
                    }
                case "rtrim":
                    {
                        var strValue = Convert.ToString(args[0]);
                        return strValue.TrimEnd();
                    }
                case "getdaysinmonth":
                    {
                        if (ExpressionHelper.IsDateTime(args[0]))
                        {
                            var dateValue = ExpressionHelper.GetDateTime(args[0]);
                            return DateTime.DaysInMonth(dateValue.Year, dateValue.Month);
                        }

                        var currDate = DateTime.Now.Date;

                        var year = (int)ExpressionHelper.GetNumber(args.Count > 0 ? args[0] : currDate.Year);
                        var month = (int)ExpressionHelper.GetNumber(args.Count > 1 ? args[1] : currDate.Month);

                        return DateTime.DaysInMonth(year, month);
                    }
                case "getyear":
                case "getyears":
                    {
                        if (args.Count == 0)
                            return DateTime.Now.Year;

                        var dateValue = ExpressionHelper.GetDateTime(args[0]);
                        return dateValue.Year;
                    }
                case "getmonth":
                case "getmonths":
                    {
                        if (args.Count == 0)
                            return DateTime.Now.Month;

                        var dateValue = ExpressionHelper.GetDateTime(args[0]);
                        return dateValue.Month;
                    }
                case "getday":
                case "getdays":
                case "getdayofmonth":
                    {
                        if (args.Count == 0)
                            return DateTime.Now.Day;

                        var dateValue = ExpressionHelper.GetDateTime(args[0]);
                        return dateValue.Day;
                    }
                case "getdayofweek":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(args[0]);
                        return dateValue.DayOfWeek;
                    }
                case "getdayofyear":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(args[0]);
                        return dateValue.DayOfYear;
                    }
                case "gethours":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(args[0]);
                        return dateValue.Hour;
                    }
                case "getminutes":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(args[0]);
                        return dateValue.Minute;
                    }
                case "getseconds":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(args[0]);
                        return dateValue.Second;
                    }
                case "addyears":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(args[0]);
                        return dateValue.AddYears((int)ExpressionHelper.GetNumber(args[1]));
                    }
                case "addmonths":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(args[0]);
                        return dateValue.AddMonths((int)ExpressionHelper.GetNumber(args[1]));
                    }
                case "adddays":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(args[0]);
                        return dateValue.AddDays((int)ExpressionHelper.GetNumber(args[1]));
                    }
                case "addhours":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(args[0]);
                        return dateValue.AddHours((int)ExpressionHelper.GetNumber(args[1]));
                    }
                case "addminutes":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(args[0]);
                        return dateValue.AddMinutes((int)ExpressionHelper.GetNumber(args[1]));
                    }
                case "addseconds":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(args[0]);
                        return dateValue.AddSeconds((int)ExpressionHelper.GetNumber(args[1]));
                    }
                case "print":
                    {
                        foreach (var item in args)
                            Console.WriteLine(item);
                    }
                    break;
            }

            return null;
        }
    }
}