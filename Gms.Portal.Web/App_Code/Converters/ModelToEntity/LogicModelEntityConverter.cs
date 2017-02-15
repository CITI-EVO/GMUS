using System;
using System.Xml.Linq;
using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Models.Common;
using NHibernate;

namespace Gms.Portal.Web.Converters.ModelToEntity
{
    public class LogicModelEntityConverter : SingleModelConverterBase<LogicModel, GM_Logic>
    {
        public LogicModelEntityConverter(ISession session) : base(session)
        {
        }

        public override GM_Logic Convert(LogicModel source)
        {
            var entity = new GM_Logic
            {
                ID = Guid.NewGuid(),
                DateCreated = DateTime.Now,
            };

            FillObject(entity, source);

            return entity;
        }

        public override void FillObject(GM_Logic target, LogicModel source)
        {
            //target.ID = source.ID;
            target.Name = source.Name;
            target.Type = source.Type;
            target.SourceType = source.SourceType;

            if (source.SourceType == "Table")
                target.TableID = source.SourceID;

            if (source.SourceType == "Logic")
                target.LogicID = source.SourceID;

            target.RawData = GetRawData(source);
        }

        private XDocument GetRawData(LogicModel model)
        {
            if (model.Type == "Query")
                return GetQueryXml(model);

            return GetLogicXml(model);
        }

        private XDocument GetQueryXml(LogicModel model)
        {
            return new XDocument(new XElement("Query", model.Query));
        }
        private XDocument GetLogicXml(LogicModel model)
        {
            var expressionsLogic = model.ExpressionsLogic;
            if (expressionsLogic != null)
                return GetLogicXml(expressionsLogic);

            return null;
        }

        private XDocument GetLogicXml(ExpressionsLogicModel model)
        {
            var logicXElem = new XElement("Logic");

            var filterByXElem = GetExpressionsXElem("FilterBy", model.FilterBy);
            if (filterByXElem != null)
                logicXElem.Add(filterByXElem);

            var groupByXElem = GetExpressionsXElem("GroupBy", model.GroupBy);
            if (groupByXElem != null)
                logicXElem.Add(groupByXElem);

            var orderByXElem = GetExpressionsXElem("OrderBy", model.OrderBy);
            if (orderByXElem != null)
                logicXElem.Add(orderByXElem);

            var selectXElem = GetExpressionsXElem("Select", model.Select);
            if (selectXElem != null)
                logicXElem.Add(selectXElem);

            return new XDocument(logicXElem);
        }

        private XElement GetExpressionsXElem(String name, ExpressionsListModel model)
        {
            if (model == null || model.Expressions == null)
                return null;

            var expressionsByXElem = new XElement(name);

            foreach (var item in model.Expressions)
            {
                var itemXElem = new XElement("Item",
                    new XElement("Key", item.Key),
                    new XElement("Expression", item.Expression),
                    new XElement("OutputType", item.OutputType));

                expressionsByXElem.Add(itemXElem);
            }

            return expressionsByXElem;
        }

        private XElement GetExpressionsXElem(String name, NamedExpressionsListModel model)
        {
            if (model == null || model.Expressions == null)
                return null;

            var expressionsByXElem = new XElement(name);

            foreach (var item in model.Expressions)
            {
                var itemXElem = new XElement("Item",
                    new XElement("Key", item.Key),
                    new XElement("Name", item.Name),
                    new XElement("Expression", item.Expression),
                    new XElement("OutputType", item.OutputType));

                expressionsByXElem.Add(itemXElem);
            }

            return expressionsByXElem;
        }
    }
}
