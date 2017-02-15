using CITI.EVO.Core.Common;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate;

namespace CITI.EVO.UserManagement.Web.Converters.EntityToModel
{
    public class ResourceEntityModelConverter : SingleModelConverterBase<UM_Resource, ResourceModel>
    {
        public ResourceEntityModelConverter(ISession session) : base(session)
        {
        }

        public override ResourceModel Convert(UM_Resource source)
        {
            var model = new ResourceModel();
            FillObject(model, source);

            return model;
        }

        public override void FillObject(ResourceModel target, UM_Resource source)
        {
            target.ID = source.ID;
            target.Name = source.Name;
            target.Type = source.Type;
            target.Value = source.Value;
            target.ParentID = source.ParentID;
            target.ProjectID = source.ProjectID;
            target.Description = source.Description;
        }
    }
}