using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Controls.User.Monitoring
{
    public partial class MonitoringItemControl : BaseUserControlExtend<MonitoringItemModel>
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
                                   n.Alias == "Budget" &&
                                   n.Alias == "Goals"
                             select n);

            var controlLp = dataGrids.ToLookup(n => n.Alias, comparer);

            var budget = controlLp["Budget"].SingleOrDefault() as ContentEntity;
            var goals = controlLp["Goals"].SingleOrDefault() as ContentEntity;

            if (budget == null)
                throw new Exception("Unable to find 'Budget'");

            if (goals == null)
                throw new Exception("Unable to find 'Goals'");

            var taskNameField = (from n in FormStructureUtil.PreOrderIndexedTraversal(budget)
                                 where comparer.Equals(n.Alias, "TaskName")
                                 select n).SingleOrDefault();

            var goalNameField = (from n in FormStructureUtil.PreOrderIndexedTraversal(budget)
                                 where comparer.Equals(n.Alias, "GoalName")
                                 select n).SingleOrDefault();

            if (taskNameField == null)
                throw new Exception(@"Unable to find 'Budget\TaskName'");

            if (goalNameField == null)
                throw new Exception(@"Unable to find 'Goals\GoalName'");

            var budgetKey = Convert.ToString(budget.ID);
            var goalsKey = Convert.ToString(goals.ID);

            var budgetData = formData[budgetKey];
            if (budgetData is FormDataListRef)
                budgetData = new FormDataLazyList((FormDataListRef)budgetData);

            var goalsData = formData[goalsKey];
            if (goalsData is FormDataListRef)
                goalsData = new FormDataLazyList((FormDataListRef)goalsData);

            var budgetQuery = (from n in (IEnumerable<FormDataUnit>)budgetData
                               let id = DataConverter.ToNullableGuid(n[FormDataConstants.IDField])
                               let name = Convert.ToString(n[budgetKey])
                               select new
                               {
                                   ID = id,
                                   Name = name,
                               });

            var goalsQuery = (from n in (IEnumerable<FormDataUnit>)goalsData
                              let id = DataConverter.ToNullableGuid(n[FormDataConstants.IDField])
                              let name = Convert.ToString(n[budgetKey])
                              select new
                              {
                                  ID = id,
                                  Name = name,
                              });

            cbxBudget.BindData(budgetQuery);
            cbxGoals.BindData(goalsQuery);
        }
    }
}