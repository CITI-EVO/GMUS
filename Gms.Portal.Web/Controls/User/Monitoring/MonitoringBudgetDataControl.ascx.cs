using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Controls.User.Monitoring
{
    public partial class MonitoringBudgetDataControl : BaseUserControl
    {
        private ControlEntity _taskNameField;
        private IDictionary<Guid?, FormDataUnit> _budgetData;

        public event EventHandler<GenericEventArgs<Guid>> View;
        protected virtual void OnView(GenericEventArgs<Guid> e)
        {
            if (View != null)
                View(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Edit;
        protected virtual void OnEdit(GenericEventArgs<Guid> e)
        {
            if (Edit != null)
                Edit(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Delete;
        protected virtual void OnDelete(GenericEventArgs<Guid> e)
        {
            if (Delete != null)
                Delete(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Accept;
        protected virtual void OnAccept(GenericEventArgs<Guid> e)
        {
            if (Accept != null)
                Accept(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Return;
        protected virtual void OnReturn(GenericEventArgs<Guid> e)
        {
            if (Return != null)
                Return(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnView_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID == null)
                OnView(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnEdit_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID == null)
                OnEdit(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnDelete_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID == null)
                OnDelete(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnAccept_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID == null)
                OnAccept(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnReturn_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID == null)
                OnReturn(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void gridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        public void BindData(ContentEntity entity, FormDataUnit formData, IEnumerable<IDictionary<String, Object>> transfers)
        {
            var entities = transfers.Select(n => new MoneyTransferEntity(n));
            BindData(entity, formData, entities);
        }
        public void BindData(ContentEntity entity, FormDataUnit formData, IEnumerable<MoneyTransferEntity> transfers)
        {
            if (entity == null)
                return;

            var comparer = StringLogicalComparer.OrdinalIgnoreCase;
            var controls = FormStructureUtil.PreOrderIndexedTraversal(entity);

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

            _taskNameField = (from n in FormStructureUtil.PreOrderIndexedTraversal(budget)
                              where comparer.Equals(n.Alias, "TaskName")
                              select n).SingleOrDefault();

            if (_taskNameField == null)
                errors.Add(@"Unable to find 'Budget\TaskName'");

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
                _budgetData = ((FormDataLazyList)budgetData).ToDictionary(n => n.ID);

            gvData.DataSource = transfers;
            gvData.DataBind();
        }

        protected String GetUserName(object eval)
        {
            var userID = DataConverter.ToNullableGuid(eval);

            var user = UmUsersCache.GetUser(userID);
            if (user == null)
                return null;

            var name = $"{user.LoginName} - {user.FirstName} {user.LastName}";
            return name;
        }

        protected String GetTaskName(object eval)
        {
            if (_budgetData == null)
                return null;

            var itemID = DataConverter.ToNullableGuid(eval);
            if (itemID == null)
                return null;

            var formData = _budgetData.GetValueOrDefault(itemID);
            if (formData == null)
                return null;

            var key = Convert.ToString(_taskNameField.ID);
            return Convert.ToString(formData[key]);
        }

        protected String GetGoalName(object eval)
        {
            return Convert.ToString(eval);
        }

        protected bool GetViewVisible(object dataItem)
        {
            return true;
        }

        protected bool GetEditVisible(object dataItem)
        {
            return true;
        }

        protected bool GetDeleteVisible(object dataItem)
        {
            return true;
        }

        protected bool GetAcceptVisible(object dataItem)
        {
            return true;
        }

        protected bool GetReturnVisible(object dataItem)
        {
            return true;
        }
    }
}