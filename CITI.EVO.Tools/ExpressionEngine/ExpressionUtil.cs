using System;
using CITI.EVO.Tools.Collections;

namespace CITI.EVO.Tools.ExpressionEngine
{
	public class ExpressionUtil
	{
		#region common

		public Object Evaluate(String expression, NameValueDataList dataList)
		{
			var result = ExpressionEvaluator.Eval(expression, n => dataList[n]);
			return result;
		}

		#endregion

		#region Static Methods

		public static double EvaluateMath(String expression)
		{
			return EvaluateMath(expression, null);
		}
		public static double EvaluateMath(String expression, NameValueDataList dataList)
		{
			var evaluator = new ExpressionUtil();

			var value = evaluator.Evaluate(expression, dataList);
			return Convert.ToDouble(value);
		}

		public static bool EvaluateLogic(String expression)
		{
			return EvaluateLogic(expression, null);
		}
		public static bool EvaluateLogic(String expression, NameValueDataList dataList)
		{
			var evaluator = new ExpressionUtil();

			var results = evaluator.Evaluate(expression, dataList);
			return Convert.ToBoolean(results);
		}

		public static string EvaluateText(String expression)
		{
			return EvaluateText(expression, null);
		}
		public static String EvaluateText(String expression, NameValueDataList dataList)
		{
			var evaluator = new ExpressionUtil();

			var value = evaluator.Evaluate(expression, dataList);
			return Convert.ToString(value);
		}

		public static DateTime EvaluateDate(String expression)
		{
			return EvaluateDate(expression, null);
		}
		public static DateTime EvaluateDate(String expression, NameValueDataList dataList)
		{
			var evaluator = new ExpressionUtil();

			var value = evaluator.Evaluate(expression, dataList);
			return Convert.ToDateTime(value);
		}

		#endregion
	}
}