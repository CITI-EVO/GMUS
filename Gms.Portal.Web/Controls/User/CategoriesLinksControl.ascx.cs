using System;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Models.Helpers;

namespace Gms.Portal.Web.Controls.User
{
    public partial class CategoriesLinksControl : BaseUserControlExtend<CategoriesFormsModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void SetModel(CategoriesFormsModel model)
        {
            rptForms.DataSource = model.List;
            rptForms.DataBind();
        }
    }
}