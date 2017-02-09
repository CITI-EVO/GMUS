using System;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Web.UI.Controls;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Converters.EntityToModel;
using CITI.EVO.UserManagement.Web.Converters.ModelToEntity;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate.Linq;
using CITI.EVO.Tools.Extensions;

namespace CITI.EVO.UserManagement.Web.Pages.Management
{
    public partial class ProjectsList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyPermissions();
            FillProjectsGrid();
        }

        protected void btnOK_OnClick(object sender, EventArgs e)
        {
            var model = projectControl.Model;

            if (String.IsNullOrWhiteSpace(model.Name))
            {
                lblErrorMessage.Text = "შეიყვანეთ სახელი";

                mpeProject.Show();
                return;
            }

            var converter = new ProjectModelEntityConverter(HbSession);

            var project = HbSession.Query<UM_Project>().FirstOrDefault(n => n.ID == model.ID);
            if (project == null)
            {
                project = new UM_Project
                {
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.Now
                };
            }

            converter.FillObject(project, model);

            HbSession.SubmitChanges(project);

            FillProjectsGrid();
        }

        protected void btNewProject_Click(object sender, EventArgs e)
        {
            projectControl.Model = new ProjectModel();

            mpeProject.Show();
        }

        protected void projectsControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Query<UM_Project>().FirstOrDefault(n => n.ID == e.Value);
            if (entity == null)
                return;

            var converter = new ProjectEntityModelConverter(HbSession);
            var model = converter.Convert(entity);

            projectControl.Model = model;
            mpeProject.Show();
        }

        protected void projectsControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Query<UM_Project>().FirstOrDefault(n => n.ID == e.Value);
            if (entity == null)
                return;

            entity.DateDeleted = DateTime.Now;

            HbSession.SubmitUpdate(entity);

            FillProjectsGrid();
        }

        #region Methods

        protected void FillProjectsGrid()
        {
            var projects = HbSession.Query<UM_Project>().Where(n => n.DateDeleted == null).ToList();

            var converter = new ProjectEntityModelConverter(HbSession);

            var model = new ProjectsModel
            {
                List = projects.Select(n => converter.Convert(n)).ToList()
            };

            projectsControl.Model = model;
            projectsControl.DataBind();

            //ucMessage.Update();
        }

        private void ApplyPermissions()
        {
            //if (!UmUtil.Instance.HasAccess("ProjectsList"))
            //{
            //    Response.Redirect("~/Pages/Management/UsersList.aspx");
            //}

            //btNewProject.Visible = UmUtil.Instance.HasAccess("NewProjectButton");

            //gvProjects.Columns["Edit"].Visible =
            //    gvProjects.Columns["Delete"].Visible =
            //        UmUtil.Instance.HasAccess("ProjectsGrid");
        }

        #endregion
    }
}