using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Monitoring;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Controls.User.Monitoring
{
    public partial class MonitoringProjectsDataControl : BaseUserControl
    {
        private IDictionary<Guid?, MonitoringFlewEntity> _flews;

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

        public event EventHandler<GenericEventArgs<Guid>> Flaw;
        protected virtual void OnFlaw(GenericEventArgs<Guid> e)
        {
            if (Flaw != null)
                Flaw(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Status;
        protected virtual void OnStatus(GenericEventArgs<Guid> e)
        {
            if (Status != null)
                Status(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Upload;
        protected virtual void OnUpload(GenericEventArgs<Guid> e)
        {
            if (Upload != null)
                Upload(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnView_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnView(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnEdit_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnEdit(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnDelete_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnDelete(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnAccept_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnAccept(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnFlaw_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnFlaw(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void bntStatus_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnStatus(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnFiles_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnUpload(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void gridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var task = e.Row.DataItem as ProjectTaskEntity;
                if (task != null)
                {
                    if (task.Status == MonitoringItemStatuses.Accepted)
                        e.Row.BackColor = Color.FromArgb(0, 210, 255, 191);
                }
            }
        }

        public void BindData(IEnumerable<MonitoringFlewEntity> flews, IEnumerable<ProjectTaskEntity> projects)
        {
            _flews = flews.ToDictionary(n => n.ID);

            gvData.DataSource = projects;
            gvData.DataBind();
        }

        public IEnumerable<ProjectsDataEntity> GetData(IEnumerable<MonitoringFlewEntity> flews, IEnumerable<ProjectTaskEntity> projects)
        {
            var result = (from n in projects
                          where n.DateDeleted == null
                          select new ProjectsDataEntity
                          {
                              Task = n.Name,
                              Status = n.Status,
                              Flaws = GetFlewsNames(n.FlawsID),
                              FlawsScore = GetFlewsScores(n.FlawsID),
                              FlawDescription = n.Comment,
                              DoneStatus = n.DoneStatus,
                              DoneDescription = n.DoneDescription,
                              CreatedBy = GetUserName(n.CreateUserID),
                              DateCreated = n.DateCreated.GetValueOrDefault(),
                              StartDate = n.StartDate,
                              EndDate = n.EndDate,
                              SubmitDate = n.SubmitDate,
                              SubmitUser = GetUserName(n.SubmitUserID)
                          });

            return result;
        }
        
        public void ShowCommands()
        {
            if (gvData.Columns.Count > 0)
                gvData.Columns[0].Visible = true;
        }
        public void HideCommands()
        {
            if (gvData.Columns.Count > 0)
                gvData.Columns[0].Visible = false;
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

        protected bool GetViewVisible(object dataItem)
        {
            return true;
        }

        protected bool GetFilesVisible(object dataItem)
        {
            return true;
        }

        protected bool GetEditVisible(object dataItem)
        {
            var entity = dataItem as ProjectTaskEntity;
            if (entity == null)
                return false;

            return entity.SubmitDate == null;
        }

        protected bool GetDeleteVisible(object dataItem)
        {
            var entity = dataItem as ProjectTaskEntity;
            if (entity == null)
                return false;

            return entity.SubmitDate == null;
        }

        protected bool GetFlawVisible(object dataItem)
        {
            return UmUtil.Instance.HasAccess("Admin") || UmUtil.Instance.HasAccess("MonitoringStatus");
        }

        protected bool GetStatusVisible(object dataItem)
        {
            var entity = dataItem as ProjectTaskEntity;
            if (entity == null)
                return false;

            return entity.SubmitDate == null && (UmUtil.Instance.HasAccess("Admin") || !UmUtil.Instance.HasAccess("MonitoringStatus"));
        }

        protected bool GetReturnVisible(object dataItem)
        {
            return true;
        }

        protected String GetStatusName(object eval)
        {
            var isDone = DataConverter.ToNullableBoolean(eval).GetValueOrDefault();
            if (isDone)
                return "შესრულებულია";

            return "შეუსრულებულია";
        }


        protected String GetFlewsNames(object eval)
        {
            var flewsID = eval as IEnumerable<Guid?>;
            if (flewsID == null)
                return null;

            var query = (from n in flewsID
                         where n != null
                         let f = _flews.GetValueOrDefault(n)
                         where f != null
                         select f.Name);

            var flews = String.Join("; ", query);
            return flews;
        }

        protected double? GetFlewsScores(object eval)
        {
            var flewsID = eval as IEnumerable<Guid?>;
            if (flewsID == null)
                return null;

            var query = (from n in flewsID
                         where n != null
                         let f = _flews.GetValueOrDefault(n)
                         where f != null
                         select f.Score);

            var sum = query.Sum();
            return sum;
        }



    }
}