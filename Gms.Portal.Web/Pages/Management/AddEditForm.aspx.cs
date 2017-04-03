using System;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Converters.ModelToEntity;
using NHibernate.Linq;
using System.Linq;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class AddEditForm : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var model = new FormModel
                {
                    Number = Convert.ToString((uint)Guid.NewGuid().GetHashCode()),
                };

                var formID = DataConverter.ToNullableGuid(RequestUrl["FormID"]);
                if (formID != null)
                {
                    var entity = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == formID);
                    if (entity != null)
                    {
                        var converter = new FormEntityModelConverter(HbSession);
                        model = converter.Convert(entity);
                    }
                }

                formControl.Model = model;
            }
        }

        protected void btnCancelForm_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Management/FormsList.aspx");
        }

        protected void btnSaveForm_OnClick(object sender, EventArgs e)
        {
            var formID = DataConverter.ToNullableGuid(RequestUrl["FormID"]);

            var entity = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == formID);

            var converter = new FormModelEntityConverter(HbSession);

            var model = formControl.Model;
            if (entity == null)
                entity = converter.Convert(model);
            else
                converter.FillObject(entity, model);

            HbSession.SubmitChanges(entity);

            Response.Redirect("~/Pages/Management/FormsList.aspx");
        }
    }
}