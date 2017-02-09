using System;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Models;

namespace CITI.EVO.UserManagement.Web.Controls
{
    public partial class SelectUserControl : BaseUserControlExtend<SelectUserModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void searchUsersControl_OnDataChanged(object sender, EventArgs e)
        {
            OnDataChanged(e);
        }
    }
}