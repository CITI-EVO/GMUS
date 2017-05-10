using System;
using CITI.EVO.Tools.Helpers;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Models.Helpers;

namespace Gms.Portal.Web.Controls.User
{
    public partial class FormsLinksControl : BaseUserControlExtend<CategoriesFormsModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void SetModel(CategoriesFormsModel model)
        {
            rptForms.DataSource = model.List;
            rptForms.DataBind();
        }

        protected String GetLinkUrl(object eval)
        {
            var urlHelper = new UrlHelper("~/Pages/User/FormDataGrid.aspx")
            {
                ["FormID"] = eval,
                ["OwnerID"] = eval
            };

            return urlHelper.ToEncodedUrl();
        }
    }
}