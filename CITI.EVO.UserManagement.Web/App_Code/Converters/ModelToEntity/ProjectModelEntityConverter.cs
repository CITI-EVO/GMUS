
using CITI.EVO.Core.Common;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate;

namespace CITI.EVO.UserManagement.Web.Converters.ModelToEntity
{
    public class ProjectModelEntityConverter : SingleModelConverterBase<ProjectModel, UM_Project>
    {
        public ProjectModelEntityConverter(ISession session) : base(session)
        {
        }

        public override UM_Project Convert(ProjectModel source)
        {
            var target = EntityFactory.CreateEntity<UM_Project>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(UM_Project target, ProjectModel source)
        {
            //target.ID = source.ID;
            target.Name = source.Name;
            target.IsActive = source.IsActive.GetValueOrDefault();
        }
    }
}