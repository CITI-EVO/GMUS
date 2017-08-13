using System;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Controls.User
{
    public partial class UserMessageControl : BaseUserControlExtend<UserMessageExModel>
    {
        public bool CreateMode { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            pnlAnswer.Visible = !CreateMode;
        }
    }
}