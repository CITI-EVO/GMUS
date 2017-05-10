using System;
using System.Linq;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;
using NHibernate.Linq;
using CITI.EVO.Tools.Extensions;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class RecipientGroupControl : BaseUserControlExtend<RecipientGroupModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var forms = (from n in HbSession.Query<GM_Form>()
                               where n.DateDeleted == null
                               orderby n.Name
                               select n).ToList();

            cbxForm.BindData(forms);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ApplyViewMode();
        }

        private void ApplyViewMode()
        {
            var model = Model;

            pnlForm.Visible = (model.Type == "Expression");
            pnlExpression.Visible = (model.Type == "Expression");
        }
    }
}