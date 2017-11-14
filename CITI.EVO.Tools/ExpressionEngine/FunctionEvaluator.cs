using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CITI.EVO.Tools.ExpressionEngine
{
    internal static class FunctionEvaluator
    {
        private static readonly Regex dayRx;
        private static readonly Regex yearRx;
        private static readonly Regex monthRx;

        private static readonly ISet<String> lazyFuncs;

        static FunctionEvaluator()
        {
            dayRx = new Regex(@"^(0[1-9]|[12][0-9]|3[01])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            yearRx = new Regex(@"^(19|20)\d\d$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            monthRx = new Regex(@"^(0[1-9]|1[012])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            lazyFuncs = new HashSet<String>(StringComparer.OrdinalIgnoreCase) { "if", "any", "all", "switch", "first", "last", "sortasc", "sortdesc", "select" };
        }

        public static Object Eval(ExpressionNode node, IDataResolver dataResolver)
        {
            var @params = new List<Object>();

            foreach (var paramNode in node.Params)
            {
                if (paramNode != null)
                {
                    var value = (Object)null;

                    if (@params.Count == 0)
                        value = ExpressionEvaluator.Eval(paramNode, dataResolver);
                    else if (lazyFuncs.Contains(node.Action))
                        value = paramNode;
                    else
                        value = ExpressionEvaluator.Eval(paramNode, dataResolver);

                    @params.Add(value);
                }
            }

            switch (node.Action.ToLower())
            {
                case "sqrt":
                    return Math.Sqrt(ExpressionHelper.GetNumber(@params[0]));
                case "sin":
                    return Math.Sin(ExpressionHelper.GetNumber(@params[0]));
                case "cos":
                    return Math.Cos(ExpressionHelper.GetNumber(@params[0]));
                case "tan":
                    return Math.Tan(ExpressionHelper.GetNumber(@params[0]));
                case "asin":
                    return Math.Asin(ExpressionHelper.GetNumber(@params[0]));
                case "acos":
                    return Math.Acos(ExpressionHelper.GetNumber(@params[0]));
                case "atan":
                    return Math.Atan(ExpressionHelper.GetNumber(@params[0]));
                case "abs":
                    return Math.Abs(ExpressionHelper.GetNumber(@params[0]));
                case "atan2":
                    return Math.Atan2(ExpressionHelper.GetNumber(@params[0]), ExpressionHelper.GetNumber(@params[1]));
                case "ceil":
                    return Math.Ceiling(ExpressionHelper.GetNumber(@params[0]));
                case "cosh":
                    return Math.Cosh(ExpressionHelper.GetNumber(@params[0]));
                case "exp":
                    return Math.Exp(ExpressionHelper.GetNumber(@params[0]));
                case "floor":
                    return Math.Floor(ExpressionHelper.GetNumber(@params[0]));
                case "log":
                    return Math.Log(ExpressionHelper.GetNumber(@params[0]));
                case "log10":
                    return Math.Log10(ExpressionHelper.GetNumber(@params[0]));
                case "rnd":
                    {
                        var len = 8D;

                        if (@params.Count > 0)
                        {
                            len = ExpressionHelper.GetNumber(@params[0]);
                            len = (len < 1 ? 8D : len);
                        }

                        var max = Math.Pow(10, len) - 1D;
                        var rnd = new Random();

                        return rnd.Next(0, (int)max);
                    }
                case "round":
                    {
                        if (@params.Count > 1)
                            return Math.Round(ExpressionHelper.GetNumber(@params[0]), (int)ExpressionHelper.GetNumber(@params[1]));

                        return Math.Round(ExpressionHelper.GetNumber(@params[0]));
                    }
                case "sign":
                    return Math.Sign(ExpressionHelper.GetNumber(@params[0]));
                case "sinh":
                    return Math.Sinh(ExpressionHelper.GetNumber(@params[0]));
                case "tanh":
                    return Math.Tanh(ExpressionHelper.GetNumber(@params[0]));
                case "trunc":
                    return Math.Truncate(ExpressionHelper.GetNumber(@params[0]));
                case "pow":
                    return Math.Pow(ExpressionHelper.GetNumber(@params[0]), ExpressionHelper.GetNumber(@params[1]));
                case "or":
                    return (long)ExpressionHelper.GetNumber(@params[0]) | (long)ExpressionHelper.GetNumber(@params[1]);
                case "and":
                    return (long)ExpressionHelper.GetNumber(@params[0]) & (long)ExpressionHelper.GetNumber(@params[1]);
                case "xor":
                    return (long)ExpressionHelper.GetNumber(@params[0]) ^ (long)ExpressionHelper.GetNumber(@params[1]);
                case "mod":
                    return ExpressionHelper.GetNumber(@params[0]) % ExpressionHelper.GetNumber(@params[1]);
                case "len":
                    {
                        var val = @params[0];
                        if (val == null)
                            return 0;

                        if (val is ICollection)
                            return ((ICollection)val).Count;

                        var type = val.GetType();
                        if (ExpressionHelper.IsListOrDictionary(val))
                        {
                            var countProperty = type.GetProperty("Count");
                            if (countProperty == null)
                                return 0;

                            return countProperty.GetValue(val);
                        }

                        var strVal = Convert.ToString(val);
                        return strVal.Length;
                    }
                case "set":
                    {
                        var name = Convert.ToString(@params[0]);
                        dataResolver.SetValue(name, @params[1]);

                        return true;
                    }
                case "get":
                    {
                        var val = @params[0];
                        if (val == null)
                            return null;

                        if (val is IList)
                        {
                            var list = (IList)val;
                            return list[0];
                        }

                        var type = val.GetType();
                        if (ExpressionHelper.IsListOrDictionary(type))
                        {
                            var properties = type.GetProperties();

                            var indexerProperty = properties.FirstOrDefault(n => n.GetIndexParameters().Length > 0);
                            if (indexerProperty != null)
                            {
                                var idxParams = new[] { @params[1] };

                                var result = indexerProperty.GetValue(val, idxParams);
                                return result;
                            }
                        }

                        var idx = (int)ExpressionHelper.GetNumber(@params[1]);

                        var strVal = Convert.ToString(val);
                        return strVal[idx];
                    }
                case "join":
                    {
                        var val = @params[0];
                        if (val == null)
                            return null;

                        var coll = val as ICollection;
                        if (coll == null)
                            return val;

                        var separator = Convert.ToString(@params[1]);
                        return String.Join(separator, coll.OfType<Object>());
                    }
                case "concat":
                    {
                        var val = @params[0];
                        if (val == null)
                            return null;

                        var coll = val as ICollection;
                        if (coll == null)
                            return val;

                        return String.Concat(coll.OfType<Object>());
                    }
                case "format":
                    {
                        var format = Convert.ToString(@params[0]);
                        var args = new Object[@params.Count - 1];

                        @params.CopyTo(1, args, 0, args.Length);

                        return String.Format(format, args);
                    }
                case "substring":
                    {
                        var text = Convert.ToString(@params[0]);
                        var index = (int)ExpressionHelper.GetNumber(@params[1]);

                        var length = (@params.Count > 2 ? (int)ExpressionHelper.GetNumber(@params[2]) : text.Length);
                        length = Math.Min(length, text.Length - index);

                        return text.Substring(index, length);
                    }
                case "getdatetime":
                    {
                        var currDate = DateTime.Now;
                        if (@params.Count == 0)
                            return currDate;

                        var year = (int)ExpressionHelper.GetNumber(@params.Count > 0 ? @params[0] : currDate.Year);
                        var month = (int)ExpressionHelper.GetNumber(@params.Count > 1 ? @params[1] : currDate.Month);
                        var day = (int)ExpressionHelper.GetNumber(@params.Count > 2 ? @params[2] : currDate.Day);

                        var hour = (int)ExpressionHelper.GetNumber(@params.Count > 3 ? @params[3] : currDate.Hour);
                        var minute = (int)ExpressionHelper.GetNumber(@params.Count > 4 ? @params[4] : currDate.Minute);
                        var second = (int)ExpressionHelper.GetNumber(@params.Count > 5 ? @params[5] : currDate.Second);

                        var milisecond = (int)ExpressionHelper.GetNumber(@params.Count > 6 ? @params[6] : currDate.Millisecond);

                        return new DateTime(year, month, day, hour, minute, second, milisecond);
                    }
                case "getdate":
                    {
                        var currDate = DateTime.Now.Date;
                        if (@params.Count == 0)
                            return currDate;

                        var year = (int)ExpressionHelper.GetNumber(@params.Count > 0 ? @params[0] : currDate.Year);
                        var month = (int)ExpressionHelper.GetNumber(@params.Count > 1 ? @params[1] : currDate.Month);
                        var day = (int)ExpressionHelper.GetNumber(@params.Count > 2 ? @params[2] : currDate.Day);

                        return new DateTime(year, month, day);
                    }
                case "isempty":
                    {
                        var val = @params[0];
                        if (val == null)
                            return true;

                        if (val is ICollection)
                            return (((ICollection)val).Count == 0);

                        var type = val.GetType();
                        if (ExpressionHelper.IsListOrDictionary(val))
                        {
                            var countProperty = type.GetProperty("Count");
                            if (countProperty == null)
                                return true;

                            var count = (int)countProperty.GetValue(val);
                            return (count == 0);
                        }

                        var strVal = Convert.ToString(val);
                        return (strVal.Length == 0);
                    }
                case "isdate":
                    return ExpressionHelper.IsDateTime(@params[0]);
                case "isday":
                    {
                        var input = Convert.ToString(@params[0]);
                        return dayRx.IsMatch(input);
                    }
                case "ismonth":
                    {
                        var input = Convert.ToString(@params[0]);
                        return monthRx.IsMatch(input);
                    }
                case "isyear":
                    {
                        var input = Convert.ToString(@params[0]);
                        return yearRx.IsMatch(input);
                    }
                case "isnumber":
                    {
                        return ExpressionHelper.IsNumber(@params[0]);
                    }
                case "isinteger":
                    {
                        return ExpressionHelper.IsInteger(@params[0]);
                    }
                case "rgx":
                    {
                        var input = Convert.ToString(@params[0]);
                        var pattern = Convert.ToString(@params[1]);

                        return Regex.IsMatch(input, pattern);
                    }
                case "min":
                    {
                        if (!(@params[0] is String) && @params[0] is IEnumerable)
                        {
                            var val = (Object)null;

                            var list = (IEnumerable)@params[0];
                            foreach (var item in list)
                            {
                                if (item == null)
                                    continue;

                                var order = ExpressionHelper.Compare(val, item);
                                if (order < 0)
                                    val = item;
                            }

                            return val;
                        }

                        var query = (from n in @params
                                     let m = ExpressionHelper.GetNumber(n)
                                     select m);

                        return query.Min();
                    }
                case "max":
                    {
                        if (!(@params[0] is String) && @params[0] is IEnumerable)
                        {
                            var val = (Object)null;

                            var list = (IEnumerable)@params[0];
                            foreach (var item in list)
                            {
                                if (item == null)
                                    continue;

                                if (val == null)
                                    val = item;
                                else
                                {
                                    var order = ExpressionHelper.Compare(val, item);
                                    if (order > 0)
                                        val = item;
                                }
                            }

                            return val;
                        }

                        var query = (from n in @params
                                     let m = ExpressionHelper.GetNumber(n)
                                     select m);

                        return query.Max();
                    }
                case "avg":
                    {
                        if (!(@params[0] is String) && @params[0] is IEnumerable)
                        {
                            var val = 0D;
                            var count = 0;

                            var list = (IEnumerable)@params[0];
                            foreach (var item in list)
                            {
                                if (item == null)
                                    continue;

                                val += ExpressionHelper.GetNumber(item);
                                count++;
                            }

                            return val / count;
                        }

                        var query = (from n in @params
                                     let m = ExpressionHelper.GetNumber(n)
                                     select m);

                        return query.Average();
                    }
                case "sum":
                    {
                        if (!(@params[0] is String) && @params[0] is IEnumerable)
                        {
                            var val = 0D;

                            var list = (IEnumerable)@params[0];
                            foreach (var item in list)
                            {
                                if (item == null)
                                    continue;

                                val += ExpressionHelper.GetNumber(item);
                            }

                            return val;
                        }

                        var query = (from n in @params
                                     let m = ExpressionHelper.GetNumber(n)
                                     select m);

                        return query.Sum();
                    }
                case "all":
                    {
                        if (!(@params[0] is String) && @params[0] is IEnumerable)
                        {
                            var conditionNde = (ExpressionNode)@params[1];

                            var dictionary = new Dictionary<String, Object> { ["@"] = null };
                            var subResolver = new DefaultDataResolver(dictionary, dataResolver);

                            var list = (IEnumerable)@params[0];
                            foreach (var item in list)
                            {
                                subResolver.SetValue("@", item);

                                var res = ExpressionEvaluator.Eval(conditionNde, subResolver);
                                if (!Convert.ToBoolean(res))
                                    return false;
                            }

                            return true;
                        }

                        return false;
                    }
                case "any":
                    {
                        if (!(@params[0] is String) && @params[0] is IEnumerable)
                        {
                            var conditionNde = (ExpressionNode)@params[1];

                            var dictionary = new Dictionary<String, Object> { ["@"] = null };
                            var subResolver = new DefaultDataResolver(dictionary, dataResolver);

                            var list = (IEnumerable)@params[0];
                            foreach (var item in list)
                            {
                                subResolver.SetValue("@", item);

                                var res = ExpressionEvaluator.Eval(conditionNde, subResolver);
                                if (Convert.ToBoolean(res))
                                    return true;
                            }
                        }

                        return false;
                    }
                case "single":
                    {
                        if (!(@params[0] is String) && @params[0] is IEnumerable)
                        {

                            var selectorNode = (ExpressionNode)null;
                            if (@params.Count > 1)
                                selectorNode = (ExpressionNode)@params[1];

                            var conditionNode = (ExpressionNode)null;
                            if (@params.Count > 2)
                                conditionNode = (ExpressionNode)@params[2];

                            var dictionary = new Dictionary<String, Object> { ["@"] = null };
                            var subResolver = new DefaultDataResolver(dictionary, dataResolver);

                            var list = (IEnumerable)@params[0];

                            var flag = false;
                            var result = (object)null;

                            foreach (var item in list)
                            {
                                subResolver.SetValue("@", item);

                                if (flag)
                                    throw new Exception();

                                if (conditionNode != null)
                                {
                                    var res = ExpressionEvaluator.Eval(conditionNode, subResolver);
                                    if (!Convert.ToBoolean(res))
                                        continue;
                                }

                                if (selectorNode == null)
                                    result = item;
                                else
                                    result = ExpressionEvaluator.Eval(selectorNode, subResolver);

                                flag = true;
                            }

                            return result;
                        }

                        return null;
                    }
                case "first":
                    {
                        if (!(@params[0] is String) && @params[0] is IEnumerable)
                        {
                            var selectorNode = (ExpressionNode)null;
                            if (@params.Count > 1)
                                selectorNode = (ExpressionNode)@params[1];

                            var conditionNode = (ExpressionNode)null;
                            if (@params.Count > 2)
                                conditionNode = (ExpressionNode)@params[2];

                            var dictionary = new Dictionary<String, Object> { ["@"] = null };
                            var subResolver = new DefaultDataResolver(dictionary, dataResolver);

                            var list = (IEnumerable)@params[0];

                            foreach (var item in list)
                            {
                                subResolver.SetValue("@", item);

                                if (conditionNode != null)
                                {
                                    var res = ExpressionEvaluator.Eval(conditionNode, subResolver);
                                    if (!Convert.ToBoolean(res))
                                        continue;
                                }

                                if (selectorNode == null)
                                    return item;

                                var obj = ExpressionEvaluator.Eval(selectorNode, subResolver);
                                return obj;
                            }
                        }

                        return null;
                    }
                case "last":
                    {
                        var result = (Object)null;

                        if (!(@params[0] is String) && @params[0] is IEnumerable)
                        {
                            var selectorNode = (ExpressionNode)null;
                            if (@params.Count > 1)
                                selectorNode = (ExpressionNode)@params[1];

                            var conditionNode = (ExpressionNode)null;
                            if (@params.Count > 2)
                                conditionNode = (ExpressionNode)@params[2];

                            var dictionary = new Dictionary<String, Object> { ["@"] = null };
                            var subResolver = new DefaultDataResolver(dictionary, dataResolver);

                            var list = (IEnumerable)@params[0];

                            foreach (var item in list)
                            {
                                subResolver.SetValue("@", item);

                                if (conditionNode != null)
                                {
                                    var res = ExpressionEvaluator.Eval(conditionNode, subResolver);
                                    if (!Convert.ToBoolean(res))
                                        continue;
                                }

                                if (selectorNode == null)
                                {
                                    result = item;
                                    continue;
                                }

                                result = ExpressionEvaluator.Eval(selectorNode, subResolver);
                            }
                        }

                        return result;
                    }
                case "skip":
                    {
                        var result = new List<Object>();

                        if (!(@params[0] is String) && @params[0] is IEnumerable)
                        {
                            var list = (IEnumerable)@params[0];
                            var count = ExpressionHelper.GetNumber(@params[1]);

                            foreach (var item in list)
                            {
                                if (count <= 0D)
                                    result.Add(item);

                                count--;
                            }
                        }

                        return result.ToArray();
                    }
                case "take":
                    {
                        var result = new List<Object>();

                        if (!(@params[0] is String) && @params[0] is IEnumerable)
                        {
                            var list = (IEnumerable)@params[0];
                            var count = ExpressionHelper.GetNumber(@params[1]);

                            foreach (var item in list)
                            {
                                if (result.Count == count)
                                    break;

                                result.Add(item);
                            }
                        }

                        return result.ToArray();
                    }
                case "sortasc":
                    {
                        var items = new List<DictionaryEntry>();

                        if (!(@params[0] is String) && @params[0] is IEnumerable)
                        {
                            var selectorNode = (ExpressionNode)null;
                            if (@params.Count > 1)
                                selectorNode = (ExpressionNode)@params[1];

                            var dictionary = new Dictionary<String, Object> { ["@"] = null };
                            var subResolver = new DefaultDataResolver(dictionary, dataResolver);

                            var list = (IEnumerable)@params[0];

                            foreach (var item in list)
                            {
                                subResolver.SetValue("@", item);

                                var key = item;
                                if (selectorNode != null)
                                    key = ExpressionEvaluator.Eval(selectorNode, subResolver);

                                var entry = new DictionaryEntry(key, item);
                                items.Add(entry);
                            }
                        }

                        var ordered = items.OrderBy(n => n.Key, ExpressionHelper.Comparer);
                        var result = ordered.Select(n => n.Value);

                        return result.ToArray();
                    }
                case "sortdesc":
                    {
                        var items = new List<DictionaryEntry>();

                        if (!(@params[0] is String) && @params[0] is IEnumerable)
                        {
                            var selectorNode = (ExpressionNode)null;
                            if (@params.Count > 1)
                                selectorNode = (ExpressionNode)@params[1];

                            var dictionary = new Dictionary<String, Object> { ["@"] = null };
                            var subResolver = new DefaultDataResolver(dictionary, dataResolver);

                            var list = (IEnumerable)@params[0];

                            foreach (var item in list)
                            {
                                subResolver.SetValue("@", item);

                                var key = item;
                                if (selectorNode != null)
                                    key = ExpressionEvaluator.Eval(selectorNode, subResolver);

                                var entry = new DictionaryEntry(key, item);
                                items.Add(entry);
                            }
                        }
                        
                        var ordered = items.OrderByDescending(n => n.Key, ExpressionHelper.Comparer);
                        var result = ordered.Select(n => n.Value);

                        return result.ToArray();
                    }
                case "select":
                    {
                        var result = new List<Object>();

                        if (!(@params[0] is String) && @params[0] is IEnumerable)
                        {
                            var selectorNode = (ExpressionNode)null;
                            if (@params.Count > 1)
                                selectorNode = (ExpressionNode)@params[1];

                            var conditionNode = (ExpressionNode)null;
                            if (@params.Count > 2)
                                conditionNode = (ExpressionNode)@params[2];

                            var dictionary = new Dictionary<String, Object> { ["@"] = null };
                            var subResolver = new DefaultDataResolver(dictionary, dataResolver);

                            var list = (IEnumerable)@params[0];

                            foreach (var item in list)
                            {
                                subResolver.SetValue("@", item);

                                if (conditionNode != null)
                                {
                                    var res = ExpressionEvaluator.Eval(conditionNode, subResolver);
                                    if (!Convert.ToBoolean(res))
                                        continue;
                                }

                                if (selectorNode == null)
                                {
                                    result.Add(item);
                                    continue;
                                }

                                var obj = ExpressionEvaluator.Eval(selectorNode, subResolver);
                                result.Add(obj);
                            }
                        }

                        return result.ToArray();
                    }
                case "if":
                    {
                        var flag = Convert.ToBoolean(@params[0]);

                        var trueNode = (ExpressionNode)@params[1];
                        var falseNode = (ExpressionNode)@params[2];

                        if (flag)
                            return ExpressionEvaluator.Eval(trueNode, dataResolver);

                        return ExpressionEvaluator.Eval(falseNode, dataResolver);
                    }
                case "switch":
                    {
                        if (@params.Count % 2 > 0)
                            throw new ArgumentOutOfRangeException();

                        var switchValue = @params[0];

                        for (int i = 1; i < @params.Count; i += 2)
                        {
                            var caseValue = @params[i];
                            var resultNode = (ExpressionNode)@params[i + 1];

                            if (Equals(switchValue, caseValue))
                                return ExpressionEvaluator.Eval(resultNode, dataResolver);
                        }

                        var defNode = (ExpressionNode)@params[@params.Count - 1];
                        return ExpressionEvaluator.Eval(defNode, dataResolver);
                    }
                case "comp":
                    return ExpressionHelper.Compare(@params[0], @params[1]);
                case "lower":
                    return Convert.ToString(@params[0]).ToLower();
                case "upper":
                    return Convert.ToString(@params[0]).ToUpper();
                case "contains":
                    {
                        var collection = @params[0];
                        if (collection == null)
                            return null;

                        var item = @params[1];
                        if (item == null)
                            return null;

                        if (collection is IDictionary)
                        {
                            var dictionary = (IDictionary)collection;
                            return dictionary.Contains(item);
                        }

                        var type = collection.GetType();
                        if (type.IsGenericType)
                        {
                            var genericTypeDef = type.GetGenericTypeDefinition();
                            if (genericTypeDef == typeof(ISet<>) ||
                                genericTypeDef == typeof(IList<>) ||
                                genericTypeDef == typeof(IDictionary<,>))
                            {
                                var itemMethod = type.GetMethod("ContainsKey");
                                if (itemMethod == null)
                                    itemMethod = type.GetMethod("Contains");

                                return itemMethod.Invoke(collection, new[] { item });
                            }
                        }

                        if (collection is IEnumerable && !(collection is String))
                        {
                            var list = (IEnumerable)collection;

                            var query = (from n in list.OfType<Object>()
                                         where ExpressionHelper.Compare(n, item) == 0
                                         select n);

                            return query.Any();
                        }

                        var strValue = Convert.ToString(collection);
                        return strValue.Contains(Convert.ToString(item));
                    }
                case "startswith":
                    {
                        var strValue = Convert.ToString(@params[0]);
                        return strValue.StartsWith(Convert.ToString(@params[1]));
                    }
                case "endswith":
                    {
                        var strValue = Convert.ToString(@params[0]);
                        return strValue.EndsWith(Convert.ToString(@params[1]));
                    }
                case "split":
                    {
                        var result = new String[0];

                        if (@params.Count > 1)
                        {
                            var strValue = Convert.ToString(@params[0]);
                            var delimiter = Convert.ToString(@params[1]);

                            if (!ExpressionHelper.IsEmptyOrSpace(strValue) &&
                                !ExpressionHelper.IsEmptyOrSpace(delimiter))
                            {
                                var delimites = new[] { delimiter };
                                result = strValue.Split(delimites, StringSplitOptions.None);
                            }
                        }

                        return result;
                    }
                case "trim":
                    {
                        var strValue = Convert.ToString(@params[0]);
                        return strValue.Trim();
                    }
                case "ltrim":
                    {
                        var strValue = Convert.ToString(@params[0]);
                        return strValue.TrimStart();
                    }
                case "rtrim":
                    {
                        var strValue = Convert.ToString(@params[0]);
                        return strValue.TrimEnd();
                    }
                case "getdaysinmonth":
                    {
                        if (ExpressionHelper.IsDateTime(@params[0]))
                        {
                            var dateValue = ExpressionHelper.GetDateTime(@params[0]);
                            return DateTime.DaysInMonth(dateValue.Year, dateValue.Month);
                        }

                        var currDate = DateTime.Now.Date;

                        var year = (int)ExpressionHelper.GetNumber(@params.Count > 0 ? @params[0] : currDate.Year);
                        var month = (int)ExpressionHelper.GetNumber(@params.Count > 1 ? @params[1] : currDate.Month);

                        return DateTime.DaysInMonth(year, month);
                    }
                case "getyear":
                case "getyears":
                    {
                        if (@params.Count == 0)
                            return DateTime.Now.Year;

                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);
                        return dateValue.Year;
                    }
                case "getmonth":
                case "getmonths":
                    {
                        if (@params.Count == 0)
                            return DateTime.Now.Month;

                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);
                        return dateValue.Month;
                    }
                case "getday":
                case "getdays":
                case "getdayofmonth":
                    {
                        if (@params.Count == 0)
                            return DateTime.Now.Day;

                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);
                        return dateValue.Day;
                    }
                case "getdayofweek":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);
                        return dateValue.DayOfWeek;
                    }
                case "getdayofyear":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);
                        return dateValue.DayOfYear;
                    }
                case "gethours":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);
                        return dateValue.Hour;
                    }
                case "getminutes":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);
                        return dateValue.Minute;
                    }
                case "getseconds":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);
                        return dateValue.Second;
                    }
                case "gettotaldays":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);

                        var timeSpan = TimeSpan.FromTicks(dateValue.Ticks);
                        return timeSpan.TotalDays;
                    }
                case "gettotalhours":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);

                        var timeSpan = TimeSpan.FromTicks(dateValue.Ticks);
                        return timeSpan.TotalHours;
                    }
                case "gettotalminutes":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);

                        var timeSpan = TimeSpan.FromTicks(dateValue.Ticks);
                        return timeSpan.TotalMinutes;
                    }
                case "gettotalseconds":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);

                        var timeSpan = TimeSpan.FromTicks(dateValue.Ticks);
                        return timeSpan.TotalSeconds;
                    }
                case "gettotalmiliseconds":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);

                        var timeSpan = TimeSpan.FromTicks(dateValue.Ticks);
                        return timeSpan.TotalMilliseconds;
                    }
                case "addyears":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);
                        return dateValue.AddYears((int)ExpressionHelper.GetNumber(@params[1]));
                    }
                case "addmonths":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);
                        return dateValue.AddMonths((int)ExpressionHelper.GetNumber(@params[1]));
                    }
                case "adddays":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);
                        return dateValue.AddDays((int)ExpressionHelper.GetNumber(@params[1]));
                    }
                case "addhours":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);
                        return dateValue.AddHours((int)ExpressionHelper.GetNumber(@params[1]));
                    }
                case "addminutes":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);
                        return dateValue.AddMinutes((int)ExpressionHelper.GetNumber(@params[1]));
                    }
                case "addseconds":
                    {
                        var dateValue = ExpressionHelper.GetDateTime(@params[0]);
                        return dateValue.AddSeconds((int)ExpressionHelper.GetNumber(@params[1]));
                    }
                case "print":
                    {
                        foreach (var item in @params)
                            Console.WriteLine(item);
                    }
                    break;
            }

            var message = $"Unknown function '{node}', check function name or count of parameters";
            throw new Exception(message);
        }
    }
}