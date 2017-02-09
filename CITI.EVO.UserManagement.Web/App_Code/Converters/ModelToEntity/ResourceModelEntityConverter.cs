using CITI.EVO.UserManagement.DAL.Common;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Converters.Common;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate;

namespace CITI.EVO.UserManagement.Web.Converters.ModelToEntity
{
    public class ResourceModelEntityConverter : SingleModelConverterBase<ResourceModel, UM_Resource>
    {
        public ResourceModelEntityConverter(ISession session) : base(session)
        {
        }

        public override UM_Resource Convert(ResourceModel source)
        {
            var target = EntityFactory.CreateEntity<UM_Resource>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(UM_Resource target, ResourceModel source)
        {
            //target.ID = source.ID;
            target.Name = source.Name;
            target.Type = source.Type.GetValueOrDefault();
            target.Value = source.Value;
            target.ParentID = source.ParentID;
            target.ProjectID = source.ProjectID;
            target.Description = source.Description;
        }
    }
}