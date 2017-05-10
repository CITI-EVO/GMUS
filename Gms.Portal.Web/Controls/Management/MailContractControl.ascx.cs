using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Core.Common;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using DevExpress.Web;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class MailContractControl : BaseUserControlExtend<MailContactModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dbForms = HbSession.Query<GM_Form>().Where(n => n.DateDeleted == null);

            cbxForm.BindData(dbForms);

            if (!IsPostBack)
                FillLists();

            ApplyViewMode();
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            var model = Model;

            using (var transaction = HbSession.BeginTransaction())
            {
                var messageTemplate = HbSession.Query<GM_MessageTemplate>().FirstOrDefault(n => n.ID == model.TemplateID);
                if (messageTemplate == null)
                {
                    messageTemplate = EntityFactory.CreateEntity<GM_MessageTemplate>();
                    HbSession.Save(messageTemplate);
                }
                else
                {
                    HbSession.Update(messageTemplate);
                }

                messageTemplate.Name = model.TemplateName;
                messageTemplate.Subject = model.Subject;
                messageTemplate.Body = model.Body;

                transaction.Commit();
            }

            FillLists();
            ApplyViewMode();
        }

        protected void btnGenLink_OnClick(object sender, EventArgs e)
        {
            mpeDataChangeLinkGen.Show();
        }

        protected void cbCheckAll_OnCheckedChanged(object sender, EventArgs e)
        {
            foreach (ListItem item in cblRecipients.Items)
                item.Selected = cbCheckAll.Checked;
        }

        protected void btnDataChangeLinkGenCancel_OnClick(object sender, EventArgs e)
        {
            mpeDataChangeLinkGen.Hide();
        }

        protected void FillLists()
        {
            var recipientGroups = (from n in HbSession.Query<GM_RecipientGroup>()
                                   where n.DateDeleted == null
                                   orderby n.Name
                                   select n);

            cbxRecipientGroups.DataSource = recipientGroups.ToList();
            cbxRecipientGroups.DataBind();

            var templatesQuery = (from n in HbSession.Query<GM_MessageTemplate>()
                                  where n.DateDeleted == null
                                  orderby n.Name
                                  select n);

            var templatesList = templatesQuery.ToList();
            templatesList.Insert(0, new GM_MessageTemplate() { Name = "- New Template -" });

            cbxMessageTemplate.BindData(templatesList);
        }

        protected void ApplyViewMode()
        {
            var model = Model;

            if (model.RecipientGroupID != null)
            {
                var selectedItems = (from n in cblRecipients.Items.OfType<ListItem>()
                                     where n.Selected
                                     select n.Value).ToHashSet();

                var recipients = RecipientsGroupUtil.GetRecipients(model.RecipientGroupID);

                var users = (from n in recipients
                             let u = UmUsersCache.GetUser(n.UserID)
                             select new 
                             {
                                 Value = Convert.ToString(u.ID),
                                 Text = $"{u.LastName} {u.FirstName}, {u.Email}, {u.Phone}"
                             }).ToList();

                cblRecipients.DataSource = users;
                cblRecipients.DataBind();

                foreach (var item in cblRecipients.Items.OfType<ListItem>())
                    item.Selected = selectedItems.Contains(item.Value);
            }

            if (model.TemplateID != null)
            {
                pnlSaveTemplate.Visible = true;
                pnlTemplateName.Visible = true;

                if (model.TemplateID == Guid.Empty)
                    tbxTemplateName.Text = String.Empty;

                var messageTemplate = HbSession.Query<GM_MessageTemplate>().FirstOrDefault(n => n.ID == model.TemplateID);
                if (messageTemplate != null)
                {
                    tbxTemplateName.Text = messageTemplate.Name;
                    tbxSubject.Text = messageTemplate.Subject;
                    tbxBody.Text = messageTemplate.Body;
                }
            }
        }

        public override MailContactModel GetModel()
        {
            var model = base.GetModel();

            var selectedIds = (from n in cblRecipients.Items.Cast<ListItem>()
                               let v = DataConverter.ToNullableGuid(n.Value)
                               where v != null && n.Selected
                               select v.Value).ToList();

            model.RecipientsID = selectedIds;

            return model;
        }
    }
}