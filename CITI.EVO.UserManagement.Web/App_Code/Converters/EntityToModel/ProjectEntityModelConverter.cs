
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Converters.Common;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate;

namespace CITI.EVO.UserManagement.Web.Converters.EntityToModel
{
    public class ProjectEntityModelConverter : SingleModelConverterBase<UM_Project, ProjectModel>
    {
        public ProjectEntityModelConverter(ISession session) : base(session)
        {
        }

        public override ProjectModel Convert(UM_Project source)
        {
            var model = new ProjectModel();
            FillObject(model, source);

            return model;
        }

        public override void FillObject(ProjectModel target, UM_Project source)
        {
            target.ID = source.ID;
            target.Name = source.Name;
            target.IsActive = source.IsActive;
        }
    }
}