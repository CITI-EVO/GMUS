using System;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Converters.ModelToEntity;
using NHibernate.Linq;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class AddEditLogic : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var logicID = DataConverter.ToNullableGuid(RequestUrl["LogicID"]);
                if (logicID == null)
                    return;

                var entity = HbSession.Query<GM_Logic>().FirstOrDefault(n => n.ID == logicID);
                if (entity == null)
                    return;

                var converter = new LogicEntityModelConverter(HbSession);
                var model = converter.Convert(entity);

                logicControl.Model = model;
            }
        }

        protected void btnCancelLogic_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Management/LogicsList.aspx");
        }

        protected void btnSaveLogic_OnClick(object sender, EventArgs e)
        {
            var logicID = DataConverter.ToNullableGuid(RequestUrl["LogicID"]);

            var entity = HbSession.Query<GM_Logic>().FirstOrDefault(n => n.ID == logicID);

            var converter = new LogicModelEntityConverter(HbSession);

            var model = logicControl.Model;
            if (entity == null)
                entity = converter.Convert(model);
            else
                converter.FillObject(entity, model);

            HbSession.SubmitChanges(entity);

            Response.Redirect("~/Pages/Management/LogicsList.aspx");
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            mpePreview.Hide();
        }
    }
}