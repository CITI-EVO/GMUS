using System;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Models;

namespace CITI.EVO.UserManagement.Web.Controls
{
    public partial class CreateUserControl : BaseUserControlExtend<CreateUserModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void userDataControl_OnDataChanged(object sender, EventArgs e)
        {
            OnDataChanged(e);
        }

        protected void SelectGroupsControl_OnDataChanged(object sender, EventArgs e)
        {
            OnDataChanged(e);
        }
    }
}