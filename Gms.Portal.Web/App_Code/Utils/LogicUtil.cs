using System.Collections.Generic;
using System.Linq;
using Gms.Portal.Web.Entities.LogicStructure;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Models.Common;

namespace Gms.Portal.Web.Utils
{
    public static class LogicUtil
    {
        public static LogicModel ConvertToModel(LogicEntity entity)
        {
            var model = new LogicModel();

            if (entity.Logic is LogicQueryEntity)
            {
                var queryEntity = (LogicQueryEntity)entity.Logic;
                model.Query = queryEntity.Text;
            }
            else if (entity.Logic is LogicExpressionsEntity)
            {
                var expressionsLogic = (LogicExpressionsEntity)entity.Logic;

                var espressionsEntity = new ExpressionsLogicModel
                {
                    FilterBy = ConvertToEntity(expressionsLogic.FilterBy),
                    OrderBy = ConvertToEntity(expressionsLogic.OrderBy),
                    GroupBy = ConvertToEntity(expressionsLogic.GroupBy),
                    Select = ConvertToEntity(expressionsLogic.Select),
                };

                model.ExpressionsLogic = espressionsEntity;
            }

            return model;
        }

        private static ExpressionsListModel ConvertToEntity(IEnumerable<ExpressionEntity> entities)
        {
            if (entities == null)
                return null;

            var listModel = new ExpressionsListModel();
            listModel.Expressions = new List<ExpressionModel>();

            foreach (var entity in entities)
            {
                if (entity == null)
                    continue;

                var model = new ExpressionModel
                {
                    Key = entity.ID,
                    Expression = entity.Expression,
                    OutputType = entity.OutputType,
                };

                listModel.Expressions.Add(model);
            }

            return listModel;
        }

        private static NamedExpressionsListModel ConvertToEntity(IEnumerable<NamedExpressionEntity> entities)
        {
            if (entities == null)
                return null;

            var listModel = new NamedExpressionsListModel();
            listModel.Expressions = new List<NamedExpressionModel>();

            foreach (var entity in entities)
            {
                if (entity == null)
                    continue;

                var model = new NamedExpressionModel
                {
                    Key = entity.ID,
                    Name = entity.Name,
                    Expression = entity.Expression,
                    OutputType = entity.OutputType,
                };

                listModel.Expressions.Add(model);
            }

            return listModel;
        }

        public static LogicEntity ConvertToEntity(LogicModel model)
        {
            var logicEntity = new LogicEntity();

            if (model.Type == "Query")
            {
                var queryEntity = new LogicQueryEntity
                {
                    Text = model.Query
                };

                logicEntity.Logic = queryEntity;
            }
            else if (model.ExpressionsLogic != null)
            {
                var expressionsLogic = model.ExpressionsLogic;

                var espressionsEntity = new LogicExpressionsEntity
                {
                    FilterBy = ConvertToEntity(expressionsLogic.FilterBy).ToList(),
                    OrderBy = ConvertToEntity(expressionsLogic.OrderBy).ToList(),
                    GroupBy = ConvertToEntity(expressionsLogic.GroupBy).ToList(),
                    Select = ConvertToEntity(expressionsLogic.Select).ToList(),
                };

                logicEntity.Logic = espressionsEntity;
            }

            return logicEntity;
        }

        private static IEnumerable<ExpressionEntity> ConvertToEntity(ExpressionsListModel listModel)
        {
            if (listModel == null || listModel.Expressions == null)
                yield break;

            foreach (var model in listModel.Expressions)
            {
                if (model == null)
                    continue;

                var entity = new ExpressionEntity
                {
                    ID = model.Key,
                    Expression = model.Expression,
                    OutputType = model.OutputType,
                };

                yield return entity;
            }
        }

        private static IEnumerable<NamedExpressionEntity> ConvertToEntity(NamedExpressionsListModel listModel)
        {
            if (listModel == null || listModel.Expressions == null)
                yield break;

            foreach (var model in listModel.Expressions)
            {
                if (model == null)
                    continue;

                var entity = new NamedExpressionEntity
                {
                    ID = model.Key,
                    Name = model.Name,
                    Expression = model.Expression,
                    OutputType = model.OutputType,
                };

                yield return entity;
            }
        }
    }
}