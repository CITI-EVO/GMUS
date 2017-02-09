
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Converters.Common;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate;

namespace CITI.EVO.UserManagement.Web.Converters.EntityToModel
{
    public class GroupEntityModelConverter : SingleModelConverterBase<UM_Group, GroupModel>
    {
        public GroupEntityModelConverter(ISession session) : base(session)
        {
        }

        public override GroupModel Convert(UM_Group source)
        {
            var model = new GroupModel();
            FillObject(model, source);

            return model;
        }

        public override void FillObject(GroupModel target, UM_Group source)
        {
            target.ID = source.ID;
            target.ProjectID = source.ProjectID;
            target.ParentID = source.ParentID;
            target.Name = source.Name;
        }
    }
}