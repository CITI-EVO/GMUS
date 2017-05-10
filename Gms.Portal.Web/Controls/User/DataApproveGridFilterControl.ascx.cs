using System;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;

namespace Gms.Portal.Web.Controls.User
{
    public partial class DataApproveGridFilterControl : BaseUserControlExtend<DataApproveGridFilterModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyViewMode();
        }

        protected void ApplyViewMode()
        {
            var dbForms = (from n in HbSession.Query<GM_Form>()
                           where n.DateDeleted == null &&
                                 n.RequiresApprove == true &&
                                 n.Visible == true
                           orderby n.OrderIndex, n.Name
                           select n).ToList();

            var converter = new FormEntityModelConverter(HbSession);
            var models = dbForms.Select(n => converter.Convert(n)).ToList();

            cbxForm.BindData(models);

            var formID = DataConverter.ToNullableGuid(RequestUrl["FormID"]);
            if (formID != null && !IsPostBack)
                cbxForm.TrySetSelectedValue(formID);

            formID = cbxForm.TryGetGuidValue();

            if (formID != null)
            {
                var fields = (from n in models
                              where n.ID == formID &&
                                    n.Entity != null
                              from m in FormStructureUtil.PreOrderFirstLevelTraversal(n.Entity).OfType<FieldEntity>()
                              where (m.Type == "Lookup" || m.Type == "ComboBox") &&
                                    !String.IsNullOrWhiteSpace(m.DataSourceID) &&
                                    m.ValueExpression == FormDataConstants.IDField &&
                                    m.RequiresApproval == true
                              orderby m.OrderIndex, m.Name
                              select m).ToList();

                cbxField.BindData(fields);

                var fieldID = DataConverter.ToNullableGuid(RequestUrl["FieldID"]);
                if (fieldID != null)
                {
                    if (!IsPostBack)
                        cbxField.TrySetSelectedValue(fieldID);
                }
            }
        }
    }
}