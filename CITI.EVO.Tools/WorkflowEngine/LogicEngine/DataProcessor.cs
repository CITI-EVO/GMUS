using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.WorkflowEngine.Configs;
using CITI.EVO.Tools.WorkflowEngine.LogicEngine.Interfaces;

namespace CITI.EVO.Tools.WorkflowEngine.LogicEngine
{
	public class DataProcessor : IDataProcessor
	{
		private static readonly Regex _funcRegex;

		static DataProcessor()
		{
			_funcRegex = new Regex(@"^(?<funcName>.*?)\((?<expression>.*?)\) as (?<outputName>.*?)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}


		private readonly IList<FieldInfoPair> _orderByFields;
		private readonly IList<FieldInfoPair> _groupByFields;
		private readonly IList<FieldInfoPair> _selectFields;

		public DataProcessor(ProcessorConfig config)
		{
			FilterByFields = config.FilterByFields.Select(n => n.Expression).ToList();
			OrderByFields = config.OrderByFields.Select(n => n.Expression).ToList();
			GroupByFields = config.GroupByFields.Select(n => n.Expression).ToList();
			SelectFields = config.SelectFields.Select(n => n.Expression).ToList();

			_orderByFields = GetFieldInfos(OrderByFields).ToList();
			_groupByFields = GetFieldInfos(GroupByFields).ToList();
			_selectFields = GetFieldInfos(SelectFields).ToList();

			var outputFields = _selectFields.Select(n => n.OutputName).ToList();
			OutputFields = outputFields.AsReadOnly();
		}

		public IList<String> FilterByFields { get; private set; }
		public IList<String> OrderByFields { get; private set; }
		public IList<String> GroupByFields { get; private set; }
		public IList<String> SelectFields { get; private set; }

		public IList<String> OutputFields { get; private set; }

		public IEnumerable<IDataItem> Load(IEnumerable<IDataItem> collection)
		{
			var query = ProcessFilterBy(collection);

			if (_groupByFields != null && _groupByFields.Count > 0)
			{
				var groupedQuery = (from n in query
									let m = GetFieldsValues(_groupByFields, n)
									group n by m into grp
									select grp);

				var orderedQuery = ProcessOrderBy(groupedQuery);

				foreach (var grp in orderedQuery)
				{
					var dataItem = ProcessSelect(grp);
					yield return dataItem;
				}
			}
			else
			{
				var orderedQuery = ProcessOrderBy(query);

				foreach (var item in orderedQuery)
				{
					var dataItem = ProcessSelect(item);
					yield return dataItem;
				}
			}
		}

		private IEnumerable<IDataItem> ProcessFilterBy(IEnumerable<IDataItem> collection)
		{
			if (FilterByFields != null && FilterByFields.Count > 0)
			{
				collection = (from n in collection
							  where FitsToWhereCongdition(FilterByFields, n)
							  select n);
			}

			return collection;
		}

		private IEnumerable<IGrouping<IComparableDataItem, IDataItem>> ProcessOrderBy(IEnumerable<IGrouping<IComparableDataItem, IDataItem>> collection)
		{
			if (_orderByFields != null && _orderByFields.Count > 0)
			{
				collection = (from n in collection
							  let m = GetComputedValues(_orderByFields, n)
							  orderby m
							  select n);
			}

			return collection;
		}
		private IEnumerable<IDataItem> ProcessOrderBy(IEnumerable<IDataItem> collection)
		{
			if (_orderByFields != null && _orderByFields.Count > 0)
			{
				collection = (from n in collection
							  let m = GetComputedValues(_orderByFields, n)
							  orderby m
							  select n);
			}

			return collection;
		}

		private IDataItem ProcessSelect(IGrouping<IComparableDataItem, IDataItem> grouping)
		{
			if (_selectFields != null && _selectFields.Count > 0)
			{
				var itemPair = GetComputedValues(_selectFields, grouping);
				return itemPair;
			}

			return grouping.Key;
		}
		private IDataItem ProcessSelect(IDataItem dataItem)
		{
			if (_selectFields != null && _selectFields.Count > 0)
			{
				var itemPair = GetComputedValues(_selectFields, dataItem);
				return itemPair;
			}

			return dataItem;
		}

		private IEnumerable<FieldInfoPair> GetFieldInfos(IEnumerable<String> expressions)
		{
			if (expressions == null)
				yield break;

			var index = 0;

			foreach (var funcExpression in expressions)
			{
				if (!_funcRegex.IsMatch(funcExpression))
				{
					var entity = new FieldInfoPair
					{
						OutputName = funcExpression
					};

					yield return entity;
				}
				else
				{
					var match = _funcRegex.Match(funcExpression);

					var funcName = match.Groups["funcName"].Value.Trim();
					var expression = match.Groups["expression"].Value.Trim();
					var outputName = match.Groups["outputName"].Value.Trim();

					if (String.IsNullOrWhiteSpace(outputName))
						outputName = String.Format("Field_{0}", ++index);

					var entity = new FieldInfoPair
					{
						FuncName = funcName,
						Expression = expression,
						OutputName = outputName
					};

					yield return entity;
				}
			}
		}

		private IDataItem GetComputedValues(IEnumerable<FieldInfoPair> fieldPairs, IGrouping<IComparableDataItem, IDataItem> grouping)
		{
			var groupingPair = new GroupingPair();

			foreach (var fieldPair in fieldPairs)
			{
				if (String.IsNullOrWhiteSpace(fieldPair.FuncName) && String.IsNullOrWhiteSpace(fieldPair.Expression))
					groupingPair[fieldPair.OutputName] = grouping.Key[fieldPair.OutputName];
				else
					groupingPair[fieldPair.OutputName] = GetExecFunc(fieldPair.FuncName, fieldPair.Expression, grouping);
			}

			return groupingPair;
		}
		private IDataItem GetComputedValues(IEnumerable<FieldInfoPair> fieldPairs, IDataItem dataItem)
		{
			var dataItemPair = new DataItemPair();

			foreach (var fieldPair in fieldPairs)
			{
				if (String.IsNullOrWhiteSpace(fieldPair.Expression))
					dataItemPair[fieldPair.OutputName] = dataItem[fieldPair.OutputName];
				else
					dataItemPair[fieldPair.OutputName] = GetExpressionsValue(fieldPair.Expression, dataItem);
			}

			return dataItemPair;
		}

		private IComparableDataItem GetFieldsValues(IEnumerable<FieldInfoPair> fieldPairs, IDataItem item)
		{
			var groupingPair = new GroupingPair();

			foreach (var fieldPair in fieldPairs)
			{
				var value = item.GetValueOrDefault(fieldPair.OutputName, item);
				groupingPair[fieldPair.OutputName] = value;
			}

			return groupingPair;
		}

		private Object GetExecFunc(String funcName, String expression, IGrouping<IDataItem, IDataItem> grouping)
		{
			switch (funcName)
			{
				case "All":
					{
						var values = GetExpressionsValues(expression, grouping);
						var result = values.All(n => DataConverter.ToNullableBool(n).GetValueOrDefault());

						return result;
					}
				case "Any":
					{
						var values = GetExpressionsValues(expression, grouping);
						var result = values.Any(n => DataConverter.ToNullableBool(n).GetValueOrDefault());

						return result;
					}
				case "Min":
					{
						var values = GetExpressionsValues(expression, grouping);
						var result = values.Min(n => DataConverter.ToNullableDecimal(n));

						return result;
					}
				case "Max":
					{
						var values = GetExpressionsValues(expression, grouping);
						var result = values.Max(n => DataConverter.ToNullableDecimal(n));

						return result;
					}
				case "Sum":
					{
						var values = GetExpressionsValues(expression, grouping);
						var result = values.Sum(n => DataConverter.ToNullableDecimal(n));

						return result;
					}
				case "Avg":
					{
						var values = GetExpressionsValues(expression, grouping);
						var result = values.Average(n => DataConverter.ToNullableDecimal(n));

						return result;
					}
				case "Count":
					return grouping.Count();
				default:
					throw new Exception();
			}
		}

		private bool FitsToWhereCongdition(IEnumerable<String> whereExpressions, IDataItem dataItem)
		{
			foreach (var expression in whereExpressions)
			{
				if (!FitsToWhereCongdition(expression, dataItem))
					return false;
			}

			return true;
		}
		private bool FitsToWhereCongdition(String expression, IDataItem dataItem)
		{
			var expValue = GetExpressionsValue(expression, dataItem);
			var @bool = DataConverter.ToNullableBool(expValue);

			return @bool.GetValueOrDefault();
		}

		private IEnumerable<Object> GetExpressionsValues(String expression, IEnumerable<IDataItem> dataItems)
		{
			foreach (var item in dataItems)
			{
				var expValue = GetExpressionsValue(expression, item);
				yield return expValue;
			}
		}
		private Object GetExpressionsValue(String expression, IDataItem item)
		{
			var expEval = new ExpressionEvaluator(item.GetValueOrDefault);
			var expValue = expEval.Eval(expression);

			return expValue;
		}
	}
}