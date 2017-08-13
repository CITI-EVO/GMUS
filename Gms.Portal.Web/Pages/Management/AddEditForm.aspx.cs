using System;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Converters.ModelToEntity;
using NHibernate.Linq;
using System.Linq;
using CITI.EVO.Tools.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class AddEditForm : BasePage
    {
        public Guid? FormID
        {
            get
            {
                var formID = DataConverter.ToNullableGuid(RequestUrl["FormID"]);
                return formID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var model = new FormModel
                {
                    Number = Convert.ToString((uint)Guid.NewGuid().GetHashCode()),
                };

                if (FormID != null)
                {
                    var entity = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == FormID);
                    if (entity != null)
                    {
                        var converter = new FormEntityModelConverter(HbSession);
                        model = converter.Convert(entity);
                    }
                }

                formControl.Model = model;
            }

            btnPreview.Visible = (FormID != null);
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            var returnUrl = DataConverter.ToString(RequestUrl["ReturnUrl"]);

            if (string.IsNullOrWhiteSpace(returnUrl))
                returnUrl = "~/Pages/Management/FormsList.aspx";

            Response.Redirect(returnUrl);
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            var entity = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == FormID);

            var converter = new FormModelEntityConverter(HbSession);

            var model = formControl.Model;
            if (entity == null)
                entity = converter.Convert(model);
            else
                converter.FillObject(entity, model);

            HbSession.SubmitChanges(entity);

            var url = new UrlHelper(RequestUrl) { ["FormID"] = entity.ID };

            Response.Redirect(url.ToEncodedUrl());
        }

        protected void btnPreview_OnClick(object sender, EventArgs e)
        {
            var returnUrl = RequestUrl.ToEncodedUrl();

            var url = new UrlHelper("~/Pages/User/FormDataView.aspx")
            {
                ["Mode"] = "Preview",
                ["FormID"] = RequestUrl["FormID"],
                ["OwnerID"] = RequestUrl["FormID"],
                ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrl),
            };

            Response.Redirect(url.ToEncodedUrl());
        }
    }
}