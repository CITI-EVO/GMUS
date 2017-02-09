using System;
using System.Collections.Generic;
using System.Data.Entity;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Converters.Common;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate;

namespace CITI.EVO.UserManagement.Web.Converters.EntityToModel
{
    public class AttributeSchemaEntityModelConverter : SingleModelConverterBase<UM_AttributeSchema, AttributeSchemaModel>
    {
        public AttributeSchemaEntityModelConverter(ISession session) : base(session)
        {
        }

        public override AttributeSchemaModel Convert(UM_AttributeSchema source)
        {
            var model = new AttributeSchemaModel();
            FillObject(model, source);

            return model;
        }

        public override void FillObject(AttributeSchemaModel target, UM_AttributeSchema source)
        {
            target.ID = source.ID;
            target.Name = source.Name;
            target.ProjectID = source.ProjectID;

            target.Fields = new AttributeFieldsModel();
            FillObject(target.Fields, source.AttributeFields);
        }

        private void FillObject(AttributeFieldsModel target, IEnumerable<UM_AttributeField> source)
        {
            var converter = new AttributeFieldEntityModelConverter(Session);

            target.List = new List<AttributeFieldModel>();

            foreach (var attributesField in source)
            {
                var model = converter.Convert(attributesField);
                target.List.Add(model);
            }
        }
    }
}