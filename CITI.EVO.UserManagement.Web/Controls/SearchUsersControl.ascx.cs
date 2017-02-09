using System;
using System.Linq;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Controls
{
    public partial class SearchUsersControl : BaseUserControlExtend<SearchUsersModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btSearchUsers_Click(object sender, EventArgs e)
        {
            var users = (from n in HbSession.Query<UM_User>()
                         where n.DateDeleted == null
                         select n).ToList();

            if (!String.IsNullOrWhiteSpace(tbUsersKeyword.Text))
            {
                users = (from n in users
                         where n.LoginName.Trim().Contains(tbUsersKeyword.Text.Trim())
                         select n).ToList();
            }

            lstUsers.DataSource = users;
            lstUsers.DataBind();

            OnDataChanged(e);
        }
    }
}