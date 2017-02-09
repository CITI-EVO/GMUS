using System;
using CITI.EVO.UserManagement.Web.Bases;

public partial class _Default : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/Management/UsersList.aspx");
    }
}
