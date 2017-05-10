using System;
using System.Linq;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class RecipientControl : BaseUserControlExtend<RecipientModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                FillLists();
        }

        public void FillLists()
        {
            cbxUsers.DataSource = UmUsersCache.Users.Where(n => n.DateDeleted == null && n.IsActive).ToList();
            cbxUsers.DataBind();
        }
    }
}