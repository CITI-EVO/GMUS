using CITI.EVO.Core.Common;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate;

namespace CITI.EVO.UserManagement.Web.Converters.EntityToModel
{
    public class AttributeValueEntityModelConverter : SingleModelConverterBase<UM_AttributeValue, ObjectAttributeModel>
    {
        public AttributeValueEntityModelConverter(ISession session) : base(session)
        {
        }

        public override ObjectAttributeModel Convert(UM_AttributeValue source)
        {
            var model = new ObjectAttributeModel();
            FillObject(model, source);

            return model;
        }

        public override void FillObject(ObjectAttributeModel target, UM_AttributeValue source)
        {
            target.ID = source.ID;
            target.FieldID = source.AttributeFieldID;
            target.ParentID = source.ParentID;
            target.Value = source.Value;

            target.SchemaID = source.AttributeField.AttributeSchemaID;
            target.ProjectID = source.AttributeField.AttributeSchema.ProjectID;
        }
    }
}