using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Comparers;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Controls.User.Monitoring
{
    public partial class SummaryBudgetDataControl : BaseUserControlExtend<SummaryBudgetDataModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        public void BindData(FormEntity formEntity, FormDataUnit formData, IEnumerable<IDictionary<String, Object>> transfers)
        {
            var entities = transfers.Select(n => new MoneyTransferEntity(n));
            BindData(formEntity, formData, entities);
        }
        public void BindData(ContentEntity formEntity, FormDataUnit formData, IEnumerable<MoneyTransferEntity> transfers)
        {
            if (formEntity == null)
                return;

            var comparer = StringLogicalComparer.OrdinalIgnoreCase;
            var controls = FormStructureUtil.PreOrderIndexedTraversal(formEntity);

            var dataGrids = (from n in controls
                             where (n is GridEntity || n is TreeEntity) &&
                                   (n.Alias == "Budget")
                             select n);

            var controlLp = dataGrids.ToLookup(n => n.Alias, comparer);

            var budget = controlLp["Budget"].SingleOrDefault() as ContentEntity;

            var errors = new List<String>();

            if (budget == null)
                errors.Add(@"Unable to find 'Budget'");

            if (errors.Count > 0)
            {
                lblError.Text = String.Join("<br/>", errors);
                return;
            }

            var taskNameField = (from n in FormStructureUtil.PreOrderIndexedTraversal(budget)
                                 where comparer.Equals(n.Alias, "TaskName")
                                 select n).SingleOrDefault();

            if (taskNameField == null)
                errors.Add(@"Unable to find 'Budget\TaskName'");

            var taskAmountField = (from n in FormStructureUtil.PreOrderIndexedTraversal(budget)
                                  where comparer.Equals(n.Alias, "TaskAmount")
                                  select n).SingleOrDefault();

            if (taskAmountField == null)
                errors.Add(@"Unable to find 'Budget\TaskPrice'");

            if (errors.Count > 0)
            {
                lblError.Text = String.Join("<br/>", errors);
                return;
            }

            var budgetKey = Convert.ToString(budget.ID);

            var budgetData = formData[budgetKey];
            if (budgetData is FormDataListRef)
                budgetData = new FormDataLazyList((FormDataListRef)budgetData);

            if (budgetData != null)
                budgetData = ((FormDataLazyList)budgetData).ToDictionary(n => n.ID);

            var budgetDataDict = budgetData as IDictionary<Guid?, FormDataUnit>;
            if (budgetDataDict == null)
                return;

            var transfersLp = transfers.ToLookup(n => n.TaskID);

            var taskNameKey = Convert.ToString(taskNameField.ID);
            var taskAmountKey = Convert.ToString(taskAmountField.ID);

            var query = (from n in budgetDataDict
                         let task = n.Value[taskNameKey]
                         let plan = n.Value[taskAmountKey]
                         let enroll = (from m in transfersLp[n.Key]
                                       where m.Incoming != null &&
                                             m.Outgoing == null
                                       select m.Incoming).Sum()
                         let cashExpense = (from m in transfersLp[n.Key]
                                            where m.Outgoing != null &&
                                                  m.Incoming == null
                                            select m.Outgoing).Sum()
                         let confirmedExpense = (from m in transfersLp[n.Key]
                                                 where m.Outgoing != null &&
                                                       m.Incoming == null &&
                                                       m.Accepted.GetValueOrDefault()
                                                 select m.Outgoing).Sum()
                         let unconfirmedExpense = (from m in transfersLp[n.Key]
                                                   where m.Outgoing != null &&
                                                         m.Incoming == null &&
                                                         !m.Accepted.GetValueOrDefault()
                                                   select m.Outgoing).Sum()
                         select new
                         {
                             Task = task,
                             Plan = plan,
                             Enroll = enroll,
                             CashExpense = cashExpense,
                             ConfirmedExpense = confirmedExpense,
                             UnconfirmedExpense = unconfirmedExpense,
                         });

            gvData.DataSource = query;
            gvData.DataBind();
        }

    }
}