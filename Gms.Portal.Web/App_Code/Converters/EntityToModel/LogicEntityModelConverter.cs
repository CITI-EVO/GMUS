using System;
using System.Collections.Generic;
using System.Xml.Linq;
using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Models.Common;
using NHibernate;

namespace Gms.Portal.Web.Converters.EntityToModel
{
    public class LogicEntityModelConverter : SingleModelConverterBase<GM_Logic, LogicModel>
    {
        public LogicEntityModelConverter(ISession session) : base(session)
        {
        }

        public override LogicModel Convert(GM_Logic source)
        {
            var target = new LogicModel();

            FillObject(target, source);

            return target;
        }

        public override void FillObject(LogicModel target, GM_Logic source)
        {
            target.ID = source.ID;
            target.Name = source.Name;
            target.Type = source.Type;
            target.SourceType = source.SourceType;

            if (source.SourceType == "Table")
                target.SourceID = source.TableID;

            if (source.SourceType == "Logic")
                target.SourceID = source.LogicID;

            var logicXElem = source.RawData;
            if (logicXElem == null)
                return;

            target.Query = GetQuery(logicXElem);
            target.ExpressionsLogic = GetExpressionsLogic(logicXElem);

        }

        private String GetQuery(XDocument logicXDoc)
        {
            var logicXElem = logicXDoc.Root;
            if (logicXElem.Name == "Query")
                return logicXElem.Value;

            return null;
        }

        private ExpressionsLogicModel GetExpressionsLogic(XDocument logicXDoc)
        {
            var model = new ExpressionsLogicModel();
            var logicXElem = logicXDoc.Root;

            model.FilterBy = GetExpressionsList("FilterBy", logicXElem);
            model.GroupBy = GetExpressionsList("GroupBy", logicXElem);
            model.OrderBy = GetExpressionsList("OrderBy", logicXElem);
            model.Select = GetNamedExpressionsList("Select", logicXElem);

            return model;
        }

        private ExpressionsListModel GetExpressionsList(String name, XElement logicXElem)
        {
            var expressionsByXElem = logicXElem.Element(name);
            if (expressionsByXElem == null)
                return null;

            var expressionsModel = new ExpressionsListModel
            {
                Expressions = new List<ExpressionModel>()
            };

            foreach (var itemXElem in expressionsByXElem.Elements("Item"))
            {
                var model = new ExpressionModel
                {
                    Key = (Guid?)itemXElem.Element("Key"),
                    Expression = (String)itemXElem.Element("Expression"),
                    OutputType = (String)itemXElem.Element("OutputType")
                };

                expressionsModel.Expressions.Add(model);
            }

            return expressionsModel;
        }

        private NamedExpressionsListModel GetNamedExpressionsList(String name, XElement logicXElem)
        {
            var expressionsByXElem = logicXElem.Element(name);
            if (expressionsByXElem == null)
                return null;

            var expressionsModel = new NamedExpressionsListModel
            {
                Expressions = new List<NamedExpressionModel>()
            };

            foreach (var itemXElem in expressionsByXElem.Elements("Item"))
            {
                var model = new NamedExpressionModel
                {
                    Key = (Guid?)itemXElem.Element("Key"),
                    Name = (String)itemXElem.Element("Name"),
                    Expression = (String)itemXElem.Element("Expression"),
                    OutputType = (String)itemXElem.Element("OutputType")
                };

                expressionsModel.Expressions.Add(model);
            }

            return expressionsModel;
        }
    }
}
