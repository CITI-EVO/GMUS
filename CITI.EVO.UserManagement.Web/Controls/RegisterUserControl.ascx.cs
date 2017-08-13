using System;
using System.Collections;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Models;
using CITI.EVO.UserManagement.Web.Utils;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Controls
{
    public partial class RegisterUserControl : BaseUserControlExtend<RegisterUserModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var groups = (from n in HbSession.Query<UM_Group>()
                          where n.DateDeleted == null &&
                                Enumerable.Contains(ConfigUtil.UserRegisterGroupID, n.ID)
                          select n).ToList();

            foreach (var @group in groups)
            {
                var trn = new DefaultTranslatable(@group.Name);
                @group.Name = trn.Text;
            }

            BindData(cbxGroups, groups);

            pnlGroup.Visible = true;

            if (groups.Count == 1)
            {
                var group = groups[0];

                pnlGroup.Visible = false;
                cbxGroups.TrySetSelectedValue(group.ID);
            }

            ApplyViewMode();
        }

        protected void BindData(ListControl control, IEnumerable source)
        {
            var selValue = control.TryGetStringValue();

            control.Items.Clear();

            control.DataSource = source;
            control.DataBind();

            control.Items.Insert(0, new ListItem("Select an Option", String.Empty));

            control.TrySetSelectedValue(selValue);
        }

        protected void ApplyViewMode()
        {
            var orgGroupID = Guid.Parse("22D342C4-158D-4312-915D-9B0C1EF818A5");

            var model = Model;

            if (model.GroupID == orgGroupID)
            {
                pnlPersonalID.Visible = true;

                lblGroups.Text = "ჯგუფი";
                lblPersonalID.Text = "ორგანიზაციის დასახელება";
                lblFirstName.Text = "პასუხისმგებელი პირის სახელი";
                lblLastName.Text = "პასუხისმგებელი პირის გვარი";
                lblLoginName.Text = "ელექტრონული ფოსტა";
                lblPassword.Text = "პაროლი";
                lblConfirmPassword.Text = "დაადასტურეთ პაროლი";
            }
            else
            {
                pnlPersonalID.Visible = false;

                lblGroups.Text = "ჯგუფი";
                lblPersonalID.Text = "პირადი N";
                lblFirstName.Text = "სახელი";
                lblLastName.Text = "გვარი";
                lblLoginName.Text = "ელ.ფოსტა";
                lblPassword.Text = "პაროლი";
                lblConfirmPassword.Text = "დაადასტურეთ პაროლი";
            }
        }
    }
}