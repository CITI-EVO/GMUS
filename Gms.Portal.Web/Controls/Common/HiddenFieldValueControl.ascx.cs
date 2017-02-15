using System;

namespace Gms.Portal.Web.Controls.Common
{
    public partial class HiddenFieldValueControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public Object Value
        {
            get { return hdValue.Value; }
            set { hdValue.Value = Convert.ToString(value); }
        }
    }
}