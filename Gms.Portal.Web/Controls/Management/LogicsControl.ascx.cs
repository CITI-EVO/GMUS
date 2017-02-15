using System;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Controls.Management
{
	public partial class LogicsControl : BaseUserControlExtend<LogicsModel>
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        public override void SetModel(object model)
        {
			var logicsModel = (LogicsModel)model;
			gvData.DataSource = logicsModel.List;
			gvData.DataBind();
		}
	}
}