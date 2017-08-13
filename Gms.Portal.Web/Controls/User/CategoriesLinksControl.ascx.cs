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

        protected string GetFormatedName(object eval)
        {
            var name = Convert.ToString(eval);
            if (string.IsNullOrWhiteSpace(name) || name.Length <= 40)
                return name;

            return $"{name.Substring(0, 37)}...";
        }
    }
}