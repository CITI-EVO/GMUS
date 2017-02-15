using System;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models.Common;

namespace Gms.Portal.Web.Controls.Common
{
	public partial class NamedExpressionControl : BaseUserControlExtend<NamedExpressionModel>
	{
		public String Key
		{
			get { return Convert.ToString(ViewState["Key"]); }
			set { ViewState["Key"] = value; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}