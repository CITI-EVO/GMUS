using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Controls.User.Monitoring
{
    public partial class MonitoringBudgetItemControl : BaseUserControlExtend<MonitoringBudgetItemModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindData(FormEntity entity, FormDataUnit formData)
        {
            if (entity == null)
                return;

            var comparer = StringLogicalComparer.OrdinalIgnoreCase;
            var controls = FormStructureUtil.PreOrderIndexedTraversal(entity);

            var dataGrids = (from n in controls
                             where (n is GridEntity || n is TreeEntity) &&
                                   (n.Alias == "ProjectBudget")
                             select n);

            var controlLp = dataGrids.ToLookup(n => n.Alias, comparer);

            var budget = controlLp["ProjectBudget"].SingleOrDefault() as ContentEntity;
            if (budget == null)
                return;

            var taskNameField = (from n in FormStructureUtil.PreOrderIndexedTraversal(budget)
                                 where comparer.Equals(n.Alias, "ProjectBudget_Name")
                                 select n).SingleOrDefault();

            if (taskNameField == null)
                throw new Exception(@"Unable to find 'ProjectBudget\ProjectBudget_Name'");

            var budgetKey = Convert.ToString(budget.ID);
            var taskNameKey = Convert.ToString(taskNameField.ID);

            var budgetData = formData[budgetKey];
            if (budgetData is FormDataListRef)
                budgetData = new FormDataLazyList((FormDataListRef)budgetData);

            if (budgetData != null)
            {
                var taskQuery = (from n in (IEnumerable<FormDataUnit>)budgetData
                                 let id = DataConverter.ToNullableGuid(n[FormDataConstants.IDField])
                                 let name = GetCorrectName(n[taskNameKey])
                                 select new
                                 {
                                     ID = id,
                                     Name = name,
                                 });

                cbxTasks.BindData(taskQuery);
            }
        }

        private String GetCorrectName(Object val)
        {
            var name = Convert.ToString(val);

            if (String.IsNullOrWhiteSpace(name))
                return "[Name Is Empty]";

            return name;
        }
    }
}