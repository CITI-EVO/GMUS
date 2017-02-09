using CITI.EVO.UserManagement.DAL.Common;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Converters.Common;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate;

namespace CITI.EVO.UserManagement.Web.Converters.ModelToEntity
{
    public class AttributeValueModelEntityConverter : SingleModelConverterBase<ObjectAttributeModel, UM_AttributeValue>
    {
        public AttributeValueModelEntityConverter(ISession session) : base(session)
        {
        }

        public override UM_AttributeValue Convert(ObjectAttributeModel source)
        {
            var target = EntityFactory.CreateEntity<UM_AttributeValue>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(UM_AttributeValue target, ObjectAttributeModel source)
        {
            //target.ID = source.ID;
            target.ParentID = source.ParentID.GetValueOrDefault();
            target.AttributeFieldID = source.FieldID.GetValueOrDefault();
            target.Value = source.Value;
        }
    }
}