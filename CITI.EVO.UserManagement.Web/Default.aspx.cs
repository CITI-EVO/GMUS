using System;
using System.Globalization;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.Web.Bases;

public partial class _Default : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var command = Convert.ToString(RequestUrl["command"]);
        if (command == "setup")
        {
            var schema = new NHibernate.Tool.hbm2ddl.SchemaExport(Hb8Factory.Configuration);
            schema.Create(false, true);
        }

        Response.Redirect("~/Pages/Management/UsersList.aspx");
    }
}
