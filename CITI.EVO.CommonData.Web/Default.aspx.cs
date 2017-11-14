using System;
using CITI.EVO.CommonData.Web.Helpers.GovTalk;
using CITI.EVO.CommonData.Web.Services.Managers;
using CITI.EVO.Tools.Utils;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var command = Convert.ToString(Request["command"]);
        if (command == "setup")
        {
            var schema = new NHibernate.Tool.hbm2ddl.SchemaExport(Hb8Factory.Configuration);
            schema.Create(false, true);
        }
    }
}