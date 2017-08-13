using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;

namespace Gms.Portal.Web.Controls.User
{
    public partial class AssigneUsersControl : BaseUserControlExtend<AssigneUsersModel>
    {
        protected ISet<Guid?> Users
        {
            get { return ViewState["Users"] as ISet<Guid?>; }
            set { ViewState["Users"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var users = (from n in UmUsersCache.Users
                         where n.DateDeleted == null
                         select new IDNameEntity
                         {
                             ID = n.ID,
                             Name = $"{n.LoginName} - {n.Email}"
                         });

            cbxUsers.BindData(users);

            var recipientsGroups = (from n in HbSession.Query<GM_RecipientGroup>()
                                    where n.DateDeleted == null
                                    select new IDNameEntity
                                    {
                                        ID = n.ID,
                                        Name = n.Name
                                    });

            cbxRecipientsGroup.BindData(recipientsGroups);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Users == null)
                return;

            var users = (from n in Users
                         select new
                         {
                             UserID = n
                         });

            gvData.DataSource = users;
            gvData.DataBind();
        }

        public override void SetModel(AssigneUsersModel model)
        {
            base.SetModel(model);

            Users = model.Users as HashSet<Guid?>;
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            var @set = (Users ?? new HashSet<Guid?>());

            var userID = cbxUsers.TryGetGuidValue();
            if (userID == null)
                return;

            if (@set.Contains(userID))
                return;

            @set.Add(userID);

            Users = @set;
        }

        protected void btnDeleteUser_OnCommand(object sender, CommandEventArgs e)
        {
            var @set = Users;
            if (@set == null)
                return;

            var commandArg = Convert.ToString(e.CommandArgument);

            var parts = commandArg.Split('@');
            if (parts.Length < 2)
                return;

            var step = DataConverter.ToNullableInt(parts[0]);
            var userID = DataConverter.ToNullableGuid(parts[1]);

            if (step == null || userID == null)
                return;

            @set.Remove(userID);

            Users = @set;

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

        protected void cbxRecipientsGroup_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var groupID = cbxRecipientsGroup.TryGetGuidValue();
            if (groupID == null)
                return;

            var group = HbSession.Query<GM_RecipientGroup>().FirstOrDefault(n => n.ID == groupID);
            if (group == null)
                return;

            var newUsers = RecipientsGroupUtil.GetRecipients(@group).Select(n => n.UserID);

            var users = Users;
            if (users == null)
                users = new HashSet<Guid?>();

            users.Clear();
            users.UnionWith(newUsers);

            Users = users;
        }
    }
}