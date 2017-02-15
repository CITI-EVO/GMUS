using CITI.EVO.Core.Common;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate;

namespace CITI.EVO.UserManagement.Web.Converters.ModelToEntity
{
    public class AttributeFieldModelEntityConverter : SingleModelConverterBase<AttributeFieldModel, UM_AttributeField>
    {
        public AttributeFieldModelEntityConverter(ISession session) : base(session)
        {
        }

        public override UM_AttributeField Convert(AttributeFieldModel source)
        {
            var target = EntityFactory.CreateEntity<UM_AttributeField>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(UM_AttributeField target, AttributeFieldModel source)
        {
            //target.ID = source.ID;
            target.Name = source.Name;
            target.AttributeSchemaID = source.SchemaID.GetValueOrDefault();
        }
    }
}