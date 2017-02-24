using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Models.Filters;
using NHibernate.Linq;
using CITI.EVO.Tools.Extensions;

namespace CITI.EVO.UserManagement.Web.Controls.Filters
{
    public partial class ResourcesFilterControl : BaseUserControl<ResourcesFilterModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillProjects();
        }

        protected void FillProjects()
        {
            var projects = (from n in HbSession.Query<UM_Project>()
                            where n.DateDeleted == null
                            select n).ToList();

            var list = projects.Select(n => new KeyValuePair<Guid?, String>(n.ID, n.Name)).ToList();
            list.Insert(0, new KeyValuePair<Guid?, String>(null, "Global"));

            cmbProject.BindData(list);

            if (!IsPostBack)
                cmbProject.SelectedIndex = 0;
        }
    }
}