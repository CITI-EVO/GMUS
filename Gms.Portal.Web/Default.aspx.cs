using System;
using CITI.EVO.Tools.Utils;
using DocumentFormat.OpenXml.Bibliography;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

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
