using System;
using System.Data;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Bases;

public partial class _Default : BasePage
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
