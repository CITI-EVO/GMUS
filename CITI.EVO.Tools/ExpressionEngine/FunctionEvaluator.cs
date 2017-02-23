using System;
using System.Collections;
using System.Collections.Generic;
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
					var value = ExpressionEvaluator.Eval(paramNode, varResolver);

					//value = ExpressionHelper.GetAny(value);

					args.Add(value);
				}
			}

			switch (node.Action.ToLower())
			{
				case "sqrt":
						return Math.Sqrt(Convert.ToDouble(args[0]));
				case "sin":
						return Math.Sin(Convert.ToDouble(args[0]));
				case "cos":
						return Math.Cos(Convert.ToDouble(args[0]));
				case "tan":
						return Math.Tan(Convert.ToDouble(args[0]));
				case "asin":
						return Math.Asin(Convert.ToDouble(args[0]));
				case "acos":
						return Math.Acos(Convert.ToDouble(args[0]));
				case "atan":
						return Math.Atan(Convert.ToDouble(args[0]));
				case "abs":
						return Math.Abs(Convert.ToDouble(args[0]));
				case "atan2":
						return Math.Atan2(Convert.ToDouble(args[0]), Convert.ToDouble(args[1]));
				case "ceil":
						return Math.Ceiling(Convert.ToDouble(args[0]));
				case "cosh":
						return Math.Cosh(Convert.ToDouble(args[0]));
				case "exp":
						return Math.Exp(Convert.ToDouble(args[0]));
				case "floor":
						return Math.Floor(Convert.ToDouble(args[0]));
				case "log":
						return Math.Log(Convert.ToDouble(args[0]));
				case "log10":
						return Math.Log10(Convert.ToDouble(args[0]));
				case "round":
					{
						if (args.Count > 1)
							return Math.Round(Convert.ToDouble(args[0]), Convert.ToInt32(args[1]));

						return Math.Round(Convert.ToDouble(args[0]));
					}
				case "sign":
						return Math.Sign(Convert.ToDouble(args[0]));
				case "sinh":
						return Math.Sinh(Convert.ToDouble(args[0]));
				case "tanh":
						return Math.Tanh(Convert.ToDouble(args[0]));
				case "trunc":
						return Math.Truncate(Convert.ToDouble(args[0]));
				case "pow":
						return Math.Pow(Convert.ToDouble(args[0]), Convert.ToDouble(args[1]));
				case "xor":
						return Convert.ToInt64(args[0]) ^ Convert.ToInt64(args[1]);
				case "mod":
						return Convert.ToDouble(args[0]) % Convert.ToDouble(args[1]);
				case "substring":
					{
						var text = Convert.ToString(args[0]);
						var index = Convert.ToInt32(args[1]);

						var length = (args.Count > 2 ? Convert.ToInt32(args[2]) : text.Length);
						length = Math.Min(length, text.Length - index);

						return text.Substring(index, length);
					}
				case "getdatetime":
					{
						var currDate = DateTime.Now;
						if (args.Count == 0)
							return currDate;

						var year = Convert.ToInt32(args.Count > 0 ? args[0] : currDate.Year);
						var month = Convert.ToInt32(args.Count > 1 ? args[1] : currDate.Month);
						var day = Convert.ToInt32(args.Count > 2 ? args[2] : currDate.Day);

						var hour = Convert.ToInt32(args.Count > 3 ? args[3] : currDate.Hour);
						var minute = Convert.ToInt32(args.Count > 4 ? args[4] : currDate.Minute);
						var second = Convert.ToInt32(args.Count > 5 ? args[5] : currDate.Second);

						var milisecond = Convert.ToInt32(args.Count > 6 ? args[6] : currDate.Millisecond);

						return new DateTime(year, month, day, hour, minute, second, milisecond);
					}
				case "getdate":
					{
						var currDate = DateTime.Now.Date;
						if (args.Count == 0)
							return currDate;

						var year = Convert.ToInt32(args.Count > 0 ? args[0] : currDate.Year);
						var month = Convert.ToInt32(args.Count > 1 ? args[1] : currDate.Month);
						var day = Convert.ToInt32(args.Count > 2 ? args[2] : currDate.Day);

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
						return Math.Min(Convert.ToDouble(args[0]), Convert.ToDouble(args[1]));
				case "max":
						return Math.Max(Convert.ToDouble(args[0]), Convert.ToDouble(args[1]));
				case "if":
					{
						var flag = Convert.ToBoolean(args[0]);
						return (flag ? args[1] : args[2]);
					}
				case "switch":
					{
						if (args.Count % 2 > 0)
							throw new ArgumentOutOfRangeException();

						var switchValue = args[0];

						for (int i = 1; i < args.Count; i += 2)
						{
							var caseValue = args[i];
							var result = args[i + 1];

							if (Equals(switchValue, caseValue))
								return result;
						}

						return args[args.Count - 1];
					}
				case "comp":
						return Comparer.DefaultInvariant.Compare(args[0], args[1]);
				case "lower":
						return Convert.ToString(args[0]).ToLower();
				case "upper":
						return Convert.ToString(args[0]).ToUpper();
				case "contains":
					{
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
						else
						{
							var currDate = DateTime.Now.Date;

							var year = Convert.ToInt32(args.Count > 0 ? args[0] : currDate.Year);
							var month = Convert.ToInt32(args.Count > 1 ? args[1] : currDate.Month);

							return DateTime.DaysInMonth(year, month);
						}
					}
				case "getyears":
					{
						if (args.Count == 0)
							return DateTime.Now.Year;

						var dateValue = ExpressionHelper.GetDateTime(args[0]);
						return dateValue.Year;
					}
				case "getmonths":
					{
						if (args.Count == 0)
							return DateTime.Now.Month;

						var dateValue = ExpressionHelper.GetDateTime(args[0]);
						return dateValue.Month;
					}
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
						return dateValue.AddYears(Convert.ToInt32(args[1]));
					}
				case "addmonths":
					{
						var dateValue = ExpressionHelper.GetDateTime(args[0]);
						return dateValue.AddMonths(Convert.ToInt32(args[1]));
					}
				case "adddays":
					{
						var dateValue = ExpressionHelper.GetDateTime(args[0]);
						return dateValue.AddDays(Convert.ToInt32(args[1]));
					}
				case "addhours":
					{
						var dateValue = ExpressionHelper.GetDateTime(args[0]);
						return dateValue.AddHours(Convert.ToInt32(args[1]));
					}
				case "addminutes":
					{
						var dateValue = ExpressionHelper.GetDateTime(args[0]);
						return dateValue.AddMinutes(Convert.ToInt32(args[1]));
					}
				case "addseconds":
					{
						var dateValue = ExpressionHelper.GetDateTime(args[0]);
						return dateValue.AddSeconds(Convert.ToInt32(args[1]));
					}
				case "print":
					{
						foreach (var arg in args)
							Console.WriteLine(arg);
					}
					break;
			}

			return null;
		}
	}
}