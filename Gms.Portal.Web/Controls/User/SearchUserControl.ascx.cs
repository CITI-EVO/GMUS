using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Controls.User
{
    public partial class SearchUserControl : BaseUserControlExtend<SearchUsersModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var users = (from n in UmUsersCache.Users
                         where n.DateDeleted == null
                         select new
                         {
                             n.ID,
                             Name = $"{n.LoginName} - {n.Email}"
                         });


            lstUsers.BindData(users);
        }

        public override SearchUsersModel GetModel()
        {
            var @set = (from n in lstUsers.Items.OfType<ListItem>()
                        where n.Selected
                        select n.Value).ToHashSet();

            var model = base.GetModel();

            var users = (from n in @set
                         let v = DataConverter.ToNullableGuid(n.Trim())
                         where v != null
                         select v.Value);

            model.Users = users.ToHashSet();
            return model;
        }

        public override void SetModel(SearchUsersModel model)
        {
            if (model != null && model.Users != null)
            {
                foreach (var item in lstUsers.Items.OfType<ListItem>())
                {
                    var userID = DataConverter.ToNullableGuid(item.Value);
                    item.Selected = model.Users.Contains(userID.GetValueOrDefault());
                }
            }

            base.SetModel(model);
        }
    }
}