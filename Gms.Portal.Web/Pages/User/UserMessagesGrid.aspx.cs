using System;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Converters.ModelToEntity;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;

namespace Gms.Portal.Web.Pages.User
{
    public partial class UserMessagesGrid : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillDataGrid();
        }

        protected void userMessagesGridControl_OnView(object sender, GenericEventArgs<Guid> e)
        {
            var dbEntity = HbSession.Query<GM_UserMessage>().FirstOrDefault(n => n.ID == e.Value);
            if (dbEntity == null)
                return;

            var converter = new UserMessageEntityModelConverter(HbSession);
            var model = converter.Convert(dbEntity);

            userMessageControl.Model = new UserMessageExModel(model);
            mpeUserMessage.Show();
        }

        protected void userMessagesGridControl_OnApprove(object sender, GenericEventArgs<Guid> e)
        {
            var dbEntity = HbSession.Query<GM_UserMessage>().FirstOrDefault(n => n.ID == e.Value);
            if (dbEntity == null)
                return;

            dbEntity.StatusUserID = UserUtil.GetCurrentUserID();
            dbEntity.StatusID = DataStatusCache.Accepted.ID;

            HbSession.SubmitChanges(dbEntity);

            FillDataGrid();
        }

        protected void userMessagesGridControl_OnReject(object sender, GenericEventArgs<Guid> e)
        {
            var model = new MessageRejectModel
            {
                MessageID = e.Value
            };

            messageRejectControl.Model = model;
            mpeRejectMessage.Show();
        }

        protected void userMessagesGridControl_OnMarkAsRead(object sender, GenericEventArgs<Guid> e)
        {
            var dbEntity = HbSession.Query<GM_UserMessage>().FirstOrDefault(n => n.ID == e.Value);
            if (dbEntity == null)
                return;

            dbEntity.Readed = true;

            HbSession.SubmitChanges(dbEntity);

            FillDataGrid();
        }

        protected void btnUserMessageOK_Click(object sender, EventArgs e)
        {
            var model = userMessageControl.Model;
            if (String.IsNullOrWhiteSpace(model.Replay))
            {
                mpeUserMessage.Show();
                return;
            }

            if (model.ID != null)
            {
                var dbEntity = HbSession.Query<GM_UserMessage>().FirstOrDefault(n => n.ID == model.ID);
                if (dbEntity != null)
                {
                    dbEntity.Readed = true;
                    HbSession.SubmitChanges(dbEntity);
                }
            }

            var subject = model.Subject;
            if (!subject.StartsWith("RE:"))
                subject = $"RE: {subject}";

            var newMessage = new UserMessageModel
            {
                ParentID = model.ID,
                FromUserID = UserUtil.GetCurrentUserID(),
                ToUserID = model.FromUserID,
                FormID = model.FormID,
                RecordID = model.RecordID,
                Subject = subject,
                Text = model.Replay,
            };

            var converter = new UserMessageModelEntityConverter(HbSession);
            var entity = converter.Convert(newMessage);

            HbSession.SubmitChanges(entity);
            mpeUserMessage.Hide();

            FillDataGrid();
        }

        protected void btnUserMessageCancel_OnClick(object sender, EventArgs e)
        {
            if (UmUtil.Instance.HasAccess("Admin"))
            {
                mpeUserMessage.Hide();
                return;
            }

            var model = userMessageControl.Model;
            if (model.ID != null)
            {
                var dbEntity = HbSession.Query<GM_UserMessage>().FirstOrDefault(n => n.ID == model.ID);
                if (dbEntity != null)
                {
                    dbEntity.Readed = true;
                    HbSession.SubmitChanges(dbEntity);
                }
            }

            mpeUserMessage.Hide();
        }

        protected void btnRejectMessageOK_Click(object sender, EventArgs e)
        {
            var model = messageRejectControl.Model;

            var dbEntity = HbSession.Query<GM_UserMessage>().FirstOrDefault(n => n.ID == model.MessageID);
            if (dbEntity == null)
                return;

            dbEntity.Description = model.Description;
            dbEntity.StatusUserID = UserUtil.GetCurrentUserID();
            dbEntity.StatusID = DataStatusCache.Rejected.ID;

            HbSession.SubmitChanges(dbEntity);
            mpeRejectMessage.Hide();

            FillDataGrid();
        }

        protected void btnRejectMessageCancel_OnClick(object sender, EventArgs e)
        {
            mpeRejectMessage.Hide();
        }

        protected void FillDataGrid()
        {
            var query = from n in HbSession.Query<GM_UserMessage>()
                        where n.DateDeleted == null
                        select n;

            if (!UmUtil.Instance.HasAccess("Admin"))
            {
                var userID = UserUtil.GetCurrentUserID();

                if (UmUtil.Instance.HasAccess("User") || UmUtil.Instance.HasAccess("Expert"))
                {
                    query = from n in query
                            where n.FromUserID == userID ||
                                  (
                                      n.ToUserID == userID &&
                                      n.StatusID == DataStatusCache.Accepted.ID
                                  )
                            select n;
                }
            }

            query = (from n in query
                     orderby n.DateCreated descending
                     select n);

            var dbItems = query.ToList();

            var converter = new UserMessageEntityModelConverter(HbSession);

            var model = new UserMessagesModel
            {
                List = dbItems.Select(n => converter.Convert(n)).ToList()
            };

            userMessagesGridControl.Model = model;
            userMessagesGridControl.DataBind();
        }


    }
}