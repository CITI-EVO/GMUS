using System;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Collections;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Controls.User
{
    public partial class AssigneUsersControl : BaseUserControlExtend<AssigneUsersModel>
    {
        protected HashLookup<int?, Guid?> Users
        {
            get { return ViewState["Users"] as HashLookup<int?, Guid?>; }
            set { ViewState["Users"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var users = (from n in UmUsersCache.Users
                         where n.DateDeleted == null
                         select new
                         {
                             n.ID,
                             Name = $"{n.LoginName} - {n.Email}"
                         });

            cbxUsers.BindData(users);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Users == null)
                return;

            var users = (from n in Users
                         from m in n
                         select new
                         {
                             Step = n.Key,
                             UserID = m
                         });

            gvData.DataSource = users;
            gvData.DataBind();
        }

        public override void SetModel(AssigneUsersModel model)
        {
            base.SetModel(model);

            Users = model.Users as HashLookup<int?, Guid?>;
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            var lookup = Users;
            if (lookup == null)
                lookup = new HashLookup<int?, Guid?>();

            var step = DataConverter.ToNullableInt(seStep.Text);
            var userID = cbxUsers.TryGetGuidValue();

            if (step == null || userID == null)
                return;

            if (lookup.Contains(step, userID))
                return;

            lookup.Add(step, userID);

            Users = lookup;
        }

        protected void btnDeleteUser_OnCommand(object sender, CommandEventArgs e)
        {
            var lookup = Users;
            if (lookup == null)
                return;

            var commandArg = Convert.ToString(e.CommandArgument);

            var parts = commandArg.Split('@');
            if (parts.Length < 2)
                return;

            var step = DataConverter.ToNullableInt(parts[0]);
            var userID = DataConverter.ToNullableGuid(parts[1]);

            if (step == null || userID == null)
                return;

            lookup.Remove(step, userID);

            Users = lookup;

            OnDataChanged(e);
        }

        public override AssigneUsersModel GetModel()
        {
            var model = base.GetModel();
            model.Users = Users;

            return model;
        }

        protected String GetUserLogin(Object eval)
        {
            var userID = DataConverter.ToNullableGuid(eval);
            if (userID == null)
                return null;

            var user = UmUsersCache.GetUser(userID);
            if (user == null)
                return null;

            return user.LoginName;
        }

        protected String GetUserEmail(Object eval)
        {
            var userID = DataConverter.ToNullableGuid(eval);
            if (userID == null)
                return null;

            var user = UmUsersCache.GetUser(userID);
            if (user == null)
                return null;

            return user.LoginName;
        }

        protected String GetUserName(Object eval)
        {
            var userID = DataConverter.ToNullableGuid(eval);
            if (userID == null)
                return null;

            var user = UmUsersCache.GetUser(userID);
            if (user == null)
                return null;

            return $"{user.FirstName} {user.LastName}";
        }

        protected String GetCommandArg(Object step, Object userID)
        {
            return $"{step}@{userID}";
        }
    }
}