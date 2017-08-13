using System;
using System.Collections.Generic;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Models.Common;

namespace Gms.Portal.Web.Helpers
{
    /// <summary>
    /// Summary description for LogicInvoker
    /// </summary>
    public class LogicInvoker
    {
        private readonly LogicModel _model;
        private readonly List<ExpressionModel> _filterBy;
        private readonly List<ExpressionModel> _groupBy;
        private readonly List<ExpressionModel> _orderBy;
        private readonly List<NamedExpressionModel> _select;

        public LogicInvoker(LogicModel model)
        {
            _model = model;
            var expressionsLogic = _model.ExpressionsLogic;
            if (expressionsLogic != null)
            {
                var filterBy = expressionsLogic.FilterBy;
                if (filterBy != null && filterBy.Expressions != null && filterBy.Expressions.Count > 0)
                    _filterBy = filterBy.Expressions;

                var groupBy = expressionsLogic.GroupBy;
                if (groupBy != null && groupBy.Expressions != null && groupBy.Expressions.Count > 0)
                    _groupBy = groupBy.Expressions;

                var orderBy = expressionsLogic.OrderBy;
                if (orderBy != null && orderBy.Expressions != null && orderBy.Expressions.Count > 0)
                    _orderBy = orderBy.Expressions;

                var select = expressionsLogic.Select;
                if (select != null && select.Expressions != null && select.Expressions.Count > 0)
                    _select = select.Expressions;
            }
        }

        public IEnumerable<IDictionary<String, Object>> Invoke()
        {
            if (_model.SourceType == "Logic")
            {
                var source = GetOtherLogicData(_model.SourceID);
                return Invoke(source);
            }

            if (_model.SourceType == "Database")
            {
                var source = GetDatabaseData(_model.SourceID);
                return Invoke(source);
            }

            throw new Exception("Unknown logic source");
        }

        public IEnumerable<IDictionary<String, Object>> Invoke(IEnumerable<IDictionary<String, Object>> source)
        {
            var result = source;

            var expressionsLogic = _model.ExpressionsLogic;
            if (expressionsLogic == null)
                return result;

            if (_filterBy != null)
                result = ProcessFilterBy(result);

            if (_groupBy != null)
                result = ProcessGroupBy(result);

            if (_orderBy != null)
                result = ProcessOrderBy(result);

            if (_select != null)
                result = ProcessSelect(result);

            return result;
        }

        private IEnumerable<IDictionary<String, Object>> ProcessFilterBy(IEnumerable<IDictionary<String, Object>> source)
        {
            foreach (var item in source)
            {
                if (FitsToCondition(item))
                    yield return item;
            }
        }

        private IEnumerable<IDictionary<String, Object>> ProcessGroupBy(IEnumerable<IDictionary<String, Object>> source)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<IDictionary<String, Object>> ProcessOrderBy(IEnumerable<IDictionary<String, Object>> source)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<IDictionary<String, Object>> ProcessSelect(IEnumerable<IDictionary<String, Object>> source)
        {
            throw new NotImplementedException();
        }

        private bool FitsToCondition(IDictionary<String, Object> item, ExpressionModel model)
        {
            throw new NotImplementedException();
        }

        private bool FitsToCondition(IDictionary<String, Object> item)
        {
            foreach (var model in _filterBy)
            {
                if (!FitsToCondition(item, model))
                    return false;
            }

            return true;
        }

        private IEnumerable<IDictionary<String, Object>> GetDatabaseData(String sourceID)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<IDictionary<String, Object>> GetOtherLogicData(String sourceID)
        {
            throw new NotImplementedException();
        }


    }
}