using System;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class DataChangeLinkGenControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var session = Hb8Factory.InitSession();
            var dbForms = session.Query<GM_Form>().Where(n => n.DateDeleted == null);

            cbxForm.BindData(dbForms);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ApplyViewMode();
        }

        protected void cbxForm_OnSelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btnGenerate_OnClick(object sender, EventArgs e)
        {
            var formID = cbxForm.TryGetGuidValue();
            var fieldKey = cbxField.TryGetStringValue();

            var fieldVal = tbxValue.Text;

            var currUrl = Request.Url;

            var newUrl = $"{currUrl.Scheme}://{currUrl.Host}";
            if(!currUrl.IsDefaultPort)
                newUrl = $"{newUrl}:{currUrl.Port}";

            var changerUrl = ResolveUrl("~/Handlers/DataChanger.ashx");
            newUrl = $"{newUrl}{changerUrl}";

            var url = new UrlHelper(newUrl);
            url["FormID"] = formID;
            url["FieldKey"] = fieldKey;
            url["FieldVal"] = fieldVal;

            var hyperLink = new HyperLink
            {
                NavigateUrl = url.ToEncodedUrl(),
                Text = tbxText.Text
            };

            var link = hyperLink.RenderString();
            tbxLink.Text = link;
        }

        protected void ApplyViewMode()
        {
            var formID = cbxForm.TryGetGuidValue();
            var session = Hb8Factory.InitSession();

            if (formID != null)
            {
                var dbForm = session.Query<GM_Form>().FirstOrDefault(n => n.ID == formID);

                var converter = new FormEntityModelConverter(session);
                var formModel = converter.Convert(dbForm);

                var controls = FormStructureUtil.PreOrderFirstLevelTraversal(formModel.Entity);
                var fields = controls.OfType<FieldEntity>();

                cbxField.BindData(fields);
            }
        }
    }
}