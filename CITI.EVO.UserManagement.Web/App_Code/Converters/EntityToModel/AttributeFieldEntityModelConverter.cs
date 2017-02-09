
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Converters.Common;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate;

namespace CITI.EVO.UserManagement.Web.Converters.EntityToModel
{
    public class AttributeFieldEntityModelConverter : SingleModelConverterBase<UM_AttributeField, AttributeFieldModel>
    {
        public AttributeFieldEntityModelConverter(ISession session) : base(session)
        {
        }

        public override AttributeFieldModel Convert(UM_AttributeField source)
        {
            var model = new AttributeFieldModel();
            FillObject(model, source);

            return model;
        }

        public override void FillObject(AttributeFieldModel target, UM_AttributeField source)
        {
            target.ID = source.ID;
            target.Name = source.Name;
            target.SchemaID = source.AttributeSchemaID;
        }
    }
}