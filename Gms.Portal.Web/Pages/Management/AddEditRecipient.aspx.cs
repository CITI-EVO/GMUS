using System;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.ModelToEntity;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class AddEditRecipient : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillRecipientGrid();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            var model = new RecipientModel
            {
                GroupID = DataConverter.ToGuid(RequestUrl["groupID"])
            };

            recipientControl.Model = model;
            mpeRecipient.Show();
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Management/RecipientGroupsList.aspx");
        }

        protected void recipientsControl_OnView(object sender, GenericEventArgs<Guid> e)
        {
            var returnUrl = RequestUrl.ToEncodedUrl();

            var url = new UrlHelper("~/Pages/Management/NotificationHistory.aspx")
            {
                ["RecipientID"] = e.Value,
                ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrl),
            };

            Response.Redirect(url.ToEncodedUrl());
        }

        protected void recipientsControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Query<GM_Recipient>().FirstOrDefault(n => n.ID == e.Value);
            if (entity != null)
                entity.DateDeleted = DateTime.Now;

            HbSession.SubmitChanges(entity);

            FillRecipientGrid();
        }

        protected void btnRecipientOK_OnClick(object sender, EventArgs e)
        {
            var model = recipientControl.Model;
            var groupId = DataConverter.ToGuid(RequestUrl["groupID"]);

            var converter = new RecipientModelEntityConverter(HbSession);


            var entity = HbSession.Get<GM_Recipient>(model.ID);
            if (entity == null)
            {
                var existingItem = HbSession.Query<GM_Recipient>().FirstOrDefault(n => n.UserID == model.UserID && n.GroupID == groupId && n.DateDeleted == null);
                if (existingItem != null)
                {
                    lblRecipient.Text = "User already has been added";
                    mpeRecipient.Show();
                    return;
                }
                entity = converter.Convert(model);
            }
            else
            {
                converter.FillObject(entity, model);
            }

            HbSession.SubmitChanges(entity);

            FillRecipientGrid();

            mpeRecipient.Show();
        }

        protected void btnRecipientCancel_OnClick(object sender, EventArgs e)
        {
            mpeRecipient.Hide();
        }

        private void FillRecipientGrid()
        {
            var groupID = DataConverter.ToNullableGuid(RequestUrl["groupID"]);
            if (groupID == null)
                return;

            var group = HbSession.Query<GM_RecipientGroup>().FirstOrDefault(n => n.ID == groupID);
            if (group == null)
                return;

            lblDescription.Text = group.Description;
            btnNew.Visible = (group.Type != "Expression");
            recipientsControl.CommandsVisible = (group.Type != "Expression");

            var model = new RecipientsModel();
            model.List = RecipientsGroupUtil.GetRecipients(group).ToList();
            model.Description = group.Description;

            recipientsControl.Model = model;
            recipientsControl.DataBind();
        }
    }
}