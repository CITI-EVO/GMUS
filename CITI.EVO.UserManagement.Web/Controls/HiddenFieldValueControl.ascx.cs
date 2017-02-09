using System;

namespace CITI.EVO.UserManagement.Web.Controls
{
    public partial class HiddenFieldValueControl : System.Web.UI.UserControl
    {
        public Object Value
        {
            get { return hdValue.Value; }
            set { hdValue.Value = Convert.ToString(value); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}