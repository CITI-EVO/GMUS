using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.UserManagement.DAL.Common;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Converters.Common;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate;

namespace CITI.EVO.UserManagement.Web.Converters.ModelToEntity
{
    public class AttributeSchemaModelEntityConverter : SingleModelConverterBase<AttributeSchemaModel, UM_AttributeSchema>
    {
        public AttributeSchemaModelEntityConverter(ISession session) : base(session)
        {
        }

        public override UM_AttributeSchema Convert(AttributeSchemaModel source)
        {
            var target = EntityFactory.CreateEntity<UM_AttributeSchema>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(UM_AttributeSchema target, AttributeSchemaModel source)
        {
            //target.ID = source.ID;
            target.Name = source.Name;
            target.ProjectID = source.ProjectID;

            FillObject(target.AttributeFields, source.Fields);
        }

        private void FillObject(ICollection<UM_AttributeField> target, AttributeFieldsModel source)
        {
            if (source == null || source.List == null)
                return;

            var newAttributes = source.List.ToDictionary(n => n.ID);
            var oldAttributes = target.ToDictionary(n => (Guid?)n.ID);

            var deleteAttributes = (from n in target
                                    where !newAttributes.ContainsKey(n.ID)
                                    select n).ToList();

            var insertAttributes = (from n in source.List
                                    where !oldAttributes.ContainsKey(n.ID)
                                    select n).ToList();

            var updateAttributes = (from n in source.List
                                    where oldAttributes.ContainsKey(n.ID)
                                    select n).ToList();

            var converter = new AttributeFieldModelEntityConverter(Session);

            foreach (var item in deleteAttributes)
                item.DateDeleted = DateTime.Now;

            foreach (var item in insertAttributes)
            {
                var entity = converter.Convert(item);
                target.Add(entity);
            }

            foreach (var item in updateAttributes)
            {
                var entity = oldAttributes[item.ID];
                converter.FillObject(entity, item);
            }
        }
    }
}