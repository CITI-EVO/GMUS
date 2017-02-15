using CITI.EVO.Core.Common;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate;

namespace CITI.EVO.UserManagement.Web.Converters.ModelToEntity
{
    public class GroupModelEntityConverter : SingleModelConverterBase<GroupModel, UM_Group>
    {
        public GroupModelEntityConverter(ISession session) : base(session)
        {
        }

        public override UM_Group Convert(GroupModel source)
        {
            var target = EntityFactory.CreateEntity<UM_Group>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(UM_Group target, GroupModel source)
        {
            //target.ID = source.ID;
            target.Name = source.Name;
            target.ProjectID = source.ProjectID.GetValueOrDefault();
            target.ParentID = source.ParentID;
            target.Name = source.Name;
        }
    }
}